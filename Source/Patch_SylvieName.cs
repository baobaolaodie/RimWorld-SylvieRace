using HarmonyLib;
using RimWorld;
using Verse;

#nullable disable
namespace SylvieMod;

[HarmonyPatch(typeof(PawnBioAndNameGenerator), "GiveAppropriateBioAndNameTo")]
public static class Patch_SylvieName
{
    public static void Postfix(Pawn pawn)
    {
        if (pawn?.def?.defName != "Sylvie_Race")
            return;
        
        if (pawn.Name is NameTriple nameTriple)
        {
            string firstName = "SylvieRace_FirstName".Translate();
            pawn.Name = new NameTriple(firstName, nameTriple.Nick, nameTriple.Last);
        }
    }
}
