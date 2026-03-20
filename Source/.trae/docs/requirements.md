# SylvieRace 文档更新需求

## 文档更新背景

本次文档更新是为了配合 SylvieRace v0.0.6-pre 版本中新实现的"寻求抚摸"功能。该功能已完整实现并经过多轮修复，现在需要更新三个文档以反映最新的实现状态。

## 需要更新的文档

1. **SylvieRace/README.md** - 用户文档（Mod根目录）
2. **SylvieRace/Source/README.md** - 开发者文档
3. **工作区根目录 README.md** - 项目索引文档

## 功能需求摘要：寻求抚摸系统

### 功能概述
希尔薇殖民者会主动寻求其他殖民者的关爱和抚摸。这是希尔薇表达情感需求的方式，能够显著提升她的心情并增进与其他殖民者的关系。

### 核心组件

| 组件 | 文件路径 | 职责 |
|------|----------|------|
| JobGiver_SeekPetting | Source/Jobs/JobGiver_SeekPetting.cs | AI决策，检查条件并分配工作 |
| JobDriver_SeekPetting | Source/Jobs/JobDriver_SeekPetting.cs | 执行抚摸交互，应用效果 |
| SylvieSeekPettingTracker | Source/Components/SylvieSeekPettingTracker.cs | 冷却时间管理（6小时） |

### XML配置

| 文件 | 内容 |
|------|------|
| Defs/Jobs/Sylvie_SeekPettingJobDefs.xml | JobDef定义（Sylvie_SeekPetting） |
| Defs/Thoughts/Sylvie_SeekPettingThoughts.xml | 4个ThoughtDef（心情+社交） |
| Patches/Sylvie_ThinkTreePatches.xml | ThinkTree补丁，插入JobGiver |
| Languages/*/Keyed/Sylvie_SeekPetting.xml | 本地化文本 |
| Languages/*/DefInjected/ThoughtDef/*.xml | ThoughtDef翻译 |

### 触发条件（7步检查）

1. **间隔检查**: 每小时检查一次，20%概率触发
2. **年龄检查**: 必须≥10岁
3. **状态检查**: 清醒、未倒地、未精神崩溃
4. **冷却检查**: 不在6小时冷却期内
5. **空闲检查**: 非关键工作（非战斗、非紧急医疗等）
6. **目标检查**: 40格范围内有符合条件的殖民者
7. **JobDef检查**: JobDef存在且可用

### 目标选择逻辑

- 必须是同派系殖民者
- 双方好感度必须>0
- 目标必须清醒
- 评分机制：好感度基础分 + 高好感度加成(>40时+100) + 心情加成(>50%时+50)

### 效果系统

**心情效果**:
- 希尔薇: "被温柔地抚摸了" (+10，持续16小时)
- 抚摸者: "抚摸了希尔薇" (+6/+8，持续12小时)
  - +6: 好感度≤40（普通关系）
  - +8: 好感度>40（亲密关系）

**社交关系**:
- 基础+10好感度（双向）
- 希尔薇心情<30%时额外+5

**通知**: 触发游戏内消息通知

### 关键常量

```csharp
// JobGiver_SeekPetting.cs
MinCheckInterval = GenDate.TicksPerHour (2500 ticks)
CheckChance = 0.20f (20%)
MaxSearchDistance = 40
MinAgeYears = 10
HighOpinionThreshold = 40
TargetMinMoodThreshold = 0.50f (50%)

// JobDriver_SeekPetting.cs
BaseOpinionOffset = 10
LowMoodBonusOpinion = 5
LowMoodThreshold = 0.30f (30%)
IntimateOpinionThreshold = 40

// SylvieSeekPettingTracker.cs
CooldownTicks = 15000 (6小时)
```

### Def名称常量（SylvieDefNames.cs）

```csharp
Job_SeekPetting = "Sylvie_SeekPetting"
Thought_WasPetted = "Sylvie_WasPetted"
Thought_PettedSomeone = "Sylvie_PettedSomeone"
Thought_PettedMe_Social = "Sylvie_PettedMe_Social"
Thought_WasPetted_Social = "Sylvie_WasPetted_Social"
```

## 文档更新约束

### 用户文档 (README.md)
- ✅ 必须包含：功能描述、触发条件、效果说明、注意事项
- ❌ 禁止包含：技术实现细节、代码片段

### 开发者文档 (Source/README.md)
- ✅ 必须包含：项目结构、核心组件说明、代码架构、实现逻辑
- ❌ 禁止包含：更新日志（已在用户文档中）

### 工作区文档 (根目录 README.md)
- ✅ 必须包含：项目索引、总体更新日志摘要
- ❌ 禁止包含：详细功能说明

## 版本信息

- **当前版本**: v0.0.6-pre
- **游戏版本**: RimWorld 1.6
- **更新日期**: 2026-03-20

## 一致性要求

1. 三个文档中的版本号必须一致
2. 功能描述必须与实际代码实现一致
3. 数值参数必须与代码中的常量一致
4. 术语使用必须统一（如"寻求抚摸"而非"求抚摸"）
