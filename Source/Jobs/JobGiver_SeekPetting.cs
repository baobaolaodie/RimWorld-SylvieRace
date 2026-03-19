#nullable enable
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace SylvieMod;

/// <summary>
/// ThinkNode that gives Sylvie pawns the job to seek petting from colonists.
/// Checks various conditions including mood, cooldown, and target availability.
/// </summary>
public class JobGiver_SeekPetting : ThinkNode_JobGiver
{
    private const int MinCheckInterval = GenDate.TicksPerHour; // 最小检查间隔 1 小时 (2500 ticks)
    private const float CheckChance = 0.20f; // 每次检查有 20% 概率继续
    private const int MaxSearchDistance = 40;
    private const int MinAgeYears = 10;
    private const int HighOpinionThreshold = 40;
    private const float TargetMinMoodThreshold = 0.50f; // 50%
    
    // 每个pawn的最后检查时间
    private static Dictionary<Pawn, int> lastCheckTicks = new Dictionary<Pawn, int>();

    /// <summary>
    /// Attempts to give the seek petting job to the pawn.
    /// Performs all necessary checks before returning a job.
    /// </summary>
    /// <param name="pawn">The Sylvie pawn seeking petting</param>
    /// <returns>A Job instance if all conditions are met, null otherwise</returns>
    protected override Job TryGiveJob(Pawn pawn)
    {
        // Race check first - 非希尔薇种族静默返回，不输出任何日志
        if (!SylvieDefNames.IsSylvieRace(pawn))
        {
            return null!;
        }

        int currentTick = Find.TickManager.TicksGame;
        
        // 检查间隔 - 静默检查，未到达间隔时不输出日志
        int lastCheck = lastCheckTicks.TryGetValue(pawn, out int last) ? last : -1;
        int ticksSinceLastCheck = lastCheck < 0 ? int.MaxValue : currentTick - lastCheck;
        
        // 必须满足最小间隔，否则静默返回
        if (ticksSinceLastCheck < MinCheckInterval)
        {
            return null!;
        }
        
        // 概率检查 - 20% 概率继续
        float checkRoll = Rand.Value;
        bool checkPassed = checkRoll <= CheckChance;
        lastCheckTicks[pawn] = currentTick;
        
        if (!checkPassed)
        {
            Log.Message($"[SylvieMod] {pawn.LabelShort}: [1/7] Interval check failed - ticksSinceLast: {ticksSinceLastCheck}, roll: {checkRoll:F4} ({checkRoll*100:F1}%), required: <= {CheckChance:F2} ({CheckChance*100:F0}%)");
            return null!;
        }
        
        // 通过间隔和概率检查，输出日志
        Log.Message($"[SylvieMod] {pawn.LabelShort}: [1/7] Interval check passed - ticksSinceLast: {ticksSinceLastCheck}, roll: {checkRoll:F4} ({checkRoll*100:F1}%)");

        // Age check - must be at least 10 years old
        int currentAge = pawn.ageTracker.AgeBiologicalYears;
        bool agePassed = currentAge >= MinAgeYears;
        Log.Message($"[SylvieMod] {pawn.LabelShort}: [2/7] Age check - current: {currentAge}, required: >= {MinAgeYears}, passed: {agePassed}");
        if (!agePassed)
        {
            return null!;
        }

        // State checks - not sleeping, not downed, not in mental state
        bool isAwake = pawn.Awake();
        bool isDowned = pawn.Downed;
        bool inMentalState = pawn.InMentalState;
        bool statePassed = isAwake && !isDowned && !inMentalState;
        Log.Message($"[SylvieMod] {pawn.LabelShort}: [3/7] State check - Awake: {isAwake}, Downed: {isDowned}, MentalState: {inMentalState}, passed: {statePassed}");
        if (!statePassed)
        {
            return null!;
        }

        // Cooldown check - 总是输出日志，即使在冷却中
        SylvieSeekPettingTracker? tracker = Current.Game?.GetComponent<SylvieSeekPettingTracker>();
        bool inCooldown = tracker != null && tracker.IsInCooldown(pawn);
        int remainingCooldown = inCooldown ? tracker!.GetCooldownRemaining(pawn) : 0;
        int remainingHours = remainingCooldown / GenDate.TicksPerHour;
        Log.Message($"[SylvieMod] {pawn.LabelShort}: [4/7] Cooldown check - trackerExists: {tracker != null}, inCooldown: {inCooldown}, remaining: {remainingCooldown} ticks (~{remainingHours}h), passed: {!inCooldown}");
        if (inCooldown)
        {
            return null!;
        }

        // Idle check - must be idle or have interruptible job
        string? currentJobName = pawn.CurJob?.def?.defName ?? "null (idle)";
        bool idlePassed = IsIdleForPetting(pawn);
        Log.Message($"[SylvieMod] {pawn.LabelShort}: [5/7] Idle check - currentJob: {currentJobName}, passed: {idlePassed}");
        if (!idlePassed)
        {
            return null!;
        }

        // Find best target
        Pawn? target = FindBestTarget(pawn);
        bool targetPassed = target != null;
        Log.Message($"[SylvieMod] {pawn.LabelShort}: [6/7] Target check - found: {target?.LabelShort ?? "null"}, passed: {targetPassed}");
        if (!targetPassed)
        {
            return null!;
        }

        // JobDef check
        JobDef? seekPettingJobDef = SylvieDefNames.Job_SeekPettingDef;
        bool jobDefPassed = seekPettingJobDef != null;
        Log.Message($"[SylvieMod] {pawn.LabelShort}: [7/7] JobDef check - found: {jobDefPassed}");
        if (!jobDefPassed)
        {
            return null!;
        }

        // Interrupt current non-critical job if any
        InterruptNonCriticalJob(pawn);

        // Create and return the job
        Job job = JobMaker.MakeJob(seekPettingJobDef, target);
        job.locomotionUrgency = LocomotionUrgency.Jog;
        job.expiryInterval = 5000;

        Log.Message($"[SylvieMod] {pawn.LabelShort}: ====== SUCCESS! Assigned seek petting job to {target!.LabelShort} ======");
        return job;
    }

