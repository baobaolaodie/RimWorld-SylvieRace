#nullable enable

namespace SylvieMod
{
    /// <summary>
    /// 集中管理 SylvieRace Mod 的所有常量值。
    /// 避免在代码中分散使用魔法数字，提高可维护性。
    /// </summary>
    public static class SylvieConstants
    {
        #region 动画相关常量

        /// <summary>
        /// 默认动画帧持续时间（单位：ticks）。
        /// 30 ticks ≈ 0.5 秒（@ 60 TPS）
        /// </summary>
        public const int DefaultAnimationDuration = 30;

        /// <summary>
        /// 研究动画的帧数量。
        /// </summary>
        public const int ResearchAnimationFrameCount = 8;

        #endregion

        #region 时间间隔常量

        /// <summary>
        /// 游戏组件检查间隔（单位：ticks）。
        /// 2500 ticks ≈ 41.7 秒（@ 60 TPS）
        /// </summary>
        public const int CheckIntervalTicks = 2500;

        /// <summary>
        /// 初始事件延迟（单位：ticks）。
        /// 5000 ticks ≈ 83.3 秒（@ 60 TPS）
        /// </summary>
        public const int InitialEventDelayTicks = 5000;

        /// <summary>
        /// Hediff 添加延迟（单位：ticks）。
        /// 300000 ticks = 5 天（RimWorld 标准）
        /// </summary>
        public const int HediffDelayTicks = 300000;

        /// <summary>
        /// 护士服治疗间隔（单位：ticks）。
        /// 600 ticks = 10 秒（@ 60 TPS）
        /// </summary>
        public const int NurseHealIntervalTicks = 600;

        #endregion

        #region 渲染层级常量

        /// <summary>
        /// 基础渲染层级。
        /// </summary>
        public const float BaseRenderLayer = 61f;

        /// <summary>
        /// 猫耳渲染层级。
        /// 位于头发（62）之上，头盔（75+）之下。
        /// </summary>
        public const float CatEarRenderLayer = 74f;

        /// <summary>
        /// 汗液渲染层级。
        /// </summary>
        public const float SweatRenderLayer = 61.1f;

        /// <summary>
        /// 弹匣渲染层级。
        /// </summary>
        public const float MagazineRenderLayer = 61.2f;

        /// <summary>
        /// 子弹渲染层级。
        /// </summary>
        public const float BulletRenderLayer = 61.3f;

        #endregion

        #region 数值常量

        /// <summary>
        /// 默认持续时间除数。
        /// 用于计算动画进度等。
        /// </summary>
        public const int DefaultDurationDivisor = 30;

        /// <summary>
        /// 护士服治疗效果。
        /// 每次治疗恢复的数值。
        /// </summary>
        public const float NurseHealAmount = 0.01f;

        /// <summary>
        /// 护士服治疗阈值。
        /// 低于此值时开始治疗。
        /// </summary>
        public const float NurseHealThreshold = 0.99f;

        #endregion
    }
}
