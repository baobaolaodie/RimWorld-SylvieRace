#nullable enable
using System;
using System.Collections.Generic;
using Verse;

namespace SylvieMod;

/// <summary>
/// 集中管理 Sylvie 种族的所有 ThingComp 组件注册。
/// 提供统一的组件注册、查询和管理功能。
/// </summary>
public static class SylvieComponentRegistry
{
    #region Component Type Registry

    /// <summary>
    /// Sylvie 种族需要的所有 ThingComp 类型列表。
    /// 新增组件时只需在此添加类型即可。
    /// </summary>
    private static readonly List<Type> RegisteredComponentTypes = new()
    {
        typeof(SylvieAimingTracker),
        typeof(SylvieCooldownTracker),
        typeof(SylvieCooldownOverlayComp),
        typeof(SylvieCatEarComp)
    };

    #endregion

    #region Public API

    /// <summary>
    /// 获取所有注册的组件类型列表。
    /// </summary>
    public static IReadOnlyList<Type> GetRegisteredComponentTypes() => RegisteredComponentTypes.AsReadOnly();

    /// <summary>
    /// 为指定的 Pawn 注册所有缺失的 Sylvie 组件。
    /// </summary>
    /// <param name="pawn">目标 Pawn</param>
    public static void RegisterAllComponents(Pawn pawn)
    {
        if (pawn == null) return;
        if (!SylvieDefNames.IsSylvieRace(pawn)) return;

        foreach (var compType in RegisteredComponentTypes)
        {
            RegisterComponentIfMissing(pawn, compType);
        }
    }

    /// <summary>
    /// 检查指定类型是否已注册为 Sylvie 组件。
    /// </summary>
    public static bool IsRegisteredComponentType(Type type)
    {
        return RegisteredComponentTypes.Contains(type);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// 如果指定组件不存在，则为 Pawn 注册该组件。
    /// </summary>
    private static void RegisterComponentIfMissing(Pawn pawn, Type compType)
    {
        if (HasComponent(pawn, compType)) return;

        try
        {
            var newComp = (ThingComp)Activator.CreateInstance(compType);
            newComp.parent = pawn;
            pawn.AllComps.Add(newComp);
        }
        catch (Exception ex)
        {
            Log.Error($"[SylvieMod] Failed to register component {compType.Name}: {ex.Message}");
        }
    }

    /// <summary>
    /// 检查 Pawn 是否已拥有指定类型的组件。
    /// </summary>
    private static bool HasComponent(Pawn pawn, Type compType)
    {
        foreach (var comp in pawn.AllComps)
        {
            if (comp.GetType() == compType)
                return true;
        }
        return false;
    }

    #endregion
}
