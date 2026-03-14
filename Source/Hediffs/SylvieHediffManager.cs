#nullable enable
using RimWorld;
using Verse;

namespace SylvieMod;

/// <summary>
/// 管理希尔薇种族的 Hediff 相关功能。
/// </summary>
public static class SylvieHediffManager
{
    /// <summary>
    /// 计算触发 Hediff 的游戏 tick。
    /// </summary>
    /// <returns>触发 tick</returns>
    public static int CalculateTriggerTick()
    {
        return Find.TickManager.TicksGame + SylvieConstants.HediffDelayTicks;
    }

    /// <summary>
    /// 尝试为指定 Pawn 触发初始创伤 Hediff。
    /// </summary>
    /// <param name="pawn">目标 Pawn</param>
    /// <returns>成功返回 true</returns>
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
    /// 发送 Hediff 触发信件。
    /// </summary>
    private static void SendHediffLetter(Pawn pawn)
    {
        TaggedString label = "SylvieRace_HediffLetterLabel".Translate();
        TaggedString text = "SylvieRace_HediffLetterText".Translate(pawn.LabelShort);
        Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, new LookTargets(pawn));
    }
}
