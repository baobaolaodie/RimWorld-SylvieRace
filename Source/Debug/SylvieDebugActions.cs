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
    #region Constants

    /// <summary>
    /// Error message displayed when target is not Sylvie race.
    /// </summary>
    private const string ErrorNotSylvieRace = "目标不是希尔薇种族。";

    /// <summary>
    /// Error message displayed when no valid target is found.
    /// </summary>
    private const string ErrorNoValidTarget = "找不到可用的目标殖民者（需要：非睡眠、非倒地、非精神崩溃、同派系、好感度>0）。";

    /// <summary>
    /// Error message displayed when JobDef is not found.
    /// </summary>
    private const string ErrorJobDefNotFound = "找不到 Sylvie_SeekPetting JobDef。";

    /// <summary>
    /// Error message displayed when tracker component is not found.
    /// </summary>
    private const string ErrorTrackerNotFound = "找不到 SylvieSeekPettingTracker 组件。";

    /// <summary>
    /// Success message format for cooldown reset.
    /// </summary>
    private const string SuccessCooldownReset = "{0} 的抚摸冷却已重置。";

    /// <summary>
    /// Success message format for petting trigger.
    /// </summary>
    private const string SuccessPettingTriggered = "{0} 开始寻求 {1} 的抚摸。";

    #endregion

    #region Debug Actions

    /// <summary>
    /// Forces a Sylvie pawn to seek petting from an available colonist.
    /// Ignores cooldown, mood, and random chance checks.
    /// </summary>
    /// <param name="pawn">The Sylvie pawn to trigger petting for</param>
    [DebugAction("Sylvie", "强制触发抚摸", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
    private static void ForceTriggerPetting(Pawn pawn)
    {
        // Validate Sylvie race
        if (!ValidateSylvieRace(pawn, out string errorMessage))
        {
            Messages.Message(errorMessage, MessageTypeDefOf.RejectInput);
            return;
        }

        // Find available target colonist
        Pawn? target = FindAvailableTarget(pawn);
        if (target == null)
        {
            Messages.Message(ErrorNoValidTarget, MessageTypeDefOf.RejectInput);
            return;
        }

        // Get the job definition
        JobDef? seekPettingJobDef = SylvieDefNames.Job_SeekPettingDef;
        if (seekPettingJobDef == null)
        {
            Messages.Message(ErrorJobDefNotFound, MessageTypeDefOf.RejectInput);
            return;
        }

        // Create and assign the job
        Job job = JobMaker.MakeJob(seekPettingJobDef, target);
        job.locomotionUrgency = LocomotionUrgency.Walk;
        job.expiryInterval = 5000;

        // Force assign the job
        pawn.jobs?.StartJob(job, JobCondition.InterruptForced);

        Messages.Message(string.Format(SuccessPettingTriggered, pawn.LabelShort, target.LabelShort), MessageTypeDefOf.PositiveEvent);
    }

    /// <summary>
    /// Resets the petting cooldown for a Sylvie pawn.
    /// </summary>
    /// <param name="pawn">The Sylvie pawn to reset cooldown for</param>
    [DebugAction("Sylvie", "重置抚摸冷却", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
    private static void ResetPettingCooldown(Pawn pawn)
    {
        // Validate Sylvie race
        if (!ValidateSylvieRace(pawn, out string errorMessage))
        {
            Messages.Message(errorMessage, MessageTypeDefOf.RejectInput);
            return;
        }

        // Get the tracker component
        SylvieSeekPettingTracker? tracker = Current.Game?.GetComponent<SylvieSeekPettingTracker>();
        if (tracker == null)
        {
            Messages.Message(ErrorTrackerNotFound, MessageTypeDefOf.RejectInput);
            return;
        }

        // Reset cooldown by setting last tick to -1
        tracker.SetLastPettingTick(pawn, -1);

        Messages.Message(string.Format(SuccessCooldownReset, pawn.LabelShort), MessageTypeDefOf.PositiveEvent);
    }

    #endregion

    #region Validation Methods

    /// <summary>
    /// Validates that the pawn is of Sylvie race.
    /// </summary>
    /// <param name="pawn">The pawn to validate</param>
    /// <param name="errorMessage">Output error message if validation fails</param>
    /// <returns>True if validation passes, false otherwise</returns>
    private static bool ValidateSylvieRace(Pawn pawn, out string errorMessage)
    {
        if (!SylvieDefNames.IsSylvieRace(pawn))
        {
            errorMessage = ErrorNotSylvieRace;
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    #endregion

    #region Target Finding

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

        // Find valid targets using utility class
        Pawn? target = map.mapPawns.SpawnedPawnsInFaction(sylvie.Faction)
            .Where(p => SylvieValidationUtils.IsValidPettingTarget(p, sylvie))
            .OrderBy(p => p.Position.DistanceToSquared(sylvie.Position))
            .FirstOrDefault();

        return target;
    }

    #endregion
}