    /// <summary>
    /// Checks if the pawn is idle or has a job that can be interrupted for petting.
    /// </summary>
    /// <param name="pawn">The pawn to check</param>
    /// <returns>True if the pawn is available for petting</returns>
    private bool IsIdleForPetting(Pawn pawn)
    {
        Job? curJob = pawn.CurJob;
        if (curJob == null)
            return true;

        // If current job is critical, cannot interrupt
        if (IsCriticalJob(curJob.def))
            return false;

        // Job can be interrupted
        return true;
    }

    /// <summary>
    /// Determines if a job is critical and should not be interrupted.
    /// </summary>
    /// <param name="jobDef">The job definition to check</param>
    /// <returns>True if the job is critical</returns>
    private bool IsCriticalJob(JobDef jobDef)
    {
        // Critical jobs that should not be interrupted
        if (jobDef == JobDefOf.Rescue)
            return true;
        if (jobDef == JobDefOf.TendPatient)
            return true;
        if (jobDef == JobDefOf.BeatFire)
            return true;
        if (jobDef == JobDefOf.AttackMelee)
            return true;
        if (jobDef == JobDefOf.AttackStatic)
            return true;
        if (jobDef == JobDefOf.Ingest)
            return true;
        if (jobDef == JobDefOf.LayDown)
            return true;

        return false;
    }

