#nullable enable
using HarmonyLib;
using Verse;

namespace SylvieMod;

/// <summary>
/// Harmony initialization for SylvieMod.
/// </summary>
[StaticConstructorOnStartup]
public static class HarmonyInit
{
    private const string HarmonyId = "com.sylvie.specialtrader";

    static HarmonyInit()
    {
        Log.Message("[SylvieMod] HarmonyInit: Starting patch process...");
        
        Harmony harmony = new Harmony(HarmonyId);
        harmony.PatchAll();
        
        Log.Message("[SylvieMod] HarmonyInit: Patch process completed.");
        LogPatchedMethods(harmony);
    }

    private static void LogPatchedMethods(Harmony harmony)
    {
        foreach (var method in harmony.GetPatchedMethods())
        {
            Log.Message($"[SylvieMod] - Patched: {method.DeclaringType?.Name}.{method.Name}");
        }
    }
}
