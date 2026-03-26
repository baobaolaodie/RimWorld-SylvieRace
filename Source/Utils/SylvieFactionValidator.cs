#nullable enable
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace SylvieMod;

/// <summary>
/// 验证派系是否适合触发希尔薇事件。
/// </summary>
public static class SylvieFactionValidator
{
    #region Constants

    /// <summary>
    /// 最大尝试获取派系的次数。
    /// </summary>
    private const int MaxFactionSelectionAttempts = 10;

    #endregion

    #region Public Methods

    /// <summary>
    /// 获取一个适合触发希尔薇事件的有效派系。
    /// </summary>
    /// <returns>有效的派系，如果没有则返回 null</returns>
    public static Faction? GetValidFaction()
    {
        IEnumerable<Faction> validFactions = GetAllValidFactions();
        return validFactions.RandomElementWithFallback(null);
    }

    /// <summary>
    /// 检查是否存在任何有效的派系。
    /// </summary>
    /// <returns>如果存在有效派系返回 true</returns>
    public static bool HasAnyValidFaction()
    {
        return GetAllValidFactions().Any();
    }

    /// <summary>
    /// 验证指定派系是否适合触发希尔薇事件。
    /// </summary>
    /// <param name="faction">要验证的派系</param>
    /// <returns>如果派系有效返回 true</returns>
    public static bool IsValidFaction(Faction? faction)
    {
        if (faction == null)
            return false;

        // 排除玩家派系
        if (faction.IsPlayer)
            return false;

        // 排除隐藏派系
        if (faction.Hidden)
            return false;

        // 排除已击败的派系
        if (faction.defeated)
            return false;

        // 排除临时派系
        if (faction.temporary)
            return false;

        // 排除与玩家敌对的派系
        if (faction.HostileTo(Faction.OfPlayer))
            return false;

        // 必须有商队交易种类（排除商会等无商队派系）
        if (faction.def.caravanTraderKinds.NullOrEmpty())
            return false;

        // 必须有能够生成商队的 PawnGroupMaker
        if (faction.def.pawnGroupMakers == null)
            return false;

        bool hasTraderGroupMaker = faction.def.pawnGroupMakers.Any(pgm =>
            pgm.kindDef == PawnGroupKindDefOf.Trader);

        if (!hasTraderGroupMaker)
            return false;

        return true;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// 获取所有适合触发希尔薇事件的有效派系。
    /// </summary>
    /// <returns>有效派系的集合</returns>
    private static IEnumerable<Faction> GetAllValidFactions()
    {
        if (Find.FactionManager == null)
            return Enumerable.Empty<Faction>();

        return Find.FactionManager.AllFactions.Where(IsValidFaction);
    }

    #endregion
}
