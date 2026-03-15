#nullable enable
using HarmonyLib;
using FacialAnimation;

namespace SylvieMod;

/// <summary>
/// Patch FaceAnimationDef.IsSame 方法，仅为 Sylvie Race 修改 raceName 匹配逻辑：
/// - 对于 Sylvie Race：如果 raceName 为空，允许使用（FA 原版动画）；如果 raceName 不为空，只允许对应种族使用（Sylvie 专属动画）
/// - 对于其他种族：保持 FA 原版行为
/// </summary>
[HarmonyPatch(typeof(FaceAnimationDef), nameof(FaceAnimationDef.IsSame))]
public static class Patch_FaceAnimationDef_IsSame
{
    public static bool Prefix(FaceAnimationDef __instance, string jobName, string targetName, ref bool __result)
    {
        // 如果 jobName 为空，返回 false（保持原版逻辑）
        if (string.IsNullOrEmpty(jobName))
        {
            __result = false;
            return false; // 跳过原版方法
        }

        // 只对 Sylvie Race 应用新的逻辑
        if (targetName == "Sylvie_Race")
        {
            // 修改后的 raceName 匹配逻辑：
            // - 如果 raceName 不为空且不等于 targetName，返回 false（专属动画，种族不匹配）
            // - 如果 raceName 为空，允许任何种族使用（通用动画，如 FA 原版）
            if (!string.IsNullOrEmpty(__instance.raceName) && __instance.raceName != targetName)
            {
                __result = false;
                return false; // 跳过原版方法
            }

            // 检查 targetJobs 是否包含 jobName（保持原版逻辑）
            __result = __instance.targetJobs.Contains(jobName);
            return false; // 跳过原版方法
        }

        // 对于其他种族，调用原版方法
        return true;
    }
}
