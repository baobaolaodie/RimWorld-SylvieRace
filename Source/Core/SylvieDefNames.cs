#nullable enable
using RimWorld;
using Verse;

namespace SylvieMod;

/// <summary>
/// 集中管理 SylvieRace Mod 的所有 Def 名称常量。
/// 提供类型安全的 Def 访问和种族检查功能。
/// </summary>
public static class SylvieDefNames
{
    #region Def Name Constants

    // 事件相关
    public const string Incident_ArrivalEvent = "Sylvie_ArrivalEvent";

    // Pawn 类型
    public const string PawnKind_Sylvie = "Sylvie_PawnKind";

    // Hediff
    public const string Hediff_InitialTrauma = "SylvieRace_InitialTrauma";

    // 信件
    public const string Letter_OfferLetter = "Sylvie_OfferLetter";

    // 纹身
    public const string Tattoo_ScarHead = "SylvieRace_ScarHead";
    public const string Tattoo_ScarBody = "SylvieRace_ScarBody";

    // 商人
    public const string Trader_ClothingTrader = "Sylvie_ClothingTrader";

    // 基因
    public const string Gene_SkinSheerWhite = "Skin_SheerWhite";
    public const string Gene_HairSnowWhite = "Hair_SnowWhite";

    // 种族
    /// <summary>
    /// 希尔薇种族的 ThingDef 名称。
    /// </summary>
    public const string Race_Sylvie = "Sylvie_Race";

    // JobDef
    public const string Job_SeekPetting = "Sylvie_SeekPetting";
    public const string Job_Research = "Research";

    // ThoughtDef - 寻求抚摸功能
    public const string Thought_WasPetted = "Sylvie_WasPetted";
    public const string Thought_PettedSomeone = "Sylvie_PettedSomeone";
    public const string Thought_PettedMe_Social = "Sylvie_PettedMe_Social";
    public const string Thought_WasPetted_Social = "Sylvie_WasPetted_Social";

    // AnimationDef
    public const string Animation_Aiming = "Sylvie_AimingAnimation";
    public const string Animation_Research = "Sylvie_ResearchAnimation";

    #endregion

    #region Cached Def Accessors

    // Hediff
    public static HediffDef? Hediff_InitialTraumaDef => GetDef<HediffDef>(Hediff_InitialTrauma);

    // PawnKind
    public static PawnKindDef? PawnKind_SylvieDef => GetDef<PawnKindDef>(PawnKind_Sylvie);

    // Incident
    public static IncidentDef? Incident_ArrivalEventDef => GetDef<IncidentDef>(Incident_ArrivalEvent);

    // Letter
    public static LetterDef? Letter_OfferLetterDef => GetDef<LetterDef>(Letter_OfferLetter);

    // Trader
    public static TraderKindDef? Trader_ClothingTraderDef => GetDef<TraderKindDef>(Trader_ClothingTrader);

    // Tattoo
    public static TattooDef? Tattoo_ScarHeadDef => GetDef<TattooDef>(Tattoo_ScarHead);
    public static TattooDef? Tattoo_ScarBodyDef => GetDef<TattooDef>(Tattoo_ScarBody);

    // Gene
    public static GeneDef? Gene_SkinSheerWhiteDef => GetDef<GeneDef>(Gene_SkinSheerWhite);
    public static GeneDef? Gene_HairSnowWhiteDef => GetDef<GeneDef>(Gene_HairSnowWhite);

    // Job
    public static JobDef? Job_SeekPettingDef => GetDef<JobDef>(Job_SeekPetting);
    public static JobDef? Job_ResearchDef => GetDef<JobDef>(Job_Research);

    // Thought
    public static ThoughtDef? Thought_WasPettedDef => GetDef<ThoughtDef>(Thought_WasPetted);
    public static ThoughtDef? Thought_PettedSomeoneDef => GetDef<ThoughtDef>(Thought_PettedSomeone);
    public static ThoughtDef? Thought_PettedMe_SocialDef => GetDef<ThoughtDef>(Thought_PettedMe_Social);
    public static ThoughtDef? Thought_WasPetted_SocialDef => GetDef<ThoughtDef>(Thought_WasPetted_Social);

    #endregion

    #region Race Checking

    /// <summary>
    /// 检查指定的 Pawn 是否为希尔薇种族。
    /// </summary>
    /// <param name="pawn">要检查的 Pawn</param>
    /// <returns>如果是希尔薇种族返回 true，否则返回 false</returns>
    public static bool IsSylvieRace(Pawn? pawn)
    {
        return pawn?.def?.defName == Race_Sylvie;
    }

    /// <summary>
    /// 检查指定的 ThingDef 是否为希尔薇种族。
    /// </summary>
    /// <param name="raceDef">要检查的 ThingDef</param>
    /// <returns>如果是希尔薇种族返回 true，否则返回 false</returns>
    public static bool IsSylvieRace(ThingDef? raceDef)
    {
        return raceDef?.defName == Race_Sylvie;
    }

    #endregion

    #region Private Helpers

    /// <summary>
    /// 安全地获取 Def，使用缓存提高性能。
    /// </summary>
    private static TDef? GetDef<TDef>(string defName) where TDef : Def
    {
        return DefDatabase<TDef>.GetNamed(defName, false);
    }

    #endregion
}
