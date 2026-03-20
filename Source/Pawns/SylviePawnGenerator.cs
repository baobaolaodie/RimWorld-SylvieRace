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

    #endregion

    #region Public Methods

    /// <summary>
    /// Generates a new Sylvie pawn with proper configuration.
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

        XenotypeDef? xenotypeDef = DefDatabase<XenotypeDef>.GetNamed("Baseliner", false);

        Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(
            kind: pawnKindDef,
            faction: faction,
            tile: PlanetTile.Invalid,
            forceGenerateNewPawn: true,
            fixedBiologicalAge: DefaultBiologicalAge,
            fixedChronologicalAge: DefaultChronologicalAge,
            fixedGender: Gender.Female,
            forcedXenotype: xenotypeDef
        ));

        if (pawn == null)
        {
            Log.Error("[SylvieMod] Pawn generation returned null");
            return null;
        }

        ConfigureName(pawn);
        ConfigureGenes(pawn);
        ConfigureTraits(pawn);
        ConfigureTattoos(pawn);

        return pawn;
    }

    #endregion

    #region Configuration Methods

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
