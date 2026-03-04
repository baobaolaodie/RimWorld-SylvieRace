#nullable enable
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace SylvieMod;

/// <summary>
/// Harmony patch for CommsConsole to add special trader option.
/// </summary>
[HarmonyPatch(typeof(Building_CommsConsole), nameof(Building_CommsConsole.GetFloatMenuOptions))]
public static class Patch_CommsConsole
{
    public static IEnumerable<FloatMenuOption> Postfix(
        IEnumerable<FloatMenuOption> __result,
        Building_CommsConsole __instance,
        Pawn myPawn)
    {
        foreach (FloatMenuOption opt in __result)
        {
            yield return opt;
        }

        yield return new FloatMenuOption(
            "CallSpecialApparelTrader".Translate(),
            () => SpawnSpecialTrader(__instance.Map)
        );
    }

    private static void SpawnSpecialTrader(Map map)
    {
        string shipName = "Sylvie_ClothingSupplyShipName".Translate();

        if (IsTraderAlreadyInOrbit(map, shipName))
        {
            Messages.Message(
                "Sylvie_TraderAlreadyInOrbit".Translate(),
                MessageTypeDefOf.RejectInput
            );
            return;
        }

        TraderKindDef? traderDef = SylvieDefNames.Trader_ClothingTraderDef;
        if (traderDef == null)
        {
            Log.Error("[SylvieMod] Could not find Sylvie_ClothingTrader TraderKindDef");
            return;
        }

        TradeShip tradeShip = new TradeShip(traderDef);
        tradeShip.name = shipName;
        map.passingShipManager.AddShip(tradeShip);
        tradeShip.GenerateThings();

        Messages.Message(
            "Sylvie_TraderArrived".Translate(),
            MessageTypeDefOf.PositiveEvent
        );
    }

    private static bool IsTraderAlreadyInOrbit(Map map, string shipName)
    {
        return map.passingShipManager.passingShips
            .Any(s => s.name.Contains(shipName));
    }
}
