using HarmonyLib;
using Verse;

#nullable disable
namespace SylvieMod;

[StaticConstructorOnStartup]
public static class HarmonyInit
{
  static HarmonyInit()
  {
    Log.Message("[SylvieMod] HarmonyInit: Starting patch process...");
    Harmony harmony = new Harmony("com.sylvie.specialtrader");
    harmony.PatchAll();
    Log.Message("[SylvieMod] HarmonyInit: Patch process completed. Patched methods:");
    foreach (var method in harmony.GetPatchedMethods())
    {
      Log.Message($"[SylvieMod] - Patched: {method.DeclaringType?.Name}.{method.Name}");
    }
  }
}
