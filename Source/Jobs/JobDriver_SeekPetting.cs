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
    /// <summary>
    /// The target pawn to seek petting from.
    /// </summary>
    protected Pawn TargetPawn => job.targetA.Pawn;

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

    /// <summary>
    /// Applies all petting effects including mood thoughts, relationship changes,
    /// notification messages, and cooldown tracking.
    /// </summary>
    private void ApplyPettingEffects()
    {
        if (TargetPawn == null || pawn == null)
            return;

        // Safety check: only Sylvie race can apply petting effects
        if (!SylvieDefNames.IsSylvieRace(pawn))
        {
            Log.Warning($"[SylvieMod] Non-Sylvie pawn {pawn.LabelShort} attempted to apply petting effects. Aborting.");
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
    /// Applies mood thoughts to both Sylvie and the target pawn.
    /// </summary>
    private void ApplyMoodThoughts()
    {
        // Add "was petted" thought to Sylvie (the actor)
        ThoughtDef? wasPettedThought = SylvieDefNames.Thought_WasPettedDef;
        if (wasPettedThought != null)
        {
            pawn.needs?.mood?.thoughts?.memories?.TryGainMemory(wasPettedThought);
        }

        // Add "petted someone" thought to target
        ThoughtDef? pettedSomeoneThought = SylvieDefNames.Thought_PettedSomeoneDef;
        if (pettedSomeoneThought != null)
        {
            TargetPawn.needs?.mood?.thoughts?.memories?.TryGainMemory(pettedSomeoneThought);
        }
    }

    /// <summary>
    /// Updates the relationship between Sylvie and the target pawn using social thoughts.
    /// Base +10 opinion for both, extra +5 when Sylvie's mood is below 30%.
    /// </summary>
    private void UpdateRelationship()
    {
        // Base relationship increase
        int opinionOffset = 10;

        // Extra bonus when Sylvie's mood is low (< 30%)
        float? moodLevel = pawn.needs?.mood?.CurLevelPercentage;
        if (moodLevel.HasValue && moodLevel.Value < 0.30f)
        {
            opinionOffset += 5;
        }

        // Apply social thought to target about Sylvie (target gains opinion of Sylvie)
        ApplySocialThought(TargetPawn, pawn, SylvieDefNames.Thought_PettedMe_Social, opinionOffset);
        
        // Apply social thought to Sylvie about target (Sylvie gains opinion of target)
        ApplySocialThought(pawn, TargetPawn, SylvieDefNames.Thought_WasPetted_Social, opinionOffset);
    }

    /// <summary>
    /// Applies a social thought from one pawn to another with the specified opinion offset.
    /// </summary>
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
}
