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


    public class SylvieRace_CompProperties_AutoHeal : CompProperties
    {
        public int healIntervalTicks = 120000;
        public HediffDef paralysisHediff;

        public SylvieRace_CompProperties_AutoHeal()
        {
            this.compClass = typeof(SylvieRace_CompAutoHeal);
        }
    }
    public class SylvieRace_CompAutoHeal : ThingComp
    {
        private int tickCounter = 0;

        public SylvieRace_CompProperties_AutoHeal Props
            => (SylvieRace_CompProperties_AutoHeal)props;

        public override void CompTickRare()
        {
            base.CompTickRare();
            if (parent is Apparel apparel && apparel.Wearer != null)
            {
                tickCounter += 250; 

                if (tickCounter >= Props.healIntervalTicks)
                {
                    tickCounter = 0;
                    DoAutoHeal(apparel.Wearer);
                }
            }
            else
            {
                tickCounter = 0;
            }
        }

        private void DoAutoHeal(Pawn wearer)
        {
            bool hasWounds = false;
            foreach (Hediff hediff in wearer.health.hediffSet.hediffs)
            {
                if (hediff is Hediff_Injury injury && !injury.IsTended())
                {
                    injury.Tended(1.0f, 1.0f, 0);
                    hasWounds = true;
                }
            }

            if (hasWounds)
            {
                Hediff paralysis = HediffMaker.MakeHediff(
                    Props.paralysisHediff, wearer);
                wearer.health.AddHediff(paralysis);
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref tickCounter, "SylvieRace_tickCounter", 0);
        }
    }