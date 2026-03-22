#nullable enable
using System.Collections.Generic;
using FacialAnimation;
using Verse;

namespace SylvieMod;

/// <summary>
/// 动画注册管理器。
/// 统一管理 FaceAnimation 到 Pawn 的映射关系。
/// </summary>
public static class SylvieAnimationRegistry
{
    #region Animation Mappings

    /// <summary>
    /// 动画实例到 Pawn 的映射。
    /// </summary>
    private static readonly Dictionary<FaceAnimation, Pawn> AnimationToPawnMap = new();

    /// <summary>
    /// 动画定义名称到注册回调的映射。
    /// </summary>
    private static readonly Dictionary<string, System.Action<FaceAnimation, Pawn>> AnimationRegistrations = new();

    #endregion

    #region Public API

    /// <summary>
    /// 注册一个动画实例与 Pawn 的关联。
    /// </summary>
    public static void RegisterAnimation(FaceAnimation animation, Pawn pawn)
    {
        if (animation == null || pawn == null) return;
        AnimationToPawnMap[animation] = pawn;
    }

    /// <summary>
    /// 获取与动画实例关联的 Pawn。
    /// </summary>
    public static Pawn? GetAssociatedPawn(FaceAnimation animation)
    {
        if (animation == null) return null;
        AnimationToPawnMap.TryGetValue(animation, out var pawn);
        return pawn;
    }

    /// <summary>
    /// 检查动画是否已注册。
    /// </summary>
    public static bool IsAnimationRegistered(FaceAnimation animation)
    {
        return animation != null && AnimationToPawnMap.ContainsKey(animation);
    }

    /// <summary>
    /// 注册特定动画类型的处理回调。
    /// </summary>
    public static void RegisterAnimationType(string animationDefName, System.Action<FaceAnimation, Pawn> onRegister)
    {
        if (string.IsNullOrEmpty(animationDefName)) return;
        AnimationRegistrations[animationDefName] = onRegister;
    }

    /// <summary>
    /// 处理动画注册事件。
    /// </summary>
    public static void ProcessAnimationRegistration(FaceAnimation animation, Pawn pawn)
    {
        if (animation?.animationDef == null) return;

        string defName = animation.animationDef.defName;

        // 通用注册
        RegisterAnimation(animation, pawn);

        // 特定类型处理
        if (AnimationRegistrations.TryGetValue(defName, out var callback))
        {
            callback.Invoke(animation, pawn);
        }
    }

    /// <summary>
    /// 清除所有注册信息。
    /// 通常在场景切换时调用。
    /// </summary>
    public static void ClearRegistrations()
    {
        AnimationToPawnMap.Clear();
        AnimationRegistrations.Clear();
    }

    #endregion

    #region Batch Registration

    /// <summary>
    /// 批量注册动画字典中的所有动画。
    /// </summary>
    public static void RegisterAllAnimations(Dictionary<string, List<FaceAnimation>> animationDict, Pawn pawn)
    {
        if (animationDict == null || pawn == null) return;

        foreach (var kvp in animationDict)
        {
            foreach (var animation in kvp.Value)
            {
                ProcessAnimationRegistration(animation, pawn);
            }
        }
    }

    /// <summary>
    /// 批量注册特定动画类型的所有实例。
    /// </summary>
    public static void RegisterAnimationsByType(
        Dictionary<string, List<FaceAnimation>> animationDict,
        Pawn pawn,
        string targetAnimationDefName)
    {
        if (animationDict == null || pawn == null || string.IsNullOrEmpty(targetAnimationDefName)) return;

        foreach (var kvp in animationDict)
        {
            foreach (var animation in kvp.Value)
            {
                if (animation?.animationDef?.defName == targetAnimationDefName)
                {
                    RegisterAnimation(animation, pawn);
                }
            }
        }
    }

    #endregion
}
