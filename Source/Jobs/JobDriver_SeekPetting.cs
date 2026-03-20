#nullable enable
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace SylvieMod;

/// <summary>
/// JobDriver for Sylvie seeking petting from a target pawn.
/// Handles movement to target and application of petting effects.
/// </summary>
public class JobDriver_SeekPetting : JobDriver
{
    #region Constants

    /// <summary>
    /// Base opinion offset for social relationship gain.
    /// </summary>
    private const int BaseOpinionOffset = 10;

    /// <summary>
    /// Additional opinion offset when Sylvie's mood is low.
    /// </summary>
    private const int LowMoodBonusOpinion = 5;

    /// <summary>
    /// Mood threshold for low mood bonus (30%).
    /// </summary>
    private const float LowMoodThreshold = 0.30f;

    /// <summary>
    /// Opinion threshold for intimate relationship stage (40).
    /// </summary>
    private const int IntimateOpinionThreshold = 40;

    #endregion

    #region Properties

    /// <summary>
    /// The target pawn to seek petting from.
    /// </summary>
    protected Pawn TargetPawn => job.targetA.Pawn;

    #endregion

    #region Job Driver Implementation

    /// <summary>
    /// Attempts to reserve the target pawn before starting the job.
    /// </summary>
    /// <param name="errorOnFailed">Whether to show error message if reservation fails</param>
    /// <returns>True if reservation succeeded, false otherwise</returns>
    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return pawn.Reserve(TargetPawn, job, 1, -1, null, errorOnFailed);
    }

    /// <summary>
    /// Creates the sequence of toils for this job.
    /// Toil 1: Move to target pawn
    /// Toil 2: Apply petting effects instantly
    /// </summary>
    /// <returns>Sequence of toils to execute</returns>
    protected override IEnumerable<Toil> MakeNewToils()
    {
        // Toil 1: Move to target pawn with PathEndMode.Touch
        yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch)
            .FailOnDespawnedOrNull(TargetIndex.A)
            .FailOn(() => TargetPawn.Dead);

        // Toil 2: Apply petting effects instantly
        Toil pettingToil = new Toil();
        pettingToil.initAction = ApplyPettingEffects;
        pettingToil.defaultCompleteMode = ToilCompleteMode.Instant;
        yield return pettingToil;
    }

    #endregion

    #region Effect Application

    /// <summary>
    /// Applies all petting effects including mood thoughts, relationship changes,
    /// notification messages, and cooldown tracking.
    /// </summary>
    private void ApplyPettingEffects()
    {
        if (!ValidatePettingConditions())
        {
            return;
        }

        // Apply mood thoughts
        ApplyMoodThoughts();

        // Update relationship
        UpdateRelationship();

        // Send notification message
        SendNotification();

        // Record cooldown
        RecordCooldown();
    }

    /// <summary>
    /// Validates that all conditions are met for applying petting effects.
    /// </summary>
    /// <returns>True if conditions are valid, false otherwise</returns>
    private bool ValidatePettingConditions()
    {
        if (TargetPawn == null || pawn == null)
        {
            return false;
        }

        // Safety check: only Sylvie race can apply petting effects
        if (!SylvieDefNames.IsSylvieRace(pawn))
        {
            Log.Warning($"[SylvieMod] Non-Sylvie pawn {pawn.LabelShort} attempted to apply petting effects. Aborting.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Applies mood thoughts to both Sylvie and the target pawn.
    /// Target's thought stage is determined by their opinion of Sylvie:
    /// - Stage 0 (+6 mood): Opinion <= 40
    /// - Stage 1 (+8 mood): Opinion > 40 (intimate)
    /// </summary>
    private void ApplyMoodThoughts()
    {
        ApplySylvieMoodThought();
        ApplyTargetMoodThought();
    }

    /// <summary>
    /// Applies the "was petted" mood thought to Sylvie.
    /// </summary>
    private void ApplySylvieMoodThought()
    {
        ThoughtDef? wasPettedThought = SylvieDefNames.Thought_WasPettedDef;
        if (wasPettedThought != null)
        {
            pawn.needs?.mood?.thoughts?.memories?.TryGainMemory(wasPettedThought);
        }
    }

    /// <summary>
    /// Applies the "petted someone" mood thought to the target pawn.
    /// The thought stage depends on the target's opinion of Sylvie.
    /// </summary>
    private void ApplyTargetMoodThought()
    {
        ThoughtDef? pettedSomeoneThought = SylvieDefNames.Thought_PettedSomeoneDef;
        if (pettedSomeoneThought == null)
        {
            return;
        }

        Thought_Memory? thought = ThoughtMaker.MakeThought(pettedSomeoneThought) as Thought_Memory;
        if (thought == null)
        {
            return;
        }

        // Determine stage based on target's opinion of Sylvie
        // Stage 1 (+8 mood) requires opinion > 40 (intimate relationship)
        int targetOpinionOfSylvie = TargetPawn.relations?.OpinionOf(pawn) ?? 0;
        int thoughtStage = DetermineThoughtStage(targetOpinionOfSylvie);
        
        thought.SetForcedStage(thoughtStage);
        LogThoughtStage(targetOpinionOfSylvie, thoughtStage);
        
        TargetPawn.needs?.mood?.thoughts?.memories?.TryGainMemory(thought);
    }

    /// <summary>
    /// Determines the thought stage based on opinion value.
    /// </summary>
    /// <param name="opinion">The opinion value</param>
    /// <returns>0 for normal relationship, 1 for intimate relationship</returns>
    private int DetermineThoughtStage(int opinion)
    {
        return opinion > IntimateOpinionThreshold ? 1 : 0;
    }

    /// <summary>
    /// Logs the thought stage assignment for debugging.
    /// </summary>
    private void LogThoughtStage(int opinion, int stage)
    {
        string relationshipType = stage == 1 ? "intimate" : "normal";
        int moodValue = stage == 1 ? 8 : 6;
        
        Log.Message($"[SylvieMod] {TargetPawn.LabelShort} has {relationshipType} opinion ({opinion}) of {pawn.LabelShort}, applying {relationshipType} petting thought (+{moodValue})");
    }

    #endregion

    #region Relationship Updates

    /// <summary>
    /// Updates the relationship between Sylvie and the target pawn using social thoughts.
    /// Base +10 opinion for both, extra +5 when Sylvie's mood is below 30%.
    /// </summary>
    private void UpdateRelationship()
    {
        int opinionOffset = CalculateOpinionOffset();

        // Apply social thought to target about Sylvie (target gains opinion of Sylvie)
        ApplySocialThought(TargetPawn, pawn, SylvieDefNames.Thought_PettedMe_Social, opinionOffset);
        
        // Apply social thought to Sylvie about target (Sylvie gains opinion of target)
        ApplySocialThought(pawn, TargetPawn, SylvieDefNames.Thought_WasPetted_Social, opinionOffset);
    }

    /// <summary>
    /// Calculates the total opinion offset based on base value and mood bonus.
    /// </summary>
    /// <returns>The total opinion offset</returns>
    private int CalculateOpinionOffset()
    {
        int opinionOffset = BaseOpinionOffset;

        // Extra bonus when Sylvie's mood is low (< 30%)
        float? moodLevel = pawn.needs?.mood?.CurLevelPercentage;
        if (moodLevel.HasValue && moodLevel.Value < LowMoodThreshold)
        {
            opinionOffset += LowMoodBonusOpinion;
        }

        return opinionOffset;
    }

    /// <summary>
    /// Applies a social thought from one pawn to another with the specified opinion offset.
    /// </summary>
    /// <param name="recipient">The pawn receiving the thought</param>
    /// <param name="targetPawn">The pawn the thought is about</param>
    /// <param name="thoughtDefName">The name of the thought definition</param>
    /// <param name="opinionOffset">The opinion offset to apply</param>
    private void ApplySocialThought(Pawn recipient, Pawn targetPawn, string thoughtDefName, int opinionOffset)
    {
        ThoughtDef? thoughtDef = DefDatabase<ThoughtDef>.GetNamed(thoughtDefName, false);
        if (thoughtDef == null)
        {
            Log.Warning($"[SylvieMod] ThoughtDef '{thoughtDefName}' not found.");
            return;
        }

        Thought_MemorySocial? socialThought = ThoughtMaker.MakeThought(thoughtDef) as Thought_MemorySocial;
        if (socialThought != null)
        {
            socialThought.otherPawn = targetPawn;
            socialThought.opinionOffset = opinionOffset;
            recipient.needs?.mood?.thoughts?.memories?.TryGainMemory(socialThought);
        }
    }

    #endregion

    #region Notification and Cooldown

    /// <summary>
    /// Sends a notification message about the petting interaction.
    /// </summary>
    private void SendNotification()
    {
        string message = "Sylvie_SeekPetting_Message".Translate(pawn.LabelShort, TargetPawn.LabelShort);
        Messages.Message(message, pawn, MessageTypeDefOf.PositiveEvent);
    }

    /// <summary>
    /// Records the petting cooldown using the SylvieSeekPettingTracker.
    /// </summary>
    private void RecordCooldown()
    {
        SylvieSeekPettingTracker? tracker = Current.Game?.GetComponent<SylvieSeekPettingTracker>();
        if (tracker != null)
        {
            tracker.SetLastPettingTick(pawn, Find.TickManager.TicksGame);
        }
    }

    #endregion
}
