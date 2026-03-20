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
    #region Constants

    private const int MinCheckInterval = GenDate.TicksPerHour; // 最小检查间隔 1 小时 (2500 ticks)
    private const float CheckChance = 0.20f; // 每次检查有 20% 概率继续
    private const int MaxSearchDistance = 40;
    private const int MinAgeYears = 10;
    private const int HighOpinionThreshold = 40;
    private const float TargetMinMoodThreshold = 0.50f; // 50%

    #endregion

    #region Static Fields

    // 每个pawn的最后检查时间
    private static readonly Dictionary<Pawn, int> LastCheckTicks = new Dictionary<Pawn, int>();

    #endregion

    #region Job Giving Logic

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
        
        // 执行所有条件检查
        if (!TryPerformChecks(pawn, currentTick, out string? failReason))
        {
            return null!;
        }

        // Find best target
        Pawn? target = FindBestTarget(pawn);
        if (target == null)
        {
            Log.Message($"[SylvieMod] {pawn.LabelShort}: [6/7] Target check - found: null, passed: False");
            return null!;
        }

        // JobDef check
        JobDef? seekPettingJobDef = SylvieDefNames.Job_SeekPettingDef;
        if (seekPettingJobDef == null)
        {
            Log.Message($"[SylvieMod] {pawn.LabelShort}: [7/7] JobDef check - found: False");
            return null!;
        }

        // Interrupt current non-critical job if any
        InterruptNonCriticalJob(pawn);

        // Create and return the job
        Job job = JobMaker.MakeJob(seekPettingJobDef, target);
        job.locomotionUrgency = LocomotionUrgency.Jog;
        job.expiryInterval = 5000;

        Log.Message($"[SylvieMod] {pawn.LabelShort}: ====== SUCCESS! Assigned seek petting job to {target.LabelShort} ======");
        return job;
    }

    /// <summary>
    /// Performs all prerequisite checks for giving the petting job.
    /// </summary>
    /// <param name="pawn">The pawn to check</param>
    /// <param name="currentTick">Current game tick</param>
    /// <param name="failReason">Output parameter for failure reason</param>
    /// <returns>True if all checks pass, false otherwise</returns>
    private bool TryPerformChecks(Pawn pawn, int currentTick, out string? failReason)
    {
        failReason = null;

        // Check 1: Interval and probability
        if (!CheckIntervalAndProbability(pawn, currentTick, out failReason))
        {
            return false;
        }

        // Check 2: Age
        if (!CheckAge(pawn, out failReason))
        {
            return false;
        }

        // Check 3: State (awake, not downed, not in mental state)
        if (!CheckPawnState(pawn, out failReason))
        {
            return false;
        }

        // Check 4: Cooldown
        if (!CheckCooldown(pawn, out failReason))
        {
            return false;
        }

        // Check 5: Idle status
        if (!CheckIdleStatus(pawn, out failReason))
        {
            return false;
        }

        return true;
    }

    #endregion

    #region Individual Check Methods

    /// <summary>
    /// Checks the interval and probability conditions.
    /// </summary>
    private bool CheckIntervalAndProbability(Pawn pawn, int currentTick, out string? failReason)
    {
        failReason = null;
        
        int lastCheck = LastCheckTicks.TryGetValue(pawn, out int last) ? last : -1;
        int ticksSinceLastCheck = lastCheck < 0 ? int.MaxValue : currentTick - lastCheck;
        
        // 必须满足最小间隔，否则静默返回
        if (ticksSinceLastCheck < MinCheckInterval)
        {
            return false;
        }
        
        // 概率检查 - 20% 概率继续
        float checkRoll = Rand.Value;
        bool checkPassed = checkRoll <= CheckChance;
        LastCheckTicks[pawn] = currentTick;
        
        if (!checkPassed)
        {
            Log.Message($"[SylvieMod] {pawn.LabelShort}: [1/7] Interval check failed - ticksSinceLast: {ticksSinceLastCheck}, roll: {checkRoll:F4} ({checkRoll*100:F1}%), required: <= {CheckChance:F2} ({CheckChance*100:F0}%)");
            failReason = "Probability check failed";
            return false;
        }
        
        Log.Message($"[SylvieMod] {pawn.LabelShort}: [1/7] Interval check passed - ticksSinceLast: {ticksSinceLastCheck}, roll: {checkRoll:F4} ({checkRoll*100:F1}%)");
        return true;
    }

    /// <summary>
    /// Checks if the pawn meets the age requirement.
    /// </summary>
    private bool CheckAge(Pawn pawn, out string? failReason)
    {
        failReason = null;
        
        bool agePassed = SylvieValidationUtils.MeetsAgeRequirement(pawn, MinAgeYears);
        int currentAge = pawn.ageTracker.AgeBiologicalYears;
        Log.Message($"[SylvieMod] {pawn.LabelShort}: [2/7] Age check - current: {currentAge}, required: >= {MinAgeYears}, passed: {agePassed}");
        
        if (!agePassed)
        {
            failReason = $"Age requirement not met (current: {currentAge}, required: >= {MinAgeYears})";
            return false;
        }
        
        return true;
    }

    /// <summary>
    /// Checks if the pawn is in a valid state (awake, not downed, not in mental state).
    /// </summary>
    private bool CheckPawnState(Pawn pawn, out string? failReason)
    {
        failReason = null;
        
        bool isAwake = pawn.Awake();
        bool isDowned = pawn.Downed;
        bool inMentalState = pawn.InMentalState;
        bool statePassed = SylvieValidationUtils.IsValidStateForPetting(pawn);
        
        Log.Message($"[SylvieMod] {pawn.LabelShort}: [3/7] State check - Awake: {isAwake}, Downed: {isDowned}, MentalState: {inMentalState}, passed: {statePassed}");
        
        if (!statePassed)
        {
            failReason = $"Invalid pawn state (Awake: {isAwake}, Downed: {isDowned}, MentalState: {inMentalState})";
            return false;
        }
        
        return true;
    }

    /// <summary>
    /// Checks if the pawn is in petting cooldown.
    /// </summary>
    private bool CheckCooldown(Pawn pawn, out string? failReason)
    {
        failReason = null;
        
        SylvieSeekPettingTracker? tracker = Current.Game?.GetComponent<SylvieSeekPettingTracker>();
        bool inCooldown = tracker != null && tracker.IsInCooldown(pawn);
        int remainingCooldown = inCooldown ? tracker!.GetCooldownRemaining(pawn) : 0;
        int remainingHours = remainingCooldown / GenDate.TicksPerHour;
        
        Log.Message($"[SylvieMod] {pawn.LabelShort}: [4/7] Cooldown check - trackerExists: {tracker != null}, inCooldown: {inCooldown}, remaining: {remainingCooldown} ticks (~{remainingHours}h), passed: {!inCooldown}");
        
        if (inCooldown)
        {
            failReason = $"In cooldown (remaining: {remainingCooldown} ticks)";
            return false;
        }
        
        return true;
    }

    /// <summary>
    /// Checks if the pawn is idle or has an interruptible job.
    /// </summary>
    private bool CheckIdleStatus(Pawn pawn, out string? failReason)
    {
        failReason = null;
        
        string? currentJobName = pawn.CurJob?.def?.defName ?? "null (idle)";
        bool idlePassed = SylvieValidationUtils.IsAvailableForPetting(pawn);
        
        Log.Message($"[SylvieMod] {pawn.LabelShort}: [5/7] Idle check - currentJob: {currentJobName}, passed: {idlePassed}");
        
        if (!idlePassed)
        {
            failReason = $"Not idle (current job: {currentJobName})";
            return false;
        }
        
        return true;
    }

    #endregion

    #region Target Finding

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
        
        IEnumerable<Pawn> candidates = allPawns.Where(p => SylvieValidationUtils.IsValidPettingTarget(p, sylvie, MaxSearchDistance));

        if (!candidates.Any())
        {
            Log.Message($"[SylvieMod] {sylvie.LabelShort}: FindBestTarget - no valid candidates found");
            return null;
        }

        Log.Message($"[SylvieMod] {sylvie.LabelShort}: FindBestTarget - found {candidates.Count()} candidate(s)");

        return SelectBestCandidate(candidates, sylvie);
    }

    /// <summary>
    /// Selects the best candidate from the list based on scoring.
    /// </summary>
    private Pawn? SelectBestCandidate(IEnumerable<Pawn> candidates, Pawn sylvie)
    {
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

    #endregion

    #region Utility Methods

    /// <summary>
    /// Interrupts the pawn's current non-critical job if any.
    /// </summary>
    /// <param name="pawn">The pawn whose job might be interrupted</param>
    private void InterruptNonCriticalJob(Pawn pawn)
    {
        Job? curJob = pawn.CurJob;
        if (curJob != null && !SylvieValidationUtils.IsCriticalJob(curJob.def))
        {
            Log.Message($"[SylvieMod] {pawn.LabelShort}: Interrupting current job {curJob.def.defName}");
            pawn.jobs?.EndCurrentJob(JobCondition.InterruptForced);
        }
    }

    #endregion
}
