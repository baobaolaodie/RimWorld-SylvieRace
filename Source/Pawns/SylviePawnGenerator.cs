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
    private const float DefaultBiologicalAge = 19f;
    private const float DefaultChronologicalAge = 19f;

    /// <summary>
    /// Generates a new Sylvie pawn with proper configuration.
    /// </summary>
    public static Pawn GenerateSylvie(Faction faction)
    {
        PawnKindDef? pawnKindDef = SylvieDefNames.PawnKind_SylvieDef;
        if (pawnKindDef == null)
        {
            Log.Error("[SylvieMod] Could not find Sylvie PawnKindDef");
            return null!;
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

        ConfigureName(pawn);
        ConfigureGenes(pawn);
        ConfigureTraits(pawn);
        ConfigureTattoos(pawn);

        return pawn;
    }

    /// <summary>
    /// Sets Sylvie's name using translated first name.
    /// </summary>
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
    private static void ConfigureGenes(Pawn pawn)
    {
        if (pawn.genes == null) return;

        TryAddGene(pawn, SylvieDefNames.Gene_SkinSheerWhiteDef, EndogeneCategory.Melanin);
        TryAddGene(pawn, SylvieDefNames.Gene_HairSnowWhiteDef, EndogeneCategory.HairColor);

        if (pawn.story.hairDef != null)
        {
            pawn.Drawer.renderer.SetAllGraphicsDirty();
        }
    }

    /// <summary>
    /// Attempts to add a gene after removing conflicting genes.
    /// </summary>
    private static void TryAddGene(Pawn pawn, GeneDef? geneDef, EndogeneCategory categoryToRemove)
    {
        if (geneDef == null) return;

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
    private static void ConfigureTraits(Pawn pawn)
    {
        pawn.story.traits.allTraits.Clear();
        pawn.story.traits.GainTrait(new Trait(TraitDefOf.Kind));
    }

    /// <summary>
    /// Configures Sylvie's tattoos (face and body scars).
    /// </summary>
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
}
