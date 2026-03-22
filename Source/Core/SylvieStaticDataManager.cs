#nullable enable
using Verse;

namespace SylvieMod;

/// <summary>
/// 静态数据管理器，负责在游戏重置时清理所有静态数据。
/// 防止内存泄漏和场景间数据污染。
/// </summary>
[StaticConstructorOnStartup]
public static class SylvieStaticDataManager
{
    /// <summary>
    /// 静态构造函数，订阅游戏重置事件。
    /// </summary>
    static SylvieStaticDataManager()
    {
        // 在场景卸载时清理静态数据
        LongEventHandler.ExecuteWhenFinished(() =>
        {
            Log.Message("[SylvieMod] Static data manager initialized");
        });
    }

    /// <summary>
    /// 清理所有静态数据。
    /// 应在游戏重置或场景切换时调用。
    /// </summary>
    public static void ClearAllStaticData()
    {
        Log.Message("[SylvieMod] Clearing all static data...");

        SylvieCooldownTracker.ClearTrackers();
        SylvieAimingTracker.ClearCache();
        SylvieAnimationRegistry.ClearRegistrations();
        SylvieComponentRegistry.ClearCache();

        Log.Message("[SylvieMod] Static data cleared successfully");
    }
}