    /// <summary>
    /// Finds the best target pawn for petting based on relationship, mood, and distance.
    /// </summary>
    /// <param name="sylvie">The Sylvie pawn seeking petting</param>
    /// <returns>The best target pawn, or null if no suitable target found</returns>
    private Pawn? FindBestTarget(Pawn sylvie)
    {
        if (sylvie.Map == null)
        {
            Log.Message($"[SylvieMod] {sylvie.LabelShort}: FindBestTarget - Map is null");
            return null;
        }

        Map map = sylvie.Map;
        IEnumerable<Pawn> allPawns = map.mapPawns.SpawnedPawnsInFaction(sylvie.Faction);
        Log.Message($"[SylvieMod] {sylvie.LabelShort}: FindBestTarget - total pawns in faction: {allPawns.Count()}");
        
        IEnumerable<Pawn> candidates = allPawns.Where(p => IsValidTarget(p, sylvie));

        if (!candidates.Any())
        {
            Log.Message($"[SylvieMod] {sylvie.LabelShort}: FindBestTarget - no valid candidates found");
            return null;
        }

        Log.Message($"[SylvieMod] {sylvie.LabelShort}: FindBestTarget - found {candidates.Count()} candidate(s)");

        // Score and sort candidates
        List<(Pawn pawn, float score)> scoredCandidates = new List<(Pawn, float)>();

        foreach (Pawn candidate in candidates)
        {
            float score = CalculateTargetScore(candidate, sylvie);
            scoredCandidates.Add((candidate, score));
            
            int opinion = sylvie.relations?.OpinionOf(candidate) ?? 0;
            float? targetMood = candidate.needs?.mood?.CurLevelPercentage;
            float distance = sylvie.Position.DistanceTo(candidate.Position);
            Log.Message($"[SylvieMod] {sylvie.LabelShort}: Candidate {candidate.LabelShort} - opinion: {opinion}, mood: {targetMood?.ToString("F2") ?? "null"}, distance: {distance:F1}, score: {score:F2}");
        }

        // Sort by score descending, then by distance
        scoredCandidates = scoredCandidates
            .OrderByDescending(x => x.score)
            .ThenBy(x => x.pawn.Position.DistanceToSquared(sylvie.Position))
            .ToList();

        // Return the best candidate
        if (scoredCandidates.Count == 0)
        {
            return null;
        }
        
        Pawn? bestTarget = scoredCandidates[0].pawn;
        if (bestTarget != null)
        {
            Log.Message($"[SylvieMod] {sylvie.LabelShort}: Best target selected: {bestTarget.LabelShort} with score {scoredCandidates[0].score:F2}");
        }
        return bestTarget;
    }

    /// <summary>
    /// Checks if a pawn is a valid target for petting.
    /// </summary>
    /// <param name="target">The potential target</param>
    /// <param name="sylvie">The Sylvie pawn</param>
    /// <returns>True if the target is valid</returns>
    private bool IsValidTarget(Pawn target, Pawn sylvie)
    {
        // Cannot target self
        if (target == sylvie)
            return false;

        // Must be humanlike
        if (!target.RaceProps.Humanlike)
            return false;

        // Must be in same faction
        if (target.Faction != sylvie.Faction)
            return false;

        // Must be within search distance
        if (!target.Position.InHorDistOf(sylvie.Position, MaxSearchDistance))
            return false;

        // Target must be awake, not downed, not in mental state
        if (!target.Awake() || target.Downed || target.InMentalState)
            return false;

        // Must have positive opinion
        int opinion = sylvie.relations?.OpinionOf(target) ?? 0;
        if (opinion <= 0)
            return false;

        // Can reserve target
        if (!sylvie.CanReserve(target))
            return false;

        return true;
    }

    /// <summary>
    /// Calculates a score for a target pawn based on relationship and mood.
    /// Higher score = better target.
    /// </summary>
    /// <param name="target">The target pawn</param>
    /// <param name="sylvie">The Sylvie pawn</param>
    /// <returns>A score value (higher is better)</returns>
    private float CalculateTargetScore(Pawn target, Pawn sylvie)
    {
        float score = 0f;

        // Opinion factor - higher opinion = better
        int opinion = sylvie.relations?.OpinionOf(target) ?? 0;
        score += opinion;

        // High opinion bonus (>40 gets priority boost)
        if (opinion > HighOpinionThreshold)
        {
            score += 100f; // Significant priority boost
        }

        // Target mood factor - mood > 50% preferred
        float? targetMood = target.needs?.mood?.CurLevelPercentage;
        if (targetMood.HasValue && targetMood.Value > TargetMinMoodThreshold)
        {
            score += 50f; // Bonus for good mood
        }

        return score;
    }

    /// <summary>
    /// Interrupts the pawn's current non-critical job if any.
    /// </summary>
    /// <param name="pawn">The pawn whose job might be interrupted</param>
    private void InterruptNonCriticalJob(Pawn pawn)
    {
        Job? curJob = pawn.CurJob;
        if (curJob != null && !IsCriticalJob(curJob.def))
        {
            Log.Message($"[SylvieMod] {pawn.LabelShort}: Interrupting current job {curJob.def.defName}");
            pawn.jobs?.EndCurrentJob(JobCondition.InterruptForced);
        }
    }
}
