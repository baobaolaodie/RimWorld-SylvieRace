# SylvieRace 开发者文档

本文档面向开发者，介绍项目结构和技术实现细节。

## 项目结构

```
SylvieRace/
├── 1.6/
│   └── Assemblies/
│       └── SylvieRace.dll     # 编译输出
├── About/
│   └── About.xml              # 模组元数据
├── Defs/
│   ├── Races/
│   │   └── Sylvie_Race.xml        # 种族定义
│   ├── PawnKinds/
│   │   └── Sylvie_PawnKind.xml    # PawnKind 定义
│   ├── ThingCategories/
│   │   └── Sylvie_ThingCategories.xml  # 物品类别定义
│   ├── Apparel/
│   │   ├── Apparel_Dresses.xml    # 连衣裙类 (3件)
│   │   ├── Apparel_Outfits.xml    # 套装类 (10件)
│   │   ├── Apparel_Pants.xml      # 下装类 (3件)
│   │   ├── Apparel_Special.xml    # 特殊服装 (3件)
│   │   └── Apparel_Headwear.xml   # 头饰 (1件)
│   ├── Hair/
│   │   └── Sylvie_Hair.xml        # 发型定义 (3种发型)
│   ├── Incidents/
│   │   └── Sylvie_Incident.xml    # 事件定义
│   ├── Letters/
│   │   └── Sylvie_Letter.xml      # 信件定义
│   ├── Backstories/
│   │   └── Sylvie_Backstory.xml   # 背景故事
│   ├── Traders/
│   │   └── ClothingTrader.xml     # 贸易商定义
│   ├── Tattoos/
│   │   └── Sylvie_Tattoos.xml     # 纹身定义
│   ├── Hediffs/
│   │   └── Sylvie_Hediffs.xml     # 健康状态定义
│   ├── Thoughts/
│   │   └── Sylvie_Thoughts.xml    # 心情效果定义
│   └── FacialAnimation/       # 动态表情定义
│       ├── Animations/        # 动画定义
│       │   ├── AimingAnimation.xml   # 瞄准动画
│       │   ├── IngestAnimation.xml   # 进食动画
│       │   ├── CooldownAnimation.xml # 冷却动画
│       │   ├── ResearchAnimation.xml # 研究动画
│       │   └── LovinAnimation.xml    # Lovin 动画
│       ├── Types/             # 类型定义
│       │   ├── EyeType.xml           # 眼睛/眼球类型
│       │   ├── MouthType.xml         # 嘴巴类型
│       │   ├── BrowType.xml          # 眉毛类型
│       │   ├── LidType.xml           # 眼睑类型
│       │   ├── LidOptionType.xml     # 眼睑选项类型
│       │   ├── EmotionType.xml       # 情绪类型
│       │   ├── HeadType.xml          # 头部类型
│       │   └── SkinType.xml          # 皮肤类型
│       ├── Shapes/            # 形状扩展定义
│       │   ├── EyeShapeEx.xml        # 眼睛形状扩展（lookdown）
│       │   ├── EyeballShapeEx.xml    # 眼球形状扩展
│       │   ├── MouthShapeEx.xml      # 嘴巴形状扩展（eating1-3）
│       │   ├── BrowShapeEx.xml       # 眉毛形状扩展（aiming, confused）
│       │   ├── LidOptionShapeEx.xml  # 准星形状定义
│       │   └── CooldownShapeEx.xml   # 冷却动画形状定义
│       └── Sylvie_RaceFaceAdjustment.xml  # 面部调整
├── Patches/
│   └── Sylvie_Race_FacialAnimation_Patches.xml  # 动态表情补丁
├── Source/
│   ├── Core/
│   │   ├── HarmonyInit.cs           # Harmony 初始化
│   │   ├── SylvieDefNames.cs        # Def 名称常量 + IsSylvieRace 辅助方法
│   │   └── SylvieConstants.cs       # 全局常量定义（14个常量）
│   ├── Components/
│   │   ├── SylvieGameComponent.cs   # 游戏组件（状态管理、事件触发）
│   │   ├── SylvieCooldownTracker.cs # 冷却状态跟踪组件
│   │   ├── SylvieCooldownOverlayComp.cs # 冷却叠加层渲染组件
│   │   └── SylvieCatEarComp.cs      # 猫耳渲染组件
│   ├── Incidents/
│   │   └── IncidentWorker_SylvieTrader.cs  # 事件处理器
│   ├── Pawns/
│   │   └── SylviePawnGenerator.cs   # 希尔薇生成逻辑
│   ├── Letters/
│   │   └── ChoiceLetter_SylvieOffer.cs  # 信件类
│   ├── Hediffs/
│   │   ├── CompNurseHeal.cs         # 护士服治疗组件
│   │   └── SylvieHediffManager.cs   # Hediff 管理逻辑
│   ├── Patches/
│   │   ├── Patch_FaceAnimationDef_IsSame.cs  # 种族限制核心补丁
│   │   ├── Patch_CommsConsole.cs    # 通讯台补丁
│   │   ├── Patch_Stance_Warmup.cs   # 瞄准动画同步补丁
│   │   └── Patch_ResearchAnimation.cs # 研究动画与猫耳同步补丁
│   ├── SylvieRace.csproj            # 项目文件
│   └── SylvieRace.sln               # 解决方案
├── Textures/
│   └── Things/
│       ├── Clothes/           # 服装贴图
│       ├── Tattoos/           # 纹身贴图
│       │   ├── Head/          # 面部纹身
│       │   └── Body/          # 身体纹身
│       └── Pawn/
│           ├── Humanlike/Sylvie/  # 种族贴图
│           └── Sylvie/          # 动态表情贴图
│               ├── Eyes/
│               ├── Mouth/
│               ├── Brows/
│               ├── Lids/
│               ├── LidOptions/
│               ├── Emotions/
│               ├── Heads_Blank/
│               ├── Skins/
│               └── CooldownOverlay/  # 冷却动画叠加层贴图
├── Languages/
│   ├── English/
│   │   ├── Keyed/
│   │   │   └── SylvieRace.xml    # 英文翻译键
│   │   └── DefInjected/          # 英文 DefInjected 翻译
│   └── ChineseSimplified/
│       ├── Keyed/
│       │   └── SylvieRace.xml    # 中文翻译键
│       └── DefInjected/          # 中文 DefInjected 翻译
└── README.md                  # 用户文档
```

## 核心组件

### 1. SylvieDefNames（Def 名称常量类）

**文件位置**: `Source/Core/SylvieDefNames.cs`

集中管理所有 Def 名称，便于维护和避免拼写错误：

```csharp
// File: Source/Core/SylvieDefNames.cs
#nullable enable
using RimWorld;
using Verse;

namespace SylvieMod;

/// <summary>
/// Centralized definition names for SylvieRace mod.
/// </summary>
public static class SylvieDefNames
{
    public const string Incident_ArrivalEvent = "Sylvie_ArrivalEvent";
    
    public const string PawnKind_Sylvie = "Sylvie_PawnKind";
    
    public const string Hediff_InitialTrauma = "SylvieRace_InitialTrauma";
    
    public const string Letter_OfferLetter = "Sylvie_OfferLetter";
    
    public const string Tattoo_ScarHead = "SylvieRace_ScarHead";
    public const string Tattoo_ScarBody = "SylvieRace_ScarBody";
    
    public const string Trader_ClothingTrader = "Sylvie_ClothingTrader";
    
    public const string Gene_SkinSheerWhite = "Skin_SheerWhite";
    public const string Gene_HairSnowWhite = "Hair_SnowWhite";
    
    /// <summary>
    /// 希尔薇种族的 ThingDef 名称。
    /// </summary>
    public const string Race_Sylvie = "Sylvie_Race";
    
    public static HediffDef? Hediff_InitialTraumaDef => HediffDef.Named(Hediff_InitialTrauma);
    public static PawnKindDef? PawnKind_SylvieDef => PawnKindDef.Named(PawnKind_Sylvie);
    public static IncidentDef? Incident_ArrivalEventDef => IncidentDef.Named(Incident_ArrivalEvent);
    public static LetterDef? Letter_OfferLetterDef => DefDatabase<LetterDef>.GetNamed(Letter_OfferLetter, false);
    public static TraderKindDef? Trader_ClothingTraderDef => DefDatabase<TraderKindDef>.GetNamed(Trader_ClothingTrader, false);
    public static TattooDef? Tattoo_ScarHeadDef => DefDatabase<TattooDef>.GetNamed(Tattoo_ScarHead, false);
    public static TattooDef? Tattoo_ScarBodyDef => DefDatabase<TattooDef>.GetNamed(Tattoo_ScarBody, false);
    public static GeneDef? Gene_SkinSheerWhiteDef => DefDatabase<GeneDef>.GetNamed(Gene_SkinSheerWhite, false);
    public static GeneDef? Gene_HairSnowWhiteDef => DefDatabase<GeneDef>.GetNamed(Gene_HairSnowWhite, false);
    
    /// <summary>
    /// 检查指定的 Pawn 是否为希尔薇种族。
    /// </summary>
    /// <param name="pawn">要检查的 Pawn</param>
    /// <returns>如果是希尔薇种族返回 true，否则返回 false</returns>
    public static bool IsSylvieRace(Pawn? pawn)
    {
        return pawn?.def?.defName == Race_Sylvie;
    }
    
    /// <summary>
    /// 检查指定的 ThingDef 是否为希尔薇种族。
    /// </summary>
    /// <param name="raceDef">要检查的 ThingDef</param>
    /// <returns>如果是希尔薇种族返回 true，否则返回 false</returns>
    public static bool IsSylvieRace(ThingDef? raceDef)
    {
        return raceDef?.defName == Race_Sylvie;
    }
}
```

**种族判断辅助方法**：

- `IsSylvieRace(Pawn?)` - 检查 Pawn 是否为希尔薇种族，使用空值传播运算符安全处理 null
- `IsSylvieRace(ThingDef?)` - 检查 ThingDef 是否为希尔薇种族定义

这些方法统一种族判断逻辑，避免在代码中分散使用字符串比较（如 `pawn.def.defName == "Sylvie_Race"`），提高代码可维护性和类型安全性。

### 2. SylvieConstants（全局常量类）

**文件位置**: `Source/Core/SylvieConstants.cs`

集中管理所有魔法数字常量，提高代码可读性和可维护性：

```csharp
// File: Source/Core/SylvieConstants.cs
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
```

**设计目的**：
1. **消除魔法数字** - 将分散在代码中的数字常量集中管理，便于理解和修改
2. **提高可维护性** - 修改常量只需修改一处，避免遗漏
3. **文档化** - 每个常量都有详细的 XML 文档注释，说明其用途和计算依据
4. **分类组织** - 按功能区域分节（动画、时间、渲染、数值），便于查找

### 3. SylviePawnGenerator（Pawn 生成器）

**文件位置**: `Source/Pawns/SylviePawnGenerator.cs`

封装希尔薇 Pawn 生成逻辑：
- `GenerateSylvie(Faction)` - 生成希尔薇 Pawn（固定 19 岁，女性，强制 Baseliner 异种型）
- `ConfigureName(Pawn)` - 设置名字（使用翻译键 `SylvieRace_FirstName`）
- `ConfigureGenes(Pawn)` - 设置基因（透白皮肤、雪白发色）
- `TryAddGene(Pawn, GeneDef, EndogeneCategory)` - 添加基因并移除冲突基因
- `ConfigureTraits(Pawn)` - 设置特性（清除所有特性并添加 Kind）
- `ConfigureTattoos(Pawn)` - 设置纹身（面部和身体疤痕）

### 4. SylvieHediffManager（Hediff 管理器）

**文件位置**: `Source/Hediffs/SylvieHediffManager.cs`

封装 Hediff 相关逻辑：
- `CalculateTriggerTick()` - 计算触发时间
- `TryTriggerHediff(Pawn)` - 触发 Hediff，返回 `bool` 表示是否成功触发
- `SendHediffLetter(Pawn)` - 发送信件通知

### 5. 护士服主动技能组件

**文件位置**: `Source/Hediffs/CompNurseHeal.cs`

**SylvieRace_CompProperties_NurseHeal**:
- `cooldownTicks` - 冷却时间（默认 5000 ticks = 2游戏小时）
- `paralysisHediff` - 麻痹 Hediff 定义（XML 注入字段）

**SylvieRace_CompNurseHeal**:
- `CompGetWornGizmosExtra()` - 返回技能 Gizmo 按钮
- `TryUseAbility()` - 执行治疗逻辑
- `PostExposeData()` - 存档数据持久化（保存冷却状态）
- `IsOnCooldown` - 检查冷却状态
- `CooldownTicksRemaining` - 剩余冷却时间

**使用方式**:
- 穿着护士服后，装备栏显示"紧急治疗"技能按钮
- 点击后立即包扎所有未处理的伤口
- 触发 1游戏小时（2500 ticks）麻痹效果
- 2游戏小时（5000 ticks）冷却时间

**Hediff 定义** (`Defs/Hediffs/Sylvie_Hediffs.xml`):
- `SylvieRace_NurseParalysis` - 麻痹 Hediff
- 持续时间：2500 ticks（1游戏小时）
- 效果：移动和操纵能力设为 0

### 6. SylvieGameComponent（游戏组件）

**文件位置**: `Source/Components/SylvieGameComponent.cs`

负责状态管理和事件触发：
- `hasSylvieSpawned` - 希尔薇是否已生成
- `CheckForExistingSylvie()` - 检查殖民地是否已有希尔薇种族的殖民者
- `RegisterSylviePawn(Pawn)` - 注册希尔薇并安排 Hediff 触发
- `GameComponentTick()` - 定期检查事件触发

**防止重复生成机制**：
在触发事件前，会先检查殖民地是否已有希尔薇种族的殖民者（通过 `SylvieDefNames.IsSylvieRace(Pawn)` 判断）。如果已有，则设置 `hasSylvieSpawned = true` 并跳过事件触发。

**游戏组件常量**：
- `CheckInterval = SylvieConstants.CheckIntervalTicks` (**2500 ticks**) - 检查间隔（约 41.7 秒 @ 60 TPS，即 1 游戏小时）
- `InitialEventTick = SylvieConstants.InitialEventDelayTicks` (**5000 ticks**) - 初始事件触发时间（约 83.3 秒 @ 60 TPS，即 2 游戏小时）
- `HediffTriggerDelay = SylvieConstants.HediffDelayTicks` (**300000 ticks**) - Hediff 触发延迟（5 游戏日）

### 7. SylvieCooldownTracker（冷却状态跟踪组件）

**文件位置**: `Source/Components/SylvieCooldownTracker.cs`

ThingComp（组件），用于跟踪远程武器冷却状态。该组件只负责状态跟踪，不负责渲染。

**核心属性**：
- `IsInRangedCooldown` - 检测是否处于远程武器冷却状态（排除连发和近战攻击）
- `CooldownProgress` - 冷却进度（0-1 浮点数，基于 elapsedTicks / totalCooldownTicks）

**静态方法**：
- `GetTracker(Pawn)` - 获取或缓存指定 Pawn 的 SylvieCooldownTracker 实例

**动画帧计算方法**：
- `GetSweatFrame()` - 根据冷却进度计算汗液动画帧（1-3）
  - 进度 0-33%：帧 1
  - 进度 33-66%：帧 2
  - 进度 66-100%：帧 3
- `GetBulletAnimationState()` - 计算子弹装填动画状态
  - 返回 `(insertFrame, bulletCount)` 元组
  - `insertFrame`：子弹投入帧（0-3，0 表示不显示）
  - `bulletCount`：已装填子弹数量（1-5）
  - 将冷却进度分为 5 个循环周期，每个周期内根据进度显示不同的投入帧

**实现细节**：
```csharp
// File: Source/Components/SylvieCooldownTracker.cs
#nullable enable

public class SylvieCooldownTracker : ThingComp
{
    private Pawn? cachedPawn;
    private int cooldownStartTick = -1;
    private int totalCooldownTicks = 0;
    private Verb? lastVerb;
    
    public Pawn Pawn => cachedPawn ??= (parent as Pawn)!;
    
    private static Dictionary<Pawn, SylvieCooldownTracker> trackers = new Dictionary<Pawn, SylvieCooldownTracker>();
    
    public static SylvieCooldownTracker? GetTracker(Pawn pawn)
    {
        if (trackers.TryGetValue(pawn, out var tracker))
            return tracker;
        tracker = pawn.GetComp<SylvieCooldownTracker>();
        if (tracker != null)
            trackers[pawn] = tracker;
        return tracker;
    }
    
    public bool IsInRangedCooldown
    {
        get
        {
            var stance = Pawn.stances?.curStance as Stance_Cooldown;
            if (stance == null) return false;
            
            Verb? verb = stance.verb;
            if (verb == null) return false;
            
            if (verb.state == VerbState.Bursting) return false;
            if (verb.verbProps.IsMeleeAttack) return false;
            
            return true;
        }
    }
    
    public float CooldownProgress
    {
        get
        {
            var stance = Pawn.stances?.curStance as Stance_Cooldown;
            if (stance == null || stance.verb == null)
                return 0f;
            
            Verb verb = stance.verb;
            
            if (lastVerb != verb)
            {
                lastVerb = verb;
                cooldownStartTick = Find.TickManager.TicksGame;
                totalCooldownTicks = stance.ticksLeft;
            }
            
            if (totalCooldownTicks <= 0)
                return 1f;
            
            int ticksLeft = stance.ticksLeft;
            int elapsedTicks = totalCooldownTicks - ticksLeft;
            
            return (float)elapsedTicks / totalCooldownTicks;
        }
    }
    
    public int GetSweatFrame()
    {
        float progress = CooldownProgress;
        if (progress < 0.33f) return 1;
        if (progress < 0.66f) return 2;
        return 3;
    }
    
    public (int insertFrame, int bulletCount) GetBulletAnimationState()
    {
        float progress = CooldownProgress;
        
        int cycle = (int)(progress * 5);
        if (cycle > 4) cycle = 4;
        
        float cycleProgress = (progress * 5) - cycle;
        
        int insertFrame;
        if (cycleProgress < 0.25f) insertFrame = 1;
        else if (cycleProgress < 0.5f) insertFrame = 2;
        else if (cycleProgress < 0.75f) insertFrame = 3;
        else insertFrame = 0;
        
        int bulletCount = cycle + 1;
        if (bulletCount > 5) bulletCount = 5;
        
        return (insertFrame, bulletCount);
    }
    
    public override void CompTick()
    {
        base.CompTick();
        
        if (!IsInRangedCooldown)
        {
            cooldownStartTick = -1;
            lastVerb = null;
        }
    }
}
```

