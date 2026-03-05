#nullable enable
using RimWorld;
using Verse;

namespace SylvieMod;

public static class SylvieHediffManager
{
    private const int HediffDelayTicks = 300000;

    public static int CalculateTriggerTick()
    {
        return Find.TickManager.TicksGame + HediffDelayTicks;
    }

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

    private static void SendHediffLetter(Pawn pawn)
    {
        TaggedString label = "SylvieRace_HediffLetterLabel".Translate();
        TaggedString text = "SylvieRace_HediffLetterText".Translate(pawn.LabelShort);
        Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, new LookTargets(pawn));
    }
}

public class SylvieRace_CompProperties_NurseHeal : CompProperties
{
    public int cooldownTicks = 5000;
    public HediffDef paralysisHediff = null!;

    public SylvieRace_CompProperties_NurseHeal()
    {
        this.compClass = typeof(SylvieRace_CompNurseHeal);
    }
}

public class SylvieRace_CompNurseHeal : ThingComp
{
    private int lastUseTick = -999999;

    public SylvieRace_CompProperties_NurseHeal Props
        => (SylvieRace_CompProperties_NurseHeal)props;

    private Apparel? Apparel => parent as Apparel;
    
    private Pawn? Wearer => Apparel?.Wearer;

    private bool IsOnCooldown
    {
        get
        {
            return Find.TickManager.TicksGame < lastUseTick + Props.cooldownTicks;
        }
    }

    private int CooldownTicksRemaining
    {
        get
        {
            return UnityEngine.Mathf.Max(0, lastUseTick + Props.cooldownTicks - Find.TickManager.TicksGame);
        }
    }

    public override System.Collections.Generic.IEnumerable<Gizmo> CompGetWornGizmosExtra()
    {
        Pawn? wearer = Wearer;
        if (wearer == null || !wearer.IsColonistPlayerControlled)
        {
            yield break;
        }

        Command_Action cmd = new Command_Action();
        cmd.defaultLabel = "SylvieRace_NurseHeal_Label".Translate();
        cmd.defaultDesc = "SylvieRace_NurseHeal_Desc".Translate();
        cmd.icon = ContentFinder<UnityEngine.Texture2D>.Get("UI/Commands/MedicalRest", true);
        cmd.action = TryUseAbility;

        if (IsOnCooldown)
        {
            cmd.Disabled = true;
            cmd.disabledReason = "SylvieRace_NurseHeal_Cooldown".Translate(CooldownTicksRemaining.ToStringTicksToPeriod());
        }

        yield return cmd;
    }

    private void TryUseAbility()
    {
        Pawn? wearer = Wearer;
        if (wearer == null) return;

        if (IsOnCooldown)
        {
            Messages.Message("SylvieRace_NurseHeal_Cooldown".Translate(CooldownTicksRemaining.ToStringTicksToPeriod()), 
                MessageTypeDefOf.RejectInput);
            return;
        }

        bool hasWounds = false;
        foreach (Hediff hediff in wearer.health.hediffSet.hediffs)
        {
            if (hediff is Hediff_Injury injury && !injury.IsTended())
            {
                injury.Tended(1.0f, 1.0f, 0);
                hasWounds = true;
            }
        }

        if (!hasWounds)
        {
            Messages.Message("SylvieRace_NurseHeal_NoWounds".Translate(wearer.LabelShort), 
                MessageTypeDefOf.RejectInput);
            return;
        }

        Hediff paralysis = HediffMaker.MakeHediff(Props.paralysisHediff, wearer);
        wearer.health.AddHediff(paralysis);

        lastUseTick = Find.TickManager.TicksGame;

        Messages.Message("SylvieRace_NurseHeal_Success".Translate(wearer.LabelShort), 
            wearer, MessageTypeDefOf.PositiveEvent);
    }

    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref lastUseTick, "SylvieRace_NurseHeal_lastUseTick", -999999);
    }
}
