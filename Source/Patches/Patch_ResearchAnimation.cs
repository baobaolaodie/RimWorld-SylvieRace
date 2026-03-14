#nullable enable
using HarmonyLib;
using Verse;
using RimWorld;
using FacialAnimation;
using System.Collections.Generic;
using System.Reflection;

namespace SylvieMod;

[HarmonyPatch(typeof(FaceAnimation), nameof(FaceAnimation.GetCurrentFrame))]
public static class Patch_FaceAnimation_GetCurrentFrame_Research
{
    private static Dictionary<FaceAnimation, Pawn> animationToPawn = new Dictionary<FaceAnimation, Pawn>();
    private static FieldInfo? startTickField;
    private static FieldInfo? animationFramesField;
    
    public static void RegisterAnimation(FaceAnimation animation, Pawn pawn)
    {
        animationToPawn[animation] = pawn;
    }
    
    private static int GetStartTick(FaceAnimation animation)
    {
        if (startTickField == null)
        {
            startTickField = typeof(FaceAnimation).GetField("startTick", BindingFlags.NonPublic | BindingFlags.Instance);
        }
        return (int?)startTickField?.GetValue(animation) ?? 0;
    }
    
    private static List<FaceAnimationDef.AnimationFrame>? GetAnimationFrames(FaceAnimationDef animationDef)
    {
        if (animationFramesField == null)
        {
            animationFramesField = typeof(FaceAnimationDef).GetField("animationFrames", BindingFlags.NonPublic | BindingFlags.Instance);
        }
        return animationFramesField?.GetValue(animationDef) as List<FaceAnimationDef.AnimationFrame>;
    }
    
    public static bool Prefix(FaceAnimation __instance, int tickGame, ref FaceAnimationDef.AnimationFrame? __result)
    {
        if (__instance?.animationDef?.defName != "Sylvie_ResearchAnimation") return true;
        
        if (!animationToPawn.TryGetValue(__instance, out var pawn) || pawn == null)
            return true;
        
        // 只处理希尔薇种族
        if (!SylvieDefNames.IsSylvieRace(pawn))
            return true;
        
        var frames = __instance.animationDef.GetSequentialAnimationFrames();
        if (frames == null || frames.Count == 0) return true;
        
        // 使用反射获取 FaceAnimation 的 startTick
        int startTick = GetStartTick(__instance);
        int elapsedTicks = tickGame - startTick;
        
        // 确保 elapsedTicks 非负且在有效范围内
        if (elapsedTicks < 0) elapsedTicks = 0;
        
        // FA 原生行为：GetSequentialAnimationFrames 已经根据 duration 展开帧
        // 例如：8帧，每帧duration=30，展开后共240帧
        // 所以直接使用 elapsedTicks 作为索引，不需要取模
        int frameCount = frames.Count;
        if (frameCount <= 0) return true;
        
        // 计算循环索引：当动画循环时，使用取模
        int frameIndex = elapsedTicks % frameCount;
        
        // 最终安全检查
        if (frameIndex < 0 || frameIndex >= frameCount)
            frameIndex = 0;
        
        __result = frames[frameIndex];
        
        // 根据动画帧索引设置猫耳
        // 需要从展开后的帧列表计算原始动画帧索引
        var catEarComp = pawn.GetComp<SylvieCatEarComp>();
        if (catEarComp != null)
        {
            // 获取原始动画定义
            var animationDef = __instance.animationDef;
            var originalFrames = GetAnimationFrames(animationDef);
            if (originalFrames != null && originalFrames.Count > 0)
            {
                // 计算每帧的 duration（取第一帧的 duration 作为基准）
                int durationPerFrame = originalFrames[0].duration;
                // 使用默认值防止除零（30 ticks ≈ 0.5秒 @ 60 TPS）
                if (durationPerFrame <= 0) durationPerFrame = SylvieConstants.DefaultAnimationDuration;
                
                // 计算原始动画帧索引
                int originalFrameIndex = frameIndex / durationPerFrame;
                int originalFrameCount = originalFrames.Count;
                if (originalFrameIndex >= originalFrameCount) 
                    originalFrameIndex = originalFrameCount - 1;
                
                // 猫耳帧映射：
                // 原始帧0,2,4 (第1,3,5帧) -> 猫耳1
                // 原始帧1,3,5,6,7 (第2,4,6,7,8帧) -> 猫耳2
                int earFrame = (originalFrameIndex == 0 || originalFrameIndex == 2 || originalFrameIndex == 4) ? 0 : 1;
                catEarComp.SetCurrentEarFrame(earFrame);
                catEarComp.SetShouldRender(true);
            }
        }
        
        return false;
    }
}

[HarmonyPatch(typeof(FacialAnimationControllerComp), nameof(FacialAnimationControllerComp.CompTick))]
public static class Patch_FacialAnimationControllerComp_CompTick
{
    private static Dictionary<Pawn, bool> wasResearchActive = new Dictionary<Pawn, bool>();
    
    public static void Postfix(FacialAnimationControllerComp __instance)
    {
        Pawn? pawn = __instance.parent as Pawn;
        if (pawn == null) return;
        if (!SylvieDefNames.IsSylvieRace(pawn)) return;
        
        var catEarComp = pawn.GetComp<SylvieCatEarComp>();
        if (catEarComp == null) return;
        
        // 检查当前是否在研究
        bool isResearchNow = pawn.CurJobDef != null && pawn.CurJobDef.defName == "Research";
        
        wasResearchActive.TryGetValue(pawn, out bool wasResearchBefore);
        
        // 如果刚刚停止研究，隐藏猫耳
        if (wasResearchBefore && !isResearchNow)
        {
            catEarComp.SetShouldRender(false);
        }
        
        wasResearchActive[pawn] = isResearchNow;
    }
}

[HarmonyPatch(typeof(FacialAnimationControllerComp), "InitializeIfNeed")]
public static class Patch_FacialAnimationControllerComp_InitializeIfNeed_Research
{
    public static void Postfix(FacialAnimationControllerComp __instance, Pawn ___pawn, Dictionary<string, List<FaceAnimation>> ___animationDict)
    {
        if (!SylvieDefNames.IsSylvieRace(___pawn)) return;
        
        foreach (var kvp in ___animationDict)
        {
            foreach (var animation in kvp.Value)
            {
                if (animation.animationDef.defName == "Sylvie_ResearchAnimation")
                {
                    Patch_FaceAnimation_GetCurrentFrame_Research.RegisterAnimation(animation, ___pawn);
                }
            }
        }
    }
}
