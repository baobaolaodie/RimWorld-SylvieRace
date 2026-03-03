using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

#nullable disable
namespace SylvieMod;

[HarmonyPatch(typeof (Building_CommsConsole), "GetFloatMenuOptions")]
public static class Patch_CommsConsole
{
  public static IEnumerable<FloatMenuOption> Postfix(
    IEnumerable<FloatMenuOption> __result,
    Building_CommsConsole __instance,
    Pawn myPawn)
  {
    foreach (FloatMenuOption opt in __result)
      yield return opt;
        yield return new FloatMenuOption(
            "CallSpecialApparelTrader".Translate(),
            (Action)(() => Patch_CommsConsole.SpawnSpecialTrader(__instance.Map)));
  }

    private static void SpawnSpecialTrader(Map map)
    {
        string shipName = "Sylvie_ClothingSupplyShipName".Translate();

        if (map.passingShipManager.passingShips.Any<PassingShip>(
            (Predicate<PassingShip>)(s => s.name.Contains(shipName))))
        {
            Messages.Message(
                "Sylvie_TraderAlreadyInOrbit".Translate(),
                MessageTypeDefOf.RejectInput);
        }
        else
        {
            TraderKindDef named = DefDatabase<TraderKindDef>.GetNamed("Sylvie_ClothingTrader");
            if (named == null)
            {
                Log.Error("XML的bug。"); ;
            }
            else
            {
                TradeShip vis = new TradeShip(named);
                vis.name = shipName;
                map.passingShipManager.AddShip((PassingShip)vis);
                vis.GenerateThings();
                Messages.Message(
                    "Sylvie_TraderArrived".Translate(),
                    MessageTypeDefOf.PositiveEvent);
            }
        }
    }
}
