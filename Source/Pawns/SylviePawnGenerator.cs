#nullable enable
using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using Verse;

namespace SylvieMod;

/// <summary>
/// Handles generation and configuration of Sylvie pawns.
/// </summary>
public static class SylviePawnGenerator
{
    #region Constants

    /// <summary>
    /// Default biological age for Sylvie pawns.
    /// </summary>
    private const float DefaultBiologicalAge = 19f;

    /// <summary>
    /// Default chronological age for Sylvie pawns.
    /// </summary>
    private const float DefaultChronologicalAge = 19f;

    /// <summary>
    /// 最大重试生成次数，防止无限循环。
    /// </summary>
    private const int MaxGenerationAttempts = 5;

    #endregion

    #region Public Methods

    /// <summary>
    /// Generates a new Sylvie pawn with proper configuration.
    /// 包含种族验证和重生成机制，确保生成的始终是希尔薇种族。
    /// </summary>
    /// <param name="faction">The faction for the new pawn</param>
    /// <returns>The generated Sylvie pawn, or null if generation failed</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when faction is null</exception>
    public static Pawn? GenerateSylvie(Faction faction)
    {
        if (faction == null)
        {
            throw new System.ArgumentNullException(nameof(faction));
        }

        PawnKindDef? pawnKindDef = SylvieDefNames.PawnKind_SylvieDef;
        if (pawnKindDef == null)
        {
            Log.Error("[SylvieMod] Could not find Sylvie PawnKindDef");
            return null;
        }

        // 获取希尔薇种族定义
        ThingDef? sylvieRaceDef = DefDatabase<ThingDef>.GetNamed(SylvieDefNames.Race_Sylvie, false);
        if (sylvieRaceDef == null)
        {
            Log.Error("[SylvieMod] Could not find Sylvie_Race ThingDef");
            return null;
        }

        // 尝试生成，带重试机制
        for (int attempt = 0; attempt < MaxGenerationAttempts; attempt++)
        {
            Pawn? pawn = TryGeneratePawnInternal(faction, pawnKindDef);

            if (pawn == null)
            {
                Log.Warning($"[SylvieMod] Pawn generation returned null on attempt {attempt + 1}");
                continue;
            }

            // 验证种族是否正确
            if (pawn.def.defName != SylvieDefNames.Race_Sylvie)
            {
                Log.Warning($"[SylvieMod] Generated pawn has wrong race: {pawn.def.defName}, expected: {SylvieDefNames.Race_Sylvie}. Retrying...");

                // 销毁错误种族的 Pawn
                pawn.Destroy();

                // 继续下一次尝试
                continue;
            }

            // 种族正确，配置属性
            ConfigureName(pawn);
            ConfigureGenes(pawn);
            ConfigureTraits(pawn);
            ConfigureTattoos(pawn);

            return pawn;
        }

        Log.Error($"[SylvieMod] Failed to generate correct Sylvie race after {MaxGenerationAttempts} attempts");
        return null;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// 内部生成 Pawn 方法。
    /// </summary>
    private static Pawn? TryGeneratePawnInternal(Faction faction, PawnKindDef pawnKindDef)
    {
        try
        {
            XenotypeDef? xenotypeDef = DefDatabase<XenotypeDef>.GetNamed("Baseliner", false);

            PawnGenerationRequest request = new PawnGenerationRequest(
                kind: pawnKindDef,
                faction: faction,
                tile: PlanetTile.Invalid,
                forceGenerateNewPawn: true,
                fixedBiologicalAge: DefaultBiologicalAge,
                fixedChronologicalAge: DefaultChronologicalAge,
                fixedGender: Gender.Female,
                forcedXenotype: xenotypeDef
            );

            Pawn pawn = PawnGenerator.GeneratePawn(request);
            return pawn;
        }
        catch (System.Exception ex)
        {
            Log.Warning($"[SylvieMod] Exception during pawn generation: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Sets Sylvie's name using translated first name.
    /// </summary>
    /// <param name="pawn">The pawn to configure</param>
    private static void ConfigureName(Pawn pawn)
    {
        if (pawn.Name is NameTriple nameTriple)
        {
            string firstName = "SylvieRace_FirstName".Translate();
            pawn.Name = new NameTriple(firstName, firstName, nameTriple.Last);
        }
    }

    /// <summary>
    /// Configures Sylvie's genes for skin and hair color.
    /// </summary>
    /// <param name="pawn">The pawn to configure</param>
    private static void ConfigureGenes(Pawn pawn)
    {
        if (pawn.genes == null) return;

        TryAddGene(pawn, SylvieDefNames.Gene_SkinSheerWhiteDef, EndogeneCategory.Melanin);
        TryAddGene(pawn, SylvieDefNames.Gene_HairSnowWhiteDef, EndogeneCategory.HairColor);

        if (pawn.story?.hairDef != null)
        {
            pawn.Drawer.renderer.SetAllGraphicsDirty();
        }
    }

    /// <summary>
    /// Attempts to add a gene after removing conflicting genes.
    /// </summary>
    /// <param name="pawn">The pawn to add gene to</param>
    /// <param name="geneDef">The gene definition to add</param>
    /// <param name="categoryToRemove">The endogene category to remove conflicts from</param>
    private static void TryAddGene(Pawn pawn, GeneDef? geneDef, EndogeneCategory categoryToRemove)
    {
        if (geneDef == null) return;
        if (pawn.genes == null) return;

        List<Gene> genesToRemove = new List<Gene>();
        foreach (Gene gene in pawn.genes.GenesListForReading)
        {
            if (gene.def.endogeneCategory == categoryToRemove)
            {
                genesToRemove.Add(gene);
            }
        }

        foreach (Gene gene in genesToRemove)
        {
            pawn.genes.RemoveGene(gene);
        }

        pawn.genes.AddGene(geneDef, false);
    }

    /// <summary>
    /// Configures Sylvie's traits (clears all and adds Kind trait).
    /// </summary>
    /// <param name="pawn">The pawn to configure</param>
    private static void ConfigureTraits(Pawn pawn)
    {
        if (pawn.story?.traits == null) return;

        pawn.story.traits.allTraits.Clear();
        pawn.story.traits.GainTrait(new Trait(TraitDefOf.Kind));
    }

    /// <summary>
    /// Configures Sylvie's tattoos (face and body scars).
    /// </summary>
    /// <param name="pawn">The pawn to configure</param>
    private static void ConfigureTattoos(Pawn pawn)
    {
        if (pawn.style == null) return;

        TattooDef? faceTattoo = SylvieDefNames.Tattoo_ScarHeadDef;
        TattooDef? bodyTattoo = SylvieDefNames.Tattoo_ScarBodyDef;

        if (faceTattoo != null)
        {
            pawn.style.FaceTattoo = faceTattoo;
        }

        if (bodyTattoo != null)
        {
            pawn.style.BodyTattoo = bodyTattoo;
        }
    }

    #endregion
}
