using HarmonyLib;
using Verse;

#nullable disable
namespace SylvieMod;

[StaticConstructorOnStartup]
public static class HarmonyInit
{
  static HarmonyInit() => new Harmony("com.sylvie.specialtrader").PatchAll();
}
