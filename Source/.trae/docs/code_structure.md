# 代码结构概览

## 目录结构

```
SylvieRace/Source/
├── Animation/                   # 动画系统
│   ├── SylvieAnimationRegistry.cs      # 动画注册管理器
│   └── SylvieAnimationHelper.cs        # 动画辅助工具
├── Components/                  # 游戏组件
│   ├── SylvieAimingTracker.cs          # 瞄准状态追踪器
│   ├── SylvieCooldownTracker.cs        # 冷却状态跟踪
│   ├── SylvieCooldownOverlayComp.cs    # 冷却动画渲染
│   ├── SylvieCatEarComp.cs             # 猫耳渲染
│   ├── SylvieGameComponent.cs          # 状态管理与事件触发
│   └── SylvieSeekPettingTracker.cs     # 抚摸冷却跟踪 [NEW]
├── Core/                        # 核心基础设施
│   ├── HarmonyInit.cs                  # Harmony 初始化
│   ├── SylvieDefNames.cs               # Def 名称常量中心
│   ├── SylvieConstants.cs              # 全局常量定义
│   └── SylvieComponentRegistry.cs      # 组件注册中心
├── Jobs/                        # 工作系统 [NEW DIRECTORY]
│   ├── JobGiver_SeekPetting.cs         # 寻求抚摸 AI
│   └── JobDriver_SeekPetting.cs        # 抚摸交互执行
├── Patches/                     # Harmony 补丁
│   ├── Patch_FaceAnimationDef_IsSame.cs    # FA 种族匹配
│   ├── Patch_Stance_Warmup.cs              # 瞄准动画同步
│   ├── Patch_ResearchAnimation.cs          # 研究动画
│   └── Patch_CommsConsole.cs               # 通讯台补丁
├── Pawns/                       # Pawn 生成
│   └── SylviePawnGenerator.cs
├── Hediffs/                     # 健康状态
│   ├── CompNurseHeal.cs                # 护士服治疗技能
│   └── SylvieHediffManager.cs          # Hediff 管理
├── Incidents/                   # 事件
│   └── IncidentWorker_SylvieTrader.cs
├── Letters/                     # 信件
│   └── ChoiceLetter_SylvieOffer.cs
├── Debug/                       # 调试功能
│   └── SylvieDebugActions.cs
└── Utils/                       # 工具类
    └── SylvieValidationUtils.cs        # 验证工具
```

## 寻求抚摸系统相关文件

### 核心实现文件

| 文件 | 行数 | 核心职责 |
|------|------|----------|
| Jobs/JobGiver_SeekPetting.cs | 376 | AI决策，7步条件检查，目标选择 |
| Jobs/JobDriver_SeekPetting.cs | 291 | 执行交互，应用效果（心情、关系、通知） |
| Components/SylvieSeekPettingTracker.cs | 71 | 冷却时间管理（6小时=15000ticks） |

### 支持文件

| 文件 | 相关功能 |
|------|----------|
| Core/SylvieDefNames.cs | Def名称常量（Job、Thought） |
| Utils/SylvieValidationUtils.cs | 验证工具（年龄、状态、目标有效性） |

## 核心类职责

### 寻求抚摸系统

#### JobGiver_SeekPetting (L14-L376)
- **继承**: ThinkNode_JobGiver
- **核心方法**:
  - TryGiveJob() - 主入口，返回Job或null
  - TryPerformChecks() - 执行所有条件检查
  - FindBestTarget() - 寻找最佳目标
  - CalculateTargetScore() - 计算目标评分

#### JobDriver_SeekPetting (L13-L291)
- **继承**: JobDriver
- **核心方法**:
  - MakeNewToils() - 创建Toil序列
  - ApplyPettingEffects() - 应用所有效果
  - ApplyMoodThoughts() - 应用心情效果
  - UpdateRelationship() - 更新社交关系

#### SylvieSeekPettingTracker (L12-L71)
- **继承**: GameComponent
- **核心方法**:
  - IsInCooldown() - 检查是否在冷却期
  - GetCooldownRemaining() - 获取剩余冷却时间
  - SetLastPettingTick() - 记录抚摸时间

## 代码统计

### 寻求抚摸系统代码量

| 文件 | 代码行数 | 注释行数 |
|------|----------|----------|
| JobGiver_SeekPetting.cs | ~250 | ~126 |
| JobDriver_SeekPetting.cs | ~200 | ~91 |
| SylvieSeekPettingTracker.cs | ~45 | ~26 |
| **总计** | **~495** | **~243** |

### 关键常量汇总

```csharp
// JobGiver_SeekPetting.cs (L16-L25)
MinCheckInterval = GenDate.TicksPerHour (2500 ticks)
CheckChance = 0.20f
MaxSearchDistance = 40
MinAgeYears = 10
HighOpinionThreshold = 40
TargetMinMoodThreshold = 0.50f

// JobDriver_SeekPetting.cs (L17-L36)
BaseOpinionOffset = 10
LowMoodBonusOpinion = 5
LowMoodThreshold = 0.30f
IntimateOpinionThreshold = 40

// SylvieSeekPettingTracker.cs (L21)
CooldownTicks = 15000 (6 hours)
```

## 依赖关系

### 寻求抚摸系统依赖

```
JobGiver_SeekPetting
├── SylvieDefNames (种族检查、Def获取)
├── SylvieValidationUtils (验证工具)
└── SylvieSeekPettingTracker (冷却检查)

JobDriver_SeekPetting
├── SylvieDefNames (ThoughtDef获取)
└── SylvieSeekPettingTracker (记录冷却)

SylvieSeekPettingTracker
└── Game (RimWorld核心)
```

## 文件变更记录

### v0.0.6-pre 新增文件

1. **Jobs/JobGiver_SeekPetting.cs** - 新增
2. **Jobs/JobDriver_SeekPetting.cs** - 新增
3. **Components/SylvieSeekPettingTracker.cs** - 新增

### v0.0.6-pre 修改文件

1. **Core/SylvieDefNames.cs** - 添加寻求抚摸相关常量
2. **Core/SylvieComponentRegistry.cs** - 可能添加组件注册
