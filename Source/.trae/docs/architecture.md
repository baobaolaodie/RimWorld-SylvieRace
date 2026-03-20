# SylvieRace 核心架构

## 寻求抚摸系统架构

### 系统概述

寻求抚摸系统是SylvieRace v0.0.6-pre新增的核心功能，允许希尔薇殖民者主动寻求其他殖民者的关爱和抚摸。该系统通过RimWorld的ThinkNode/JobDriver架构实现，包含条件检查、目标选择、效果应用三个主要阶段。

### 架构图

```
┌─────────────────────────────────────────────────────────────────────────┐
│                     寻求抚摸系统架构                                      │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                          │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │                     ThinkNode Layer                              │   │
│  │  ┌─────────────────────────────────────────────────────────┐   │   │
│  │  │           JobGiver_SeekPetting                           │   │   │
│  │  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────────┐  │   │   │
│  │  │  │ 条件检查    │→│ 目标选择    │→│ 创建Job         │  │   │   │
│  │  │  │ (7步验证)   │  │ (评分机制)  │  │ (Sylvie_Seek    │  │   │   │
│  │  │  │             │  │             │  │  Petting)       │  │   │   │
│  │  │  └─────────────┘  └─────────────┘  └─────────────────┘  │   │   │
│  │  └─────────────────────────────────────────────────────────┘   │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                              ↓                                          │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │                      JobDriver Layer                             │   │
│  │  ┌─────────────────────────────────────────────────────────┐   │   │
│  │  │           JobDriver_SeekPetting                          │   │   │
│  │  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────────┐  │   │   │
│  │  │  │ 移动到目标  │→│ 应用效果    │→│ 记录冷却        │  │   │   │
│  │  │  │ (GotoToil)  │  │ (Instant)   │  │ (Tracker)       │  │   │   │
│  │  │  └─────────────┘  └─────────────┘  └─────────────────┘  │   │   │
│  │  └─────────────────────────────────────────────────────────┘   │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                              ↓                                          │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │                      Effect Layer                                │   │
│  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────────────────┐  │   │
│  │  │ 心情效果    │  │ 社交关系    │  │ 通知消息                │  │   │
│  │  │ (+10/+6/+8) │  │ (+10/+15)   │  │ (Messages.Message)      │  │   │
│  │  └─────────────┘  └─────────────┘  └─────────────────────────┘  │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                          │
└─────────────────────────────────────────────────────────────────────────┘
```

### 数据流

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│   游戏Tick   │────→│ ThinkNode   │────→│  JobGiver   │────→│ 条件检查    │
│             │     │  (Idle)     │     │             │     │ (7步验证)   │
└─────────────┘     └─────────────┘     └─────────────┘     └──────┬──────┘
                                                                    │
                                                                    ↓