### 8. SylvieCooldownOverlayComp（冷却叠加层渲染组件）

**文件位置**: `Source/Components/SylvieCooldownOverlayComp.cs`

ThingComp（组件），在远程武器冷却期间渲染汗液、弹匣、子弹动画。

#### 字段定义

```csharp
private static readonly Vector2 OverlaySize = new Vector2(1.5f, 1.5f);
private static readonly Vector3 DrawScale = Vector3.one;
private const float SweatLayer = 61f;
```

#### 渲染逻辑

**核心流程**：
1. 使用 `PostDraw` 方法在 Pawn 绘制后执行
2. 通过 `Matrix4x4.TRS` 进行缩放变换
3. **自动缩放**：使用 `CurLifeStage.headSizeFactor` 自动适配不同年龄的 pawn 大小（小孩 0.5/0.75，成年人 1.0）
4. **位置计算**：使用 `BaseHeadOffsetAt` 获取头部位置，组件偏移保持固定值（不缩放）
5. **智能朝向处理**：使用 `Graphic.MeshAt(rot)` 和 `Graphic.MatAt(rot)` 自动处理不同朝向

**渲染层级**（来自 `SylvieConstants.cs`）：
- **Sweat 组件**：`SweatRenderLayer = 61.1f`（位于胡子 60 和头发 62 之间）
- **Magazine 组件**：`MagazineRenderLayer = 61.2f`
- **Bullet 组件**：`BulletRenderLayer = 61.3f`

> **注意**：当前实现使用局部常量 `SweatLayer = 61f` 配合 `PawnRenderUtility.AltitudeForLayer()` 进行层级转换。建议后续统一使用 `SylvieConstants` 中定义的常量值。

#### 朝向处理逻辑

| 朝向 | 行为 |
|------|------|
| South | 显示 south 贴图 |
| East | 显示 east 贴图 |
| West | 显示 east 贴图（自动翻转 via MeshAt） |
| North | 智能判断 - 有 north 贴图的组件显示，没有的不显示 |

**重要**：必须使用 `Graphic.MeshAt(rot)` 获取 mesh，它会自动处理 West 朝向的翻转。不能使用 `MeshPool.plane10` 配合 scale 负值来翻转 - 这样不会翻转 UV！

#### 渲染元素

1. **汗液动画**（3 帧）：`sweat1`, `sweat2`, `sweat3`
2. **弹匣贴图**：`magazine`（全程显示）
3. **子弹投入动画**（3 帧）：`bullet_insert1`, `bullet_insert2`, `bullet_insert3`
4. **子弹计数**（5 帧）：`bullet1` - `bullet5`

#### 缩放机制说明

RimWorld 原版使用 `headSizeFactor` 控制头部图形大小（小孩 0.5/0.75，成年人 1.0）。本组件的缩放策略：

| 元素 | 缩放方式 | 说明 |
|------|----------|------|
| 组件偏移 (`faceOffset`) | **不缩放** | 保持固定值，确保组件相对于头部的位置不变 |
| 组件大小 (`drawScale`) | `headSizeFactor` 缩放 | 与头部图形大小同步 |
| 头部位置 (`headOffset`) | `BaseHeadOffsetAt` | 已包含 `sqrt(bodySizeFactor)` 缩放 |

**缩放计算流程**：
1. `headOffset` = `BaseHeadOffsetAt(rot)` - 已包含身体大小缩放
2. `faceOffset` = `GetFaceDrawOffset()` - 固定值，不缩放
3. `drawScale` = `Vector3.one * headSizeFactor` - 组件大小缩放

这样无论 pawn 是小孩还是成年人，组件都能正确显示在头部，并保持相对位置不变。

#### 完整渲染代码示例

```csharp
// File: Source/Components/SylvieCooldownOverlayComp.cs
#nullable enable

public override void PostDraw()
{
    base.PostDraw();
    
    // 1. 种族检查
    if (!SylvieDefNames.IsSylvieRace(Pawn))
        return;
    
    // 2. 冷却状态检查
    var tracker = SylvieCooldownTracker.GetTracker(Pawn);
    if (tracker == null || !tracker.IsInRangedCooldown)
        return;
    
    Rot4 rot = Pawn.Rotation;
    
    // 3. 获取头部大小因子（Biotech DLC）
    float headSizeFactor = 1f;
    if (ModsConfig.BiotechActive && Pawn.ageTracker.CurLifeStage.headSizeFactor.HasValue)
    {
        headSizeFactor = Pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
    }
    
    // 4. 计算头部基础偏移（已包含 bodySizeFactor 的平方根缩放）
    Vector3 headOffset = Pawn.Drawer.renderer.BaseHeadOffsetAt(rot);
    
    // 5. 计算面部组件偏移（不缩放，保持固定值）
    Vector3 faceOffset = GetFaceDrawOffset();
    
    // 6. 计算最终绘制位置
    Vector3 drawPos = Pawn.DrawPos + headOffset + faceOffset;
    drawPos.y += 0.01f; // 层级微调
    
    // 7. 计算缩放矩阵（组件大小按 headSizeFactor 缩放）
    Vector3 drawScale = DrawScale * headSizeFactor;
    Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, drawScale);
    
    // 8. 渲染汗液动画（North 朝向不显示，因为没有 north 贴图）
    int sweatFrame = tracker.GetSweatFrame();
    RenderSweat(rot, drawPos, drawScale, sweatFrame);
    
    // 9. 渲染弹匣
    RenderMagazine(rot, drawPos, drawScale);
    
    // 10. 渲染子弹投入动画
    var (insertFrame, bulletCount) = tracker.GetBulletAnimationState();
    RenderBulletInsert(rot, drawPos, drawScale, insertFrame);
    
    // 11. 渲染子弹计数
    RenderBulletCount(rot, drawPos, drawScale, bulletCount);
}

/// <summary>
/// 渲染汗液动画 - 使用 SweatLayer 61（在胡子和头发之间）
/// </summary>
private void RenderSweat(Rot4 rot, Vector3 baseDrawPos, Vector3 drawScale, int sweatFrame)
{
    if (sweatFrame < 1 || sweatFrame > 3) return;
    
    // North 朝向时，没有 north 贴图的组件不显示
    if (rot == Rot4.North)
    {
        // sweat 没有 north 贴图，不显示
        return;
    }
    
    Graphic sweatGraphic = SweatGraphics[sweatFrame - 1];
    Material mat = sweatGraphic.MatAt(rot);
    Mesh mesh = sweatGraphic.MeshAt(rot);
    
    if (mat != null)
    {
        Vector3 sweatDrawPos = baseDrawPos;
        sweatDrawPos.y = Pawn.DrawPos.y + PawnRenderUtility.AltitudeForLayer(SweatLayer);
        Matrix4x4 matrix = Matrix4x4.TRS(sweatDrawPos, Quaternion.identity, drawScale);
        Graphics.DrawMesh(mesh, matrix, mat, 0);
    }
}

/// <summary>
/// 渲染弹匣 - 使用 Graphic.MeshAt/MatAt 处理朝向
/// </summary>
private void RenderMagazine(Rot4 rot, Vector3 drawPos, Vector3 drawScale)
{
    Material magazineMat = MagazineGraphic.MatAt(rot);
    Mesh magazineMesh = MagazineGraphic.MeshAt(rot);
    if (magazineMat != null)
    {
        Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, drawScale);
        Graphics.DrawMesh(magazineMesh, matrix, magazineMat, 0);
    }
}

/// <summary>
/// 智能判断是否有 north 贴图
/// 原理：比较 MatNorth 和 MatSouth 是否为同一材质
/// </summary>
private bool HasNorthTexture(Graphic graphic)
{
    return graphic.MatNorth != graphic.MatSouth;
}

/// <summary>
/// 渲染子弹投入动画
/// </summary>
private void RenderBulletInsert(Rot4 rot, Vector3 drawPos, Vector3 drawScale, int insertFrame)
{
    if (insertFrame < 1 || insertFrame > 3) return;
    
    Graphic insertGraphic = BulletInsertGraphics[insertFrame - 1];
    Material insertMat = insertGraphic.MatAt(rot);
    Mesh insertMesh = insertGraphic.MeshAt(rot);
    if (insertMat != null)
    {
        Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, drawScale);
        Graphics.DrawMesh(insertMesh, matrix, insertMat, 0);
    }
}

/// <summary>
/// 渲染子弹计数
/// </summary>
private void RenderBulletCount(Rot4 rot, Vector3 drawPos, Vector3 drawScale, int bulletCount)
{
    if (bulletCount < 1 || bulletCount > 5) return;
    
    Graphic bulletGraphic = BulletCountGraphics[bulletCount - 1];
    Material bulletMat = bulletGraphic.MatAt(rot);
    Mesh bulletMesh = bulletGraphic.MeshAt(rot);
    if (bulletMat != null)
    {
        Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, drawScale);
        Graphics.DrawMesh(bulletMesh, matrix, bulletMat, 0);
    }
}

private static readonly Vector3[] FaceOffsets = new Vector3[]
{
    new Vector3(0f, 1f, 0f),   // North
    new Vector3(0f, 1f, 0f),   // East
    new Vector3(0f, 1f, 0f),   // South
    new Vector3(0f, 1f, 0f)    // West
};

/// <summary>
/// 获取面部组件偏移 - 根据朝向返回对应偏移值
/// </summary>
private Vector3 GetFaceDrawOffset()
{
    Rot4 rot = Pawn.Rotation;
    return FaceOffsets[rot.AsInt];
}
```

#### 关键实现细节

```csharp
// File: Source/Components/SylvieCooldownOverlayComp.cs
#nullable enable

public override void PostDraw()
{
    base.PostDraw();
    
    // 1. 种族检查
    if (!SylvieDefNames.IsSylvieRace(Pawn))
        return;
    
    // 2. 冷却状态检查
    var tracker = SylvieCooldownTracker.GetTracker(Pawn);
    if (tracker == null || !tracker.IsInRangedCooldown)
        return;
    
    Rot4 rot = Pawn.Rotation;
    
    // 3. 获取 headSizeFactor（Biotech DLC 支持不同年龄段头部大小）
    float headSizeFactor = 1f;
    if (ModsConfig.BiotechActive && Pawn.ageTracker.CurLifeStage.headSizeFactor.HasValue)
    {
        headSizeFactor = Pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
    }
    
    // 4. 获取头部基础偏移（已包含 bodySizeFactor 的平方根缩放）
    Vector3 headOffset = Pawn.Drawer.renderer.BaseHeadOffsetAt(rot);
    
    // 5. 计算面部组件偏移（不缩放，保持固定值）
    Vector3 faceOffset = GetFaceDrawOffset();
    
    // 6. 计算缩放后的绘制尺寸
    Vector3 drawScale = DrawScale * headSizeFactor;
    
    // 7. 计算最终绘制位置
    Vector3 drawPos = Pawn.DrawPos + headOffset + faceOffset;
    drawPos.y += 0.01f; // 层级微调
    
    // 8. 使用 Graphic.MeshAt 获取 mesh（自动处理 West 朝向翻转）
    // 使用 Graphic.MatAt 获取对应朝向的材质
    Mesh mesh = graphic.MeshAt(rot);
    Material mat = graphic.MatAt(rot);
    
    // 9. 渲染
    Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, drawScale);
    Graphics.DrawMesh(mesh, matrix, mat, 0);
}
```

#### 朝向处理实现细节

**核心原理**：
- 使用 `Graphic.MeshAt(rot)` 获取正确的 mesh（会自动处理 West 朝向的翻转）
- 使用 `Graphic.MatAt(rot)` 获取对应朝向的材质
- 通过比较 `MatNorth` 和 `MatSouth` 判断是否有独立的 north 贴图

**朝向行为表**：

| 朝向 | MeshAt 行为 | MatAt 行为 | 说明 |
|------|-------------|------------|------|
| South | 正常 mesh | 返回 south 材质 | 正面显示 |
| East | 正常 mesh | 返回 east 材质 | 右侧显示 |
| West | **翻转 mesh** | 返回 east 材质 | 左侧显示（自动翻转） |
| North | 正常 mesh | 返回 north 材质 | 背面显示 |

**错误做法（不要这样做）**：
```csharp
// File: Example - Wrong Approach
#nullable enable

// 错误：使用 MeshPool.plane10 配合 scale 负值
// 这样不会正确翻转 UV！
Vector3 wrongScale = new Vector3(-headSizeFactor, 1f, headSizeFactor);
Matrix4x4 wrongMatrix = Matrix4x4.TRS(drawPos, Quaternion.identity, wrongScale);
Graphics.DrawMesh(MeshPool.plane10, wrongMatrix, mat, 0); // UV 不会翻转！
```

**正确做法**：
```csharp
// File: Source/Components/SylvieCooldownOverlayComp.cs
#nullable enable

// 正确：使用 Graphic.MeshAt(rot) 和 Graphic.MatAt(rot)
// 这两个方法会自动处理 West 朝向的翻转
Mesh mesh = graphic.MeshAt(rot);  // 自动处理 West 朝向翻转
Material mat = graphic.MatAt(rot);
Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, drawScale);
Graphics.DrawMesh(mesh, matrix, mat, 0);
```

### 9. IncidentWorker_SylvieTrader（事件处理器）

**文件位置**: `Source/Incidents/IncidentWorker_SylvieTrader.cs`

处理奴隶商人事件：
- 生成商队
- 使用 `SylviePawnGenerator` 生成希尔薇
- 发送选择信件
- **智能检测**：如果殖民地已有希尔薇种族的殖民者，事件不会触发

### 10. ChoiceLetter_SylvieOffer（信件类）

**文件位置**: `Source/Letters/ChoiceLetter_SylvieOffer.cs`

自定义选择信件：
- 显示购买选项（100 白银）
- 处理购买和拒绝逻辑
- 转移希尔薇到玩家派系

### 11. Patch_CommsConsole（通讯台补丁）

**文件位置**: `Source/Patches/Patch_CommsConsole.cs`

Harmony 补丁，为通讯台添加呼叫专用服装贸易商选项。

**核心功能**：
- `Postfix` - 在原有选项后添加"呼叫专用服装贸易商"选项（翻译键：`CallSpecialApparelTrader`）
- `SpawnSpecialTrader` - 生成 `Sylvie_ClothingTrader` 贸易商飞船
- `IsTraderAlreadyInOrbit` - 检查是否已有同名贸易商在轨道上，防止重复呼叫

**实现细节**：
```csharp
// File: Source/Patches/Patch_CommsConsole.cs
#nullable enable

[HarmonyPatch(typeof(Building_CommsConsole), nameof(Building_CommsConsole.GetFloatMenuOptions))]
public static class Patch_CommsConsole
{
    public static IEnumerable<FloatMenuOption> Postfix(
        IEnumerable<FloatMenuOption> __result,
        Building_CommsConsole __instance,
        Pawn myPawn)
    {
        // 先返回原有选项
        foreach (FloatMenuOption opt in __result)
        {
            yield return opt;
        }

        // 添加专用服装贸易商选项
        yield return new FloatMenuOption(
            "CallSpecialApparelTrader".Translate(),
            () => SpawnSpecialTrader(__instance.Map)
        );
    }

    private static void SpawnSpecialTrader(Map map)
    {
        string shipName = "Sylvie_ClothingSupplyShipName".Translate();

        // 检查是否已有同名贸易商在轨道上
        if (IsTraderAlreadyInOrbit(map, shipName))
        {
            Messages.Message(
                "Sylvie_TraderAlreadyInOrbit".Translate(),
                MessageTypeDefOf.RejectInput
            );
            return;
        }

        // 生成贸易商
        TraderKindDef? traderDef = SylvieDefNames.Trader_ClothingTraderDef;
        TradeShip tradeShip = new TradeShip(traderDef);
        tradeShip.name = shipName;
        map.passingShipManager.AddShip(tradeShip);
        tradeShip.GenerateThings();

        Messages.Message(
            "Sylvie_TraderArrived".Translate(),
            MessageTypeDefOf.PositiveEvent
        );
    }
}
```

### 12. SylvieCatEarComp（猫耳组件）

**文件位置**: `Source/Components/SylvieCatEarComp.cs`

ThingComp（组件），用于在研究动画期间渲染动态猫耳。

**核心功能**：
- `SetCurrentEarFrame(int frameIndex)` - 设置当前猫耳帧（0=猫耳1, 1=猫耳2）
- `SetShouldRender(bool render)` - 控制是否渲染猫耳
- `PostDraw()` - 在 Pawn 绘制后渲染猫耳贴图

**渲染逻辑**：
- **渲染层级**：使用 `EarLayer = 74f`（通过 `PawnRenderUtility.AltitudeForLayer(74)` 转换）
  - 位于头发（62）之上，头盔（75+）之下
  - 常量定义：`CatEarRenderLayer = 74f`（来自 `SylvieConstants.cs`）
- 使用 `Graphic.MeshAt(rot)` 和 `Graphic.MatAt(rot)` 处理朝向
- 支持 Biotech DLC 的 `headSizeFactor` 自动缩放
- 使用 `BaseHeadOffsetAt(rot)` 获取头部位置
- 种族检查使用 `SylvieDefNames.IsSylvieRace(Pawn)` 辅助方法

