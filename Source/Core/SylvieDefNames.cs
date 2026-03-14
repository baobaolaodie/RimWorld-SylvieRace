#nullable enable
using RimWorld;
using Verse;

namespace SylvieMod;

/// <summary>
/// Centralized definition names for SylvieRace mod.
/// </summary>
public static class SylvieDefNames
{
    public const string Incident_ArrivalEvent = "Sylvie_ArrivalEvent";
    
    public const string PawnKind_Sylvie = "Sylvie_PawnKind";
    
    public const string Hediff_InitialTrauma = "SylvieRace_InitialTrauma";
    
    public const string Letter_OfferLetter = "Sylvie_OfferLetter";
    
    public const string Tattoo_ScarHead = "SylvieRace_ScarHead";
    public const string Tattoo_ScarBody = "SylvieRace_ScarBody";
    
    public const string Trader_ClothingTrader = "Sylvie_ClothingTrader";
    
    public const string Gene_SkinSheerWhite = "Skin_SheerWhite";
    public const string Gene_HairSnowWhite = "Hair_SnowWhite";
    
    /// <summary>
    /// 希尔薇种族的 ThingDef 名称。
    /// </summary>
    public const string Race_Sylvie = "Sylvie_Race";
    
    public static HediffDef? Hediff_InitialTraumaDef => HediffDef.Named(Hediff_InitialTrauma);
    public static PawnKindDef? PawnKind_SylvieDef => PawnKindDef.Named(PawnKind_Sylvie);
    public static IncidentDef? Incident_ArrivalEventDef => IncidentDef.Named(Incident_ArrivalEvent);
    public static LetterDef? Letter_OfferLetterDef => DefDatabase<LetterDef>.GetNamed(Letter_OfferLetter, false);
    public static TraderKindDef? Trader_ClothingTraderDef => DefDatabase<TraderKindDef>.GetNamed(Trader_ClothingTrader, false);
    public static TattooDef? Tattoo_ScarHeadDef => DefDatabase<TattooDef>.GetNamed(Tattoo_ScarHead, false);
    public static TattooDef? Tattoo_ScarBodyDef => DefDatabase<TattooDef>.GetNamed(Tattoo_ScarBody, false);
    public static GeneDef? Gene_SkinSheerWhiteDef => DefDatabase<GeneDef>.GetNamed(Gene_SkinSheerWhite, false);
    public static GeneDef? Gene_HairSnowWhiteDef => DefDatabase<GeneDef>.GetNamed(Gene_HairSnowWhite, false);
    
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
}
