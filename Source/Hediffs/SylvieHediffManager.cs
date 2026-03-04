#nullable enable
using RimWorld;
using Verse;

namespace SylvieMod;

/// <summary>
/// Manages Hediff-related functionality for Sylvie.
/// </summary>
public static class SylvieHediffManager
{
    private const int HediffDelayTicks = 300000; // 5 game days

    /// <summary>
    /// Calculates the tick when the hediff should trigger.
    /// </summary>
    public static int CalculateTriggerTick()
    {
        return Find.TickManager.TicksGame + HediffDelayTicks;
    }

    /// <summary>
    /// Triggers the initial trauma hediff on the specified pawn.
    /// </summary>
    public static bool TryTriggerHediff(Pawn? pawn)
    {
        if (pawn == null || pawn.Dead)
        {
            return false;
        }

        HediffDef? hediffDef = SylvieDefNames.Hediff_InitialTraumaDef;
        if (hediffDef == null)
        {
            Log.Error("[SylvieMod] Could not find SylvieRace_InitialTrauma HediffDef");
            return false;
        }

        Hediff hediff = HediffMaker.MakeHediff(hediffDef, pawn);
        hediff.Severity = 1.0f;
        pawn.health.AddHediff(hediff);
        
        SendHediffLetter(pawn);
        return true;
    }

    /// <summary>
    /// Sends a letter notification about the hediff.
    /// </summary>
    private static void SendHediffLetter(Pawn pawn)
    {
        TaggedString label = "SylvieRace_HediffLetterLabel".Translate();
        TaggedString text = "SylvieRace_HediffLetterText".Translate(pawn.LabelShort);
        Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, new LookTargets(pawn));
    }
}
