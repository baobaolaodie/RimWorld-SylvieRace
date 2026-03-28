# SylvieRace 开发者文档

本文档面向 Mod 开发者，介绍 SylvieRace 项目结构和技术实现细节。

**版本**: v1.0.4-pre  
**游戏版本**: RimWorld 1.6  
**最后更新**: 2026-03-28

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
      - [猫耳渲染系统](#猫耳渲染系统)
    - [寻求抚摸系统](#寻求抚摸系统)
  - [存档兼容性](#存档兼容性)
    - [版本控制系统](#版本控制系统)
    - [ThingComp 序列化实现](#thingcomp-序列化实现)
    - [组件注册机制](#组件注册机制)
    - [静态数据清理](#静态数据清理)
  - [代码规范](#代码规范)
    - [命名规范](#命名规范)
    - [可空引用](#可空引用)
    - [文档注释](#文档注释)
  - [开发指南](#开发指南)
    - [添加新组件](#添加新组件)
    - [使用动画注册系统](#使用动画注册系统)
    - [使用动画辅助工具](#使用动画辅助工具)
    - [添加 Def 名称](#添加-def-名称)
    - [使用派系验证工具](#使用派系验证工具)
    - [调试技巧](#调试技巧)
  - [技术经验教训](#技术经验教训)
    - [Letter 创建：必须使用 LetterMaker.MakeLetter](#letter-创建必须使用-lettermakermakeletter)
    - [组件注册时机：加载存档后需要延迟注册](#组件注册时机加载存档后需要延迟注册)
    - [类型匹配：使用类型名称匹配而非直接类型比较](#类型匹配使用类型名称匹配而非直接类型比较)
    - [静态数据：考虑存档加载时的清理问题](#静态数据考虑存档加载时的清理问题)
    - [ExposeData：所有 ThingComp 必须正确实现](#exposedata所有-thingcomp-必须正确实现)
    - [Pawn 引用安全：添加死亡检查](#pawn-引用安全添加死亡检查)
    - [反射访问：必须包含错误处理](#反射访问必须包含错误处理)
    - [动画朝向处理：使用 Graphic.MeshAt](#动画朝向处理使用-graphicmeshat)
    - [派系验证：使用工具类集中管理派系选择逻辑](#派系验证使用工具类集中管理派系选择逻辑)
    - [种族正确性：生成后验证，不正确则销毁重生成](#种族正确性生成后验证不正确则销毁重生成)
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
│   ├── Animation/               # 动画系统 (2个文件)
│   │   ├── SylvieAnimationRegistry.cs    # 动画注册管理器
│   │   └── SylvieAnimationHelper.cs      # 动画辅助工具
│   ├── Components/              # 游戏组件 (8个文件)
│   │   ├── SylvieAimingTracker.cs        # 瞄准状态追踪器
│   │   ├── SylvieCooldownTracker.cs      # 冷却状态跟踪
│   │   ├── SylvieCooldownOverlayComp.cs  # 冷却动画渲染
│   │   ├── SylvieCatEarComp.cs           # 猫耳渲染组件（管理状态和帧切换）
│   │   ├── SylvieCatEarRenderNode.cs     # 猫耳渲染节点（继承 PawnRenderNode）
│   │   ├── SylvieCatEarNodeWorker.cs     # 猫耳渲染工作器（处理偏移和开关）
│   │   ├── SylvieGameComponent.cs        # 状态管理与事件触发
│   │   └── SylvieSeekPettingTracker.cs   # 抚摸冷却跟踪
│   ├── Core/                    # 核心基础设施 (5个文件)
│   │   ├── HarmonyInit.cs       # Harmony 初始化
│   │   ├── SylvieDefNames.cs    # Def 名称常量中心
│   │   ├── SylvieConstants.cs   # 全局常量定义
│   │   ├── SylvieComponentRegistry.cs    # 组件注册中心
│   │   └── SylvieStaticDataManager.cs    # 静态数据清理管理器
│   ├── Jobs/                    # 工作系统 (2个文件)
│   │   ├── JobGiver_SeekPetting.cs       # 寻求抚摸 AI 决策
│   │   └── JobDriver_SeekPetting.cs      # 抚摸交互执行
│   ├── Patches/                 # Harmony 补丁 (4个文件)
│   │   ├── Patch_FaceAnimationDef_IsSame.cs  # FA 种族匹配
│   │   ├── Patch_Stance_Warmup.cs            # 瞄准动画同步
│   │   ├── Patch_ResearchAnimation.cs        # 研究动画
│   │   └── Patch_CommsConsole.cs             # 通讯台补丁
│   ├── Pawns/                   # Pawn 生成 (1个文件)
│   │   └── SylviePawnGenerator.cs
│   ├── Hediffs/                 # 健康状态 (2个文件)
│   │   ├── CompNurseHeal.cs         # 护士服治疗技能
│   │   └── SylvieHediffManager.cs   # Hediff 管理
│   ├── Incidents/               # 事件 (1个文件)
│   │   └── IncidentWorker_SylvieTrader.cs
│   ├── Letters/                 # 信件 (1个文件)
│   │   └── ChoiceLetter_SylvieOffer.cs
│   ├── Debug/                   # 调试功能 (1个文件)
│   │   └── SylvieDebugActions.cs
│   └── Utils/                   # 工具类 (2个文件)
│       ├── SylvieValidationUtils.cs      # 通用验证工具
│       └── SylvieFactionValidator.cs     # 派系验证工具
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
│  │  ┌─────────────────┐                                                  │   │
│  │  │SylvieStaticData │                                                  │   │
│  │  │    Manager      │                                                  │   │
│  │  │ (静态数据清理)   │                                                  │   │
│  │  └─────────────────┘                                                  │   │
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
│  │  ┌──────────────┐ ┌──────────────┐ ┌──────────────┐ ┌─────────────┐ │   │
│  │  │SylvieCatEar  │ │SylvieCatEar  │ │SylvieSeek    │ │SylvieGame   │ │   │
│  │  │  RenderNode  │ │  NodeWorker  │ │PettingTracker│ │ Component  │ │   │
│  │  └──────────────┘ └──────────────┘ └──────────────┘ └─────────────┘ │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                              │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │                     Feature Layer (功能层)                           │   │
│  │  ┌──────────────┐ ┌──────────────┐ ┌──────────────┐ ┌─────────────┐ │   │
│  │  │   Race       │ │   Events     │ │   Apparel    │ │  Incidents  │ │   │
│  │  │  (种族系统)   │ │  (事件系统)   │ │  (服装系统)   │ │  (事件触发) │ │   │
│  │  └──────────────┘ └──────────────┘ └──────────────┘ └─────────────┘ │   │
│  │  ┌──────────────┐ ┌──────────────┐ ┌──────────────┐ ┌─────────────┐ │   │
│  │  │   Hediffs    │ │    Jobs      │ │   Letters    │ │   Patches   │ │   │
│  │  │  (健康系统)   │ │  (工作系统)   │ │  (信件系统)   │ │  (Harmony)  │ │   │
│  │  └──────────────┘ └──────────────┘ └──────────────┘ └─────────────┘ │   │
│  │  ┌──────────────┐ ┌──────────────┐ ┌──────────────┐                 │   │
│  │  │   Pawns      │ │    Debug     │ │    Utils     │                 │   │
│  │  │  (Pawn生成)   │ │  (调试功能)   │ │  (工具类)    │                 │   │
│  │  └──────────────┘ └──────────────┘ └──────────────┘                 │   │
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
| `SylvieStaticDataManager` | [Core/SylvieStaticDataManager.cs](Core/SylvieStaticDataManager.cs) | 静态数据清理管理器，防止内存泄漏和场景间数据污染 |
| `SylvieAnimationRegistry` | [Animation/SylvieAnimationRegistry.cs](Animation/SylvieAnimationRegistry.cs) | 统一管理 FaceAnimation 到 Pawn 的映射关系 |
| `SylvieAnimationHelper` | [Animation/SylvieAnimationHelper.cs](Animation/SylvieAnimationHelper.cs) | 提供动画帧计算、反射访问等通用功能 |
| `SylvieAimingTracker` | [Components/SylvieAimingTracker.cs](Components/SylvieAimingTracker.cs) | 追踪 Sylvie Pawn 的瞄准状态，与面部动画同步 |
| `SylvieCooldownTracker` | [Components/SylvieCooldownTracker.cs](Components/SylvieCooldownTracker.cs) | 冷却状态跟踪器，管理所有 Sylvie 的冷却计时 |
| `SylvieCooldownOverlayComp` | [Components/SylvieCooldownOverlayComp.cs](Components/SylvieCooldownOverlayComp.cs) | 冷却动画渲染组件，处理瞄准冷却期间的视觉效果 |
| `SylvieCatEarComp` | [Components/SylvieCatEarComp.cs](Components/SylvieCatEarComp.cs) | 猫耳渲染组件，管理渲染状态和帧切换 |
| `SylvieCatEarRenderNode` | [Components/SylvieCatEarRenderNode.cs](Components/SylvieCatEarRenderNode.cs) | 猫耳渲染节点，继承 PawnRenderNode 实现与动画 Mod 的完美同步 |
| `SylvieCatEarNodeWorker` | [Components/SylvieCatEarNodeWorker.cs](Components/SylvieCatEarNodeWorker.cs) | 猫耳渲染工作器，处理偏移计算和渲染开关 |
| `SylvieSeekPettingTracker` | [Components/SylvieSeekPettingTracker.cs](Components/SylvieSeekPettingTracker.cs) | 抚摸冷却跟踪器，管理寻求抚摸的冷却时间（6小时=15000ticks），存档支持 |
| `SylvieGameComponent` | [Components/SylvieGameComponent.cs](Components/SylvieGameComponent.cs) | 游戏状态管理、存档版本控制、事件触发调度 |
| `SylviePawnGenerator` | [Pawns/SylviePawnGenerator.cs](Pawns/SylviePawnGenerator.cs) | 希尔薇 Pawn 生成与配置 |
| `JobGiver_SeekPetting` | [Jobs/JobGiver_SeekPetting.cs](Jobs/JobGiver_SeekPetting.cs) | AI 决策，7步条件检查，寻找最佳抚摸目标 |
| `JobDriver_SeekPetting` | [Jobs/JobDriver_SeekPetting.cs](Jobs/JobDriver_SeekPetting.cs) | 执行抚摸交互，应用心情、社交关系效果 |
| `SylvieHediffManager` | [Hediffs/SylvieHediffManager.cs](Hediffs/SylvieHediffManager.cs) | Hediff 管理，处理初始创伤机制的触发 |
| `CompNurseHeal` | [Hediffs/CompNurseHeal.cs](Hediffs/CompNurseHeal.cs) | 护士服主动治疗技能，包扎伤口并添加麻痹效果 |
| `SylvieValidationUtils` | [Utils/SylvieValidationUtils.cs](Utils/SylvieValidationUtils.cs) | 验证工具类，提供通用验证方法 |
| `SylvieFactionValidator` | [Utils/SylvieFactionValidator.cs](Utils/SylvieFactionValidator.cs) | 派系验证工具，提供有效派系筛选与验证（排除玩家/隐藏/已击败/临时派系，需有 Trader） |
| `SylvieDebugActions` | [Debug/SylvieDebugActions.cs](Debug/SylvieDebugActions.cs) | 调试功能，提供开发调试命令 |

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

**关键文件**:
- [Components/SylvieGameComponent.cs](Components/SylvieGameComponent.cs) - 事件调度与状态管理
- [Incidents/IncidentWorker_SylvieTrader.cs](Incidents/IncidentWorker_SylvieTrader.cs) - 商人事件触发
- [Utils/SylvieFactionValidator.cs](Utils/SylvieFactionValidator.cs) - 派系验证器

**系统架构**:

```
┌─────────────────────────────────────────────────────────────────┐
│                      事件系统架构                                │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  ┌──────────────────┐    ┌──────────────────┐                  │
│  │ SylvieGame       │    │ SylvieFaction    │                  │
│  │ Component        │───▶│ Validator        │                  │
│  │ (事件调度)        │    │ (派系筛选)        │                  │
│  └──────────────────┘    └────────┬─────────┘                  │
│           │                       │                             │
│           ▼                       ▼                             │
│  ┌──────────────────┐    ┌──────────────────┐                  │
│  │ IncidentWorker   │◀───│  有效派系列表     │                  │
│  │ SylvieTrader     │    │                  │                  │
│  │ (事件触发)        │    └──────────────────┘                  │
│  └────────┬─────────┘                                           │
│           │                                                      │
│           ▼                                                      │
│  ┌──────────────────┐    ┌──────────────────┐                  │
│  │ SylviePawn       │───▶│ 种族验证 + 重试  │                  │
│  │ Generator        │    │ (Max 5次)        │                  │
│  │ (Pawn生成)        │    └──────────────────┘                  │
│  └──────────────────┘                                           │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

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

**事件执行顺序机制** ([Incidents/IncidentWorker_SylvieTrader.cs](Incidents/IncidentWorker_SylvieTrader.cs#L38-L90)):

修复商队疯狂触发问题的核心机制：先验证并生成希尔薇，成功后再生成商队：

```csharp
// 步骤1：先尝试生成希尔薇
Pawn? sylvie = SylviePawnGenerator.GenerateSylvie(parms.faction);
if (sylvie == null)
{
    Log.Warning("[SylvieMod] Failed to generate Sylvie pawn, aborting incident");
    return false;  // 希尔薇生成失败，直接返回，不生成商队
}

// 步骤2：希尔薇生成成功，现在生成商队
if (!base.TryExecuteWorker(parms))
{
    // 商队生成失败，销毁已生成的希尔薇
    Log.Warning("[SylvieMod] Failed to spawn trader caravan, destroying generated Sylvie");
    sylvie.Destroy();
    return false;
}

// 步骤3：商队生成成功，找到 trader 并将希尔薇放到其旁边
// ... 放置希尔薇并发送信件
}
```

**关键设计决策**：
- 先调用 `SylviePawnGenerator.GenerateSylvie()` 确保希尔薇能成功生成
- 只有希尔薇生成成功后，才调用 `base.TryExecuteWorker()` 生成商队
- 如果希尔薇生成失败，事件直接返回 false，不会触发商队到达
- 如果商队生成失败，销毁已生成的希尔薇，避免内存泄漏
- 彻底解决商队疯狂触发但希尔薇不出现的问题

**派系选择机制** ([SylvieFactionValidator.cs](Utils/SylvieFactionValidator.cs)):

事件系统不再绑定特定派系（如温和部落），而是通过 `SylvieFactionValidator` 动态筛选有效派系：

```csharp
// 验证派系是否适合触发事件
public static bool IsValidFaction(Faction? faction)
{
    if (faction == null) return false;
    if (faction.IsPlayer) return false;           // 排除玩家派系
    if (faction.Hidden) return false;             // 排除隐藏派系
    if (faction.defeated) return false;           // 排除已击败派系
    if (faction.temporary) return false;          // 排除临时派系
    if (faction.HostileTo(Faction.OfPlayer)) return false;  // 排除敌对派系
    if (faction.def.caravanTraderKinds.NullOrEmpty()) return false;  // 必须有商队
    // 必须有 Trader PawnGroupMaker
    if (!faction.def.pawnGroupMakers.Any(pgm => 
        pgm.kindDef == PawnGroupKindDefOf.Trader)) return false;
    return true;
}
```

**种族正确性保证机制** ([SylviePawnGenerator.cs](Pawns/SylviePawnGenerator.cs#L64-L98)):

Pawn 生成包含种族验证和重生成机制，确保生成的始终是希尔薇种族：

```csharp
// 尝试生成，带重试机制
for (int attempt = 0; attempt < MaxGenerationAttempts; attempt++)
{
    Pawn? pawn = TryGeneratePawnInternal(faction, pawnKindDef);
    if (pawn == null) continue;

    // 验证种族是否正确
    if (pawn.def.defName != SylvieDefNames.Race_Sylvie)
    {
        Log.Warning($"[SylvieMod] Generated pawn has wrong race: {pawn.def.defName}");
        pawn.Destroy();  // 销毁错误种族的 Pawn
        continue;        // 继续下一次尝试
    }

    // 种族正确，配置属性
    ConfigureName(pawn);
    ConfigureGenes(pawn);
    ConfigureTraits(pawn);
    ConfigureTattoos(pawn);
    return pawn;
}
```

**时间常量** ([SylvieConstants.cs](Core/SylvieConstants.cs#L26-L52)):
- 检查间隔: 2500 ticks (~41.7秒 @60TPS) - `CheckIntervalTicks`
- 初始延迟: 240000 ticks (4 游戏日) - `InitialEventDelayTicks`
- Hediff 延迟: 300000 ticks (5 游戏日) - `HediffDelayTicks`
- 护士服治疗间隔: 600 ticks (10秒 @60TPS) - `NurseHealIntervalTicks`
- Pawn 生成重试次数: 5 次 - `MaxGenerationAttempts`

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

**配套追踪器**: [Components/SylvieCooldownTracker.cs](Components/SylvieCooldownTracker.cs) - 管理冷却状态和动画帧计算

**渲染层级** ([SylvieConstants.cs](Core/SylvieConstants.cs#L54-L82)):
```csharp
public const float SweatRenderLayer = 61.1f;     // 汗液
public const float MagazineRenderLayer = 61.2f;  // 弹匣
public const float BulletRenderLayer = 61.3f;    // 子弹
```

**朝向处理** ([SylvieCooldownOverlayComp.cs](Components/SylvieCooldownOverlayComp.cs#L228-L244)):
```csharp
/// <summary>
/// Renders a graphic at the specified position with the given rotation.
/// </summary>
private void RenderGraphic(Graphic graphic, Rot4 rotation, Vector3 position, Vector3 scale, float? layerOverride = null)
{
    Material mat = graphic.MatAt(rotation);
    Mesh mesh = graphic.MeshAt(rotation);

    if (mat == null)
        return;

    Vector3 renderPos = position;
    if (layerOverride.HasValue)
    {
        renderPos.y = Pawn.DrawPos.y + PawnRenderUtility.AltitudeForLayer(layerOverride.Value);
    }

    Matrix4x4 matrix = Matrix4x4.TRS(renderPos, Quaternion.identity, scale);
    Graphics.DrawMesh(mesh, matrix, mat, 0);
}
```

**缩放适配** ([SylvieCooldownOverlayComp.cs](Components/SylvieCooldownOverlayComp.cs#L147-L166)):
```csharp
/// <summary>
/// Calculates the draw scale based on head size factor.
/// </summary>
private Vector3 CalculateDrawScale()
{
    float headSizeFactor = GetHeadSizeFactor();
    return DrawScale * headSizeFactor;
}

/// <summary>
/// Gets the head size factor from the pawn's life stage.
/// </summary>
private float GetHeadSizeFactor()
{
    if (ModsConfig.BiotechActive && Pawn.ageTracker.CurLifeStage.headSizeFactor.HasValue)
    {
        return Pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
    }
    return 1f;
}
```

**动画状态计算** ([SylvieCooldownTracker.cs](Components/SylvieCooldownTracker.cs#L159-L213)):
```csharp
/// <summary>
/// Gets the current sweat animation frame (1-3) based on cooldown progress.
/// </summary>
public int GetSweatFrame()
{
    float progress = CooldownProgress;
    if (progress < SweatFrame1Threshold) return 1;
    if (progress < SweatFrame2Threshold) return 2;
    return 3;
}

/// <summary>
/// Gets the current bullet animation state.
/// </summary>
public (int insertFrame, int bulletCount) GetBulletAnimationState()
{
    float progress = CooldownProgress;

    int cycle = (int)(progress * BulletAnimationCycles);
    if (cycle >= BulletAnimationCycles) cycle = BulletAnimationCycles - 1;

    float cycleProgress = (progress * BulletAnimationCycles) - cycle;

    int insertFrame = CalculateInsertFrame(cycleProgress);
    int bulletCount = CalculateBulletCount(cycle);

    return (insertFrame, bulletCount);
}
```

#### 猫耳渲染系统

基于 PawnRenderNode 机制，实现与 Yayo's Animation 等动画 Mod 的完美同步。

**核心组件**:

| 组件 | 文件 | 职责 |
|------|------|------|
| `SylvieCatEarComp` | [Components/SylvieCatEarComp.cs](Components/SylvieCatEarComp.cs) | 管理渲染状态（`ShouldRender`、`CurrentEarFrame`），重写 `CompRenderNodes()` |
| `SylvieCatEarRenderNode` | [Components/SylvieCatEarRenderNode.cs](Components/SylvieCatEarRenderNode.cs) | 继承 PawnRenderNode，作为 Head 节点的子节点，自动继承变换矩阵 |
| `SylvieCatEarNodeWorker` | [Components/SylvieCatEarNodeWorker.cs](Components/SylvieCatEarNodeWorker.cs) | 继承 PawnRenderNodeWorker，处理渲染开关和偏移计算 |

**PawnRenderNode 机制** ([SylvieCatEarComp.cs](Components/SylvieCatEarComp.cs#L95-L121)):

```csharp
public override List<PawnRenderNode>? CompRenderNodes()
{
    // 创建 PawnRenderNodeProperties
    var props = new PawnRenderNodeProperties
    {
        debugLabel = "SylvieCatEar",
        workerClass = typeof(SylvieCatEarNodeWorker),
        baseLayer = EarLayer,  // 74f
        parentTagDef = PawnRenderNodeTagDefOf.Head  // 作为 Head 子节点
    };
    
    // 创建并返回渲染节点
    var node = new SylvieCatEarRenderNode(Pawn, props, Pawn.Drawer.renderer.renderTree, this);
    return new List<PawnRenderNode> { node };
}
```

**与 Yayo's Animation 的兼容性原理**:

Yayo's Animation 在研究动画中添加了让 Pawn 左右摇晃的效果。原来的 `PostDraw` 方案使用 `Pawn.DrawPos` 计算猫耳位置，导致猫耳固定在地面相对位置不动。

**解决方案**:

1. **渲染树结构**: RimWorld 1.6 使用 `PawnRenderTree` 管理所有渲染节点
2. **父子关系**: 猫耳节点设置为 Head 节点的子节点（`parentTagDef = PawnRenderNodeTagDefOf.Head`）
3. **变换继承**: 子节点自动继承父节点的变换矩阵（位置、旋转、缩放）
4. **动画同步**: Yayo's Animation 在 `ParallelGetPreRenderResults` 阶段修改 Head 节点的位置，猫耳节点自动跟随

**该方案的普适性原理**:

该方案不仅兼容 Yayo's Animation，还兼容任何修改 Pawn 渲染位置的动画 Mod：
- **矩阵级变换传递**: 所有变换通过渲染树的矩阵计算传递，而非直接操作位置坐标
- **相对位置保持**: 猫耳始终保持相对于 Head 的正确位置，不受父节点绝对位置变化影响
- **框架级兼容性**: 基于 RimWorld 原生 PawnRenderNode 系统，无需针对特定 Mod 做特殊处理
- **可扩展性**: 任何遵循 RimWorld 渲染规范的动画 Mod 都能自动兼容

**渲染开关控制** ([SylvieCatEarNodeWorker.cs](Components/SylvieCatEarNodeWorker.cs#L22-L43)):

```csharp
public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
{
    // 检查基础条件
    if (!base.CanDrawNow(node, parms))
    {
        return false;
    }

    // 检查是否应该渲染猫耳
    if (node is SylvieCatEarRenderNode catEarNode)
    {
        var comp = parms.pawn.GetComp<SylvieCatEarComp>();
        if (comp == null || !comp.ShouldRender)
        {
            return false;
        }
    }
    return true;
}
```

**反射调用与错误处理** ([SylvieCatEarRenderNode.cs](Components/SylvieCatEarRenderNode.cs#L95-L110)):

猫耳渲染需要在帧切换时触发渲染树重缓存。由于 `requestRecache` 是私有字段，需要使用反射访问。生产级代码必须包含完善的错误处理：

```csharp
/// <summary>
/// 请求重新缓存此节点。
/// 这会确保渲染树重新评估使用哪个图形。
/// </summary>
private void RequestRecache()
{
    try
    {
        // 将 requestRecache 字段设为 true 以触发重新初始化
        // 此字段在 EnsureInitialized 中通过 RecacheRequested 属性检查
        var field = typeof(PawnRenderNode).GetField("requestRecache", 
            System.Reflection.BindingFlags.NonPublic | 
            System.Reflection.BindingFlags.Instance);
        field?.SetValue(this, true);
    }
    catch (Exception ex)
    {
        Log.Warning($"[SylvieMod] Failed to request recache for cat ear node: {ex.Message}");
    }
}
```

**关键设计要点**:
- **防御性编程**: 反射操作包裹在 try-catch 中，防止字段不存在或访问权限问题导致崩溃
- **优雅降级**: 即使反射失败，也只是无法触发动画帧切换，不会破坏整体渲染
- **日志记录**: 使用 `Log.Warning` 记录错误，便于调试但不会影响用户体验
- **空值检查**: 使用 `field?.SetValue` 确保即使获取字段失败也不会抛出 NullReferenceException

**类关系图**:

```
┌─────────────────────────────────────────────────────────────────┐
│                     PawnRenderTree                               │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │                    Head Node                             │   │
│  │  ┌─────────────────────────────────────────────────┐   │   │
│  │  │           SylvieCatEarRenderNode                 │   │   │
│  │  │  ┌─────────────────────────────────────────┐   │   │   │
│  │  │  │      SylvieCatEarNodeWorker            │   │   │   │
│  │  │  │  - CanDrawNow()                        │   │   │   │
│  │  │  │  - OffsetFor()                         │   │   │   │
│  │  │  │  - GetFinalizedMaterial()              │   │   │   │
│  │  │  └─────────────────────────────────────────┘   │   │   │
│  │  │  - GraphicFor()                                │   │   │
│  │  │  - RequestRecache() (带错误处理)               │   │   │
│  │  └─────────────────────────────────────────────────┘   │   │
│  │  (继承变换矩阵，包括 Yayo's Animation 偏移)              │   │
│  └─────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
                    ┌─────────────────┐
                    │ SylvieCatEarComp │
                    │ - ShouldRender   │
                    │ - CurrentEarFrame│
                    │ - CompRenderNodes│
                    └─────────────────┘
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

## 存档兼容性

### 版本控制系统

**存档版本字段**: `saveDataVersion`

[SylvieGameComponent.cs](Components/SylvieGameComponent.cs#L13-L15):
```csharp
// 存档版本控制
private const int CURRENT_SAVE_VERSION = 1;
private int saveDataVersion = 1;
```

**数据迁移支持** ([SylvieGameComponent.cs](Components/SylvieGameComponent.cs#L107-L134)):
```csharp
public override void ExposeData()
{
    base.ExposeData();

    // 存档版本控制
    Scribe_Values.Look(ref saveDataVersion, "saveDataVersion", 1);

    // 数据迁移
    if (Scribe.mode == LoadSaveMode.LoadingVars && saveDataVersion < CURRENT_SAVE_VERSION)
    {
        PerformDataMigration();
        saveDataVersion = CURRENT_SAVE_VERSION;
    }

    Scribe_Values.Look(ref hasSylvieSpawned, "hasSylvieSpawned");
    Scribe_References.Look(ref sylviePawn, "sylviePawn");
    Scribe_Values.Look(ref hediffTriggerTick, "hediffTriggerTick", -1);
    Scribe_Values.Look(ref hediffTriggered, "hediffTriggered");
}

/// <summary>
/// 执行数据迁移。
/// </summary>
private void PerformDataMigration()
{
    Log.Message($"[SylvieMod] Migrating save data from v{saveDataVersion} to v{CURRENT_SAVE_VERSION}");
    // 当前版本无需特殊迁移逻辑，未来版本可在此添加
}
```

### ThingComp 序列化实现

所有 ThingComp 组件必须正确实现 `PostExposeData` 方法以确保存档兼容性。

**SylvieCatEarComp.PostExposeData** ([Components/SylvieCatEarComp.cs](Components/SylvieCatEarComp.cs#L130-L136)):
```csharp
public override void PostExposeData()
{
    base.PostExposeData();
    Scribe_Values.Look(ref currentEarFrame, "currentEarFrame", 0);
    Scribe_Values.Look(ref shouldRender, "shouldRender", false);
}
```

**SylvieCooldownTracker.PostExposeData** ([Components/SylvieCooldownTracker.cs](Components/SylvieCooldownTracker.cs#L238-L246)):
```csharp
public override void PostExposeData()
{
    base.PostExposeData();
    Scribe_Values.Look(ref cooldownStartTick, "cooldownStartTick", -1);
    Scribe_Values.Look(ref totalCooldownTicks, "totalCooldownTicks", 0);
    // lastVerb 是运行时缓存，不需要序列化
}
```

### 组件注册机制

**组件注册中心** ([Core/SylvieComponentRegistry.cs](Core/SylvieComponentRegistry.cs)):

```csharp
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

/// <summary>
/// 为指定的 Pawn 注册所有缺失的 Sylvie 组件。
/// </summary>
public static void RegisterAllComponents(Pawn pawn)
{
    if (pawn == null || !SylvieDefNames.IsSylvieRace(pawn))
        return;

    foreach (var compType in RegisteredComponentTypes)
    {
        RegisterComponentIfMissing(pawn, compType);
    }
}

/// <summary>
/// 检查 Pawn 是否已拥有指定类型的组件（使用类型名称匹配）。
/// </summary>
private static bool HasComponent(Pawn pawn, Type compType)
{
    foreach (var comp in pawn.AllComps)
    {
        // 使用 Name 进行类型匹配，避免序列化后的类型不匹配问题
        if (comp.GetType().Name == compType.Name)
        {
            return true;
        }
    }
    return false;
}
```

**LoadedGame 延迟注册** ([SylvieGameComponent.cs](Components/SylvieGameComponent.cs#L139-L148)):
```csharp
/// <summary>
/// 游戏加载完成后调用，支持存档中途加入。
/// </summary>
public override void LoadedGame()
{
    base.LoadedGame();

    // 延迟一帧执行组件注册，确保所有 Pawn 已完全初始化
    LongEventHandler.ExecuteWhenFinished(() =>
    {
        RegisterComponentsForExistingSylvie();
    });
}
```

**延迟注册实现** ([SylvieGameComponent.cs](Components/SylvieGameComponent.cs#L153-L174)):
```csharp
private void RegisterComponentsForExistingSylvie()
{
    int registeredCount = 0;
    
    foreach (Map map in Find.Maps)
    {
        foreach (Pawn pawn in map.mapPawns.AllPawnsSpawned)
        {
            if (SylvieDefNames.IsSylvieRace(pawn))
            {
                SylvieComponentRegistry.RegisterAllComponents(pawn);
                registeredCount++;
                Log.Message($"[SylvieMod] Registered components for existing Sylvie: {pawn.LabelShort}");
            }
        }
    }
    
    if (registeredCount > 0)
    {
        Log.Message($"[SylvieMod] Total Sylvie pawns registered: {registeredCount}");
    }
}
```

### 静态数据清理

**SylvieStaticDataManager 作用** ([Core/SylvieStaticDataManager.cs](Core/SylvieStaticDataManager.cs)):

```csharp
/// <summary>
/// 静态数据管理器，负责在游戏重置时清理所有静态数据。
/// 防止内存泄漏和场景间数据污染。
/// </summary>
[StaticConstructorOnStartup]
public static class SylvieStaticDataManager
{
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
```

**静态字典清理**: 所有使用静态字典的组件都应提供清理方法，在场景切换时调用。

---

## 代码规范

### 命名规范

**常量**: 使用 `PascalCase`，描述性名称
```csharp
// 常量: PascalCase
public const string Race_Sylvie = "Sylvie_Race";
public const int CooldownTicks = 15000;
public const float SweatRenderLayer = 61.1f;
```

**类/结构体**: 使用 `PascalCase`，前缀 `Sylvie` 避免命名冲突
```csharp
// 类: PascalCase
public class SylvieGameComponent : GameComponent
public static class SylvieDefNames
public class SylvieCooldownTracker : ThingComp
```

**方法**: 使用 `PascalCase`，动词开头
```csharp
// 方法: PascalCase
public static bool IsSylvieRace(Pawn? pawn)
public static void RegisterAllComponents(Pawn pawn)
public static int CalculateFrameIndex(int elapsedTicks, int frameCount)
```

**私有字段**: 使用 `camelCase`
```csharp
// 私有字段: camelCase
private int lastUseTick = -999999;
private Pawn? cachedPawn;
private static readonly Dictionary<Pawn, int> cooldownTrackers = new();
```

**静态只读字段**: 使用 `PascalCase`
```csharp
// 静态只读字段: PascalCase
private static readonly List<Type> RegisteredComponentTypes = new()
{
    typeof(SylvieAimingTracker),
    typeof(SylvieCooldownTracker),
    typeof(SylvieCooldownOverlayComp),
    typeof(SylvieCatEarComp)
};
```

### 可空引用

项目启用可空引用类型 (`#nullable enable`)，必须明确标记可空性：

```csharp
#nullable enable

// 可空类型明确标记
public static HediffDef? Hediff_InitialTraumaDef { get; }
public Pawn? GetAssociatedPawn(FaceAnimation animation)

// 非空类型（编译器保证不为 null）
public Pawn Pawn => cachedPawn ??= (parent as Pawn)!;

// 空值检查使用 ?. 运算符
if (pawn?.def?.defName == Race_Sylvie)

// 空值合并运算符提供默认值
int opinion = sylvie.relations?.OpinionOf(candidate) ?? 0;
```

**可空引用最佳实践**:
- 返回引用类型时，如果可能返回 null，使用 `?` 标记
- 方法参数如果接受 null，使用 `?` 标记
- 使用 `!` 后缀断言非空时要确保运行时确实不为 null
- 优先使用 `?.` 和 `??` 运算符进行空值处理

### 文档注释

所有公共 API 必须包含 XML 文档注释：

```csharp
/// <summary>
/// 检查指定的 Pawn 是否为希尔薇种族。
/// </summary>
/// <param name="pawn">要检查的 Pawn</param>
/// <returns>如果是希尔薇种族返回 true，否则返回 false</returns>
public static bool IsSylvieRace(Pawn? pawn)

/// <summary>
/// 计算动画帧索引，支持循环播放。
/// </summary>
/// <param name="elapsedTicks">已过去的 ticks</param>
/// <param name="frameCount">总帧数</param>
/// <returns>安全的帧索引</returns>
public static int CalculateFrameIndex(int elapsedTicks, int frameCount)

/// <summary>
/// 为指定的 Pawn 注册所有缺失的 Sylvie 组件。
/// </summary>
/// <param name="pawn">目标 Pawn</param>
public static void RegisterAllComponents(Pawn pawn)
```

**文档注释要求**:
- `<summary>`: 描述功能
- `<param>`: 描述参数用途
- `<returns>`: 描述返回值
- 复杂逻辑使用 `<remarks>` 补充说明

---

## 开发指南

### 添加新组件

**步骤 1**: 创建组件类，继承 `ThingComp`

```csharp
#nullable enable
using Verse;

namespace SylvieMod;

/// <summary>
/// 新组件的描述。
/// </summary>
public class SylvieNewComp : ThingComp
{
    #region Fields
    
    private int myField = 0;
    
    #endregion
    
    #region Properties
    
    public Pawn Pawn => (Pawn)parent;
    
    #endregion
    
    #region Lifecycle
    
    public override void CompTick()
    {
        base.CompTick();
        // 每 tick 更新逻辑
    }
    
    public override void PostDraw()
    {
        base.PostDraw();
        // 渲染逻辑（已过时，推荐使用 PawnRenderNode）
    }
    
    #endregion
    
    #region Save Compatibility
    
    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref myField, "myField", 0);
    }
    
    #endregion
}
```

**步骤 2**: 在 `SylvieComponentRegistry.cs` 中注册组件类型

```csharp
// 文件: Source/Core/SylvieComponentRegistry.cs
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

组件注册由 `SylvieComponentRegistry.RegisterAllComponents` 自动处理，在以下时机触发：
- **新游戏**: Pawn 生成时通过 Harmony 补丁
- **存档加载**: `SylvieGameComponent.LoadedGame()` 延迟注册

```csharp
// 注册流程（无需手动调用）
SylvieComponentRegistry.RegisterAllComponents(pawn);
```

**组件开发注意事项**:
- 必须实现 `PostExposeData()` 确保存档兼容性
- 使用 `cachedPawn` 模式缓存 Pawn 引用以提高性能
- 静态字典需要在 `SylvieStaticDataManager.ClearAllStaticData()` 中清理

---

### 使用动画注册系统

**动画注册系统架构**:

动画注册系统用于建立 `FaceAnimation` 实例与 `Pawn` 之间的映射关系，使补丁能够获取当前动画所属的 Pawn。

**注册动画**:

```csharp
// 注册单个动画
SylvieAnimationRegistry.RegisterAnimation(animation, pawn);

// 批量注册特定类型的动画
SylvieAnimationRegistry.RegisterAnimationsByType(
    animationDict: ___animationDict,
    pawn: ___pawn,
    targetAnimationDefName: SylvieDefNames.Animation_Aiming
);

// 注册所有动画并触发类型特定的回调
SylvieAnimationRegistry.RegisterAllAnimations(
    animationDict: ___animationDict,
    pawn: ___pawn
);
```

**查询关联 Pawn**:

```csharp
// 在 Harmony 补丁中获取动画关联的 Pawn
Pawn? pawn = SylvieAnimationRegistry.GetAssociatedPawn(animation);
if (pawn != null)
{
    // 处理动画逻辑
    Stance? curStance = pawn.stances?.curStance;
    // ...
}
```

**注册类型特定的处理回调**:

```csharp
// 为特定动画类型注册处理回调
SylvieAnimationRegistry.RegisterAnimationType(
    animationDefName: "Sylvie_AimingAnimation",
    onRegister: (animation, pawn) => {
        // 自定义处理逻辑
        Log.Message($"[SylvieMod] Registered aiming animation for {pawn.LabelShort}");
    }
);
```

**使用场景示例** (Patch_Stance_Warmup.cs):

```csharp
[HarmonyPatch(typeof(FaceAnimation), nameof(FaceAnimation.GetCurrentFrame))]
public static class Patch_FaceAnimation_GetCurrentFrame
{
    public static bool Prefix(FaceAnimation __instance, int tickGame, ref FaceAnimationDef.AnimationFrame? __result)
    {
        // 使用动画注册系统获取关联 Pawn
        Pawn? pawn = SylvieAnimationRegistry.GetAssociatedPawn(__instance);
        if (pawn == null) return true; // 不是 Sylvie，调用原版方法
        
        // 处理 Sylvie 特定的动画逻辑
        // ...
        
        return false; // 跳过原版方法
    }
}
```

---

### 使用动画辅助工具

动画辅助工具提供动画帧计算、反射访问等通用功能。

**计算帧索引**:

```csharp
// 简单循环帧计算
// 适用于等时长帧的循环动画
int frameIndex = SylvieAnimationHelper.CalculateFrameIndex(
    elapsedTicks: elapsedTicks,
    frameCount: frames.Count
);

// 基于持续时间的帧计算
// 适用于每帧有不同 duration 的动画
int frameIndex = SylvieAnimationHelper.CalculateOriginalFrameIndex(
    elapsedTicks: elapsedTicks,
    frames: frames
);

// 计算 warmup 进度 (0.0 - 1.0)
float progress = SylvieAnimationHelper.CalculateWarmupProgress(
    ticksLeft: warmup.ticksLeft,
    totalWarmupTicks: totalWarmupTicks
);
```

**获取动画帧**:

```csharp
// 安全地获取动画帧列表
var frames = SylvieAnimationHelper.GetAnimationFrames(animationDef);
if (SylvieAnimationHelper.HasValidFrames(frames))
{
    // 处理帧
    int frameIndex = SylvieAnimationHelper.CalculateFrameIndex(elapsedTicks, frames.Count);
    __result = frames[frameIndex];
    return false; // 跳过原版方法
}
```

**反射获取私有字段**:

```csharp
// 获取引用类型的私有字段
T? value = SylvieAnimationHelper.GetPrivateField<T>(obj, "fieldName");

// 获取值类型的私有字段
int startTick = SylvieAnimationHelper.GetPrivateFieldValue<int>(animation, "startTick");

// 实际使用示例：获取动画的 startTick
public static int GetStartTick(FaceAnimation animation)
{
    return SylvieAnimationHelper.GetPrivateFieldValue<int>(animation, "startTick");
}
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

**完整使用示例** (Patch_Stance_Warmup.cs):

```csharp
public static bool Prefix(FaceAnimation __instance, int tickGame, ref FaceAnimationDef.AnimationFrame? __result)
{
    // 获取关联 Pawn
    Pawn? pawn = SylvieAnimationRegistry.GetAssociatedPawn(__instance);
    if (pawn == null) return true;
    
    // 使用辅助工具获取动画帧
    var frames = SylvieAnimationHelper.GetAnimationFrames(__instance.animationDef);
    if (!SylvieAnimationHelper.HasValidFrames(frames)) return true;
    
    // 计算帧索引
    int elapsedTicks = totalWarmupTicks - warmup.ticksLeft;
    int frameIndex = SylvieAnimationHelper.CalculateFrameIndex(elapsedTicks, frames!.Count);
    
    __result = frames[frameIndex];
    return false;
}
```

---

### 添加 Def 名称

所有 Def 名称必须在 `SylvieDefNames.cs` 中集中定义，确保一致性和可维护性。

**添加步骤**:

**1. 添加常量定义** ([SylvieDefNames.cs](Core/SylvieDefNames.cs#L12-L58)):

```csharp
// 在相应的区域添加常量
#region Def Name Constants

// 事件相关
public const string Incident_ArrivalEvent = "Sylvie_ArrivalEvent";

// 新增 Def 名称
public const string MyNewDef = "MyNewDefName";

#endregion
```

**2. 添加延迟加载属性**:

```csharp
#region Cached Def Accessors

// Hediff
public static HediffDef? Hediff_InitialTraumaDef => GetDef<HediffDef>(Hediff_InitialTrauma);

// 新增 Def 访问器
public static MyDefType? MyNewDefDef => GetDef<MyDefType>(MyNewDef);

#endregion
```

**3. 或使用通用辅助方法**:

```csharp
// 通用辅助方法（已存在）
private static TDef? GetDef<TDef>(string defName) where TDef : Def
{
    return DefDatabase<TDef>.GetNamed(defName, false);
}

// 使用示例
var myDef = SylvieDefNames.GetDef<MyDefType>("MyNewDefName");
```

**命名规范**:
- 常量: `DefType_DefName` (如 `Hediff_InitialTrauma`)
- 属性: `DefType_DefNameDef` (如 `Hediff_InitialTraumaDef`)
- 使用 `Sylvie` 前缀避免与其他 Mod 冲突

---

### 使用派系验证工具

**SylvieFactionValidator** 提供集中式的派系验证逻辑，确保生成的 Pawn 属于有效派系。

**检查是否存在有效派系**:

```csharp
// 在生成 Pawn 前检查
if (!SylvieFactionValidator.HasAnyValidFaction())
{
    Log.Warning("[SylvieMod] 没有可用的有效派系，跳过生成");
    return false;
}
```

**获取随机有效派系**:

```csharp
// 获取一个随机的有效派系用于 Pawn 生成
Faction? validFaction = SylvieFactionValidator.GetValidFaction();
if (validFaction == null)
{
    Log.Error("[SylvieMod] 无法获取有效派系");
    return null;
}

// 使用派系生成 Pawn
PawnGenerationRequest request = new(
    kind: PawnKindDef.Named(SylvieDefNames.PawnKind_Sylvie),
    faction: validFaction,  // 使用验证后的派系
    context: PawnGenerationContext.NonPlayer,
    tile: tile,
    forceGenerateNewPawn: true
);
```

**有效派系判定标准**:

```csharp
// SylvieFactionValidator 内部逻辑
private static bool IsValidFaction(Faction faction)
{
    // 排除已销毁的派系
    if (faction == null || faction.IsPlayer || faction.defeated)
        return false;
    
    // 排除隐藏派系
    if (faction.def.hidden)
        return false;
    
    // 可以添加其他自定义规则
    return true;
}
```

**使用场景示例**:

- **事件生成**: 确保随机事件生成的 Sylvie 属于有效派系
- **地图生成**: 在地图初始化时验证派系可用性
- **救援/俘虏**: 确保救援的 Sylvie 来自合法派系

---

### 调试技巧

**日志输出**:

```csharp
// 普通信息日志
Log.Message($"[SylvieMod] 调试信息: {variable}");

// 警告日志（黄色）
Log.Warning($"[SylvieMod] 警告: 某些问题发生");

// 错误日志（红色，弹窗）
Log.Error($"[SylvieMod] 错误: {ex.Message}");
```

**条件断点**:

```csharp
// 特定 Pawn 触发断点
if (pawn.LabelShort == "希尔薇")
{
    Log.Warning("[SylvieMod] 断点触发 - 希尔薇执行到此处");
}

// 特定条件触发
if (SylvieDefNames.IsSylvieRace(pawn) && pawn.Downed)
{
    Log.Warning($"[SylvieMod] 断点触发 - 希尔薇倒地");
}
```

**动画调试**:

```csharp
// 验证动画有效性
if (SylvieAnimationHelper.IsValidAnimation(animation))
{
    Log.Message($"[SylvieMod] 动画: {animation.animationDef.defName}");
}

// 检查动画帧
var frames = SylvieAnimationHelper.GetAnimationFrames(animationDef);
if (SylvieAnimationHelper.HasValidFrames(frames))
{
    Log.Message($"[SylvieMod] 动画帧数: {frames!.Count}");
}
```

**组件调试**:

```csharp
// 检查组件是否存在
var comp = pawn.GetComp<SylvieCatEarComp>();
if (comp != null)
{
    Log.Message($"[SylvieMod] 猫耳组件存在: ShouldRender={comp.ShouldRender}");
}

// 检查所有组件
foreach (var comp in pawn.AllComps)
{
    Log.Message($"[SylvieMod] 组件: {comp.GetType().Name}");
}
```

**调试操作**:

游戏中使用开发者模式（Developer Mode）：
- 按 `~` 键打开日志窗口
- 查看 `[SylvieMod]` 前缀的日志消息
- 使用 `Ctrl+F12` 打开完整日志

---

## 技术经验教训

### Letter 创建：必须使用 LetterMaker.MakeLetter

**错误做法** (会导致 LoadID 冲突):
```csharp
// 不要这样做！
var letter = new ChoiceLetter_SylvieOffer();
letter.title = "Title";
Find.LetterStack.ReceiveLetter(letter);
```

**正确做法**:
```csharp
// 使用 LetterMaker 创建
ChoiceLetter_SylvieOffer letter = (ChoiceLetter_SylvieOffer)LetterMaker.MakeLetter(
    def: LetterDefOf.ThreatBig,  // 或自定义 LetterDef
    title: "Title".Translate(),
    text: "Text".Translate()
);
Find.LetterStack.ReceiveLetter(letter);
```

**原因**: RimWorld 使用 LoadID 系统来序列化对象引用。直接实例化 Letter 会导致 LoadID 未正确分配，在存档加载时引发冲突。

**经验总结**:
- 所有继承 `Letter` 的类都必须通过 `LetterMaker.MakeLetter` 创建
- 自定义 Letter 类需要正确实现 `ExposeData()` 方法
- 引用类型字段使用 `Scribe_References` 序列化

### 组件注册时机：加载存档后需要延迟注册

**问题**: 在 `LoadedGame` 中直接访问 `Find.Maps` 或 Pawn 数据时，游戏可能尚未完全初始化。

**错误做法**:
```csharp
public override void LoadedGame()
{
    base.LoadedGame();
    // 此时 Find.Maps 可能为 null 或 Pawn 尚未完全加载
    RegisterComponentsForExistingSylvie(); // 可能失败
}
```

**解决方案** ([SylvieGameComponent.cs](Components/SylvieGameComponent.cs#L139-L148)):
```csharp
public override void LoadedGame()
{
    base.LoadedGame();

    // 延迟一帧执行，确保所有 Pawn 已完全初始化
    LongEventHandler.ExecuteWhenFinished(() =>
    {
        RegisterComponentsForExistingSylvie();
    });
}
```

**经验总结**:
- `LoadedGame` 在游戏加载完成后调用，但此时游戏对象可能尚未完全初始化
- `LongEventHandler.ExecuteWhenFinished` 确保在下一帧执行
- 这是支持存档中途加入 Mod 的关键

### 类型匹配：使用类型名称匹配而非直接类型比较

**问题**: `pawn.GetComp<T>()` 在跨版本加载或 Harmony 补丁后可能失败，因为类型可能来自不同的 Assembly 实例。

**错误做法**:
```csharp
// 类型比较可能失败
if (comp.GetType() == typeof(SylvieCatEarComp)) // 可能永远为 false
```

**解决方案** ([SylvieComponentRegistry.cs](Core/SylvieComponentRegistry.cs#L120-L132)):
```csharp
private static bool HasComponent(Pawn pawn, Type compType)
{
    foreach (var comp in pawn.AllComps)
    {
        // 使用 Name 进行类型匹配，避免序列化后的类型不匹配问题
        if (comp.GetType().Name == compType.Name)
        {
            return true;
        }
    }
    return false;
}
```

**经验总结**:
- 序列化后，类型可能来自不同的 Assembly 加载上下文
- 类型名称匹配是更可靠的方式
- 这在 Mod 更新后加载旧存档时尤为重要

### 静态数据：考虑存档加载时的清理问题

**问题**: 静态字典和缓存会在场景切换时保留，可能导致：
- 内存泄漏
- 旧存档数据污染新游戏
- 引用失效的对象（已销毁的 Pawn）

**错误做法**:
```csharp
// 静态字典会一直保留
private static readonly Dictionary<Pawn, int> Trackers = new();
// 如果不清理，新游戏会包含旧数据
```

**解决方案** ([SylvieStaticDataManager.cs](Core/SylvieStaticDataManager.cs)):

1. 创建 `SylvieStaticDataManager` 集中管理清理:
```csharp
[StaticConstructorOnStartup]
public static class SylvieStaticDataManager
{
    public static void ClearAllStaticData()
    {
        SylvieCooldownTracker.ClearTrackers();
        SylvieAimingTracker.ClearCache();
        SylvieAnimationRegistry.ClearRegistrations();
        SylvieComponentRegistry.ClearCache();
    }
}
```

2. 每个使用静态数据的组件提供清理方法:
```csharp
public static void ClearTrackers()
{
    Trackers.Clear();
}
```

**经验总结**:
- 所有静态集合都必须有清理机制
- 在游戏重置（返回主菜单）时调用清理
- 防止内存泄漏和场景间数据污染

### ExposeData：所有 ThingComp 必须正确实现

**问题**: 未正确实现 `PostExposeData` 会导致存档加载后数据丢失或错误。

**必须序列化的数据类型**:
- 基础类型: `int`, `float`, `bool`, `string`
- 复杂类型: `Vector3`, `Enum`
- 集合: `List<T>`, `Dictionary<K,V>`
- 引用: `Thing`, `Pawn` (使用 `Scribe_References`)
- 定义: `Def` 子类 (使用 `Scribe_Defs`)

**正确示例** ([SylvieCooldownTracker.cs](Components/SylvieCooldownTracker.cs#L238-L244)):
```csharp
public override void PostExposeData()
{
    base.PostExposeData();
    
    // 基础值类型
    Scribe_Values.Look(ref cooldownStartTick, "cooldownStartTick", -1);
    Scribe_Values.Look(ref totalCooldownTicks, "totalCooldownTicks", 0);
    
    // 集合类型
    Scribe_Collections.Look(ref myDict, "myDict", LookMode.Value, LookMode.Value);
    
    // 引用类型
    Scribe_References.Look(ref myPawn, "myPawn");
    
    // Def 类型
    Scribe_Defs.Look(ref myHediffDef, "myHediffDef");
}
```

**常见错误**:
- 忘记调用 `base.PostExposeData()`
- 运行时缓存数据被错误地序列化
- 引用类型使用 `Scribe_Values` 而非 `Scribe_References`

### Pawn 引用安全：添加死亡检查

**问题**: 保存的 Pawn 引用可能在加载后变为无效（死亡、被销毁），导致 NullReferenceException。

**错误做法**:
```csharp
public override void GameComponentTick()
{
    base.GameComponentTick();
    
    // 可能抛出 NullReferenceException
    if (sylviePawn != null)
    {
        sylviePawn.Position = newPosition; // Pawn 可能已死亡
    }
}
```

**解决方案** ([SylvieGameComponent.cs](Components/SylvieGameComponent.cs#L31-L37)):
```csharp
public override void GameComponentTick()
{
    base.GameComponentTick();

    // 检查 Sylvie 是否仍然有效
    if (sylviePawn != null && (sylviePawn.Dead || sylviePawn.Destroyed))
    {
        Log.Message("[SylvieMod] Sylvie is no longer alive, clearing reference");
        sylviePawn = null;
        hediffTriggered = true; // 防止继续触发 Hediff
    }
    
    // ... 其他逻辑
}
```

**经验总结**:
- 所有保存的 Pawn 引用都必须检查 `Dead` 和 `Destroyed`
- 在 `GameComponentTick` 中定期检查引用有效性
- 及时清理无效引用，防止后续逻辑出错

### 反射访问：必须包含错误处理

**问题**: 访问 RimWorld 私有字段时，如果字段不存在或访问权限变化，会导致崩溃。

**错误做法**:
```csharp
// 如果字段不存在，会抛出异常
var field = typeof(PawnRenderNode).GetField("requestRecache", BindingFlags.NonPublic | BindingFlags.Instance);
field.SetValue(this, true); // 可能抛出 NullReferenceException
```

**解决方案** ([SylvieCatEarRenderNode.cs](Components/SylvieCatEarRenderNode.cs#L95-L110)):
```csharp
private void RequestRecache()
{
    try
    {
        var field = typeof(PawnRenderNode).GetField("requestRecache", 
            System.Reflection.BindingFlags.NonPublic | 
            System.Reflection.BindingFlags.Instance);
        field?.SetValue(this, true); // 使用 ?. 避免 NullReferenceException
    }
    catch (Exception ex)
    {
        Log.Warning($"[SylvieMod] Failed to request recache: {ex.Message}");
    }
}
```

**经验总结**:
- 所有反射操作都必须包裹在 try-catch 中
- 使用 `?.` 运算符进行空值检查
- 使用 `Log.Warning` 而非 `Log.Error` 避免弹窗干扰用户
- 即使反射失败，也不应影响整体功能

### 动画朝向处理：使用 Graphic.MeshAt

**问题**: 冷却动画在不同朝向时，直接使用 `MeshPool.plane10` 会导致贴图翻转不正确。

**错误做法**:
```csharp
// 不要这样做 - 不会正确翻转 UV
Mesh mesh = MeshPool.plane10;
Vector3 scale = new Vector3(-1, 1, 1); // 负值缩放不会翻转 UV
```

**解决方案** ([SylvieCooldownOverlayComp.cs](Components/SylvieCooldownOverlayComp.cs#L111-L123)):
```csharp
// 正确做法: 使用 Graphic.MeshAt(rot) 自动处理翻转
Mesh mesh = graphic.MeshAt(rot);
Material mat = graphic.MatAt(rot);

// 检查是否有独立的 north 贴图
private bool HasNorthTexture(Graphic graphic)
{
    return graphic.MatNorth != graphic.MatSouth;
}
```

**经验总结**:
- 使用 `Graphic.MeshAt(rot)` 获取正确的 mesh（自动处理翻转）
- 使用 `Graphic.MatAt(rot)` 获取对应朝向的材质
- 通过比较 `MatNorth` 和 `MatSouth` 判断是否有独立的 north 贴图

### 派系验证：使用工具类集中管理派系选择逻辑

**问题**: 在多个地方分散处理派系验证逻辑，导致代码重复且容易遗漏边界情况（如玩家派系、已击败派系）。

**错误做法**:
```csharp
// 分散的派系选择逻辑，容易出错
Faction faction = Find.FactionManager.RandomEnemyFaction();
if (faction != null && !faction.defeated)
{
    // 生成 Pawn
}
// 遗漏了隐藏派系、玩家派系等检查
```

**解决方案** ([SylvieFactionValidator.cs](Core/SylvieFactionValidator.cs)):
```csharp
// 集中式派系验证工具类
public static class SylvieFactionValidator
{
    public static bool HasAnyValidFaction()
    {
        return Find.FactionManager.GetFactions()
            .Any(IsValidFaction);
    }
    
    public static Faction? GetValidFaction()
    {
        var validFactions = Find.FactionManager.GetFactions()
            .Where(IsValidFaction)
            .ToList();
        
        return validFactions.Count > 0 
            ? validFactions.RandomElement() 
            : null;
    }
    
    private static bool IsValidFaction(Faction faction)
    {
        if (faction == null || faction.IsPlayer || faction.defeated)
            return false;
        if (faction.def.hidden)
            return false;
        return true;
    }
}
```

**使用示例**:
```csharp
// 检查是否存在有效派系
if (!SylvieFactionValidator.HasAnyValidFaction())
    return false;

// 获取随机有效派系
Faction? validFaction = SylvieFactionValidator.GetValidFaction();
```

**经验总结**:
- 将派系验证逻辑集中到单一工具类，避免代码重复
- 统一处理边界情况（玩家派系、已击败、隐藏派系等）
- 提供清晰的 API 接口：`HasAnyValidFaction()` 和 `GetValidFaction()`
- 便于后续扩展新的验证规则

### 种族正确性：生成后验证，不正确则销毁重生成

**问题**: RimWorld 的 Pawn 生成系统在某些情况下（如与其他 Mod 冲突）可能生成错误种族的 Pawn。

**错误做法**:
```csharp
// 直接返回生成的 Pawn，不验证种族
Pawn pawn = PawnGenerator.GeneratePawn(request);
return pawn; // 可能返回了错误种族的 Pawn
```

**解决方案**:
```csharp
// 生成后验证种族，不正确则重生成
private const int MaxGenerationAttempts = 5;
private const string ExpectedRaceDefName = "Sylvie_Race";

public Pawn? GenerateSylviePawn()
{
    for (int attempt = 0; attempt < MaxGenerationAttempts; attempt++)
    {
        Pawn? pawn = PawnGenerator.GeneratePawn(request);
        
        // 验证种族是否正确
        if (pawn?.def?.defName != ExpectedRaceDefName)
        {
            Log.Warning($"[SylvieMod] 生成的 Pawn 种族不匹配: {pawn?.def?.defName}，第 {attempt + 1} 次重试");
            pawn?.Destroy(); // 销毁错误的 Pawn
            continue;
        }
        
        return pawn; // 种族正确，返回
    }
    
    Log.Error("[SylvieMod] 无法生成正确种族的 Sylvie");
    return null;
}
```

**经验总结**:
- 生成 Pawn 后必须验证 `pawn.def.defName` 是否符合预期
- 设置最大重试次数防止无限循环
- 销毁错误种族的 Pawn 时使用 `pawn.Destroy()` 清理资源
- 记录警告日志便于排查生成失败的原因
- 这是处理 Mod 兼容性问题的有效防御性编程手段

---

## 相关文档

- [用户文档](../README.md) - 功能介绍与使用指南
- [工作区文档](../../README.md) - 项目索引

---

*本文档遵循 Internal README 规范编写*