┌─────────────┐     ┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│   效果应用   │←────│  JobDriver  │←────│  创建Job    │←────│  目标选择   │
│             │     │             │     │             │     │ (评分排序)  │
└─────────────┘     └─────────────┘     └─────────────┘     └─────────────┘
```

### 核心组件

#### 1. JobGiver_SeekPetting

**职责**: AI决策，负责检查条件并分配寻求抚摸工作

**关键方法**:
- `TryGiveJob()` - 主入口，返回Job或null
- `TryPerformChecks()` - 执行7步条件检查
- `FindBestTarget()` - 寻找最佳目标
- `CalculateTargetScore()` - 计算目标评分

**7步条件检查**:
1. 间隔检查 - 每小时检查一次，20%概率触发
2. 年龄检查 - 必须≥10岁
3. 状态检查 - 清醒、未倒地、未精神崩溃
4. 冷却检查 - 不在6小时冷却期内
5. 空闲检查 - 非关键工作
6. 目标检查 - 40格范围内有符合条件的殖民者
7. JobDef检查 - JobDef存在且可用

**目标评分机制**:
```csharp
score = opinion                              // 好感度基础分
if (opinion > 40) score += 100              // 高好感度加成
if (targetMood > 50%) score += 50           // 心情加成
```

#### 2. JobDriver_SeekPetting

**职责**: 执行抚摸交互，应用效果

**关键方法**:
- `MakeNewToils()` - 创建Toil序列
- `ApplyPettingEffects()` - 应用所有效果
- `ApplyMoodThoughts()` - 应用心情效果
- `UpdateRelationship()` - 更新社交关系

**Toil序列**:
1. GotoToil - 移动到目标
2. InstantToil - 应用效果（瞬间完成）

**效果应用**:
- 希尔薇心情: "被温柔地抚摸了" (+10, 16小时)
- 抚摸者心情: "抚摸了希尔薇" (+6/+8, 12小时)
  - +6: 好感度≤40（普通关系）
  - +8: 好感度>40（亲密关系）
- 社交关系: 基础+10，希尔薇心情<30%时额外+5
- 通知消息: 游戏内消息提示

#### 3. SylvieSeekPettingTracker

**职责**: 冷却时间管理

**核心方法**:
- `IsInCooldown()` - 检查是否在冷却期
- `GetCooldownRemaining()` - 获取剩余冷却时间
- `SetLastPettingTick()` - 记录抚摸时间
- `ExposeData()` - 存档/读档支持

**冷却时间**: 6小时 = 15000 ticks

### 常量定义

```csharp
// JobGiver_SeekPetting.cs
public const int MinCheckInterval = GenDate.TicksPerHour;  // 2500 ticks
public const float CheckChance = 0.20f;                     // 20%
public const int MaxSearchDistance = 40;
public const int MinAgeYears = 10;
public const int HighOpinionThreshold = 40;
public const float TargetMinMoodThreshold = 0.50f;          // 50%

// JobDriver_SeekPetting.cs
public const int BaseOpinionOffset = 10;
public const int LowMoodBonusOpinion = 5;
public const float LowMoodThreshold = 0.30f;                // 30%
public const int IntimateOpinionThreshold = 40;

// SylvieSeekPettingTracker.cs
public const int CooldownTicks = 15000;                     // 6 hours
```

### Def名称常量

```csharp
// SylvieDefNames.cs
public const string Job_SeekPetting = "Sylvie_SeekPetting";
public const string Thought_WasPetted = "Sylvie_WasPetted";
public const string Thought_PettedSomeone = "Sylvie_PettedSomeone";
public const string Thought_PettedMe_Social = "Sylvie_PettedMe_Social";
public const string Thought_WasPetted_Social = "Sylvie_WasPetted_Social";
```

### XML配置

#### JobDef
```xml
<JobDef>
  <defName>Sylvie_SeekPetting</defName>
  <driverClass>SylvieMod.JobDriver_SeekPetting</driverClass>
  <reportString>Sylvie_SeekPetting_JobReport</reportString>
  <casualInterruptible>true</casualInterruptible>
</JobDef>
```

#### ThoughtDefs
- `Sylvie_WasPetted` - 希尔薇被抚摸心情 (+10, 16小时)
- `Sylvie_PettedSomeone` - 抚摸者心情 (+6/+8, 12小时, 2阶段)
- `Sylvie_PettedMe_Social` - 社交关系 (+10好感度, 10天)
- `Sylvie_WasPetted_Social` - 社交关系 (+10好感度, 10天)

#### ThinkTree补丁
```xml
<Operation Class="PatchOperationInsert">
  <xpath>/Defs/ThinkTreeDef[defName="Humanlike"]/thinkRoot/subNodes/li[@Class="ThinkNode_ConditionalColonist"...]</xpath>
  <order>Prepend</order>
  <value>
    <li Class="SylvieMod.JobGiver_SeekPetting" />
  </value>
</Operation>
```

### 依赖关系

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

### 关键设计决策

1. **概率触发机制**: 每小时20%概率，避免过于频繁的检查
2. **评分系统**: 综合考虑好感度和心情，选择最合适的抚摸对象
3. **双向社交关系**: 同时提升双方好感度，增强社交互动
4. **低心情加成**: 当希尔薇心情低落时提供额外关系加成，帮助情绪恢复
5. **种族安全检查**: JobDriver中再次验证种族，防止非希尔薇触发
6. **存档支持**: 使用ExposeData确保冷却时间在存档间持久化