**实现细节**：
```csharp
// File: Source/Components/SylvieCatEarComp.cs
#nullable enable

public class SylvieCatEarComp : ThingComp
{
    private Pawn? cachedPawn;
    private Graphic? catEar1Graphic;
    private Graphic? catEar2Graphic;
    
    // 0 = 猫耳1, 1 = 猫耳2
    private int currentEarFrame = 0;
    private bool shouldRender = false;
    
    public Pawn Pawn => cachedPawn ??= (parent as Pawn)!;
    
    private static readonly Vector2 EarSize = new Vector2(1.5f, 1.5f);
    private const float EarLayer = 74f;
    
    private Graphic CatEar1Graphic
    {
        get
        {
            if (catEar1Graphic == null)
            {
                catEar1Graphic = GraphicDatabase.Get<Graphic_Multi>(
                    "Things/Pawn/Sylvie/CatEars/Normal/Unisex/catEar1",
                    ShaderDatabase.Transparent,
                    EarSize,
                    Color.white);
            }
            return catEar1Graphic;
        }
    }
    
    private Graphic CatEar2Graphic
    {
        get
        {
            if (catEar2Graphic == null)
            {
                catEar2Graphic = GraphicDatabase.Get<Graphic_Multi>(
                    "Things/Pawn/Sylvie/CatEars/Normal/Unisex/catEar2",
                    ShaderDatabase.Transparent,
                    EarSize,
                    Color.white);
            }
            return catEar2Graphic;
        }
    }
    
    /// <summary>
    /// 设置当前猫耳帧
    /// </summary>
    /// <param name="frameIndex">0=猫耳1, 1=猫耳2</param>
    public void SetCurrentEarFrame(int frameIndex)
    {
        currentEarFrame = frameIndex;
    }
    
    public void SetShouldRender(bool render)
    {
        shouldRender = render;
    }
    
    private Graphic GetCurrentEarGraphic()
    {
        // 0 = 猫耳1, 1 = 猫耳2
        return currentEarFrame == 0 ? CatEar1Graphic : CatEar2Graphic;
    }
    
    public override void PostDraw()
    {
        base.PostDraw();
        
        if (!shouldRender)
            return;
        
        if (!SylvieDefNames.IsSylvieRace(Pawn))
            return;
        
        Rot4 rot = Pawn.Rotation;
        
        float headSizeFactor = 1f;
        if (ModsConfig.BiotechActive && Pawn.ageTracker.CurLifeStage.headSizeFactor.HasValue)
        {
            headSizeFactor = Pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
        }
        
        Vector3 headOffset = Pawn.Drawer.renderer.BaseHeadOffsetAt(rot);
        Vector3 drawPos = Pawn.DrawPos + headOffset;
        drawPos.y = Pawn.DrawPos.y + PawnRenderUtility.AltitudeForLayer((int)EarLayer);
        
        Vector3 drawScale = Vector3.one * headSizeFactor;
        
        Graphic earGraphic = GetCurrentEarGraphic();
        Material mat = earGraphic.MatAt(rot);
        Mesh mesh = earGraphic.MeshAt(rot);
        
        if (mat != null)
        {
            Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, drawScale);
            Graphics.DrawMesh(mesh, matrix, mat, 0);
        }
    }
}
```

### 13. Patch_ResearchAnimation（研究动画补丁）

**文件位置**: `Source/Patches/Patch_ResearchAnimation.cs`

Harmony 补丁，实现研究动画（8帧）与猫耳的同步。

**研究动画定义**（`Defs/FacialAnimation/Animations/ResearchAnimation.xml`）：
| 帧索引 | 持续时间 | 眉毛形状 | 嘴巴形状 | 眼球形状 | 表情 |
|--------|----------|----------|----------|----------|------|
| 0 | 30 ticks | normal | w_mouth | white_eye | - |
| 1 | 30 ticks | normal | w_mouth | white_eye | - |
| 2 | 30 ticks | normal | w_mouth | white_eye_right | - |
| 3 | 30 ticks | normal | w_mouth | white_eye_right | - |
| 4 | 30 ticks | normal | w_mouth | white_eye_left | - |
| 5 | 30 ticks | normal | w_mouth | white_eye_left | - |
| 6 | 30 ticks | normal | w_mouth | circle_eye_down | gloomy |
| 7 | 30 ticks | normal | w_mouth | circle_eye_down | gloomy |

**核心组件**：

#### Patch_FaceAnimation_GetCurrentFrame_Research
拦截 FA 的 `GetCurrentFrame` 方法，处理 `Sylvie_ResearchAnimation` 的帧计算：

```csharp
// File: Source/Patches/Patch_ResearchAnimation.cs
#nullable enable

[HarmonyPatch(typeof(FaceAnimation), nameof(FaceAnimation.GetCurrentFrame))]
public static class Patch_FaceAnimation_GetCurrentFrame_Research
{
    private static Dictionary<FaceAnimation, Pawn> animationToPawn = new Dictionary<FaceAnimation, Pawn>();
    private static FieldInfo? startTickField;
    private static FieldInfo? animationFramesField;
    
    private static int GetStartTick(FaceAnimation animation)
    {
        if (startTickField == null)
        {
            startTickField = typeof(FaceAnimation).GetField("startTick", 
                BindingFlags.NonPublic | BindingFlags.Instance);
        }
        return (int?)startTickField?.GetValue(animation) ?? 0;
    }
    
    private static List<FaceAnimationDef.AnimationFrame>? GetAnimationFrames(FaceAnimationDef animationDef)
    {
        if (animationFramesField == null)
        {
            animationFramesField = typeof(FaceAnimationDef).GetField("animationFrames", 
                BindingFlags.NonPublic | BindingFlags.Instance);
        }
        return animationFramesField?.GetValue(animationDef) as List<FaceAnimationDef.AnimationFrame>;
    }
    
    public static bool Prefix(FaceAnimation __instance, int tickGame, 
                            ref FaceAnimationDef.AnimationFrame? __result)
    {
        // 只处理 Sylvie_ResearchAnimation（使用空值传播防止 NullReferenceException）
        if (__instance?.animationDef?.defName != "Sylvie_ResearchAnimation") 
            return true;
        
        // 检查动画到 Pawn 的映射是否存在
        if (!animationToPawn.TryGetValue(__instance, out var pawn) || pawn == null)
            return true;
        
        // 只处理希尔薇种族
        if (!SylvieDefNames.IsSylvieRace(pawn))
            return true;
        
        // 获取动画帧列表（已根据 duration 展开）
        var frames = __instance.animationDef.GetSequentialAnimationFrames();
        if (frames == null || frames.Count == 0) return true;
        
        // 使用反射获取 startTick
        int startTick = GetStartTick(__instance);
        int elapsedTicks = tickGame - startTick;
        
        // 确保 elapsedTicks 非负
        if (elapsedTicks < 0) elapsedTicks = 0;
        
        // 计算循环索引
        int frameCount = frames.Count;
        if (frameCount <= 0) return true;
        
        int frameIndex = elapsedTicks % frameCount;
        
        // 最终安全检查
        if (frameIndex < 0 || frameIndex >= frameCount)
            frameIndex = 0;
        
        __result = frames[frameIndex];
        
        // 根据原始动画帧索引设置猫耳
        var catEarComp = pawn.GetComp<SylvieCatEarComp>();
        if (catEarComp != null)
        {
            // 使用反射获取原始动画帧列表，从中读取 duration
            var originalFrames = GetAnimationFrames(__instance.animationDef);
            if (originalFrames != null && originalFrames.Count > 0)
            {
                int durationPerFrame = originalFrames[0].duration;  // 从 XML 读取 duration
                // 使用默认值防止除零
                if (durationPerFrame <= 0) durationPerFrame = SylvieConstants.DefaultAnimationDuration;
                
                int originalFrameIndex = frameIndex / durationPerFrame;
                int originalFrameCount = originalFrames.Count;
                if (originalFrameIndex >= originalFrameCount) 
                    originalFrameIndex = originalFrameCount - 1;
                
                int earFrame = (originalFrameIndex == 0 || originalFrameIndex == 2 || originalFrameIndex == 4) ? 0 : 1;
                catEarComp.SetCurrentEarFrame(earFrame);
                catEarComp.SetShouldRender(true);
            }
        }
        
        return false;  // 跳过原始方法
    }
}
```

**关键空值检查点**：

1. **动画定义检查**：`__instance?.animationDef?.defName` - 使用空值传播运算符防止 NullReferenceException
2. **Pawn 映射检查**：`animationToPawn.TryGetValue(__instance, out var pawn)` - 确保动画已注册到 Pawn
3. **种族检查**：`SylvieDefNames.IsSylvieRace(pawn)` - 使用辅助方法进行类型安全的判断
4. **帧列表检查**：`frames == null || frames.Count == 0` - 确保动画帧数据有效
5. **边界检查**：`frameIndex < 0 || frameIndex >= frameCount` - 防止数组越界
6. **组件检查**：`catEarComp != null` - 确保猫耳组件存在

**猫耳帧映射**（来自 `Patch_ResearchAnimation.cs` 第98-101行）：

| 原始动画帧 | 眼睛形状 | 猫耳帧 | 说明 |
|------------|----------|--------|------|
| 0 | white_eye | 0（猫耳1）| 白眼正视 |
| 1 | white_eye | 1（猫耳2）| 白眼正视 |
| 2 | white_eye_right | 0（猫耳1）| 白眼看右 |
| 3 | white_eye_right | 1（猫耳2）| 白眼看右 |
| 4 | white_eye_left | 0（猫耳1）| 白眼看左 |
| 5 | white_eye_left | 1（猫耳2）| 白眼看左 |
| 6 | circle_eye_down | 1（猫耳2）| 黑圈眼向下 |
| 7 | circle_eye_down | 1（猫耳2）| 黑圈眼向下 |

**映射规则**：
```csharp
// 原始帧 0, 2, 4 -> 猫耳1
// 原始帧 1, 3, 5, 6, 7 -> 猫耳2
int earFrame = (originalFrameIndex == 0 || originalFrameIndex == 2 || originalFrameIndex == 4) ? 0 : 1;
```

#### Patch_FacialAnimationControllerComp_CompTick
在 `CompTick` 中检测研究状态变化，控制猫耳显示/隐藏：

```csharp
// File: Source/Patches/Patch_ResearchAnimation.cs
#nullable enable

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
```

**空值检查模式说明**：

补丁中使用了防御性编程的空值检查模式来修复 CS8602 警告：

```csharp
// File: Source/Patches/Patch_ResearchAnimation.cs
#nullable enable

// 模式1：早期返回（Guard Clause）
Pawn? pawn = __instance.parent as Pawn;
if (pawn == null) return;

// 模式2：使用辅助方法进行类型安全的判断
if (!SylvieDefNames.IsSylvieRace(pawn)) return;

// 模式3：组件获取检查
var catEarComp = pawn.GetComp<SylvieCatEarComp>();
if (catEarComp == null) return;
```

这种模式的优点：
1. **消除 CS8602 警告** - 编译器能够识别 null 检查后的变量为非空
2. **提高代码可读性** - 每个检查点都有明确的意图
3. **避免深层嵌套** - 使用早期返回代替嵌套的 if 语句
4. **类型安全** - `IsSylvieRace` 辅助方法封装了种族判断逻辑

#### Patch_FacialAnimationControllerComp_InitializeIfNeed_Research
在动画初始化时注册动画到 Pawn 的映射：

```csharp
// File: Source/Patches/Patch_ResearchAnimation.cs
#nullable enable

[HarmonyPatch(typeof(FacialAnimationControllerComp), "InitializeIfNeed")]
public static class Patch_FacialAnimationControllerComp_InitializeIfNeed_Research
{
    public static void Postfix(FacialAnimationControllerComp __instance, 
                               Pawn ___pawn, 
                               Dictionary<string, List<FaceAnimation>> ___animationDict)
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
```

**重要技术细节**：

1. **FA 原生帧计算行为**：
   - `GetSequentialAnimationFrames()` 会根据每帧的 `duration` 展开帧列表
   - 例如：8帧，每帧 duration=30，展开后共 240 帧（8 × 30）
   - FA 原生的 `GetCurrentFrame` 使用 `tickGame - startTick` 直接索引展开后的列表
   - 这意味着同一原始帧会持续 `duration` 个 tick

2. **猫耳同步机制**：
   - 获取展开后的帧索引：`frameIndex = elapsedTicks % frameCount`
   - 使用反射获取原始动画帧列表：`GetAnimationFrames(animationDef)`
   - 从原始帧读取 duration：`durationPerFrame = originalFrames[0].duration`
   - 计算原始帧索引：`originalFrameIndex = frameIndex / durationPerFrame`
   - 根据原始帧索引设置猫耳：`earFrame = (originalFrameIndex == 0 || originalFrameIndex == 2 || originalFrameIndex == 4) ? 0 : 1`

2. **反射获取 startTick**：
   - `startTick` 是 `FaceAnimation` 的私有字段
   - 使用反射获取：`typeof(FaceAnimation).GetField("startTick", BindingFlags.NonPublic | BindingFlags.Instance)`

3. **空值检查**：
   - `__instance?.animationDef?.defName` - 防止空引用
   - `animationToPawn.TryGetValue` - 确保字典中有映射
   - `frames == null || frames.Count == 0` - 确保帧列表有效

4. **边界检查**：
   - `if (elapsedTicks < 0) elapsedTicks = 0` - 防止负数索引
   - `if (frameIndex < 0 || frameIndex >= frameCount) frameIndex = 0` - 最终安全检查

3. **动画定义** (`Defs/FacialAnimation/Animations/ResearchAnimation.xml`):

```xml
<FacialAnimation.FaceAnimationDef>
  <defName>Sylvie_ResearchAnimation</defName>
  <animationFrames>
    <!-- 8帧循环动画，每帧30 ticks -->
    <li>
      <duration>30</duration>
      <browShapeDef>normal</browShapeDef>
      <mouthShapeDef>w_mouth</mouthShapeDef>
      <eyeballShapeDef>white_eye</eyeballShapeDef>
    </li>
    <!-- ... 共8帧 ... -->
    <li>
      <duration>30</duration>
      <browShapeDef>normal</browShapeDef>
      <mouthShapeDef>w_mouth</mouthShapeDef>
      <eyeballShapeDef>circle_eye_down</eyeballShapeDef>
      <emotionShapeDef>gloomy</emotionShapeDef>
    </li>
  </animationFrames>
  <targetJobs>
    <li>Research</li>
  </targetJobs>
  <priority>10003</priority>
</FacialAnimation.FaceAnimationDef>
```

**眼睛形状定义** (`Defs/FacialAnimation/Shapes/EyeShapeEx_Research.xml`):

```xml
<!-- 白眼球形状 -->
<FacialAnimation.EyeballShapeDef>
  <defName>white_eye</defName>
  <label>white eye</label>
</FacialAnimation.EyeballShapeDef>

<!-- 右眼眨眼 -->
<FacialAnimation.EyeballShapeDef>
  <defName>white_eye_right</defName>
  <label>white eye right</label>
</FacialAnimation.EyeballShapeDef>

<!-- 左眼眨眼 -->
<FacialAnimation.EyeballShapeDef>
  <defName>white_eye_left</defName>
  <label>white eye left</label>
</FacialAnimation.EyeballShapeDef>

<!-- 圈圈眼 -->
<FacialAnimation.EyeballShapeDef>
  <defName>circle_eye</defName>
  <label>circle eye</label>
</FacialAnimation.EyeballShapeDef>

<!-- 向下圈圈眼 -->
<FacialAnimation.EyeballShapeDef>
  <defName>circle_eye_down</defName>
  <label>circle eye down</label>
</FacialAnimation.EyeballShapeDef>
```

**注意事项**：
- `EyeballShapeDef` **不能**指定 `eyeballType` 字段（该字段不存在）
- 动画定义（`FaceAnimationDef`）**不应**添加 `raceName` 限制
- TypeDef（如 `EyeballTypeDef`）**需要**添加 `raceName` 限制
- 如果动画有 `raceName` 限制而 TypeDef 也有，会导致 FA 框架匹配冲突

### 14. Patch_FaceAnimationDef_IsSame（种族限制核心补丁）

**文件位置**: `Source/Patches/Patch_FaceAnimationDef_IsSame.cs`

**核心功能**:
修改 FA 的 `FaceAnimationDef.IsSame` 方法，实现 Sylvie 专属动画与 FA 原版动画共存。

**问题背景**:
FA 原生的 `IsSame` 方法严格匹配 raceName：
```csharp
if (raceName != targetName) return false;
```
这导致如果 Sylvie 动画设置 raceName="Sylvie_Race"，FA 原版动画 raceName="" 就无法匹配。

**解决方案**:
```csharp
if (!string.IsNullOrEmpty(raceName) && raceName != targetName) return false;
```
- raceName 为空 → 允许任何种族使用（FA 原版动画）
- raceName 不为空 → 只允许对应种族使用（Sylvie 专属动画）

**代码实现**:
```csharp
[HarmonyPatch(typeof(FaceAnimationDef), nameof(FaceAnimationDef.IsSame))]
public static class Patch_FaceAnimationDef_IsSame
{
    public static bool Prefix(FaceAnimationDef __instance, string jobName,
                            string targetName, ref bool __result)
    {
        if (string.IsNullOrEmpty(jobName))
        {
            __result = false;
            return false;
        }
        if (targetName == "Sylvie_Race")
        {
            if (!string.IsNullOrEmpty(__instance.raceName) && __instance.raceName != targetName)
            {
                __result = false;
                return false;
            }
            __result = __instance.targetJobs.Contains(jobName);
            return false;
        }
        return true;
    }
}
```

**关键约束**:
- 只对 targetName == "Sylvie_Race" 应用新逻辑
- 其他种族使用原生 FA 逻辑，避免影响其他模组

### 15. Patch_Stance_Warmup（瞄准动画同步补丁）

**文件位置**: `Source/Patches/Patch_Stance_Warmup.cs`

Harmony 补丁，拦截 Facial Animation 的 `GetCurrentFrame` 方法，实现瞄准动画（3帧）同步。

**核心组件**：

#### SylvieAimingTracker
ThingComp（组件），用于缓存 Pawn 引用：
```csharp
// File: Source/Patches/Patch_Stance_Warmup.cs
#nullable enable

public class SylvieAimingTracker : ThingComp
{
    private Pawn? cachedPawn;
    
    public Pawn Pawn => cachedPawn ??= (parent as Pawn)!;
    
    private static Dictionary<Pawn, SylvieAimingTracker> trackers = new Dictionary<Pawn, SylvieAimingTracker>();
    
    public static SylvieAimingTracker? GetTracker(Pawn pawn)
    {
        if (trackers.TryGetValue(pawn, out var tracker))
            return tracker;
        tracker = pawn.GetComp<SylvieAimingTracker>();
        if (tracker != null)
            trackers[pawn] = tracker;
        return tracker;
    }
}
```

#### Patch_Pawn_SpawnSetup
为希尔薇种族 Pawn 动态添加所需组件：
```csharp
// File: Source/Patches/Patch_Stance_Warmup.cs
#nullable enable

[HarmonyPatch(typeof(Pawn), nameof(Pawn.SpawnSetup))]
public static class Patch_Pawn_SpawnSetup
{
    public static void Postfix(Pawn __instance)
    {
        if (SylvieDefNames.IsSylvieRace(__instance.def))
        {
            // 添加瞄准跟踪器
            if (__instance.GetComp<SylvieAimingTracker>() == null)
            {
                __instance.AllComps.Add(new SylvieAimingTracker { parent = __instance });
            }
            // 添加冷却跟踪器
            if (__instance.GetComp<SylvieCooldownTracker>() == null)
            {
                __instance.AllComps.Add(new SylvieCooldownTracker { parent = __instance });
            }
            // 添加冷却叠加层组件
            if (__instance.GetComp<SylvieCooldownOverlayComp>() == null)
            {
                __instance.AllComps.Add(new SylvieCooldownOverlayComp { parent = __instance });
            }
            // 添加猫耳组件
            if (__instance.GetComp<SylvieCatEarComp>() == null)
            {
                __instance.AllComps.Add(new SylvieCatEarComp { parent = __instance });
            }
        }
    }
}
```

