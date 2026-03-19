#nullable enable
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
using LudeonTK;

namespace SylvieMod;

/// <summary>
/// Debug actions for SylvieRace mod.
/// Provides developer tools for testing and debugging Sylvie-specific features.
/// </summary>
public static class SylvieDebugActions
{
    /// <summary>
    /// Forces a Sylvie pawn to seek petting from an available colonist.
    /// Ignores cooldown, mood, and random chance checks.
    /// </summary>
    /// <param name="pawn">The Sylvie pawn to trigger petting for</param>
    [DebugAction("Sylvie", "强制触发抚摸", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
    private static void ForceTriggerPetting(Pawn pawn)
    {
        // Check if pawn is Sylvie race
        if (!SylvieDefNames.IsSylvieRace(pawn))
        {
            Messages.Message("目标不是希尔薇种族。", MessageTypeDefOf.RejectInput);
            return;
        }

        // Find available target colonist
        Pawn? target = FindAvailableTarget(pawn);
        if (target == null)
        {
            Messages.Message("找不到可用的目标殖民者（需要：非睡眠、非倒地、非精神崩溃、同派系、好感度>0）。", MessageTypeDefOf.RejectInput);
            return;
        }

        // Get the job definition
        JobDef? seekPettingJobDef = SylvieDefNames.Job_SeekPettingDef;
        if (seekPettingJobDef == null)
        {
            Messages.Message("找不到 Sylvie_SeekPetting JobDef。", MessageTypeDefOf.RejectInput);
            return;
        }

        // Create and assign the job
        Job job = JobMaker.MakeJob(seekPettingJobDef, target);
        job.locomotionUrgency = LocomotionUrgency.Walk;
        job.expiryInterval = 5000;

        // Force assign the job
        pawn.jobs?.StartJob(job, JobCondition.InterruptForced);

        Messages.Message($"{pawn.LabelShort} 开始寻求 {target.LabelShort} 的抚摸。", MessageTypeDefOf.PositiveEvent);
    }

    /// <summary>
    /// Resets the petting cooldown for a Sylvie pawn.
    /// </summary>
    /// <param name="pawn">The Sylvie pawn to reset cooldown for</param>
    [DebugAction("Sylvie", "重置抚摸冷却", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
    private static void ResetPettingCooldown(Pawn pawn)
    {
        // Check if pawn is Sylvie race
        if (!SylvieDefNames.IsSylvieRace(pawn))
        {
            Messages.Message("目标不是希尔薇种族。", MessageTypeDefOf.RejectInput);
            return;
        }

        // Get the tracker component
        SylvieSeekPettingTracker? tracker = Current.Game?.GetComponent<SylvieSeekPettingTracker>();
        if (tracker == null)
        {
            Messages.Message("找不到 SylvieSeekPettingTracker 组件。", MessageTypeDefOf.RejectInput);
            return;
        }

        // Reset cooldown by setting last tick to -1
        tracker.SetLastPettingTick(pawn, -1);

        Messages.Message($"{pawn.LabelShort} 的抚摸冷却已重置。", MessageTypeDefOf.PositiveEvent);
    }

    /// <summary>
    /// Finds an available target colonist for petting.
    /// Target must be: same faction, humanlike, awake, not downed, not in mental state, positive opinion.
    /// </summary>
    /// <param name="sylvie">The Sylvie pawn seeking petting</param>
    /// <returns>An available target pawn, or null if none found</returns>
    private static Pawn? FindAvailableTarget(Pawn sylvie)
    {
        if (sylvie.Map == null)
            return null;

        Map map = sylvie.Map;

        // Find valid targets
        Pawn? target = map.mapPawns.SpawnedPawnsInFaction(sylvie.Faction)
            .Where(p => IsValidTarget(p, sylvie))
            .OrderBy(p => p.Position.DistanceToSquared(sylvie.Position))
            .FirstOrDefault();

        return target;
    }

    /// <summary>
    /// Checks if a pawn is a valid target for petting.
    /// </summary>
    /// <param name="target">The potential target</param>
    /// <param name="sylvie">The Sylvie pawn</param>
    /// <returns>True if the target is valid</returns>
    private static bool IsValidTarget(Pawn target, Pawn sylvie)
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
}
