#nullable enable
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace SylvieMod;

/// <summary>
/// 护士服组件的属性定义。
/// </summary>
public class SylvieRace_CompProperties_NurseHeal : CompProperties
{
    /// <summary>
    /// 冷却时间（单位：ticks）。
    /// </summary>
    public int cooldownTicks = 5000;
    
    /// <summary>
    /// 使用后添加的麻痹 Hediff。
    /// </summary>
    public HediffDef paralysisHediff = null!;

    /// <summary>
    /// 构造函数，设置组件类。
    /// </summary>
    public SylvieRace_CompProperties_NurseHeal()
    {
        this.compClass = typeof(SylvieRace_CompNurseHeal);
    }
}

/// <summary>
/// 护士服组件，提供治疗能力。
/// 穿戴者可以使用此能力治疗所有未处理的伤口，但会暂时麻痹。
/// </summary>
public class SylvieRace_CompNurseHeal : ThingComp
{
    private int lastUseTick = -999999;

    /// <summary>
    /// 组件的属性。
    /// </summary>
    public SylvieRace_CompProperties_NurseHeal Props
        => (SylvieRace_CompProperties_NurseHeal)props;

    /// <summary>
    /// 父对象转换为服装。
    /// </summary>
    private Apparel? Apparel => parent as Apparel;
    
    /// <summary>
    /// 服装的穿戴者。
    /// </summary>
    private Pawn? Wearer => Apparel?.Wearer;

    /// <summary>
    /// 检查能力是否在冷却中。
    /// </summary>
    private bool IsOnCooldown
    {
        get
        {
            return Find.TickManager.TicksGame < lastUseTick + Props.cooldownTicks;
        }
    }

    /// <summary>
    /// 获取剩余冷却时间（ticks）。
    /// </summary>
    private int CooldownTicksRemaining
    {
        get
        {
            return UnityEngine.Mathf.Max(0, lastUseTick + Props.cooldownTicks - Find.TickManager.TicksGame);
        }
    }

    /// <summary>
    /// 获取穿戴时的额外 Gizmo（UI 按钮）。
    /// </summary>
    public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
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

    /// <summary>
    /// 尝试使用治疗能力。
    /// </summary>
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

    /// <summary>
    /// 序列化/反序列化数据。
    /// </summary>
    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref lastUseTick, "SylvieRace_NurseHeal_lastUseTick", -999999);
    }
}