#### Patch_FaceAnimation_GetCurrentFrame
拦截动画帧计算，实现3帧瞄准动画同步：

**瞄准动画定义**（`Defs/FacialAnimation/Animations/AimingAnimation.xml`）：
| 帧索引 | 持续时间 | 眉毛形状 | 嘴巴形状 | 眼睑选项 |
|--------|----------|----------|----------|----------|
| 0 | 30 ticks | angled | inverted_v | crosshair1 |
| 1 | 30 ticks | aiming | m_shape | crosshair2 |
| 2 | 30 ticks | aiming | m_shape | crosshair3 |

**帧序列**: `angled` → `aiming` → `aiming`

**动画帧逻辑**：
- `Stance_Warmup` 状态：基于 warmup 进度计算帧（帧0 → 帧1 → 帧2）
- `VerbState.Bursting` 状态：显示最后一帧（帧2）
- `Stance_Cooldown` 状态：返回 `GetCooldownFrame()` 构造的冷却帧（包含 `browShapeDef: confused`、`eyeballShapeDef: lookdown` 和 `mouthShapeDef: m_shape`）
- 其他状态：返回 `true` 让原始方法执行

**实现代码**：
```csharp
// File: Source/Patches/Patch_Stance_Warmup.cs
#nullable enable

[HarmonyPatch(typeof(FaceAnimation), nameof(FaceAnimation.GetCurrentFrame))]
public static class Patch_FaceAnimation_GetCurrentFrame
{
    public static bool Prefix(FaceAnimation __instance, int tickGame, 
                            ref FaceAnimationDef.AnimationFrame? __result)
    {
        // 只处理 Sylvie_AimingAnimation
        if (__instance.animationDef.defName != "Sylvie_AimingAnimation") 
            return true;
        
        if (!animationToPawn.TryGetValue(__instance, out var pawn))
            return true;
        
        var frames = __instance.animationDef.GetSequentialAnimationFrames();
        if (frames == null || frames.Count == 0) return true;
        
        Stance? curStance = pawn.stances?.curStance;
        
        if (curStance is Stance_Warmup warmup)
        {
            Verb? verb = warmup.verb;
            if (verb == null) return true;
            
            // 计算总瞄准时间（考虑 AimingDelayFactor）
            float warmupTime = verb.verbProps.warmupTime;
            float aimingDelayFactor = pawn.GetStatValue(StatDefOf.AimingDelayFactor);
            int totalWarmupTicks = (warmupTime * aimingDelayFactor).SecondsToTicks();
            
            if (totalWarmupTicks <= 0) return true;
            
            int ticksLeft = warmup.ticksLeft;
            int elapsedTicks = totalWarmupTicks - ticksLeft;
            
            // 3帧动画：基于进度计算当前帧
            int totalFrames = frames.Count;  // = 3
            float progress = (float)elapsedTicks / totalWarmupTicks;
            int frameIndex = (int)(progress * totalFrames);
            
            // 边界检查
            if (frameIndex >= totalFrames) frameIndex = totalFrames - 1;
            if (frameIndex < 0) frameIndex = 0;
            
            __result = frames[frameIndex];
            return false;  // 跳过原始方法
        }
        else if (curStance is Stance_Cooldown cooldown)
        {
            Verb? verb = cooldown.verb;
            // 连发期间显示最后一帧
            if (verb != null && verb.state == VerbState.Bursting)
            {
                __result = frames[frames.Count - 1];  // 帧2
                return false;
            }
            
            // 冷却期间显示困惑表情
            __result = GetCooldownFrame();
            return false;
        }
        
        return true;  // 让原始方法执行
    }
}
```

**冷却帧构造**：
```csharp
// File: Source/Patches/Patch_Stance_Warmup.cs
#nullable enable

private static FaceAnimationDef.AnimationFrame GetCooldownFrame()
{
    if (cachedCooldownFrame == null)
    {
        cachedCooldownFrame = new FaceAnimationDef.AnimationFrame
        {
            duration = SylvieConstants.DefaultAnimationDuration,  // 30 ticks
            browShapeDef = ConfusedBrowDef,                       // confused 眉毛
            eyeballShapeDef = LookdownEyeballDef                  // lookdown 眼球
        };
    }
    return cachedCooldownFrame;
}
```

**关键 API**：
```csharp
// File: Source/Patches/Patch_Stance_Warmup.cs
#nullable enable

// 武器词条
Verb.verbProps.warmupTime           // 瞄准时间（秒）

// Pawn 属性
pawn.GetStatValue(StatDefOf.AimingDelayFactor)   // 瞄准时间乘数

// Stance_Warmup 状态
warmup.ticksLeft    // 剩余瞄准 ticks
warmup.verb         // 当前使用的 Verb
```

### 16. 初始健康状态系统 (Hediffs/)

**文件位置**: `Defs/Hediffs/Sylvie_Hediffs.xml`

**Hediff 定义**:
```xml
<HediffDef>
  <defName>SylvieRace_InitialTrauma</defName>
  <label>Initial Trauma</label>
  <description>A deep psychological and physical trauma from past experiences.</description>
  <tendable>true</tendable>
  <isBad>true</isBad>
  <initialSeverity>1.0</initialSeverity>
  <stages>
    <li>
      <minSeverity>0.0</minSeverity>
      <painOffset>0.05</painOffset>
      <capMods>
        <li>
          <capacity>Consciousness</capacity>
          <offset>-0.1</offset>
        </li>
      </capMods>
    </li>
  </stages>
  <comps>
    <li Class="HediffCompProperties_TendDuration">
      <baseTendDurationHours>12</baseTendDurationHours>
      <severityPerDayTended>-0.2</severityPerDayTended>
    </li>
  </comps>
</HediffDef>
```

**触发机制**:
- 希尔薇加入殖民地后 5 个游戏日（300000 ticks）自动触发
- 触发时显示信件通知
- 通过 `SylvieGameComponent` 管理触发逻辑

**治疗机制**:
- 使用 `HediffCompProperties_TendDuration` 组件
- 每次治疗持续 12 小时
- 治疗质量 100% 时，每日降低 20% 严重性
- 完全治愈需要约 5 次高质量治疗

**存档兼容性**:
- 使用 `Scribe_References.Look<Pawn>` 保存希尔薇引用
- 使用 `Scribe_Values.Look<int>` 保存触发时间
- 使用 `Scribe_Values.Look<bool>` 保存触发状态

### 17. 心情效果系统 (Thoughts/)

**文件位置**: `Defs/Thoughts/Sylvie_Thoughts.xml`

**ThoughtDef 定义**:
```xml
<ThoughtDef>
  <defName>SylvieRace_InitialTraumaThought</defName>
  <workerClass>ThoughtWorker_Hediff</workerClass>
  <hediff>SylvieRace_InitialTrauma</hediff>
  <validWhileDespawned>true</validWhileDespawned>
  <stages>
    <li>
      <label>initial trauma</label>
      <description>The weight of past experiences weighs heavily...</description>
      <baseMoodEffect>-15</baseMoodEffect>
    </li>
  </stages>
</ThoughtDef>
```

**心情效果特点**:
- 使用 `ThoughtWorker_Hediff` 自动关联 Hediff
- 心情效果值：-15（比默认 Sick 的 -5 更严重）
- Hediff 消除后心情效果自动消失
- 可自定义标签和描述文本

**心情数值参考**:
| 心情值 | 效果级别 |
|--------|----------|
| -5 | 轻微不适（默认 Sick） |
| -10 | 中等不适 |
| -15 | 较严重不适（当前设置） |
| -20 | 严重不适 |

### 18. 寻求抚摸系统

**文件位置**: `Source/Jobs/JobGiver_SeekPetting.cs`, `Source/Jobs/JobDriver_SeekPetting.cs`, `Source/Components/SylvieSeekPettingTracker.cs`

#### 系统概述
寻求抚摸系统允许希尔薇殖民者主动寻求其他殖民者的抚摸，提供心情加成和社交关系增益。

系统由三个核心组件组成：
1. **JobGiver_SeekPetting** - ThinkNode，负责触发检查和目标选择
2. **JobDriver_SeekPetting** - JobDriver，执行抚摸效果
3. **SylvieSeekPettingTracker** - GameComponent，管理冷却状态

#### 完整常量定义

