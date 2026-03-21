# SylvieRace 开发者文档

本文档面向 Mod 开发者，介绍 SylvieRace 项目结构和技术实现细节。

**版本**: v0.0.6-pre  
**游戏版本**: RimWorld 1.6  
**最后更新**: 2026-03-21

---

## 目录

- [SylvieRace 开发者文档](#sylvierace-开发者文档)
  - [目录](#目录)
  - [项目概述](#项目概述)
    - [技术栈](#技术栈)
  - [目录结构](#目录结构)
  - [核心架构](#核心架构)
    - [架构图](#架构图)
    - [核心类职责](#核心类职责)
  - [功能模块](#功能模块)
    - [种族系统](#种族系统)
    - [事件系统](#事件系统)
    - [服装系统](#服装系统)
    - [健康系统](#健康系统)
    - [动态表情系统](#动态表情系统)
      - [核心补丁](#核心补丁)
      - [冷却动画渲染](#冷却动画渲染)
    - [寻求抚摸系统](#寻求抚摸系统)
  - [代码规范](#代码规范)
    - [命名规范](#命名规范)
    - [可空引用](#可空引用)
    - [文档注释](#文档注释)
  - [开发指南](#开发指南)
    - [添加新组件](#添加新组件)
    - [使用动画注册系统](#使用动画注册系统)
    - [使用动画辅助工具](#使用动画辅助工具)
    - [添加 Def 名称](#添加-def-名称)
    - [调试技巧](#调试技巧)
  - [相关文档](#相关文档)

---

## 项目概述

SylvieRace 是一个基于 HAR (Human Alien Race) 框架的 RimWorld 种族 Mod，添加了希尔薇种族及其配套内容：

- **22种专属服装** - 含特殊效果的护士服
- **希尔薇商人事件** - 智能防重复生成
- **动态表情系统** - 完整 Facial Animation 框架支持
- **寻求抚摸机制** - 主动社交互动

### 技术栈

- **框架**: RimWorld 1.6 + HAR
- **语言**: C# (.NET Framework 4.7.2/4.8)
- **补丁**: Harmony 2.x
- **可选依赖**: Facial Animation

---

## 目录结构

```
SylvieRace/
├── Source/
│   ├── Animation/               # 动画系统 [新增]
│   │   ├── SylvieAnimationRegistry.cs    # 动画注册管理器
│   │   └── SylvieAnimationHelper.cs      # 动画辅助工具
│   ├── Components/              # 游戏组件
│   │   ├── SylvieAimingTracker.cs        # 瞄准状态追踪器 [新增]
│   │   ├── SylvieCooldownTracker.cs      # 冷却状态跟踪
│   │   ├── SylvieCooldownOverlayComp.cs  # 冷却动画渲染
│   │   ├── SylvieCatEarComp.cs           # 猫耳渲染
│   │   ├── SylvieGameComponent.cs        # 状态管理与事件触发
│   │   └── SylvieSeekPettingTracker.cs   # 抚摸冷却跟踪
│   ├── Core/                    # 核心基础设施
│   │   ├── HarmonyInit.cs       # Harmony 初始化
│   │   ├── SylvieDefNames.cs    # Def 名称常量中心
│   │   ├── SylvieConstants.cs   # 全局常量定义
│   │   └── SylvieComponentRegistry.cs    # 组件注册中心 [新增]
│   ├── Jobs/                    # 工作系统 [v0.0.6-pre 新增]
│   │   ├── JobGiver_SeekPetting.cs       # 寻求抚摸 AI 决策
│   │   └── JobDriver_SeekPetting.cs      # 抚摸交互执行
│   ├── Patches/                 # Harmony 补丁
│   │   ├── Patch_FaceAnimationDef_IsSame.cs  # FA 种族匹配
│   │   ├── Patch_Stance_Warmup.cs            # 瞄准动画同步
│   │   ├── Patch_ResearchAnimation.cs        # 研究动画
│   │   └── Patch_CommsConsole.cs             # 通讯台补丁
│   ├── Pawns/                   # Pawn 生成
│   │   └── SylviePawnGenerator.cs
│   ├── Hediffs/                 # 健康状态
│   │   ├── CompNurseHeal.cs         # 护士服治疗技能
│   │   └── SylvieHediffManager.cs   # Hediff 管理
│   ├── Incidents/               # 事件
│   │   └── IncidentWorker_SylvieTrader.cs
│   ├── Letters/                 # 信件
│   │   └── ChoiceLetter_SylvieOffer.cs
│   └── Debug/                   # 调试功能
│       └── SylvieDebugActions.cs
├── Defs/                        # XML 定义
│   ├── Races/                   # 种族定义
│   ├── PawnKinds/               # PawnKind 定义
│   ├── Apparel/                 # 服装定义 (22件)
│   ├── FacialAnimation/         # 动态表情定义
│   │   ├── Animations/          # 动画定义
│   │   ├── Types/               # 类型定义
│   │   └── Shapes/              # 形状扩展
│   └── ...
└── Patches/                     # XML 补丁
    └── Sylvie_Race_FacialAnimation_Patches.xml
```

---

## 核心架构

### 架构图

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                              SylvieRace Mod                                  │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │                        Core Layer (核心层)                           │   │
│  │  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐     │   │
│  │  │ SylvieDefNames  │  │ SylvieConstants │  │SylvieComponent  │     │   │
│  │  │   (Def常量中心)  │  │   (数值常量)     │  │   Registry      │     │   │
│  │  │                 │  │                 │  │ (组件注册中心)   │     │   │
│  │  └─────────────────┘  └─────────────────┘  └─────────────────┘     │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                              │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │                     Animation Layer (动画层)                         │   │
│  │  ┌─────────────────┐  ┌─────────────────┐                          │   │
│  │  │SylvieAnimation  │  │SylvieAnimation  │                          │   │
│  │  │   Registry      │  │    Helper       │                          │   │
│  │  │(动画注册管理器)  │  │  (动画辅助工具)  │                          │   │
│  │  └─────────────────┘  └─────────────────┘                          │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                              │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │                    Component Layer (组件层)                          │   │
│  │  ┌──────────────┐ ┌──────────────┐ ┌──────────────┐ ┌─────────────┐ │   │
│  │  │SylvieAiming  │ │SylvieCooldown│ │SylvieCooldown│ │SylvieCatEar │ │   │
│  │  │   Tracker    │ │   Tracker    │ │ OverlayComp  │ │    Comp     │ │   │
│  │  └──────────────┘ └──────────────┘ └──────────────┘ └─────────────┘ │   │
│  │  ┌──────────────┐ ┌──────────────┐ ┌──────────────┐                  │   │
│  │  │SylvieSeek    │ │SylvieGame    │ │SylvieHediff  │                  │   │
│  │  │PettingTracker│ │  Component   │ │   Manager   │                  │   │
│  │  └──────────────┘ └──────────────┘ └──────────────┘                  │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                              │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │                     Feature Layer (功能层)                           │   │
│  │  ┌──────────────┐ ┌──────────────┐ ┌──────────────┐ ┌─────────────┐ │   │
│  │  │   Race       │ │  Events      │ │   Apparel    │ │  Incidents  │ │   │
│  │  │  (种族定义)   │ │  (事件系统)   │ │  (服装系统)   │ │  (事件触发) │ │   │
│  │  └──────────────┘ └──────────────┘ └──────────────┘ └─────────────┘ │   │
│  │  ┌──────────────┐ ┌──────────────┐ ┌──────────────┐ ┌─────────────┐ │   │
│  │  │   Hediffs    │ │   Jobs       │ │   Letters    │ │   Patches   │ │   │
│  │  │  (健康状态)   │ │  (工作任务)   │ │  (信件系统)   │ │  (Harmony)  │ │   │
│  │  └──────────────┘ └──────────────┘ └──────────────┘ └─────────────┘ │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                              │
└─────────────────────────────────────────────────────────────────────────────┘
```

### 核心类职责

| 类 | 文件 | 职责 |
|----|------|------|
| `SylvieDefNames` | [Core/SylvieDefNames.cs](Core/SylvieDefNames.cs) | 集中管理所有 Def 名称常量，提供 `IsSylvieRace()` 判断方法 |
| `SylvieConstants` | [Core/SylvieConstants.cs](Core/SylvieConstants.cs) | 全局常量（动画、时间、渲染层级、数值） |
| `SylvieComponentRegistry` | [Core/SylvieComponentRegistry.cs](Core/SylvieComponentRegistry.cs) | 集中管理所有 ThingComp 组件注册，提供统一注册 API |
| `SylvieAnimationRegistry` | [Animation/SylvieAnimationRegistry.cs](Animation/SylvieAnimationRegistry.cs) | 统一管理 FaceAnimation 到 Pawn 的映射关系 |
| `SylvieAnimationHelper` | [Animation/SylvieAnimationHelper.cs](Animation/SylvieAnimationHelper.cs) | 提供动画帧计算、反射访问等通用功能 |
| `SylvieAimingTracker` | [Components/SylvieAimingTracker.cs](Components/SylvieAimingTracker.cs) | 追踪 Sylvie Pawn 的瞄准状态，与面部动画同步 |
| `SylvieGameComponent` | [Components/SylvieGameComponent.cs](Components/SylvieGameComponent.cs) | 游戏状态管理、事件触发调度 |
| `SylviePawnGenerator` | [Pawns/SylviePawnGenerator.cs](Pawns/SylviePawnGenerator.cs) | 希尔薇 Pawn 生成与配置 |
| `JobGiver_SeekPetting` | [Jobs/JobGiver_SeekPetting.cs](Jobs/JobGiver_SeekPetting.cs) | AI 决策，7步条件检查，寻找最佳抚摸目标 |
| `JobDriver_SeekPetting` | [Jobs/JobDriver_SeekPetting.cs](Jobs/JobDriver_SeekPetting.cs) | 执行抚摸交互，应用心情、社交关系效果 |
| `SylvieSeekPettingTracker` | [Components/SylvieSeekPettingTracker.cs](Components/SylvieSeekPettingTracker.cs) | 抚摸冷却时间管理（6小时=15000ticks），存档支持 |

---

## 功能模块

### 种族系统

**关键文件**:
- `Defs/Races/Sylvie_Race.xml` - 种族定义
- `Source/Pawns/SylviePawnGenerator.cs` - 生成逻辑

**生成配置**:
```csharp
// 固定属性
Age: 19岁
Gender: 女性
Xenotype: Baseliner (强制)

// 基因设置
Skin: Skin_SheerWhite (透白)
Hair: Hair_SnowWhite (雪白)

// 特性
清除所有随机特性 → 添加 Kind

// 纹身
面部: SylvieRace_ScarHead
身体: SylvieRace_ScarBody
```

---

### 事件系统

**关键文件**: [Components/SylvieGameComponent.cs](Components/SylvieGameComponent.cs)

**防重复机制** ([SylvieGameComponent.cs](Components/SylvieGameComponent.cs#L23-L47)):
```csharp
public override void GameComponentTick()
{
    base.GameComponentTick();

    if (Find.TickManager.TicksGame % CheckInterval != 0)
        return;

    if (!hasSylvieSpawned && Find.TickManager.TicksGame >= InitialEventTick)
    {
        if (CheckForExistingSylvie())  // 检查现有殖民者
        {
            hasSylvieSpawned = true;
            return;
        }
        TryForceSylvieEvent();  // 触发事件
    }

    if (!hediffTriggered && sylviePawn != null && hediffTriggerTick > 0 
        && Find.TickManager.TicksGame >= hediffTriggerTick)
    {
        if (SylvieHediffManager.TryTriggerHediff(sylviePawn))
        {
            hediffTriggered = true;
        }
    }
}
```

**时间常量** ([SylvieConstants.cs](Core/SylvieConstants.cs#L26-L52)):
- 检查间隔: 2500 ticks (~41.7秒 @60TPS) - `CheckIntervalTicks`
- 初始延迟: 240000 ticks (4 游戏日) - `InitialEventDelayTicks`
- Hediff 延迟: 300000 ticks (5 游戏日) - `HediffDelayTicks`
- 护士服治疗间隔: 600 ticks (10秒 @60TPS) - `NurseHealIntervalTicks`

---

### 服装系统

**护士服主动技能** ([Hediffs/CompNurseHeal.cs](Hediffs/CompNurseHeal.cs)):

```csharp
// 组件属性
public class SylvieRace_CompProperties_NurseHeal : CompProperties
{
    public int cooldownTicks = 5000;  // 冷却时间（ticks）
    public HediffDef paralysisHediff = null!; // 使用后添加的麻痹 Hediff
}

// 核心逻辑 ([CompNurseHeal.cs](Hediffs/CompNurseHeal.cs#L107-L143))
private void TryUseAbility()
{
    Pawn? wearer = Wearer;
    if (wearer == null) return;

    if (IsOnCooldown)
    {
        Messages.Message("SylvieRace_NurseHeal_Cooldown".Translate(CooldownTicksRemaining.ToStringTicksToPeriod()),
            MessageTypeDefOf.RejectInput);
        return;
    }

    // 1. 包扎所有未处理伤口
    bool hasWounds = false;
    foreach (Hediff hediff in wearer.health.hediffSet.hediffs)
    {
        if (hediff is Hediff_Injury injury && !injury.IsTended())
        {
            injury.Tended(1.0f, 1.0f, 0);
            hasWounds = true;
        }
    }

    if (!hasWounds)
    {
        Messages.Message("SylvieRace_NurseHeal_NoWounds".Translate(wearer.LabelShort),
            MessageTypeDefOf.RejectInput);
        return;
    }

    // 2. 添加麻痹 Hediff
    Hediff paralysis = HediffMaker.MakeHediff(Props.paralysisHediff, wearer);
    wearer.health.AddHediff(paralysis);

    // 3. 进入冷却
    lastUseTick = Find.TickManager.TicksGame;

    Messages.Message("SylvieRace_NurseHeal_Success".Translate(wearer.LabelShort),
        wearer, MessageTypeDefOf.PositiveEvent);
}
```

---

### 健康系统

**关键文件**: [Hediffs/SylvieHediffManager.cs](Hediffs/SylvieHediffManager.cs)

**初始创伤机制**:
- 生成后 5 游戏日自动添加
- 需要持续治疗
- 触发时发送信件通知

---

### 动态表情系统

基于 Facial Animation 框架，支持 5 种动画：

| 动画 | 触发条件 | 文件 |
|------|----------|------|
| 瞄准 | Stance_Warmup | `Animations/AimingAnimation.xml` |
| 冷却 | Stance_Cooldown | `Animations/CooldownAnimation.xml` |
| 进食 | JobDriver_Ingest | `Animations/IngestAnimation.xml` |
| 研究 | JobDriver_Research | `Animations/ResearchAnimation.xml` |
| Lovin | JobDriver_Lovin | `Animations/LovinAnimation.xml` |

#### 核心补丁

**种族匹配补丁** ([Patches/Patch_FaceAnimationDef_IsSame.cs](Patches/Patch_FaceAnimationDef_IsSame.cs)):
```csharp
[HarmonyPatch(typeof(FaceAnimationDef), nameof(FaceAnimationDef.IsSame))]
public static class Patch_FaceAnimationDef_IsSame
{
    public static bool Prefix(FaceAnimationDef __instance, string jobName,
        string targetName, ref bool __result)
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
```

**瞄准动画同步** ([Patches/Patch_Stance_Warmup.cs](Patches/Patch_Stance_Warmup.cs)):
```csharp
[HarmonyPatch(typeof(FaceAnimation), nameof(FaceAnimation.GetCurrentFrame))]
public static class Patch_FaceAnimation_GetCurrentFrame
{
    private static FaceAnimationDef.AnimationFrame? cachedCooldownFrame;
    private static BrowShapeDef? confusedBrowDef;
    private static EyeballShapeDef? lookdownEyeballDef;
    
    public static bool Prefix(FaceAnimation __instance, int tickGame, ref FaceAnimationDef.AnimationFrame? __result)
    {
        // 使用动画注册系统获取关联 Pawn
        Pawn? pawn = SylvieAnimationRegistry.GetAssociatedPawn(__instance);
        if (pawn == null) return true;
        
        // 使用动画辅助工具获取动画帧
        var frames = SylvieAnimationHelper.GetAnimationFrames(__instance.animationDef);
        if (!SylvieAnimationHelper.HasValidFrames(frames)) return true;
        
        Stance? curStance = pawn.stances?.curStance;
        
        // Warmup 阶段：根据进度计算动画帧
        if (curStance is Stance_Warmup warmup)
        {
            Verb? verb = warmup.verb;
            if (verb == null) return true;
            
            float warmupTime = verb.verbProps.warmupTime;
            float aimingDelayFactor = pawn.GetStatValue(StatDefOf.AimingDelayFactor);
            int totalWarmupTicks = (warmupTime * aimingDelayFactor).SecondsToTicks();
            
            if (totalWarmupTicks <= 0) return true;
            
            // 使用辅助工具计算帧索引
            int elapsedTicks = totalWarmupTicks - warmup.ticksLeft;
            int frameIndex = SylvieAnimationHelper.CalculateFrameIndex(elapsedTicks, frames!.Count);
            
            __result = frames[frameIndex];
            return false;
        }
        // 冷却期间显示困惑表情
        else if (curStance is Stance_Cooldown cooldown)
        {
            Verb? verb = cooldown.verb;
            if (verb != null && verb.state == VerbState.Bursting)
            {
                __result = frames![frames.Count - 1];
                return false;
            }
            
            __result = GetCooldownFrame();
            return false;
        }
        
        return true;
    }
}
```

**动画注册** ([Patches/Patch_FacialAnimationControllerComp.cs](Patches/Patch_FacialAnimationControllerComp.cs)):
```csharp
[HarmonyPatch(typeof(FacialAnimationControllerComp), "InitializeIfNeed")]
public static class Patch_FacialAnimationControllerComp_InitializeIfNeed
{
    public static void Postfix(
        Dictionary<string, List<FaceAnimation>> ___animationDict,
        Pawn ___pawn)
    {
        if (!SylvieDefNames.IsSylvieRace(___pawn))
            return;

        // 使用动画注册系统批量注册动画
        SylvieAnimationRegistry.RegisterAllAnimations(___animationDict, ___pawn);
    }
}
```

#### 冷却动画渲染

**组件**: [Components/SylvieCooldownOverlayComp.cs](Components/SylvieCooldownOverlayComp.cs)

**渲染层级** ([SylvieConstants.cs](Core/SylvieConstants.cs#L54-L82)):
```csharp
public const float SweatRenderLayer = 61.1f;     // 汗液
public const float MagazineRenderLayer = 61.2f;  // 弹匣
public const float BulletRenderLayer = 61.3f;    // 子弹
```

**朝向处理** ([SylvieCooldownOverlayComp.cs](Components/SylvieCooldownOverlayComp.cs#L116-L123)):
```csharp
/// <summary>
/// 检查 Graphic 是否有独立的 north 贴图
/// </summary>
private bool HasNorthTexture(Graphic graphic)
{
    // 如果 MatNorth 和 MatSouth 是同一个材质，说明没有独立的 north 贴图
    return graphic.MatNorth != graphic.MatSouth;
}

// 正确做法: 使用 Graphic.MeshAt(rot) 自动处理翻转
Mesh mesh = graphic.MeshAt(rot);
Material mat = graphic.MatAt(rot);
```

**缩放适配** ([SylvieCooldownOverlayComp.cs](Components/SylvieCooldownOverlayComp.cs#L138-L142)):
```csharp
// 自动适配不同年龄 (小孩 0.5/0.75, 成人 1.0)
float headSizeFactor = 1f;
if (ModsConfig.BiotechActive && Pawn.ageTracker.CurLifeStage.headSizeFactor.HasValue)
{
    headSizeFactor = Pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
}
Vector3 drawScale = DrawScale * headSizeFactor;
```

---

### 寻求抚摸系统

**核心组件**:

| 组件 | 文件 | 职责 |
|------|------|------|
| `JobGiver_SeekPetting` | [Jobs/JobGiver_SeekPetting.cs](Jobs/JobGiver_SeekPetting.cs) | AI 决策，7步条件检查，目标评分选择 |
| `JobDriver_SeekPetting` | [Jobs/JobDriver_SeekPetting.cs](Jobs/JobDriver_SeekPetting.cs) | 执行抚摸交互，应用心情、社交关系效果 |
| `SylvieSeekPettingTracker` | [Components/SylvieSeekPettingTracker.cs](Components/SylvieSeekPettingTracker.cs) | 冷却时间管理（6小时=15000ticks），存档支持 |

**常量定义**:

```csharp
// JobGiver_SeekPetting.cs (L18-L23)
private const int MinCheckInterval = GenDate.TicksPerHour;  // 2500 ticks (1小时)
private const float CheckChance = 0.20f;                     // 20% 概率
private const int MaxSearchDistance = 40;                    // 最大搜索距离
private const int MinAgeYears = 10;                          // 最小年龄
private const int HighOpinionThreshold = 40;                 // 高好感度阈值
private const float TargetMinMoodThreshold = 0.50f;          // 目标心情阈值 (50%)

// JobDriver_SeekPetting.cs (L20-L35)
private const int BaseOpinionOffset = 10;                    // 基础关系加成
private const int LowMoodBonusOpinion = 5;                   // 低心情额外加成
private const float LowMoodThreshold = 0.30f;                // 低心情阈值 (30%)
private const int IntimateOpinionThreshold = 40;             // 亲密关系阈值

// SylvieSeekPettingTracker.cs (L21)
public const int CooldownTicks = 15000;                      // 6小时冷却
```

**条件检查流程** ([JobGiver_SeekPetting.cs](Jobs/JobGiver_SeekPetting.cs#L93-L128)):

`TryGiveJob` 方法执行完整的 7 步检查流程：

```csharp
protected override Job TryGiveJob(Pawn pawn)
{
    // 种族检查 (前置过滤，不计入日志步骤)
    if (!SylvieDefNames.IsSylvieRace(pawn))
        return null!;

    int currentTick = Find.TickManager.TicksGame;
    
    // 步骤 1-5: TryPerformChecks 内部执行
    if (!TryPerformChecks(pawn, currentTick, out string? failReason))
        return null!;

    // 步骤 6: 目标查找
    Pawn? target = FindBestTarget(pawn);
    if (target == null)
        return null!;

    // 步骤 7: JobDef 检查
    JobDef? seekPettingJobDef = SylvieDefNames.Job_SeekPettingDef;
    if (seekPettingJobDef == null)
        return null!;

    // 创建并返回任务
    Job job = JobMaker.MakeJob(seekPettingJobDef, target);
    job.locomotionUrgency = LocomotionUrgency.Jog;
    job.expiryInterval = 5000;
    return job;
}
```

`TryPerformChecks` 方法执行前 5 步检查：

```csharp
private bool TryPerformChecks(Pawn pawn, int currentTick, out string? failReason)
{
    // 1. 间隔和概率检查 (1小时间隔 + 20%概率)
    if (!CheckIntervalAndProbability(pawn, currentTick, out failReason))
        return false;

    // 2. 年龄检查 (>= 10岁)
    if (!CheckAge(pawn, out failReason))
        return false;

    // 3. 状态检查 (清醒、未倒地、未精神崩溃)
    if (!CheckPawnState(pawn, out failReason))
        return false;

    // 4. 冷却检查 (6小时冷却期)
    if (!CheckCooldown(pawn, out failReason))
        return false;

    // 5. 空闲检查 (非关键工作)
    if (!CheckIdleStatus(pawn, out failReason))
        return false;

    return true;
}
```

**目标评分机制** ([JobGiver_SeekPetting.cs](Jobs/JobGiver_SeekPetting.cs#L333-L355)):
```csharp
private float CalculateTargetScore(Pawn target, Pawn sylvie)
{
    float score = 0f;

    // Opinion factor - higher opinion = better
    int opinion = sylvie.relations?.OpinionOf(target) ?? 0;
    score += opinion;

    // High opinion bonus (>40 gets priority boost)
    if (opinion > HighOpinionThreshold)
    {
        score += 100f; // Significant priority boost
    }

    // Target mood factor - mood > 50% preferred
    float? targetMood = target.needs?.mood?.CurLevelPercentage;
    if (targetMood.HasValue && targetMood.Value > TargetMinMoodThreshold)
    {
        score += 50f; // Bonus for good mood
    }

    return score;
}
```

**效果应用** ([JobDriver_SeekPetting.cs](Jobs/JobDriver_SeekPetting.cs#L88-L106)):
```csharp
private void ApplyPettingEffects()
{
    if (!ValidatePettingConditions())
    {
        return;
    }

    // Apply mood thoughts
    ApplyMoodThoughts();

    // Update relationship
    UpdateRelationship();

    // Send notification message
    SendNotification();

    // Record cooldown
    RecordCooldown();
}
```

**心情效果**:
- **希尔薇**: "被温柔地抚摸了" (+10 心情, 16小时)
- **抚摸者**: "抚摸了希尔薇" (+6/+8 心情, 12小时, 2阶段)
  - +6: 好感度 <= 40 (普通关系)
  - +8: 好感度 > 40 (亲密关系)

**社交关系效果** ([JobDriver_SeekPetting.cs](Jobs/JobDriver_SeekPetting.cs#L211-L238)):
```csharp
private void UpdateRelationship()
{
    int opinionOffset = CalculateOpinionOffset();

    // Apply social thought to target about Sylvie (target gains opinion of Sylvie)
    ApplySocialThought(TargetPawn, pawn, SylvieDefNames.Thought_PettedMe_Social, opinionOffset);
    
    // Apply social thought to Sylvie about target (Sylvie gains opinion of target)
    ApplySocialThought(pawn, TargetPawn, SylvieDefNames.Thought_WasPetted_Social, opinionOffset);
}

private int CalculateOpinionOffset()
{
    int opinionOffset = BaseOpinionOffset;

    // Extra bonus when Sylvie's mood is low (< 30%)
    float? moodLevel = pawn.needs?.mood?.CurLevelPercentage;
    if (moodLevel.HasValue && moodLevel.Value < LowMoodThreshold)
    {
        opinionOffset += LowMoodBonusOpinion;
    }

    return opinionOffset;
}
```

**依赖关系**:
```
JobGiver_SeekPetting
├── SylvieDefNames (种族检查、Def获取)
├── SylvieValidationUtils (验证工具)
└── SylvieSeekPettingTracker (冷却检查)

JobDriver_SeekPetting
├── SylvieDefNames (ThoughtDef获取)
└── SylvieSeekPettingTracker (记录冷却)
```

---

## 代码规范

### 命名规范

```csharp
// 常量: PascalCase
public const string Race_Sylvie = "Sylvie_Race";

// 类: PascalCase
public class SylvieGameComponent : GameComponent

// 方法: PascalCase
public static bool IsSylvieRace(Pawn? pawn)

// 私有字段: camelCase
private int lastUseTick = -999999;
```

### 可空引用

```csharp
#nullable enable

// 可空类型明确标记
public static HediffDef? Hediff_InitialTraumaDef { get; }

// 空值检查
if (pawn?.def?.defName == Race_Sylvie)
```

### 文档注释

```csharp
/// <summary>
/// 检查指定的 Pawn 是否为希尔薇种族。
/// </summary>
/// <param name="pawn">要检查的 Pawn</param>
/// <returns>如果是希尔薇种族返回 true，否则返回 false</returns>
public static bool IsSylvieRace(Pawn? pawn)
```

---

## 开发指南

### 添加新组件

**步骤 1**: 创建组件类
```csharp
public class SylvieNewComp : ThingComp
{
    public override void PostDraw()
    {
        // 渲染逻辑
    }

    public override void CompTick()
    {
        // 每 tick 更新逻辑
    }
}
```

**步骤 2**: 在 `SylvieComponentRegistry.cs` 中注册组件类型
```csharp
private static readonly List<Type> RegisteredComponentTypes = new()
{
    typeof(SylvieAimingTracker),
    typeof(SylvieCooldownTracker),
    typeof(SylvieCooldownOverlayComp),
    typeof(SylvieCatEarComp),
    typeof(SylvieNewComp),  // 新增组件
};
```

**步骤 3**: 组件会自动注册到所有 Sylvie 种族 Pawn
```csharp
[HarmonyPatch(typeof(Pawn), nameof(Pawn.SpawnSetup))]
public static class Patch_Pawn_SpawnSetup
{
    public static void Postfix(Pawn __instance)
    {
        // 使用组件注册中心自动注册所有组件
        SylvieComponentRegistry.RegisterAllComponents(__instance);
    }
}
```

---

### 使用动画注册系统

**注册动画**:
```csharp
// 注册单个动画
SylvieAnimationRegistry.RegisterAnimation(animation, pawn);

// 批量注册特定类型的动画
SylvieAnimationRegistry.RegisterAnimationsByType(
    ___animationDict,
    ___pawn,
    SylvieDefNames.Animation_Aiming
);

// 注册所有动画并触发类型特定的回调
SylvieAnimationRegistry.RegisterAllAnimations(___animationDict, ___pawn);
```

**查询关联 Pawn**:
```csharp
Pawn? pawn = SylvieAnimationRegistry.GetAssociatedPawn(animation);
if (pawn != null)
{
    // 处理动画逻辑
}
```

**注册类型特定的处理回调**:
```csharp
SylvieAnimationRegistry.RegisterAnimationType(
    "Sylvie_AimingAnimation",
    (animation, pawn) => {
        // 自定义处理逻辑
    }
);
```

---

### 使用动画辅助工具

**计算帧索引**:
```csharp
// 简单循环帧计算
int frameIndex = SylvieAnimationHelper.CalculateFrameIndex(elapsedTicks, frameCount);

// 基于持续时间的帧计算（适用于每帧有不同 duration 的动画）
int frameIndex = SylvieAnimationHelper.CalculateOriginalFrameIndex(elapsedTicks, frames);

// 计算 warmup 进度
float progress = SylvieAnimationHelper.CalculateWarmupProgress(ticksLeft, totalWarmupTicks);
```

**获取动画帧**:
```csharp
// 安全地获取动画帧列表
var frames = SylvieAnimationHelper.GetAnimationFrames(animationDef);
if (SylvieAnimationHelper.HasValidFrames(frames))
{
    // 处理帧
}
```

**反射获取私有字段**:
```csharp
// 获取引用类型的私有字段
T? value = SylvieAnimationHelper.GetPrivateField<T>(obj, "fieldName");

// 获取值类型的私有字段
int startTick = SylvieAnimationHelper.GetPrivateFieldValue<int>(animation, "startTick");
```

**安全检查**:
```csharp
// 验证动画是否有效
if (SylvieAnimationHelper.IsValidAnimation(animation))
{
    // 处理动画
}

// 验证帧列表是否有效
if (SylvieAnimationHelper.HasValidFrames(frames))
{
    // 处理帧
}
```

---

### 添加 Def 名称

在 [SylvieDefNames.cs](Core/SylvieDefNames.cs) 中添加 ([参考示例](Core/SylvieDefNames.cs#L12-L54)):
```csharp
// 常量定义
public const string MyNewDef = "MyNewDefName";

// 延迟加载的属性
public static MyDefType? MyNewDefDef => DefDatabase<MyDefType>.GetNamed(MyNewDef, false);

// 或使用通用辅助方法
public static TDef? GetDef<TDef>(string defName) where TDef : Def
```

---

### 调试技巧

```csharp
// 启用详细日志
Log.Message($"[SylvieMod] Debug: {variable}");

// 条件断点
if (pawn.LabelShort == "目标名字")
    Log.Warning("[SylvieMod] Breakpoint hit");

// 动画调试
if (SylvieAnimationHelper.IsValidAnimation(animation))
{
    Log.Message($"[SylvieMod] Animation: {animation.animationDef.defName}");
}
```

---

## 相关文档

- [用户文档](../README.md) - 功能介绍与使用指南
- [工作区文档](../../README.md) - 项目索引
- [架构设计](../.trae/docs/architecture.md) - 核心架构文档

---

*本文档遵循 Internal README 规范编写*
