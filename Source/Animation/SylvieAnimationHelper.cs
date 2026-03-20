#nullable enable
using System;
using System.Collections.Generic;
using System.Reflection;
using FacialAnimation;
using Verse;

namespace SylvieMod;

/// <summary>
/// 动画系统辅助工具类。
/// 提供动画帧计算、反射访问等通用功能。
/// </summary>
public static class SylvieAnimationHelper
{
    #region Reflection Cache

    private static readonly Dictionary<Type, Dictionary<string, FieldInfo>> FieldCache = new();

    #endregion

    #region Frame Calculation

    /// <summary>
    /// 计算动画帧索引，支持循环播放。
    /// </summary>
    /// <param name="elapsedTicks">已过去的 ticks</param>
    /// <param name="frameCount">总帧数</param>
    /// <returns>安全的帧索引</returns>
    public static int CalculateFrameIndex(int elapsedTicks, int frameCount)
    {
        if (frameCount <= 0) return 0;
        if (elapsedTicks < 0) elapsedTicks = 0;

        return elapsedTicks % frameCount;
    }

    /// <summary>
    /// 计算基于持续时间的动画帧索引。
    /// 适用于每帧有不同 duration 的动画。
    /// </summary>
    /// <param name="elapsedTicks">已过去的 ticks</param>
    /// <param name="frames">动画帧列表</param>
    /// <returns>原始帧索引</returns>
    public static int CalculateOriginalFrameIndex(int elapsedTicks, List<FaceAnimationDef.AnimationFrame> frames)
    {
        if (frames == null || frames.Count == 0) return 0;
        if (elapsedTicks < 0) elapsedTicks = 0;

        // 计算总持续时间
        int totalDuration = 0;
        foreach (var frame in frames)
        {
            totalDuration += Math.Max(1, frame.duration);
        }

        if (totalDuration <= 0) return 0;

        // 计算循环位置
        int positionInCycle = elapsedTicks % totalDuration;

        // 确定当前帧
        int accumulatedDuration = 0;
        for (int i = 0; i < frames.Count; i++)
        {
            accumulatedDuration += Math.Max(1, frames[i].duration);
            if (positionInCycle < accumulatedDuration)
            {
                return i;
            }
        }

        return frames.Count - 1;
    }

    /// <summary>
    /// 计算 warmup 阶段的动画进度。
    /// </summary>
    public static float CalculateWarmupProgress(int ticksLeft, int totalWarmupTicks)
    {
        if (totalWarmupTicks <= 0) return 1f;

        int elapsedTicks = totalWarmupTicks - ticksLeft;
        float progress = (float)elapsedTicks / totalWarmupTicks;
        return progress < 0f ? 0f : (progress > 1f ? 1f : progress);
    }

    #endregion

    #region Reflection Helpers

    /// <summary>
    /// 获取对象的私有字段值。
    /// </summary>
    public static T? GetPrivateField<T>(object obj, string fieldName) where T : class
    {
        if (obj == null) return null;

        Type type = obj.GetType();
        FieldInfo? field = GetCachedField(type, fieldName);

        return field?.GetValue(obj) as T;
    }

    /// <summary>
    /// 获取值类型的私有字段值。
    /// </summary>
    public static T GetPrivateFieldValue<T>(object obj, string fieldName) where T : struct
    {
        if (obj == null) return default;

        Type type = obj.GetType();
        FieldInfo? field = GetCachedField(type, fieldName);

        if (field == null) return default;

        object? value = field.GetValue(obj);
        return value is T typedValue ? typedValue : default;
    }

    /// <summary>
    /// 获取或缓存 FieldInfo。
    /// </summary>
    private static FieldInfo? GetCachedField(Type type, string fieldName)
    {
        if (!FieldCache.TryGetValue(type, out var fields))
        {
            fields = new Dictionary<string, FieldInfo>();
            FieldCache[type] = fields;
        }

        if (!fields.TryGetValue(fieldName, out var field))
        {
            field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                fields[fieldName] = field;
            }
        }

        return field;
    }

    #endregion

    #region Animation Frame Access

    /// <summary>
    /// 安全地获取动画帧列表。
    /// </summary>
    public static List<FaceAnimationDef.AnimationFrame>? GetAnimationFrames(FaceAnimationDef animationDef)
    {
        if (animationDef == null) return null;

        // 尝试使用公共方法
        var frames = animationDef.GetSequentialAnimationFrames();
        if (frames != null && frames.Count > 0)
        {
            return frames;
        }

        // 回退到反射获取
        return GetPrivateField<List<FaceAnimationDef.AnimationFrame>>(animationDef, "animationFrames");
    }

    /// <summary>
    /// 获取 FaceAnimation 的 startTick。
    /// </summary>
    public static int GetStartTick(FaceAnimation animation)
    {
        return GetPrivateFieldValue<int>(animation, "startTick");
    }

    #endregion

    #region Safety Checks

    /// <summary>
    /// 验证动画定义是否有效。
    /// </summary>
    public static bool IsValidAnimation(FaceAnimation? animation)
    {
        return animation?.animationDef != null;
    }

    /// <summary>
    /// 验证动画帧列表是否有效。
    /// </summary>
    public static bool HasValidFrames(List<FaceAnimationDef.AnimationFrame>? frames)
    {
        return frames != null && frames.Count > 0;
    }

    #endregion
}