**JobGiver_SeekPetting.cs 常量** ([L16-L21](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobGiver_SeekPetting.cs#L16-L21)):

| 常量名 | 值 | 说明 |
|--------|-----|------|
| `MinCheckInterval` | `GenDate.TicksPerHour` (2500) | 最小检查间隔，1游戏小时 |
| `CheckChance` | `0.20f` | 每次检查触发概率，20% |
| `MaxSearchDistance` | `40` | 目标搜索范围（格） |
| `MinAgeYears` | `10` | 最小生物年龄要求 |
| `HighOpinionThreshold` | `40` | 高好感度阈值，用于评分加成 |
| `TargetMinMoodThreshold` | `0.50f` | 目标心情阈值，50% |

**SylvieSeekPettingTracker.cs 常量** ([L21](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Components/SylvieSeekPettingTracker.cs#L21)):

| 常量名 | 值 | 说明 |
|--------|-----|------|
| `CooldownTicks` | `15000` | 冷却时间，6游戏小时（6 * 2500） |

**SylvieDefNames.cs 常量** ([L34-L38](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Core/SylvieDefNames.cs#L34-L38)):

| 常量名 | Def名称 |
|--------|---------|
| `Job_SeekPetting` | `Sylvie_SeekPetting` |
| `Thought_WasPetted` | `Sylvie_WasPetted` |
| `Thought_PettedSomeone` | `Sylvie_PettedSomeone` |
| `Thought_PettedMe_Social` | `Sylvie_PettedMe_Social` |
| `Thought_WasPetted_Social` | `Sylvie_WasPetted_Social` |

#### JobGiver_SeekPetting

**文件位置**: `Source/Jobs/JobGiver_SeekPetting.cs`

**职责**: 继承 `ThinkNode_JobGiver`，定期检查并分配寻求抚摸任务

**7步检查流程详解** ([L32-L134](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobGiver_SeekPetting.cs#L32-L134)):

| 步骤 | 检查内容 | 代码位置 | 失败行为 |
|------|----------|----------|----------|
| 1 | **种族检查** | [L35-L38](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobGiver_SeekPetting.cs#L35-L38) | 静默返回 `null`（非希尔薇种族不输出日志） |
| 2 | **间隔检查** | [L43-L50](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobGiver_SeekPetting.cs#L43-L50) | 静默返回，距离上次检查不足1小时 |
| 3 | **概率检查** | [L53-L61](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobGiver_SeekPetting.cs#L53-L61) | 20%概率通过，`Rand.Value <= 0.20f` |
| 4 | **年龄检查** | [L67-L73](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobGiver_SeekPetting.cs#L67-L73) | 生物年龄必须 >= 10岁 |
| 5 | **状态检查** | [L76-L84](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobGiver_SeekPetting.cs#L76-L84) | 必须清醒、未倒地、未精神崩溃 |
| 6 | **冷却检查** | [L87-L95](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobGiver_SeekPetting.cs#L87-L95) | 不在6小时冷却期内 |
| 7 | **空闲检查** | [L99-L104](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobGiver_SeekPetting.cs#L99-L104) | 当前任务可中断（非关键任务） |
| 8 | **目标检查** | [L107-L113](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobGiver_SeekPetting.cs#L107-L113) | 40格范围内存在有效目标 |
| 9 | **JobDef检查** | [L116-L122](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobGiver_SeekPetting.cs#L116-L122) | `Sylvie_SeekPetting` JobDef存在 |

**关键方法代码示例**:

```csharp
// File: Source/Jobs/JobGiver_SeekPetting.cs
#nullable enable

// 步骤1-3: 种族、间隔、概率检查
protected override Job TryGiveJob(Pawn pawn)
{
    // 种族检查 - 非希尔薇种族静默返回
    if (!SylvieDefNames.IsSylvieRace(pawn))
    {
        return null!;
    }

    int currentTick = Find.TickManager.TicksGame;
    
    // 间隔检查 - 必须满足最小间隔
    int lastCheck = lastCheckTicks.TryGetValue(pawn, out int last) ? last : -1;
    int ticksSinceLastCheck = lastCheck < 0 ? int.MaxValue : currentTick - lastCheck;
    
    if (ticksSinceLastCheck < MinCheckInterval)
    {
        return null!;  // 静默返回，未到达间隔
    }
    
    // 概率检查 - 20%概率继续
    float checkRoll = Rand.Value;
    bool checkPassed = checkRoll <= CheckChance;
    lastCheckTicks[pawn] = currentTick;
    
    if (!checkPassed)
    {
        Log.Message($"[SylvieMod] {pawn.LabelShort}: [1/7] Interval check failed...");
        return null!;
    }
    
    // 后续检查...
}
```

**空闲检查逻辑** ([L141-L153](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobGiver_SeekPetting.cs#L141-L153)):

```csharp
private bool IsIdleForPetting(Pawn pawn)
{
    Job? curJob = pawn.CurJob;
    if (curJob == null)
        return true;

    // 关键任务不可中断
    if (IsCriticalJob(curJob.def))
        return false;

    return true;
}
```

**关键任务列表** ([L160-L179](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobGiver_SeekPetting.cs#L160-L179)):
- `JobDefOf.Rescue` - 救援
- `JobDefOf.TendPatient` - 治疗
- `JobDefOf.BeatFire` - 灭火
- `JobDefOf.AttackMelee` - 近战攻击
- `JobDefOf.AttackStatic` - 远程攻击
- `JobDefOf.Ingest` - 进食
- `JobDefOf.LayDown` - 躺下

**目标评分算法详解** ([L289-L311](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobGiver_SeekPetting.cs#L289-L311)):

```csharp
private float CalculateTargetScore(Pawn target, Pawn sylvie)
{
    float score = 0f;

    // 基础好感度分数
    int opinion = sylvie.relations?.OpinionOf(target) ?? 0;
    score += opinion;

    // 高好感度加成 (>40)
    if (opinion > HighOpinionThreshold)  // 40
    {
        score += 100f;  // 显著优先级提升
    }

    // 目标心情加成 (>50%)
    float? targetMood = target.needs?.mood?.CurLevelPercentage;
    if (targetMood.HasValue && targetMood.Value > TargetMinMoodThreshold)  // 0.50f
    {
        score += 50f;  // 好心情奖励
    }

    return score;
}
```

**评分公式总结**:
```
score = opinion + (opinion > 40 ? 100 : 0) + (targetMood > 0.5 ? 50 : 0)
```

**目标选择流程** ([L186-L240](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobGiver_SeekPetting.cs#L186-L240)):
1. 获取同派系所有已生成Pawn
2. 过滤有效目标 (`IsValidTarget`)
3. 计算每个候选者的评分
4. 按评分降序、距离升序排序
5. 返回最佳目标

**有效目标条件** ([L248-L280](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobGiver_SeekPetting.cs#L248-L280)):
- 不能是希尔薇自己
- 必须是类人生物 (`RaceProps.Humanlike`)
- 必须同派系
- 必须在40格范围内 (`InHorDistOf`)
- 必须清醒、未倒地、未精神崩溃
- 希尔薇对其好感度 > 0
- 希尔薇可以预留目标 (`CanReserve`)

#### 与ThinkTree的集成方式

**ThinkTree Patch配置** (`Patches/Sylvie_ThinkTreePatches.xml`):

```xml
<Operation Class="PatchOperationInsert">
  <xpath>/Defs/ThinkTreeDef[defName="Humanlike"]/thinkRoot/subNodes/li[@Class="ThinkNode_ConditionalColonist" and subNodes/li[@Class="ThinkNode_Tagger" and tagToGive="Idle"]]</xpath>
  <order>Prepend</order>
  <value>
    <li Class="ThinkNode_ConditionalColonist">
      <subNodes>
        <li Class="ThinkNode_Tagger">
          <tagToGive>Idle</tagToGive>
          <subNodes>
            <li Class="SylvieMod.JobGiver_SeekPetting" />
          </subNodes>
        </li>
      </subNodes>
    </li>
  </value>
</Operation>
```

**集成要点**:
- 使用 `PatchOperationInsert` 插入到 Humanlike ThinkTree
- 插入位置：在第一个 `ThinkNode_ConditionalColonist` + `ThinkNode_Tagger` (Idle) 之前 (`Prepend`)
- `ThinkNode_ConditionalColonist` 限制只有殖民者会执行
- `ThinkNode_Tagger` 标记为 Idle 行为，便于调试和优先级管理
- JobGiver 内部进行种族检查，确保只有希尔薇种族触发

#### JobDriver_SeekPetting

**文件位置**: `Source/Jobs/JobDriver_SeekPetting.cs`

**职责**: 继承 `JobDriver`，执行寻求抚摸任务，应用效果

**Toil序列** ([L36-L48](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobDriver_SeekPetting.cs#L36-L48)):

```csharp
protected override IEnumerable<Toil> MakeNewToils()
{
    // Toil 1: 移动到目标，PathEndMode.Touch
    yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch)
        .FailOnDespawnedOrNull(TargetIndex.A)
        .FailOn(() => TargetPawn.Dead);

    // Toil 2: 应用抚摸效果，Instant完成
    Toil pettingToil = new Toil();
    pettingToil.initAction = ApplyPettingEffects;
    pettingToil.defaultCompleteMode = ToilCompleteMode.Instant;
    yield return pettingToil;
}
```

**效果应用流程** ([L54-L77](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobDriver_SeekPetting.cs#L54-L77)):

```csharp
private void ApplyPettingEffects()
{
    if (TargetPawn == null || pawn == null)
        return;

    // 种族安全检查
    if (!SylvieDefNames.IsSylvieRace(pawn))
    {
        Log.Warning($"[SylvieMod] Non-Sylvie pawn {pawn.LabelShort} attempted to apply petting effects.");
        return;
    }

    ApplyMoodThoughts();      // 应用心情效果
    UpdateRelationship();     // 更新社交关系
    SendNotification();       // 发送通知
    RecordCooldown();         // 记录冷却
}
```

**心情效果应用** ([L82-L97](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobDriver_SeekPetting.cs#L82-L97)):

```csharp
private void ApplyMoodThoughts()
{
    // 希尔薇获得"被抚摸"想法
    ThoughtDef? wasPettedThought = SylvieDefNames.Thought_WasPettedDef;
    if (wasPettedThought != null)
    {
        pawn.needs?.mood?.thoughts?.memories?.TryGainMemory(wasPettedThought);
    }

    // 目标获得"抚摸了希尔薇"想法
    ThoughtDef? pettedSomeoneThought = SylvieDefNames.Thought_PettedSomeoneDef;
    if (pettedSomeoneThought != null)
    {
        TargetPawn.needs?.mood?.thoughts?.memories?.TryGainMemory(pettedSomeoneThought);
    }
}
```

**社交关系更新（双向机制）** ([L103-L120](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobDriver_SeekPetting.cs#L103-L120)):

```csharp
private void UpdateRelationship()
{
    // 基础关系增加
    int opinionOffset = 10;

    // 希尔薇低心情时额外加成 (< 30%)
    float? moodLevel = pawn.needs?.mood?.CurLevelPercentage;
    if (moodLevel.HasValue && moodLevel.Value < 0.30f)
    {
        opinionOffset += 5;  // 总计 +15
    }

    // 双向更新：目标对希尔薇的好感
    ApplySocialThought(TargetPawn, pawn, SylvieDefNames.Thought_PettedMe_Social, opinionOffset);
    
    // 双向更新：希尔薇对目标的好感
    ApplySocialThought(pawn, TargetPawn, SylvieDefNames.Thought_WasPetted_Social, opinionOffset);
}
```

**社交想法应用方法** ([L125-L141](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobDriver_SeekPetting.cs#L125-L141)):

```csharp
private void ApplySocialThought(Pawn recipient, Pawn targetPawn, string thoughtDefName, int opinionOffset)
{
    ThoughtDef? thoughtDef = DefDatabase<ThoughtDef>.GetNamed(thoughtDefName, false);
    if (thoughtDef == null) return;

    Thought_MemorySocial? socialThought = ThoughtMaker.MakeThought(thoughtDef) as Thought_MemorySocial;
    if (socialThought != null)
    {
        socialThought.otherPawn = targetPawn;
        socialThought.opinionOffset = opinionOffset;
        recipient.needs?.mood?.thoughts?.memories?.TryGainMemory(socialThought);
    }
}
```

#### SylvieSeekPettingTracker

**文件位置**: `Source/Components/SylvieSeekPettingTracker.cs`

**职责**: 继承 `GameComponent`，管理每个希尔薇的冷却状态

**核心数据结构** ([L14-L16](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Components/SylvieSeekPettingTracker.cs#L14-L16)):

```csharp
public class SylvieSeekPettingTracker : GameComponent
{
    private Dictionary<Pawn, int> lastPettingTicks = new Dictionary<Pawn, int>();
    private List<Pawn> pawnKeys = new List<Pawn>();      // 用于存档
    private List<int> tickValues = new List<int>();      // 用于存档
```

**核心方法**:

| 方法 | 位置 | 功能 |
|------|------|------|
| `GetLastPettingTick(Pawn)` | [L29-L32](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Components/SylvieSeekPettingTracker.cs#L29-L32) | 获取上次抚摸时间，-1表示从未抚摸 |
| `SetLastPettingTick(Pawn, int)` | [L37-L40](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Components/SylvieSeekPettingTracker.cs#L37-L40) | 记录抚摸时间 |
| `IsInCooldown(Pawn)` | [L45-L50](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Components/SylvieSeekPettingTracker.cs#L45-L50) | 检查是否在冷却期 |
| `GetCooldownRemaining(Pawn)` | [L56-L64](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Components/SylvieSeekPettingTracker.cs#L56-L64) | 获取剩余冷却时间 |
| `ExposeData()` | [L66-L70](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Components/SylvieSeekPettingTracker.cs#L66-L70) | 存档/读档支持 |

**冷却检查实现**:

```csharp
public bool IsInCooldown(Pawn pawn)
{
    int lastTick = GetLastPettingTick(pawn);
    if (lastTick < 0) return false;
    return Find.TickManager.TicksGame - lastTick < CooldownTicks;  // 15000 ticks
}
```

**存档机制技术细节** ([L66-L70](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Components/SylvieSeekPettingTracker.cs#L66-L70)):

```csharp
public override void ExposeData()
{
    base.ExposeData();
    Scribe_Collections.Look(
        ref lastPettingTicks,           // 要保存的字典
        "lastPettingTicks",             // 存档键名
        LookMode.Reference,             // Pawn使用Reference模式
        LookMode.Value,                 // tick使用Value模式
        ref pawnKeys,                   // 临时keys列表
        ref tickValues                  // 临时values列表
    );
}
```

**存档机制说明**:
- 使用 `Scribe_Collections.Look` 保存 `Dictionary<Pawn, int>`
- `LookMode.Reference` - Pawn作为引用保存（RimWorld自动处理引用关系）
- `LookMode.Value` - tick作为原始值保存
- 需要两个临时列表 `pawnKeys` 和 `tickValues` 作为序列化辅助

#### XML配置

**JobDef** (`Defs/Jobs/Sylvie_SeekPettingJobDefs.xml`):
```xml
<JobDef>
  <defName>Sylvie_SeekPetting</defName>
  <driverClass>SylvieMod.JobDriver_SeekPetting</driverClass>
  <reportString>Sylvie_SeekPetting_JobReport</reportString>
  <casualInterruptible>true</casualInterruptible>
</JobDef>
```

**ThoughtDefs** (`Defs/Thoughts/Sylvie_SeekPettingThoughts.xml`):

| DefName | 类型 | 效果 | 持续时间 |
|---------|------|------|----------|
| `Sylvie_WasPetted` | `Thought_Memory` | +10心情 | 16小时 |
| `Sylvie_PettedSomeone` | `Thought_Memory` | +6/+8心情（2阶段） | 12小时 |
| `Sylvie_PettedMe_Social` | `Thought_MemorySocial` | +10好感度 | 10天 |
| `Sylvie_WasPetted_Social` | `Thought_MemorySocial` | +10好感度 | 10天 |

**GameComponentDef** (`Defs/GameComponents/Sylvie_GameComponents.xml`):
```xml
<GameComponentDef>
  <defName>SylvieSeekPettingTracker</defName>
  <componentClass>SylvieMod.SylvieSeekPettingTracker</componentClass>
</GameComponentDef>
```

#### 关键技术点

1. **种族安全**: 多处检查确保只有希尔薇种族能触发和执行（[JobGiver L35](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobGiver_SeekPetting.cs#L35), [JobDriver L60](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobDriver_SeekPetting.cs#L60)）
2. **可空性处理**: 使用 C# 8.0 可空引用类型 (`#nullable enable`)
3. **常量管理**: 使用 `SylvieDefNames` 集中管理 Def 名称
4. **存档兼容性**: 使用 `Scribe_Collections.Look` 实现字典的存档/读档
5. **社交关系双向更新**: 同时更新双方对彼此的好感度（[L116-L119](file:///d:/LongYinHaHa/VSCode/Rimworld/SylvieRace/Source/Jobs/JobDriver_SeekPetting.cs#L116-L119)）
6. **概率检查机制**: 每小时20%概率，避免过于频繁的检查
7. **目标评分算法**: 综合考虑好感度、心情因素

## 代码架构原则

### 单一职责原则
每个类只负责一件事：
- `SylviePawnGenerator` - 只负责生成 Pawn
- `SylvieHediffManager` - 只负责 Hediff 管理
- `SylvieGameComponent` - 只负责游戏状态管理和事件触发
- `SylvieCooldownTracker` - 只负责远程武器冷却状态跟踪
- `SylvieCooldownOverlayComp` - 只负责冷却叠加层（汗液、弹匣、子弹）渲染
- `SylvieCatEarComp` - 只负责研究时猫耳动画渲染

### 开闭原则
- 使用 `SylvieDefNames` 常量类集中管理所有 Def 名称
- 使用 `SylvieConstants` 常量类集中管理所有数值常量
- 添加新 Def 或修改数值只需修改一处

### 依赖倒置原则
- 高层模块（如 `SylvieGameComponent`）通过抽象接口依赖低层模块
- 使用 RimWorld 的 `GetComp<T>()` 模式获取组件，避免直接依赖具体实现

### 常量提取模式

**原则**：将代码中的魔法数字提取到集中管理的常量类中。

**关键常量值**（来自 SylvieConstants.cs）：
| 常量 | 值 | 说明 |
|------|-----|------|
| DefaultAnimationDuration | 30 | 默认动画帧持续时间（ticks，0.5秒）|
| ResearchAnimationFrameCount | 8 | 研究动画帧数 |
| CheckIntervalTicks | 2500 | 游戏组件检查间隔（ticks，1游戏小时）|
| InitialEventDelayTicks | 5000 | 初始事件延迟（ticks，2游戏小时）|
| HediffDelayTicks | 300000 | Hediff添加延迟（ticks，5游戏日）|
| NurseHealIntervalTicks | 600 | 护士服治疗间隔（ticks，10秒）|
| CatEarRenderLayer | 74f | 猫耳渲染层级（头发62之上，头盔75+之下）|
| SweatRenderLayer | 61.1f | 汗液渲染层级 |
| MagazineRenderLayer | 61.2f | 弹匣渲染层级 |
| BulletRenderLayer | 61.3f | 子弹渲染层级 |
| NurseHealAmount | 0.01f | 护士服治疗效果 |
| NurseHealThreshold | 0.99f | 护士服治疗阈值 |

**示例**：
```csharp
// File: Example - Before Refactoring
#nullable enable

// 重构前（魔法数字分散在代码中）
if (durationPerFrame <= 0) durationPerFrame = 30;  // 30 是什么含义？

// File: Example - After Refactoring
#nullable enable

// 重构后（使用命名常量）
if (durationPerFrame <= 0) durationPerFrame = SylvieConstants.DefaultAnimationDuration;
```

**好处**：
1. 提高代码可读性 - 常量名称说明其用途
2. 便于维护 - 修改只需改一处
3. 避免错误 - 防止不同地方使用不同值

### 辅助方法封装模式

**原则**：将重复的判断逻辑封装为辅助方法。

**示例**：
```csharp
// File: Source/Core/SylvieDefNames.cs
#nullable enable

// 重构前（分散的字符串比较）
if (pawn?.def?.defName == "Sylvie_Race") { ... }

// 重构后（使用辅助方法）
if (SylvieDefNames.IsSylvieRace(pawn)) { ... }
```

**SylvieDefNames 辅助方法**：
- `IsSylvieRace(Pawn?)` - 检查 Pawn 是否为希尔薇种族
- `IsSylvieRace(ThingDef?)` - 检查 ThingDef 是否为希尔薇种族

**好处**：
1. 统一种族判断逻辑
2. 类型安全 - 编译器检查参数类型
3. 可空安全 - 辅助方法内部处理 null 检查
4. 便于重构 - 修改判断逻辑只需改一处

### 可空性处理最佳实践

项目启用了 `#nullable enable`，遵循以下模式：

**1. 早期返回模式（Guard Clause）**
```csharp
// File: Source/Patches/Patch_ResearchAnimation.cs
#nullable enable

Pawn? pawn = __instance.parent as Pawn;
if (pawn == null) return;
if (!SylvieDefNames.IsSylvieRace(pawn)) return;
```

**2. 空值传播运算符**
```csharp
// File: Source/Patches/Patch_ResearchAnimation.cs
#nullable enable

// 安全地访问可能为 null 的属性链
if (__instance?.animationDef?.defName != "Sylvie_ResearchAnimation") 
    return true;
```

**3. TryGetValue 模式**
```csharp
// File: Source/Patches/Patch_ResearchAnimation.cs
#nullable enable

if (!animationToPawn.TryGetValue(__instance, out var pawn) || pawn == null)
    return true;
```

**4. XML 注入字段标记**
```csharp
// File: Source/Hediffs/CompNurseHeal.cs
#nullable enable

public class SylvieRace_CompProperties_NurseHeal : CompProperties
{
    /// <summary>
    /// 冷却时间（单位：ticks）。默认 5000 ticks = 2 游戏小时
    /// </summary>
    public int cooldownTicks = 5000;
    
    /// <summary>
    /// 使用后添加的麻痹 Hediff。XML 注入字段
    /// </summary>
    public HediffDef paralysisHediff = null!;
}
```

**5. 可空返回值**
```csharp
// File: Source/Core/SylvieDefNames.cs
#nullable enable

public static HediffDef? Hediff_InitialTraumaDef => 
    HediffDef.Named(Hediff_InitialTrauma);
```

## 编译配置

**项目文件**: `Source/SylvieRace.csproj`
- 目标框架：.NET Framework 4.7.2 (`net472`)
- 输出类型：Library (DLL)
- 语言版本：latest (C# 最新特性)
- 输出路径：`..\1.6\Assemblies\`
- 程序集名称：`SylvieRace.dll`
- 根命名空间：`SylvieMod`
- 目标框架不附加到输出路径：`true`

**DLL 引用路径**:
游戏 DLL 文件位于工作区的 `GameDll/` 目录：
- `0Harmony.dll` - Harmony 2.x 补丁框架
- `Assembly-CSharp.dll` - RimWorld 核心程序集
- `UnityEngine.CoreModule.dll` - Unity 核心模块

**模组 DLL 引用**:
- `FacialAnimation.dll` - [NL] Facial Animation 模组程序集（用于瞄准动画和研究动画系统）
  - 路径：`mod2cite/[NL] Facial Animation - WIP/1.6/Assemblies/FacialAnimation.dll`

**编译命令**:
```bash
cd SylvieRace/Source
dotnet build --configuration Release
```

**编译输出**:
- 输出文件：`SylvieRace/1.6/Assemblies/SylvieRace.dll`
- 依赖项不复制到输出目录（`CopyLocalLockFileAssemblies: false`）

## 可空性处理

项目启用了 `#nullable enable`，遵循以下可空性处理模式：

### 1. XML 注入字段标记

对于通过 XML 配置注入的字段（如 `CompProperties` 中的 `HediffDef`），使用 `= null!` 标记：

```csharp
// File: Source/Hediffs/CompNurseHeal.cs
#nullable enable

public class SylvieRace_CompProperties_NurseHeal : CompProperties
{
    /// <summary>
    /// 冷却时间（单位：ticks）。默认 5000 ticks = 2 游戏小时
    /// </summary>
    public int cooldownTicks = 5000;
    
    /// <summary>
    /// 使用后添加的麻痹 Hediff。XML 注入字段，运行时赋值
    /// </summary>
    public HediffDef paralysisHediff = null!;
}
```

这告诉编译器该字段会在运行时由 RimWorld 的 XML 反序列化系统赋值。

### 2. 可空返回值

对于可能返回 null 的 Def 查找操作，使用可空返回类型：

```csharp
// File: Source/Core/SylvieDefNames.cs
#nullable enable

public static HediffDef? Hediff_InitialTraumaDef => 
    DefDatabase<HediffDef>.GetNamedSilentFail(Hediff_InitialTrauma);
```

### 3. 空值检查模式

| 模式 | 使用场景 | 示例 |
|------|----------|------|
| 早期返回 | 参数/变量为 null 时直接返回 | `if (pawn == null) return;` |
| 空值传播 | 安全访问可能为 null 的属性链 | `__instance?.animationDef?.defName` |
| TryGetValue | 字典查找 | `dict.TryGetValue(key, out var value)` |
| 辅助方法 | 封装重复判断逻辑 | `SylvieDefNames.IsSylvieRace(pawn)` |

### 4. 常见可空性警告处理

| 警告代码 | 含义 | 解决方案 |
|----------|------|----------|
| CS8602 | 可能解引用 null | 添加 null 检查或使用 `?.` 运算符 |
| CS8600 | 将 null 字面量转换为非空类型 | 使用 `= null!` 标记或添加 null 检查 |
| CS8603 | 可能返回 null | 将返回类型标记为可空 `Type?` |
| CS8618 | 非空字段未初始化 | 在构造函数中初始化或使用 `= null!` |

## 注意事项

1. **服装种族限制**：使用 `apparel.tags` + `PawnKindDef.apparelTags` 机制，同时使用 `apparel.raceRestriction` 限制非希尔薇种族穿戴
2. **服装不可制作**：使用 `ApparelBase` + `recipeMaker IsNull="True"` 禁用缝纫台配方
3. **服装储存区识别**：使用自定义 `thingCategories: SylvieRace_Apparel`
4. **GameComponent 自动注册**：无需手动注册，RimWorld 会自动实例化
5. **动态表情目录结构**：Defs 和 Patches 必须在 mod 根目录下
6. **defName 命名规范**：所有 defName 使用 `SylvieRace_` 或 `Sylvie_` 前缀
7. **翻译系统**：Defs 中使用英文，中文翻译通过 Languages 目录注入
8. **希尔薇唯一生成**：只能通过 `Sylvie_ArrivalEvent` 事件生成
9. **服装层级配置**：泳装和创口贴使用 Belt 层（配件层），可与其他服装同时穿戴
10. **动画 raceName 使用方式（重要！）**:
    - **TypeDef 需要添加 `raceName` 限制**（如 EyeType、MouthType、CooldownOverlayType 等）
    - **动画定义（FaceAnimationDef）不应添加 `raceName` 限制**
    - 如果动画有 `raceName` 限制，而 TypeDef 也有 `raceName` 限制，会导致 FA 框架匹配冲突，引发 NullReferenceException
    - 正确做法：TypeDef 有 raceName，动画无 raceName，让 FA 的默认动画可以正常应用
    - 详见"种族限制机制"章节

### raceName 使用原则详解

**需要添加 `raceName` 的 TypeDef（必须）**：
| TypeDef 类型 | 示例 | 说明 |
|-------------|------|------|
| BrowTypeDef | Sylvie_BrowNormal | 眉毛类型定义 |
| EyeballTypeDef | Sylvie_EyeNormal | 眼睛/眼球类型定义 |
| MouthTypeDef | Sylvie_MouthNormal | 嘴巴类型定义 |
| LidTypeDef | Sylvie_LidNormal | 眼睑类型定义 |
| LidOptionTypeDef | Sylvie_LidOptionNormal | 眼睑选项类型定义 |
| HeadTypeDef | Sylvie_Head | 头部类型定义 |
| SkinTypeDef | Sylvie_SkinRightEye, Sylvie_SkinLeftChin | 皮肤类型定义 |
| EmotionTypeDef | Sylvie_EmotionNormal | 表情类型定义 |

**不应添加 `raceName` 的定义**：
| 定义类型 | 示例 | 说明 |
|---------|------|------|
| FaceAnimationDef | Sylvie_AimingAnimation, Sylvie_IngestAnimation, Sylvie_CooldownAnimation, Sylvie_ResearchAnimation | 动画定义 |

**原因说明**：
- TypeDef 的 `raceName` 用于告诉 FA 框架该类型属于哪个种族
- FaceAnimationDef 是动画逻辑定义，通过 `targetJobs` 和 `priority` 与 TypeDef 关联
- 如果 FaceAnimationDef 也添加 `raceName`，会导致 FA 框架在匹配动画时产生冲突，引发 NullReferenceException

## 故障排查

### 冷却动画组件朝向问题

#### 问题描述
冷却动画组件（汗液、弹匣、子弹）在 West 朝向时显示异常，或 North 朝向时贴图缺失。

#### 根本原因
1. **West 朝向翻转问题**：错误地使用 `MeshPool.plane10` 配合负 scale 来翻转，这样不会正确翻转 UV
2. **North 贴图判断问题**：某些组件可能没有独立的 north 贴图

#### 解决方案
**正确的朝向处理方式**：
```csharp
// File: Source/Components/SylvieCooldownOverlayComp.cs
#nullable enable

// 获取 Pawn 当前朝向
Rot4 rot = Pawn.Rotation;

// 使用 Graphic.MeshAt(rot) 获取正确的 mesh（自动处理 West 朝向翻转）
Mesh mesh = graphic.MeshAt(rot);

// 使用 Graphic.MatAt(rot) 获取对应朝向的材质
Material mat = graphic.MatAt(rot);

// 渲染
Graphics.DrawMesh(mesh, matrix, mat, 0);
```

**North 贴图智能判断**：
```csharp
// File: Source/Components/SylvieCooldownOverlayComp.cs
#nullable enable

/// <summary>
/// 检查 Graphic 是否有独立的 north 贴图
/// </summary>
private bool HasNorthTexture(Graphic graphic)
{
    // 如果 MatNorth 和 MatSouth 是同一材质，说明没有独立的 north 贴图
    return graphic.MatNorth != graphic.MatSouth;
}

// 使用示例（汗液组件没有 north 贴图，在 north 朝向时不显示）
if (rot == Rot4.North)
{
    // sweat 没有 north 贴图，不显示
}
else
{
    Graphic sweatGraphic = SweatGraphics[sweatFrame - 1];
    Material mat = sweatGraphic.MatAt(rot);
    Mesh mesh = sweatGraphic.MeshAt(rot);
    if (mat != null)
    {
        Graphics.DrawMesh(mesh, matrix, mat, 0);
    }
}
```

#### 朝向行为表
| 朝向 | MeshAt 行为 | MatAt 行为 | 说明 |
|------|-------------|------------|------|
| South | 正常 mesh | 返回 south 材质 | 正面显示 |
| East | 正常 mesh | 返回 east 材质 | 右侧显示 |
| West | **翻转 mesh** | 返回 east 材质 | 左侧显示（自动翻转） |
| North | 正常 mesh | 返回 north 材质 | 背面显示 |

### 动画参数被 FA 默认动画覆盖

#### 问题描述
Sylvie 动画的某些参数（如眼睛闭合、嘴巴形状）被 FA 默认动画覆盖，显示效果不符合预期。

#### 根本原因
FA 使用动画累积机制，如果 Sylvie 动画的某帧没有指定某个参数，FA 原版的对应参数可能被应用。

#### 解决方案
确保 Sylvie 动画的每一帧都包含所有关键参数：

```xml
<li>
  <duration>30</duration>
  <browShapeDef>confused</browShapeDef>
  <eyeballShapeDef>normal</eyeballShapeDef>
  <mouthShapeDef>tongue_out</mouthShapeDef>
  <lidShapeDef>normal</lidShapeDef>          <!-- 必须！ -->
  <lidOptionShapeDef>sylvie_sweat1</lidOptionShapeDef>
</li>
```

**必须指定的参数**：
- `browShapeDef` - 眉毛形状
- `eyeballShapeDef` - 眼球形状
- `mouthShapeDef` - 嘴巴形状
- `lidShapeDef` - 眼皮形状（即使是 normal 也要指定）
- `lidOptionShapeDef` - 眼皮选项（如流汗效果）

**Priority 设置**：
确保 Sylvie 动画的 priority 高于 FA 默认动画：
- FA Ingest: 10 → Sylvie Ingest: 11
- FA Lovin: 10400 → Sylvie Lovin: 10600

详见"FA 动画混合机制"章节。

### 动画 raceName 冲突导致的 NullReferenceException

#### 问题描述
游戏加载或运行时出现 NullReferenceException，堆栈跟踪涉及 Facial Animation 框架的动画匹配逻辑。

#### 根本原因
FaceAnimationDef 错误地添加了 `raceName` 限制，而对应的 TypeDef 也有 `raceName` 限制，导致 FA 框架匹配冲突。

#### 解决方案
**检查所有 FaceAnimationDef 定义**，确保不包含 `<raceName>` 节点：

```xml
<!-- 正确：FaceAnimationDef 不应有 raceName -->
<FacialAnimation.FaceAnimationDef>
  <defName>Sylvie_AimingAnimation</defName>
  <targetJobs>
    <li>AttackStatic</li>
  </targetJobs>
  <priority>10300</priority>
</FacialAnimation.FaceAnimationDef>

<!-- 错误：FaceAnimationDef 不应添加 raceName -->
<FacialAnimation.FaceAnimationDef>
  <defName>Sylvie_AimingAnimation</defName>
  <raceName>Sylvie_Race</raceName>  <!-- 删除这行！ -->
  ...
</FacialAnimation.FaceAnimationDef>
```

**同时确保 TypeDef 有 raceName**：
```xml
<!-- TypeDef 必须有 raceName -->
<FacialAnimation.BrowTypeDef>
  <defName>Sylvie_BrowNormal</defName>
  <raceName>Sylvie_Race</raceName>  <!-- 必须保留 -->
  ...
</FacialAnimation.BrowTypeDef>
```

**使用 Patch_FaceAnimationDef_IsSame**：
确保 `Patch_FaceAnimationDef_IsSame.cs` 补丁已正确加载，这是实现 Sylvie 专属动画与 FA 原版动画共存的关键。

详见"种族限制机制"章节。

### Belt 层服装渲染顺序问题

#### 问题描述
Belt 层服装（创口贴、泳衣）渲染在其他服装之上或之下，显示顺序不正确。

#### 根本原因
Belt 层服装默认渲染层级可能与预期不符，需要显式配置 `drawData` 来指定渲染层级。

#### 解决方案
在服装定义的 `apparel` 节点中添加 `drawData` 配置：

```xml
<apparel>
  <wornGraphicData>
    <renderUtilityAsPack>false</renderUtilityAsPack>
  </wornGraphicData>
  <drawData>
    <dataNorth>
      <layer>15</layer>
    </dataNorth>
    <dataSouth>
      <layer>15</layer>
    </dataSouth>
    <dataEast>
      <layer>15</layer>
    </dataEast>
    <dataWest>
      <layer>15</layer>
    </dataWest>
  </drawData>
</apparel>
```

**关键配置说明**：
- `renderUtilityAsPack: false` - 确保 Belt 层服装正常渲染为身体服装而非背包
- `layer: 15` - 渲染层级，位于 Body (0-10) 和 Apparel root (20) 之间
- `layer` 是**绝对层级值**，不是相对于 defaultLayer 的偏移

**渲染层级参考**：
| 节点 | 层级 |
|------|------|
| Body | 0-10 |
| Body tattoo | 2 |
| Wounds - pre apparel | 8 |
| **Belt 服装（配置后）** | **15** |
| Apparel root (body) | 20 |
| OnSkin/Middle/Shell 服装 | 20+ |

**冷却动画渲染层级参考**（来自 `SylvieConstants.cs`）：
| 组件 | 层级 | 说明 |
|------|------|------|
| 基础层 | 61 | Pawn 基础渲染层 |
| 汗液 | 61.1f | `SweatRenderLayer` - 位于胡子(60)和头发(62)之间 |
| 弹匣 | 61.2f | `MagazineRenderLayer` |
| 子弹 | 61.3f | `BulletRenderLayer` |
| 猫耳 | 74f | `CatEarRenderLayer` - 位于头发(62)之上，头盔(75+)之下 |

## 服装系统技术细节

### 层级配置
| 服装类型 | layers | 说明 |
|---------|--------|------|
| 连衣裙/套装 | OnSkin, Middle | 覆盖躯干、手臂、腿部 |
| 下装 | OnSkin | 仅覆盖腿部 |
| 泳装/创口贴 | Belt | 配件层，可与其他服装同时穿戴 |
| 披肩 | Shell | 最外层 |
| 头饰 | Overhead | 头部层 |

### Belt 层服装渲染顺序
Belt 层服装（创口贴、泳衣）需要特殊配置以确保正确的渲染顺序：

**问题**：Belt 层服装默认渲染在 OnSkin 服装之上

**解决方案**：使用 `apparel.drawData` 配置渲染层级

```xml
<!-- Apparel_Special.xml -->
<apparel>
  <wornGraphicData>
    <renderUtilityAsPack>false</renderUtilityAsPack>
  </wornGraphicData>
  <drawData>
    <dataNorth>
      <layer>15</layer>
    </dataNorth>
    <dataSouth>
      <layer>15</layer>
    </dataSouth>
    <dataEast>
      <layer>15</layer>
    </dataEast>
    <dataWest>
      <layer>15</layer>
    </dataWest>
  </drawData>
</apparel>
```

**关键配置说明**：
- `renderUtilityAsPack: false` - 确保 Belt 层服装正常渲染为身体服装而非背包
- `drawData` - RimWorld 的渲染数据配置，包含方向性层级设置
- `dataNorth/dataSouth/dataEast/dataWest` - 各方向的渲染数据
- `layer: 15` - 渲染层级，位于 Body (0-10) 和 Apparel root (20) 之间

**重要说明**：
- `layer: 15` 是**绝对层级值**，不是相对于 defaultLayer 的偏移
- 根据 `DrawData.LayerForRot()` 方法逻辑：如果 `dataNorth.layer` 有值，直接返回该值
- 这会使 Belt 服装渲染在 Body (0-10) 之上，Apparel root (20) 之下

**注意**：`wornGraphicData` 中的 `north/south/east/west` 节点没有 `layer` 字段，正确的配置位置是 `apparel.drawData`

**渲染层级参考**：
| 节点 | 层级 |
|------|------|
| Body | 0-10 |
| Body tattoo | 2 |
| Wounds - pre apparel | 8 |
| **Belt 服装（配置后）** | **15** |
| Apparel root (body) | 20 |
| OnSkin/Middle/Shell 服装 | 20+ |

**已排除的方案**：
1. `renderNodeProperties` + `baseLayer` - 节点添加到错误的父节点，不按预期工作
2. `renderNodeProperties` + `parentTagDef` + `baseLayer` - 仍然不按预期工作
3. Harmony 补丁修改 `PawnRenderNodeWorker.LayerFor` - 补丁未生效，存在其他对象可以成功，但考虑到能不写cs就不写cs，放弃此方法

最终使用 `wornGraphicData` 的方向性 `layer` 配置，这是 RimWorld 原生的渲染层级覆盖机制。

### 种族限制实现
使用 Humanoid Alien Races (HAR) 框架原生的 `raceRestriction` 功能：

**配置位置**: `Defs/Races/Sylvie_Race.xml`

```xml
<alienRace>
  <raceRestriction>
    <apparelList>
      <!-- 特殊服装 (3件) -->
      <li>SylvieRace_Bandaid</li>           <!-- 创口贴 -->
      <li>SylvieRace_Swimsuit</li>          <!-- 泳装 -->
      <li>SylvieRace_Shawl</li>             <!-- 披肩 -->
      <!-- 下装 (3件) -->
      <li>SylvieRace_ElegantClothingPants</li>  <!-- 典雅服装下装 -->
      <li>SylvieRace_StudentUniformPants</li>   <!-- 学生服下装 -->
      <li>SylvieRace_SuitsPants</li>            <!-- 西装下装 -->
      <!-- 头饰 (1件) -->
      <li>SylvieRace_SpringFestivalHeadwear</li>  <!-- 新春头饰 -->
      <!-- 连衣裙 (3件) -->
      <li>SylvieRace_PurpleDress</li>       <!-- 紫色连衣裙 -->
      <li>SylvieRace_BlueDress</li>         <!-- 蓝色连衣裙 -->
      <li>SylvieRace_FloralDress</li>       <!-- 连衣花裙 -->
      <!-- 套装 (10件) -->
      <li>SylvieRace_Finally</li>           <!-- 终末地服装 -->
      <li>SylvieRace_FineClothing</li>      <!-- 黑色连衣裙 -->
      <li>SylvieRace_Kimono</li>            <!-- 和服 -->
      <li>SylvieRace_MaidOutfit</li>        <!-- 女仆装 -->
      <li>SylvieRace_ElegantClothing</li>   <!-- 典雅服装 -->
      <li>SylvieRace_HeavyMaid</li>         <!-- 重装女仆 -->
      <li>SylvieRace_StudentUniform</li>    <!-- 学生服 -->
      <li>SylvieRace_Suits</li>             <!-- 西装 -->
      <li>SylvieRace_Cheongsam</li>         <!-- 旗袍 -->
      <li>SylvieRace_SpringFestivalWedding</li>  <!-- 新春花嫁 -->
      <li>SylvieRace_Steampunk</li>         <!-- 蒸汽朋克 -->
      <!-- 护士服 (1件) -->
      <li>SylvieRace_NurseUniform</li>      <!-- 护士服 -->
    </apparelList>
  </raceRestriction>
</alienRace>
```

**工作原理**：
- HAR 框架会在穿戴检查时自动验证种族限制
- 只有在 `apparelList` 中列出的服装才能被该种族穿戴
- 其他种族尝试穿戴时会收到"无法穿戴"的提示

**优势**：
- 使用框架原生功能，兼容性更好
- 无需维护 Harmony 补丁
- 与其他使用 HAR 的模组兼容

### 材料与颜色
- 服装保留 `stuffCategories` 和 `costStuffCount`，允许使用不同材料制作
- 服装颜色会随材料变化（RimWorld 原版机制）
- 如需固定颜色，需移除 `stuffCategories` 并使用固定的 `costList`

## 研究动画系统

### 功能概述
研究时显示专属动态表情动画（8帧循环），配合动态猫耳动画：
- **8帧眼睛动画**：白眼正视 → 白眼看右 → 白眼看左 → 黑圈眼向下
- **猫耳动画**：根据眼睛动画帧同步切换（猫耳1/猫耳2）
- **嘴巴形状**：全程 w_mouth（W形嘴）
- **眉毛形状**：全程 normal

### 文件结构
```
Defs/FacialAnimation/
├── Animations/
│   └── ResearchAnimation.xml    # 研究动画定义（8帧）
├── Shapes/
│   ├── EyeShapeEx_Research.xml  # 研究专用眼睛形状定义
│   └── MouthShapeEx_Research.xml # 研究专用嘴巴形状定义
└── Types/
    └── EyeType.xml              # 眼睛类型定义（需要 raceName）

Textures/Things/Pawn/Sylvie/
├── Ears/                        # 猫耳贴图
│   ├── cat_ear1_south.png
│   ├── cat_ear1_east.png
│   ├── cat_ear2_south.png
│   └── cat_ear2_east.png
└── Eyes/                        # 研究专用眼睛贴图
    ├── white_eye_south.png
    ├── white_eye_east.png
    ├── white_eye_right_south.png
    ├── white_eye_left_south.png
    ├── circle_eye_down_south.png
    └── ...
```

### 动画帧序列（8帧）
| 帧 | duration | 眉毛 | 嘴巴 | 眼睛 | 情绪 | 猫耳 |
|----|----------|------|------|------|------|------|
| 0 | 30 | normal | w_mouth | white_eye | - | 猫耳1 |
| 1 | 30 | normal | w_mouth | white_eye | - | 猫耳2 |
| 2 | 30 | normal | w_mouth | white_eye_right | - | 猫耳1 |
| 3 | 30 | normal | w_mouth | white_eye_right | - | 猫耳2 |
| 4 | 30 | normal | w_mouth | white_eye_left | - | 猫耳1 |
| 5 | 30 | normal | w_mouth | white_eye_left | - | 猫耳2 |
| 6 | 30 | normal | w_mouth | circle_eye_down | gloomy | 猫耳2 |
| 7 | 30 | normal | w_mouth | circle_eye_down | gloomy | 猫耳2 |

### 猫耳帧映射规则
```csharp
// 来自 Patch_ResearchAnimation.cs 第98-101行
// 原始帧 0, 2, 4 -> 猫耳1
// 原始帧 1, 3, 5, 6, 7 -> 猫耳2
int earFrame = (originalFrameIndex == 0 || originalFrameIndex == 2 || originalFrameIndex == 4) ? 0 : 1;
```

### 关键配置
```xml
<!-- Defs/FacialAnimation/Animations/ResearchAnimation.xml -->
<FacialAnimation.FaceAnimationDef>
  <defName>Sylvie_ResearchAnimation</defName>
  <animationFrames>
    <!-- 8帧，每帧30 ticks，共240 ticks（4秒）一个循环 -->
    <li>
      <duration>30</duration>
      <browShapeDef>normal</browShapeDef>
      <mouthShapeDef>w_mouth</mouthShapeDef>
      <eyeballShapeDef>white_eye</eyeballShapeDef>
    </li>
    <!-- ... 共8帧 ... -->
  </animationFrames>
  <roopIntervalMin>0</roopIntervalMin>
  <roopIntervalMax>30</roopIntervalMax>
  <targetJobs>
    <li>Research</li>
  </targetJobs>
  <priority>10003</priority>
  <applyWhenStandingOnly>true</applyWhenStandingOnly>
</FacialAnimation.FaceAnimationDef>
```

### 渲染层级
- **猫耳渲染层级**：`74f`（通过 `PawnRenderUtility.AltitudeForLayer(74)` 转换）
  - 位于头发（62）之上
  - 位于头盔（75+）之下

### 与猫耳同步机制
研究动画通过 `Patch_ResearchAnimation.cs` 实现与猫耳的同步：
1. 拦截 FA 的 `GetCurrentFrame` 方法
2. 计算当前动画帧索引
3. 根据原始帧索引（0-7）映射到猫耳帧（0-1）
4. 调用 `SylvieCatEarComp.SetCurrentEarFrame()` 设置猫耳
5. 研究结束时通过 `Patch_FacialAnimationControllerComp_CompTick` 隐藏猫耳

## 瞄准动画系统

### 技术实现
瞄准动画通过 Facial Animation 的 ShapeDef 实现，贴图整合到现有的 Normal 目录中。

### 重要原则
**不要创建新的 TypeDef！** 每个种族只需要一个 BrowTypeDef、一个 MouthTypeDef 和一个 LidOptionTypeDef。新形状的贴图应该添加到现有的 Normal 目录中。

### 文件结构
```
Defs/FacialAnimation/
├── BrowShapeEx.xml      # 瞄准眉毛形状定义（aiming, confused）
├── MouthShapeEx.xml     # 瞄准嘴巴形状定义
├── LidOptionShapeEx.xml # 准星形状定义
└── AimingAnimation.xml  # 瞄准动画定义

Textures/Things/Pawn/Sylvie/
├── Brows/Normal/Unisex/     # 所有眉毛形状贴图（包括 aiming）
├── Mouth/Normal/Unisex/     # 所有嘴巴形状贴图（包括 inverted_v, m_shape）
└── LidOptions/Normal/Unisex/ # 所有眼睑选项贴图（包括 crosshair1/2/3）
```

### 动画帧序列
| 帧 | 眉毛 | 嘴巴 | 准星 |
|----|------|------|------|
| 1 | angled（生气） | inverted_v（反V嘴） | crosshair1 |
| 2 | aiming（瞄准） | m_shape（M嘴） | crosshair2 |
| 3 | aiming（瞄准） | m_shape（M嘴） | crosshair3 |

### 关键配置
```xml
<FacialAnimation.FaceAnimationDef>
  <defName>Sylvie_AimingAnimation</defName>
  <targetJobs>
    <li>AttackStatic</li>  <!-- 远程攻击 Job -->
  </targetJobs>
  <priority>10300</priority>  <!-- 高于普通动画 -->
</FacialAnimation.FaceAnimationDef>
```

### 准星实现方案
准星使用 Facial Animation 的 `LidOptionType` 组件实现：
- `LidOptionType` 原本用于渲染眼睑上的额外效果（如眼泪）
- 通过 `lidOptionShapeDef` 在动画帧中切换显示不同的准星形状
- 贴图需要放到 `LidOptions/Normal/Unisex/` 目录中

### 贴图命名规范
Facial Animation 使用 `TypePath/Gender/shape_direction.png` 的命名规则：
- `aiming_south.png` - 正面瞄准眉毛
- `aiming_east.png` - 侧面瞄准眉毛
- `m_shape_south.png` - 正面M嘴
- `crosshair1_south.png` - 正面准星帧1

## 进食动画系统

### 功能概述
进食时显示专属动态表情动画，包括：
- 眼睛向下看（lookdown shape）
- 嘴巴咀嚼动画（open → eating1 → eating2 → eating3）
- 头部移动模拟（headOffset 动画）

### 文件结构
```
Defs/FacialAnimation/
├── MouthShapeEx.xml     # 进食嘴巴形状定义（eating1, eating2, eating3）
└── IngestAnimation.xml  # 进食动画定义

Textures/Things/Pawn/Sylvie/Mouth/Normal/Unisex/
├── eating1_south.png    # 进食嘴巴帧1（正面）
├── eating1_east.png     # 进食嘴巴帧1（侧面）
├── eating2_south.png    # 进食嘴巴帧2（正面）
├── eating2_east.png     # 进食嘴巴帧2（侧面）
├── eating3_south.png    # 进食嘴巴帧3（正面）
└── eating3_east.png     # 进食嘴巴帧3（侧面）
```

### 动画帧序列
| 帧 | duration | headOffset | 眼睛 | 嘴巴 | 说明 |
|----|----------|------------|------|------|------|
| 1 | 30 | (0.0,0,0.0025) | lookdown | open | 张嘴准备 |
| 2 | 40 | (0.0,0,0.0) | lookdown | eating1 | 咀嚼帧1 |
| 3 | 40 | (0.0,0,0) | lookdown | eating2 | 咀嚼帧2 |
| 4 | 40 | (0.0,0,0) | lookdown | eating3 | 咀嚼帧3 |

### 关键配置
```xml
<FacialAnimation.FaceAnimationDef>
  <defName>Sylvie_IngestAnimation</defName>
  <animationFrames>
    <li>
      <duration>30</duration>
      <headOffset>(0.0,0,0.0025)</headOffset>
      <eyeballShapeDef>lookdown</eyeballShapeDef>
      <mouthShapeDef>open</mouthShapeDef>
    </li>
    <!-- ... 其他帧 ... -->
  </animationFrames>
  <loopIntervalMin>0</loopIntervalMin>
  <loopIntervalMax>30</loopIntervalMax>
  <targetJobs>
    <li>Ingest</li>
  </targetJobs>
  <priority>11</priority>
  <applyWhenStandingOnly>true</applyWhenStandingOnly>
</FacialAnimation.FaceAnimationDef>
```

### headOffset 说明
`headOffset` 用于模拟进食时头部的移动：
- `(0.0, 0, 0.0025)` - 头部轻微抬起（准备进食）
- `(0.0, 0, 0.0)` - 头部回到正常位置（咀嚼）

### priority 设置
- FA 默认 Ingest 动画 priority 为 10
- Sylvie 进食动画 priority 设置为 11，确保覆盖默认动画

## 冷却动画系统

### 功能概述
远程武器冷却期间显示完整的装填动画，包括：
- **困惑眉毛**（confused）- 全程显示
- **向下看的眼睛**（lookdown）- 冷却期间通过代码动态替换眼球形状
- 汗液动画（3帧，根据冷却进度）
- 弹匣显示（全程）
- 子弹装填动画（投入1→投入2→投入3→N颗子弹，循环到5颗）

### 系统架构

```
┌─────────────────────────────────────────────────────────────┐
│                    冷却动画系统架构                          │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌─────────────────────┐    ┌─────────────────────────────┐ │
│  │ SylvieCooldownTracker│    │ SylvieCooldownOverlayComp  │ │
│  │   (ThingComp)        │    │   (ThingComp)              │ │
│  ├─────────────────────┤    ├─────────────────────────────┤ │
│  │ - IsInRangedCooldown │    │ - PostDraw()               │ │
│  │ - CooldownProgress   │    │ - 渲染汗液/弹匣/子弹        │ │
│  │ - GetSweatFrame()    │    │                            │ │
│  │ - GetBulletAnimation │    │                            │ │
│  └─────────────────────┘    └─────────────────────────────┘ │
│            │                              │                 │
│            └──────────────┬───────────────┘                 │
│                           │                                 │
│            ┌──────────────▼───────────────┐                 │
│            │  Patch_FaceAnimation_GetCurrentFrame │         │
│            │      (Harmony Patch)         │                 │
│            ├──────────────────────────────┤                 │
│            │ - 拦截 FA 的 GetCurrentFrame  │                 │
│            │ - 冷却期间返回冷却帧           │                 │
│            │ - browShapeDef: confused     │                 │
│            │ - eyeballShapeDef: lookdown  │                 │
│            └──────────────────────────────┘                 │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### 核心组件

#### SylvieCooldownTracker
**文件位置**: `Source/Components/SylvieCooldownTracker.cs`

ThingComp（组件），用于跟踪冷却状态和计算动画帧：
- `IsInRangedCooldown` - 是否处于远程武器冷却状态
- `CooldownProgress` - 冷却进度（0-1）
- `GetSweatFrame()` - 获取当前汗液帧（1-3）
- `GetBulletAnimationState()` - 获取子弹动画状态（投入帧，子弹数量）

#### SylvieCooldownOverlayComp
**文件位置**: `Source/Components/SylvieCooldownOverlayComp.cs`

ThingComp（组件），用于渲染冷却期间的叠加层：
- 汗液贴图（3帧）
- 弹匣贴图
- 子弹投入贴图（3帧）
- 子弹计数贴图（5帧）

**渲染流程**：
```
PostDraw()
  → 检查种族和冷却状态
  → 获取 headSizeFactor（Biotech DLC）
  → BaseHeadOffsetAt() 获取头部位置（已包含 sqrt(bodySizeFactor) 缩放）
  → GetFaceDrawOffset() 获取固定组件偏移（不缩放）
  → 计算 drawPos = Pawn.DrawPos + headOffset + faceOffset
  → 计算 drawScale = DrawScale * headSizeFactor（组件大小缩放）
  → 按顺序渲染：汗液(baseLayer=61) → 弹匣 → 子弹投入 → 子弹计数
  → 使用 Matrix4x4.TRS(drawPos, Quaternion.identity, drawScale) 进行变换
  → Graphics.DrawMesh 绘制
```

**缩放机制要点**：
- **组件偏移**：保持固定值，不随 `headSizeFactor` 缩放
- **组件大小**：按 `headSizeFactor` 缩放，与头部图形同步
- **头部位置**：`BaseHeadOffsetAt` 已包含 `sqrt(bodySizeFactor)` 缩放

### 渲染流程详解

```
┌────────────────────────────────────────┐
│           PostDraw 渲染流程            │
├────────────────────────────────────────┤
│                                        │
│  1. 检查 Pawn.def.defName == "Sylvie_Race" │
│     └─ 不匹配则直接返回                │
│                                        │
│  2. 获取 SylvieCooldownTracker         │
│     └─ 检查 IsInRangedCooldown         │
│                                        │
│  3. 获取 Pawn 朝向 rot = Pawn.Rotation │
│                                        │
│  4. 获取头部大小因子                   │
│     headSizeFactor = CurLifeStage.headSizeFactor ?? 1.0 │
│     (Biotech DLC 支持不同年龄段头部大小)  │
│     小孩: 0.5/0.75, 成年人: 1.0        │
│                                        │
│  5. 计算头部基础偏移                   │
│     headOffset = BaseHeadOffsetAt(rot) │
│     (已包含 sqrt(bodySizeFactor) 缩放)   │
│                                        │
│  6. 计算面部组件偏移（固定值，不缩放）   │
│     faceOffset = GetFaceDrawOffset()   │
│     (保持固定，确保相对位置不变)         │
│                                        │
│  7. 计算最终绘制位置                   │
│     drawPos = Pawn.DrawPos + headOffset + faceOffset    │
│     drawPos.y += 0.01f (层级调整)      │
│                                        │
│  8. 计算缩放矩阵（组件大小缩放）         │
│     drawScale = DrawScale * headSizeFactor              │
│     matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, drawScale)│
│                                        │
│  9. 渲染汗液 (3帧循环, baseLayer=61)   │
│     Mesh mesh = graphic.MeshAt(rot)    │
│     Material mat = graphic.MatAt(rot)  │
│     Graphics.DrawMesh(mesh, matrix, mat, 0)               │
│                                        │
│  10. 渲染弹匣 (全程显示，North需检查)  │
│     if (rot == North && !HasNorthTexture) skip            │
│                                        │
│  11. 渲染子弹投入动画 (3帧)            │
│                                        │
│  12. 渲染子弹计数 (1-5颗)              │
│                                        │
└────────────────────────────────────────┘
```

**缩放机制详解**：

| 步骤 | 元素 | 处理方式 | 缩放因子 | 说明 |
|------|------|----------|----------|------|
| 5 | 头部位置 | `BaseHeadOffsetAt(rot)` | `sqrt(bodySizeFactor)` | RimWorld 原版已处理 |
| 6 | 组件偏移 | `GetFaceDrawOffset()` | **无缩放** | 保持固定值 |
| 8 | 组件大小 | `Matrix4x4.TRS` | `headSizeFactor` | 与头部图形同步 |

**关键区别**：
- **组件偏移不缩放**：确保组件相对于头部的相对位置保持不变
- **组件大小缩放**：确保组件与头部图形大小同步变化
- **头部位置已缩放**：`BaseHeadOffsetAt` 返回的位置已经考虑了身体大小

### 朝向处理实现细节

**核心原理**：
- 使用 `Graphic.MeshAt(rot)` 获取正确的 mesh（会自动处理 West 朝向的翻转）
- 使用 `Graphic.MatAt(rot)` 获取对应朝向的材质
- 通过比较 `MatNorth` 和 `MatSouth` 判断是否有独立的 north 贴图

**代码实现**：
```csharp
// File: Source/Components/SylvieCooldownOverlayComp.cs
#nullable enable

// 获取 Pawn 当前朝向
Rot4 rot = Pawn.Rotation;

// 获取头部大小因子（Biotech DLC 支持不同年龄段）
float headSizeFactor = 1f;
if (ModsConfig.BiotechActive && Pawn.ageTracker.CurLifeStage.headSizeFactor.HasValue)
{
    headSizeFactor = Pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
}

// 计算头部基础偏移（已包含 bodySizeFactor 的平方根缩放）
Vector3 headOffset = Pawn.Drawer.renderer.BaseHeadOffsetAt(rot);

// 计算面部组件偏移（固定值，不缩放）
Vector3 faceOffset = GetFaceDrawOffset();

// 计算最终绘制位置
Vector3 drawPos = Pawn.DrawPos + headOffset + faceOffset;
drawPos.y += 0.01f; // 层级调整

// 使用 Graphic 的方法获取对应朝向的 mesh 和 material
Mesh mesh = graphic.MeshAt(rot);
Material mat = graphic.MatAt(rot);

// 计算缩放矩阵（组件大小按 headSizeFactor 缩放）
Vector3 drawScale = DrawScale * headSizeFactor;
Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, drawScale);

// 渲染
Graphics.DrawMesh(mesh, matrix, mat, 0);

// 智能判断是否有 north 贴图（用于决定是否渲染 North 朝向）
private bool HasNorthTexture(Graphic graphic)
{
    return graphic.MatNorth != graphic.MatSouth;
}
```

**朝向行为表**：

| 朝向 | MeshAt 行为 | MatAt 行为 | 说明 |
|------|-------------|------------|------|
| South | 正常 mesh | 返回 south 材质 | 正面显示 |
| East | 正常 mesh | 返回 east 材质 | 右侧显示 |
| West | **翻转 mesh** | 返回 east 材质 | 左侧显示（自动翻转） |
| North | 正常 mesh | 返回 north 材质 | 背面显示 |

**重要警告**：

**错误做法（不要这样做）**：
```csharp
// File: Example - Wrong Approach
#nullable enable

// 错误：使用 MeshPool.plane10 配合 scale 负值来翻转
// 这样不会正确翻转 UV，导致贴图显示错误！
Vector3 wrongScale = new Vector3(-headSizeFactor, 1f, headSizeFactor);
Matrix4x4 wrongMatrix = Matrix4x4.TRS(drawPos, Quaternion.identity, wrongScale);
Graphics.DrawMesh(MeshPool.plane10, wrongMatrix, mat, 0);
```

**正确做法**：
```csharp
// File: Source/Components/SylvieCooldownOverlayComp.cs
#nullable enable

// 正确：使用 Graphic.MeshAt(rot) 获取 mesh
// 它会自动处理 West 朝向的翻转，包括 UV 翻转
Mesh mesh = graphic.MeshAt(rot);
Material mat = graphic.MatAt(rot);
Vector3 drawScale = DrawScale * headSizeFactor;
Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, drawScale);
Graphics.DrawMesh(mesh, matrix, mat, 0);
```

**North 贴图智能判断**：

某些组件可能没有独立的 north 贴图（使用 south 贴图作为默认）。通过比较 `MatNorth` 和 `MatSouth` 可以判断：

```csharp
// File: Source/Components/SylvieCooldownOverlayComp.cs
#nullable enable

private bool HasNorthTexture(Graphic graphic)
{
    // 如果 MatNorth 和 MatSouth 是同一材质，说明没有独立的 north 贴图
    return graphic.MatNorth != graphic.MatSouth;
}

// 使用示例
if (rot == Rot4.North && !HasNorthTexture(magazineGraphic))
{
    // North 朝向且没有独立 north 贴图，跳过渲染
    return;
}
```

### 与 Facial Animation 集成

冷却动画通过 `Patch_FaceAnimation_GetCurrentFrame` 补丁与 FA 系统集成：

```csharp
// File: Source/Patches/Patch_Stance_Warmup.cs
#nullable enable

[HarmonyPatch(typeof(FaceAnimation), nameof(FaceAnimation.GetCurrentFrame))]
public static class Patch_FaceAnimation_GetCurrentFrame
{
    public static bool Prefix(FaceAnimation __instance, int tickGame, 
                            ref FaceAnimationDef.AnimationFrame? __result)
    {
        // 只处理 Sylvie_AimingAnimation
        if (__instance.animationDef.defName != "Sylvie_AimingAnimation") 
            return true;
        
        // ... 瞄准帧计算逻辑 ...
        
        // 冷却期间返回冷却帧
        else if (curStance is Stance_Cooldown cooldown)
        {
            __result = GetCooldownFrame();
            return false;  // 跳过原始方法
        }
        
        return true;
    }
    
    private static FaceAnimationDef.AnimationFrame GetCooldownFrame()
    {
        return new FaceAnimationDef.AnimationFrame
        {
            duration = 30,
            browShapeDef = ConfusedBrowDef,      // 困惑眉毛
            eyeballShapeDef = LookdownEyeballDef, // 向下看的眼球
            mouthShapeDef = CooldownMouthDef      // 冷却专用嘴巴
        };
    }
}
```

**关键点**：
- 通过 `eyeballShapeDef` 替换眼球形状为 `lookdown`（在代码中动态设置）
- 通过 `browShapeDef` 设置眉毛为 `confused`（在代码中动态设置）
- 通过 `mouthShapeDef` 设置嘴巴为 `m_shape`（在代码中动态设置）
- Prefix 返回 `false` 时跳过原始方法，直接使用 `__result`

**注意**：冷却动画的 `eyeballShapeDef: lookdown`、`browShapeDef: confused` 和 `mouthShapeDef: m_shape` 是在 `GetCooldownFrame()` 方法中通过代码动态构造的，而不是来自 XML 定义。这种设计允许冷却动画在运行时动态修改眼睛、眉毛和嘴巴形状，而不需要在 XML 中定义多个动画帧。

### 子弹装填动画逻辑

```
冷却进度 → 动画序列
0-20%    → 投入1 → 投入2 → 投入3 → 1颗
20-40%   → 投入1 → 投入2 → 投入3 → 2颗
40-60%   → 投入1 → 投入2 → 投入3 → 3颗
60-80%   → 投入1 → 投入2 → 投入3 → 4颗
80-100%  → 投入1 → 投入2 → 投入3 → 5颗
```

### 贴图资产

| 资产 | South | East |
|-----|-------|------|
| 困惑眉毛 | confused_south.png | confused_east.png |
| 向下看的眼睛 | lookdown_south.png | lookdown_east.png |
| 汗液1 | sweat1_south.png | sweat1_east.png |
| 汗液2 | sweat2_south.png | sweat2_east.png |
| 汗液3 | sweat3_south.png | sweat3_east.png |
| 弹匣 | magazine_south.png | magazine_east.png |
| 子弹投入1 | bullet_insert1_south.png | bullet_insert1_east.png |
| 子弹投入2 | bullet_insert2_south.png | bullet_insert2_east.png |
| 子弹投入3 | bullet_insert3_south.png | bullet_insert3_east.png |
| 一颗子弹 | bullet1_south.png | bullet1_east.png |
| 两颗子弹 | bullet2_south.png | bullet2_east.png |
| 三颗子弹 | bullet3_south.png | bullet3_east.png |
| 四颗子弹 | bullet4_south.png | bullet4_east.png |
| 五颗子弹 | bullet5_south.png | bullet5_east.png |
| M嘴 | m_shape_south.png | m_shape_east.png |

### XML 定义

**冷却动画定义** (`CooldownAnimation.xml`):
```xml
<FacialAnimation.FaceAnimationDef>
  <defName>Sylvie_CooldownAnimation</defName>
  <animationFrames>
    <li>
      <duration>30</duration>
      <browShapeDef>confused</browShapeDef>
      <mouthShapeDef>m_shape</mouthShapeDef>
    </li>
  </animationFrames>
  <targetJobs>
    <li>AttackStatic</li>
  </targetJobs>
  <priority>10200</priority>
</FacialAnimation.FaceAnimationDef>
```

**注意**：`CooldownAnimation.xml` 定义了困惑眉毛和冷却专用嘴巴。向下看的眼睛（lookdown）是通过 `Patch_FaceAnimation_GetCurrentFrame` 补丁在代码中动态设置的（见下文 `GetCooldownFrame()` 方法）。

**眉毛形状定义** (`BrowShapeEx.xml`):
```xml
<FacialAnimation.BrowShapeDef>
  <defName>confused</defName>
</FacialAnimation.BrowShapeDef>
```

**眼球形状定义** (`EyeShapeEx.xml`):
```xml
<FacialAnimation.EyeballShapeDef>
  <defName>lookdown</defName>
  <label>lookdown</label>
</FacialAnimation.EyeballShapeDef>
```

**嘴巴形状定义** (`MouthShapeEx.xml`):
```xml
<FacialAnimation.MouthShapeDef>
  <defName>m_shape</defName>
  <label>M嘴</label>
</FacialAnimation.MouthShapeDef>
```

**冷却叠加层形状定义** (`CooldownShapeEx.xml`):
```xml
<!-- 汗液、弹匣、子弹等 LidOptionShapeDef 定义 -->
<FacialAnimation.LidOptionShapeDef>
  <defName>sweat1</defName>
  <raceName>Sylvie_Race</raceName>  <!-- TypeDef 需要 raceName 限制 -->
</FacialAnimation.LidOptionShapeDef>
<!-- ... sweat2, sweat3, magazine, bullet_insert1-3, bullet1-5 -->
```
    <li>sweat1</li>
    <li>sweat2</li>
    <li>sweat3</li>
    <!-- ... 其他形状 -->
  </overlayShapeDefs>
</FacialAnimation.CooldownOverlayTypeDef>
```

**raceName 使用原则**（重要！）：

| 定义类型 | 是否需要 raceName | 说明 |
|----------|-------------------|------|
| `FaceAnimationDef`（动画定义） | **不需要** | 如 `Sylvie_CooldownAnimation`，添加 raceName 会导致 FA 框架匹配冲突 |
| `TypeDef`（类型定义） | **需要** | 如 `EyeballTypeDef`、`MouthTypeDef`、`LidOptionTypeDef` 等，必须添加 raceName 限制 |
| `ShapeDef`（形状定义） | 视情况而定 | 通过 TypeDef 引用的 ShapeDef 通常不需要单独添加 raceName |

**原因**：FA 框架通过 TypeDef 的 raceName 来匹配种族，动画定义（FaceAnimationDef）则是全局可用的。如果动画定义也添加 raceName，会导致匹配冲突，引发 NullReferenceException。

## 瞄准动画同步系统

## Lovin 动画系统

### 功能概述
Lovin（亲热）时显示专属动态表情动画（6帧循环），包括流汗效果、眉毛变化、眼球移动和吐舌嘴巴：
- **流汗效果**：使用 FA 原生 LidOption 系统渲染（sweat1/2/3）
- **眉毛变化**：confused → confused_down → confused_up
- **眼球移动**：normal → eye_up → eye_roll
- **嘴巴形状**：全程 tongue_out（吐舌）

### 动画帧序列表格

| 帧 | duration | Sweat | Brow | Eyeball | Mouth | Lid |
|----|----------|-------|------|---------|-------|-----|
| 1 | 30 | sweat1 | confused | normal | tongue_out | normal |
| 2 | 30 | sweat2 | confused | normal | tongue_out | normal |
| 3 | 30 | sweat3 | confused_down | eye_up | tongue_out | normal |
| 4 | 30 | sweat1 | confused_down | eye_up | tongue_out | normal |
| 5 | 30 | sweat2 | confused_up | eye_roll | tongue_out | normal |
| 6 | 30 | sweat3 | confused_up | eye_roll | tongue_out | normal |

### 技术实现

**动画定义** (`Defs/FacialAnimation/Animations/LovinAnimation.xml`):
```xml
<FacialAnimation.FaceAnimationDef>
  <defName>Sylvie_LovinAnimation</defName>
  <animationFrames>
    <!-- 6帧动画，每帧30 ticks -->
    <li>
      <duration>30</duration>
      <browShapeDef>confused</browShapeDef>
      <eyeballShapeDef>normal</eyeballShapeDef>
      <mouthShapeDef>tongue_out</mouthShapeDef>
      <lidShapeDef>normal</lidShapeDef>          <!-- 重要！ -->
      <lidOptionShapeDef>sylvie_sweat1</lidOptionShapeDef>
    </li>
    <!-- ... 共6帧 ... -->
  </animationFrames>
  <targetJobs>
    <li>Lovin</li>
    <li>MLI_Jobs_MassLoveIn</li>
    <li>MLI_Jobs_SingleLoveIn</li>
  </targetJobs>
  <priority>10600</priority>  <!-- 高于 FA Lovin2 (10500) -->
</FacialAnimation.FaceAnimationDef>
```

**关键设计**:
- 使用 FA 原生 LidOption 系统渲染流汗效果（替代已废弃的 SylvieLovinOverlayComp 组件）
- 每帧必须包含完整的参数（特别是 `lidShapeDef>normal`）
- Priority 10600 确保在 FA Lovin 动画（10400）和 Lovin2 动画（10500）之后应用

**ShapeDef 定义**:
- `BrowShapeEx.xml` - confused_up, confused_down 眉毛形状
- `EyeShapeEx.xml` - eye_up, eye_roll 眼球形状
- `MouthShapeEx.xml` - tongue_out 嘴巴形状
- `LidOptionShapeEx.xml` - sylvie_sweat1/2/3 流汗形状

### 文件结构
```
Defs/FacialAnimation/
├── Animations/
│   └── LovinAnimation.xml       # Lovin 动画定义（6帧）
├── Shapes/
│   ├── BrowShapeEx.xml          # confused_up, confused_down
│   ├── EyeShapeEx.xml           # eye_up, eye_roll
│   ├── MouthShapeEx.xml         # tongue_out
│   └── LidOptionShapeEx.xml     # sylvie_sweat1/2/3
└── Types/
    └── LidOptionType.xml        # LidOption 类型定义

Textures/Things/Pawn/Sylvie/
├── Brows/Normal/Unisex/         # confused_up/down 贴图
├── Eyes/                        # eye_up/roll 贴图
├── Mouth/Normal/Unisex/         # tongue_out 贴图
└── LidOptions/Normal/Unisex/    # sweat1/2/3 贴图
```

## 瞄准动画同步系统

### 技术实现
由于 Facial Animation 的动画选择基于 Job，而 `AttackStatic` Job 在整个射击过程中都存在（warmup + cooldown + burst），动画的 `startTick` 是在 Job 开始时设置的。因此使用 Harmony 补丁直接检查 `Stance_Warmup` 状态来实现动态动画同步。

### 核心组件

#### SylvieAimingTracker
**文件位置**: `Source/Patches/Patch_Stance_Warmup.cs`

ThingComp（组件），用于缓存 Pawn 引用和静态字典查找：
- `Pawn` - 获取缓存的 Pawn 引用
- `GetTracker(Pawn)` - 静态方法，从字典获取或创建跟踪器

#### Harmony 补丁
**文件位置**: `Source/Patches/Patch_Stance_Warmup.cs`

- `Patch_Pawn_SpawnSetup` - 为 Sylvie 种族 Pawn 添加跟踪器组件
- `Patch_FaceAnimation_GetCurrentFrame` - 使用 Prefix 拦截 Facial Animation 的帧计算
  - `Stance_Warmup` 状态：基于 warmup 进度计算帧（帧0 → 帧1 → 帧2）
  - `VerbState.Bursting` 状态：显示最后一帧（帧2）
  - `Stance_Cooldown` 状态：返回 `GetCooldownFrame()` 构造的冷却帧（包含困惑眉毛、向下看的眼球和冷却专用嘴巴）
  - 其他状态：返回 `true` 让原始方法执行
  - **注意**：Prefix 返回 `false` 时会跳过原始方法，直接返回 `__result`

**冷却帧构造**：
```csharp
// File: Source/Patches/Patch_Stance_Warmup.cs
#nullable enable

private static FaceAnimationDef.AnimationFrame GetCooldownFrame()
{
    if (cachedCooldownFrame == null)
    {
        cachedCooldownFrame = new FaceAnimationDef.AnimationFrame
        {
            duration = 30,
            browShapeDef = ConfusedBrowDef,       // 困惑眉毛
            eyeballShapeDef = LookdownEyeballDef, // 向下看的眼球
            mouthShapeDef = CooldownMouthDef      // 冷却专用嘴巴
        };
    }
    return cachedCooldownFrame;
}
```
- `Patch_FacialAnimationControllerComp_InitializeIfNeed` - 注册动画到 Pawn 的映射

### 动画帧边界处理
当计算的帧索引超出范围时，代码会进行边界检查：
- 如果 `frameIndex >= totalFrames`，设置为最后一帧
- 如果 `frameIndex < 0`，设置为第一帧

### 连发状态处理
在 `Stance_Cooldown` 状态下，如果武器处于 `VerbState.Bursting` 状态，
则继续显示最后一帧动画，直到连发结束。

### 瞄准时间计算
```
总瞄准时间 = 武器.warmupTime × Pawn.AimingDelayFactor
```

## FA 动画混合机制

### 核心原理
FA 使用动画累积机制，而非单一动画选择。这意味着：
- FA 会累积所有匹配的动画帧
- 对于每个参数取最后一个非 null 的值
- 动画按 priority 升序排序后遍历

### 动画累积流程

```csharp
// AnimationFrameAccumulator.UpdateAnimation
foreach (FaceAnimation currentJobAnimation in currentJobAnimationList)
{
    if (!currentJobAnimation.animationDef.applyWhenStandingOnly || isStanding)
    {
        Add(currentJobAnimation.GetCurrentFrame(tickGame));
    }
}
return AccumResultFrameAndClear();
```

### 参数合并规则

对于每个参数，取累积列表中最后一个非 null 的值：

```csharp
// AccumResultFrameAndClear
animationFrame.lidShapeDef = accumFrames
    .Where(x => x.lidShapeDef != null)
    .LastOrDefault()?.lidShapeDef;
```

**这意味着**：
- 如果 Sylvie 动画的某帧没有指定 `lidShapeDef`，FA 原版的 `lidShapeDef=close` 可能被应用
- 如果 Sylvie 动画的某帧指定了 `lidShapeDef=normal`，则会覆盖 FA 原版的值

### 动画排序

动画按 priority 升序排列（低 priority 在前，高 priority 在后）：

```csharp
animationDict[key].Sort((a, b) =>
    a.animationDef.priority - b.animationDef.priority);
```

**Priority 配置参考**：

| 动画 | Priority | 说明 |
|-----|----------|------|
| FA Ingest | 10 | FA 默认进食动画 |
| Sylvie Ingest | 11 | 高于 FA 默认 |
| FA Lovin | 10400 | FA 默认 Lovin |
| FA Lovin2 | 10500 | FA 默认 Lovin2 |
| Sylvie Lovin | 10600 | 高于所有 FA Lovin |
| Sylvie Cooldown | 10200 | 冷却动画 |
| Sylvie Aiming | 10300 | 瞄准动画 |
| Sylvie Research | 10003 | 研究动画 |

### 完整参数指定原则

**问题背景**：
由于动画累积机制，如果 Sylvie 动画某帧缺少某个参数，FA 原版的对应参数可能被应用。

**解决方案**：
确保 Sylvie 动画的每一帧都包含所有关键参数：

```xml
<li>
  <duration>30</duration>
  <browShapeDef>confused</browShapeDef>
  <eyeballShapeDef>normal</eyeballShapeDef>
  <mouthShapeDef>tongue_out</mouthShapeDef>
  <lidShapeDef>normal</lidShapeDef>          <!-- 必须！ -->
  <lidOptionShapeDef>sylvie_sweat1</lidOptionShapeDef>
</li>
```

**必须指定的参数**：
- `browShapeDef` - 眉毛形状
- `eyeballShapeDef` - 眼球形状
- `mouthShapeDef` - 嘴巴形状
- `lidShapeDef` - 眼皮形状（即使是 normal 也要指定）
- `lidOptionShapeDef` - 眼皮选项（如流汗效果）

### 关键 API
```csharp
// File: Source/Patches/Patch_Stance_Warmup.cs
#nullable enable

// 武器词条
Verb.verbProps.warmupTime           // 瞄准时间（秒）

// Pawn 属性
pawn.GetStatValue(StatDefOf.AimingDelayFactor)   // 瞄准时间乘数

// Stance_Warmup 状态
warmup.ticksLeft    // 剩余瞄准 ticks
warmup.verb         // 当前使用的 Verb
```

## 种族限制机制

### raceName 使用原则

**核心问题**：FA 原生的 `IsSame` 方法严格匹配 raceName，导致 Sylvie 无法同时使用专属动画和 FA 原版动画。

**解决方案**：通过 `Patch_FaceAnimationDef_IsSame` 补丁修改匹配逻辑。

### raceName 使用原则表格

| 定义类型 | 是否需要 raceName | 示例 | 说明 |
|----------|-------------------|------|------|
| FaceAnimationDef（动画定义） | **不需要** | Sylvie_LovinAnimation, Sylvie_AimingAnimation | 添加 raceName 会导致 FA 框架匹配冲突 |
| BrowTypeDef | **需要** | Sylvie_BrowNormal | TypeDef 需要 raceName 限制 |
| EyeballTypeDef | **需要** | Sylvie_EyeNormal | TypeDef 需要 raceName 限制 |
| MouthTypeDef | **需要** | Sylvie_MouthNormal | TypeDef 需要 raceName 限制 |
| LidTypeDef | **需要** | Sylvie_LidNormal | TypeDef 需要 raceName 限制 |
| LidOptionTypeDef | **需要** | Sylvie_LidOptionNormal | TypeDef 需要 raceName 限制 |
| HeadTypeDef | **需要** | Sylvie_Head | TypeDef 需要 raceName 限制 |
| SkinTypeDef | **需要** | Sylvie_SkinRightEye | TypeDef 需要 raceName 限制 |
| EmotionTypeDef | **需要** | Sylvie_EmotionNormal | TypeDef 需要 raceName 限制 |

### 需要 raceName 的 TypeDef 列表

**必须添加 `raceName>Sylvie_Race` 的 TypeDef**：

```xml
<!-- 眉毛类型 -->
<FacialAnimation.BrowTypeDef>
  <defName>Sylvie_BrowNormal</defName>
  <raceName>Sylvie_Race</raceName>
  ...
</FacialAnimation.BrowTypeDef>

<!-- 眼球类型 -->
<FacialAnimation.EyeballTypeDef>
  <defName>Sylvie_EyeNormal</defName>
  <raceName>Sylvie_Race</raceName>
  ...
</FacialAnimation.EyeballTypeDef>

<!-- 嘴巴类型 -->
<FacialAnimation.MouthTypeDef>
  <defName>Sylvie_MouthNormal</defName>
  <raceName>Sylvie_Race</raceName>
  ...
</FacialAnimation.MouthTypeDef>

<!-- 眼睑类型 -->
<FacialAnimation.LidTypeDef>
  <defName>Sylvie_LidNormal</defName>
  <raceName>Sylvie_Race</raceName>
  ...
</FacialAnimation.LidTypeDef>

<!-- 眼睑选项类型 -->
<FacialAnimation.LidOptionTypeDef>
  <defName>Sylvie_LidOptionNormal</defName>
  <raceName>Sylvie_Race</raceName>
  ...
</FacialAnimation.LidOptionTypeDef>

<!-- 头部类型 -->
<FacialAnimation.HeadTypeDef>
  <defName>Sylvie_Head</defName>
  <raceName>Sylvie_Race</raceName>
  ...
</FacialAnimation.HeadTypeDef>

<!-- 皮肤类型 -->
<FacialAnimation.SkinTypeDef>
  <defName>Sylvie_SkinRightEye</defName>
  <raceName>Sylvie_Race</raceName>
  ...
</FacialAnimation.SkinTypeDef>

<!-- 情绪类型 -->
<FacialAnimation.EmotionTypeDef>
  <defName>Sylvie_EmotionNormal</defName>
  <raceName>Sylvie_Race</raceName>
  ...
</FacialAnimation.EmotionTypeDef>
```

### 不应有 raceName 的动画定义

**FaceAnimationDef 不应添加 raceName**：

```xml
<!-- 正确：FaceAnimationDef 不应有 raceName -->
<FacialAnimation.FaceAnimationDef>
  <defName>Sylvie_LovinAnimation</defName>
  <animationFrames>...</animationFrames>
  <targetJobs><li>Lovin</li></targetJobs>
  <priority>10600</priority>
</FacialAnimation.FaceAnimationDef>

<!-- 错误：FaceAnimationDef 不应添加 raceName -->
<FacialAnimation.FaceAnimationDef>
  <defName>Sylvie_LovinAnimation</defName>
  <raceName>Sylvie_Race</raceName>  <!-- 删除这行！ -->
  ...
</FacialAnimation.FaceAnimationDef>
```

### 原因说明

- **TypeDef 的 raceName**：用于告诉 FA 框架该类型属于哪个种族，限制该类型的贴图只被对应种族使用
- **FaceAnimationDef 的 raceName**：动画定义是全局可用的，通过 `targetJobs` 和 `priority` 与 TypeDef 关联
- **冲突原因**：如果 FaceAnimationDef 也添加 raceName，会导致 FA 框架在匹配动画时产生冲突，引发 NullReferenceException

### 动画帧计算
假设动画有 3 帧，总瞄准时间为 T ticks：
- `elapsedTicks = totalWarmupTicks - warmup.ticksLeft`
- `progress = elapsedTicks / totalWarmupTicks`
- `frameIndex = (int)(progress * 3)`
- 帧1：进度 0% - 33%
- 帧2：进度 33% - 66%
- 帧3：进度 66% - 100%（射击发生）
