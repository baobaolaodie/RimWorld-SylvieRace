#nullable enable
using RimWorld;
using Verse;
using Verse.AI;

namespace SylvieMod;

/// <summary>
/// Utility class for Sylvie-specific validation logic.
/// Provides common validation methods used across the mod.
/// </summary>
public static class SylvieValidationUtils
{
    #region Constants

    /// <summary>
    /// Default maximum search distance for target finding.
    /// </summary>
    public const int DefaultMaxSearchDistance = 40;

    #endregion

    #region Target Validation

    /// <summary>
    /// Validates if a pawn is a valid target for Sylvie petting interaction.
    /// </summary>
    /// <param name="target">The potential target pawn</param>
    /// <param name="sylvie">The Sylvie pawn seeking interaction</param>
    /// <returns>True if the target is valid for petting</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when sylvie is null</exception>
    public static bool IsValidPettingTarget(Pawn? target, Pawn sylvie)
    {
        if (sylvie == null)
        {
            throw new System.ArgumentNullException(nameof(sylvie));
        }

        // Cannot target self
        if (target == sylvie)
            return false;

        // Target must not be null
        if (target == null)
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

    /// <summary>
    /// Validates if a pawn is a valid target for Sylvie petting interaction within a specified distance.
    /// </summary>
    /// <param name="target">The potential target pawn</param>
    /// <param name="sylvie">The Sylvie pawn seeking interaction</param>
    /// <param name="maxDistance">Maximum allowed distance</param>
    /// <returns>True if the target is valid and within range</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when sylvie is null</exception>
    public static bool IsValidPettingTarget(Pawn? target, Pawn sylvie, int maxDistance)
    {
        if (!IsValidPettingTarget(target, sylvie))
            return false;

        // Must be within search distance
        if (!target!.Position.InHorDistOf(sylvie.Position, maxDistance))
            return false;

        return true;
    }

    #endregion

    #region Pawn State Validation

    /// <summary>
    /// Checks if a pawn is in a valid state for seeking petting.
    /// </summary>
    /// <param name="pawn">The pawn to check</param>
    /// <returns>True if the pawn can seek petting</returns>
    public static bool IsValidStateForPetting(Pawn? pawn)
    {
        if (pawn == null)
            return false;

        // Must be awake
        if (!pawn.Awake())
            return false;

        // Must not be downed
        if (pawn.Downed)
            return false;

        // Must not be in mental state
        if (pawn.InMentalState)
            return false;

        return true;
    }

    /// <summary>
    /// Checks if a pawn meets the age requirement for petting.
    /// </summary>
    /// <param name="pawn">The pawn to check</param>
    /// <param name="minAgeYears">Minimum age in years</param>
    /// <returns>True if the pawn meets the age requirement</returns>
    public static bool MeetsAgeRequirement(Pawn? pawn, int minAgeYears)
    {
        if (pawn == null)
            return false;

        return pawn.ageTracker.AgeBiologicalYears >= minAgeYears;
    }

    #endregion

    #region Job Validation

    /// <summary>
    /// Determines if a job is critical and should not be interrupted.
    /// </summary>
    /// <param name="jobDef">The job definition to check</param>
    /// <returns>True if the job is critical</returns>
    public static bool IsCriticalJob(JobDef? jobDef)
    {
        if (jobDef == null)
            return false;

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
    /// Checks if a pawn is available for petting (idle or has interruptible job).
    /// </summary>
    /// <param name="pawn">The pawn to check</param>
    /// <returns>True if the pawn is available</returns>
    public static bool IsAvailableForPetting(Pawn? pawn)
    {
        if (pawn == null)
            return false;

        Job? curJob = pawn.CurJob;
        if (curJob == null)
            return true;

        // If current job is critical, cannot interrupt
        if (IsCriticalJob(curJob.def))
            return false;

        return true;
    }

    #endregion
}
