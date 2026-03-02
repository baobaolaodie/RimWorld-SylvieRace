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
    yield return new FloatMenuOption("呼叫专用服装贸易商 (免费)", (Action) (() => Patch_CommsConsole.SpawnSpecialTrader(__instance.Map)));
  }

  private static void SpawnSpecialTrader(Map map)
  {
    if (map.passingShipManager.passingShips.Any<PassingShip>((Predicate<PassingShip>) (s => s.name.Contains("服装供应船"))))
    {
      Messages.Message("贸易商已经在附近轨道上了。", MessageTypeDefOf.RejectInput);
    }
    else
    {
      TraderKindDef named = DefDatabase<TraderKindDef>.GetNamed("Sylvie_ClothingTrader");
      if (named == null)
      {
        Log.Error("找不到 TraderKindDef: Sylvie_ClothingTrader，请检查 XML。");
      }
      else
      {
        TradeShip vis = new TradeShip(named);
        vis.name = "服装供应船";
        map.passingShipManager.AddShip((PassingShip) vis);
        vis.GenerateThings();
        Messages.Message("一艘服装贸易船已进入通讯范围。", MessageTypeDefOf.PositiveEvent);
      }
    }
  }
}
