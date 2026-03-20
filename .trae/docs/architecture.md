# SylvieRace 寻求抚摸系统架构说明

**文档版本**: 2026-03-20  
**系统**: 寻求抚摸系统 (Seek Petting System)  
**Mod 版本**: v0.0.6-pre

---

## 系统架构图

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                        Seek Petting System                                   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │                      Trigger Layer (触发层)                          │   │
│  │  ┌─────────────────────────────────────────────────────────────┐   │   │
│  │  │                    JobGiver_SeekPetting                      │   │   │
│  │  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐          │   │   │
│  │  │  │ 7-Step Check│  │Target Finder│  │ Job Creator │          │   │   │
│  │  │  │  (7步检查)   │  │  (目标查找)  │  │  (任务创建)  │          │   │   │
│  │  │  └─────────────┘  └─────────────┘  └─────────────┘          │   │   │
│  │  └─────────────────────────────────────────────────────────────┘   │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                              │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │                      Execution Layer (执行层)                        │   │
│  │  ┌─────────────────────────────────────────────────────────────┐   │   │
│  │  │                    JobDriver_SeekPetting                     │   │   │
│  │  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐          │   │   │
│  │  │  │   MoveTo    │  │Apply Effects│  │  Cooldown   │          │   │   │
│  │  │  │  (移动到目标) │  │  (应用效果)  │  │  (记录冷却)  │          │   │   │
│  │  │  └─────────────┘  └─────────────┘  └─────────────┘          │   │   │
│  │  └─────────────────────────────────────────────────────────────┘   │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                              │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │                      Effect Layer (效果层)                           │   │
│  │  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐              │   │
│  │  │  MoodEffect  │  │Relationship  │  │ Notification │              │   │
│  │  │   (心情效果)  │  │  (社交关系)   │  │   (通知消息)  │              │   │
│  │  └──────────────┘  └──────────────┘  └──────────────┘              │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                              │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │                      State Layer (状态层)                            │   │
│  │  ┌─────────────────────────────────────────────────────────────┐   │   │
│  │  │                 SylvieSeekPettingTracker                     │   │   │
│  │  │              (GameComponent - 冷却时间管理)                   │   │   │
│  │  │  ┌─────────────────────────────────────────────────────┐   │   │   │
│  │  │  │  Dictionary<Pawn, int> lastPettingTicks             │   │   │   │
│  │  │  │  (每个Pawn的最后抚摸时间)                            │   │   │   │
│  │  │  └─────────────────────────────────────────────────────┘   │   │   │
│  │  └─────────────────────────────────────────────────────────────┘   │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                              │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │                     Support Layer (支持层)                           │   │
│  │  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐              │   │
│  │  │SylvieDefNames│  │SylvieValidat-│  │  XML Defs    │              │   │
│  │  │  (Def常量)    │  │   ionUtils   │  │ (ThoughtDef, │              │   │
│  │  │              │  │  (验证工具)   │  │  JobDef)     │              │   │
│  │  └──────────────┘  └──────────────┘  └──────────────┘              │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                              │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

## 核心类职责

| 类名 | 文件路径 | 职责描述 |
|------|----------|----------|
| `JobGiver_SeekPetting` | `Jobs/JobGiver_SeekPetting.cs` | AI决策类，执行7步条件检查，寻找最佳目标，创建Job |
| `JobDriver_SeekPetting` | `Jobs/JobDriver_SeekPetting.cs` | Job执行类，处理移动到目标、应用效果、记录冷却 |
| `SylvieSeekPettingTracker` | `Components/SylvieSeekPettingTracker.cs` | GameComponent，管理每个Pawn的冷却时间 |
| `SylvieValidationUtils` | `Utils/SylvieValidationUtils.cs` | 验证工具类，提供目标验证、状态检查等方法 |
| `SylvieDefNames` | `Core/SylvieDefNames.cs` | Def名称常量中心，提供ThoughtDef/JobDef访问 |

---

## 数据流图

```
┌─────────────┐     ┌─────────────────┐     ┌─────────────────┐
│   ThinkTree  │────→│ JobGiver_Seek   │────→│  7-Step Check   │
│   (AI系统)   │     │   Petting       │     │   (条件验证)     │
└─────────────┘     └─────────────────┘     └────────┬────────┘
                                                     │
                         ┌──────────────────────────┼──────────┐
                         │                          │          │
                         ▼                          ▼          ▼
                   ┌──────────┐              ┌──────────┐  ┌──────────┐
                   │  Age     │              │  State   │  │ Cooldown │
                   │ Check    │              │ Check    │  │ Check    │
                   └──────────┘              └──────────┘  └──────────┘
                         │                          │          │
                         └──────────────────────────┼──────────┘
                                                    │
                                                    ▼
                                          ┌─────────────────┐
                                          │  FindBestTarget │
                                          │   (目标查找)     │
                                          └────────┬────────┘
                                                   │
                                                   ▼
                                          ┌─────────────────┐
                                          │ CalculateTarget │
                                          │     Score       │
                                          │   (评分排序)     │
                                          └────────┬────────┘
                                                   │
                                                   ▼
┌─────────────┐     ┌─────────────────┐     ┌─────────────────┐
│   Target    │←────│ JobDriver_Seek  │←────│   Create Job    │
│   Pawn      │     │   Petting       │     │   (创建任务)     │
└──────┬──────┘     └─────────────────┘     └─────────────────┘
       │
       ▼
┌─────────────────────────────────────────────────────────────┐
│                      ApplyPettingEffects                     │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │ ApplyMood    │  │ UpdateRelat- │  │ SendNotifica-│      │
│  │ Thoughts     │  │ ionship      │  │ tion         │      │
│  │ (心情效果)    │  │ (社交关系)    │  │ (通知消息)    │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└─────────────────────────────────────────────────────────────┘
       │
       ▼
┌─────────────────┐
│ RecordCooldown  │
│  (记录冷却时间)  │
└─────────────────┘
```

---

## 关键常量定义

### JobGiver_SeekPetting 常量

```csharp
private const int MinCheckInterval = GenDate.TicksPerHour;  // 2500 ticks (1小时)
private const float CheckChance = 0.20f;                     // 20% 概率
private const int MaxSearchDistance = 40;                    // 40 格
private const int MinAgeYears = 10;                          // 10 岁
private const int HighOpinionThreshold = 40;                 // 高好感度阈值
private const float TargetMinMoodThreshold = 0.50f;          // 50% 心情阈值
```

### JobDriver_SeekPetting 常量

```csharp
private const int BaseOpinionOffset = 10;                    // 基础关系 +10
private const int LowMoodBonusOpinion = 5;                   // 低心情额外 +5
private const float LowMoodThreshold = 0.30f;                // 30% 低心情阈值
private const int IntimateOpinionThreshold = 40;             // 亲密关系阈值 40
```

### SylvieSeekPettingTracker 常量

```csharp
public const int CooldownTicks = 15000;                      // 6小时 (6 * 2500)
```

---

## XML Def 配置

### ThoughtDef 定义

| DefName | 类型 | 持续时间 | 效果 |
|---------|------|----------|------|
| `Sylvie_WasPetted` | Thought_Memory | 16小时 (0.67天) | +10 心情 |
| `Sylvie_PettedSomeone` | Thought_Memory | 12小时 (0.5天) | Stage0: +6, Stage1: +8 |
| `Sylvie_PettedMe_Social` | Thought_MemorySocial | 10天 | +10 好感度 |
| `Sylvie_WasPetted_Social` | Thought_MemorySocial | 10天 | +10 好感度 |

### JobDef 定义

```xml
<JobDef>
  <defName>Sylvie_SeekPetting</defName>
  <driverClass>SylvieMod.JobDriver_SeekPetting</driverClass>
  <reportString>Sylvie_SeekPetting_JobReport</reportString>
  <casualInterruptible>true</casualInterruptible>
</JobDef>
```

### ThinkTree Patch

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

---

## 7步条件检查流程

```
Step 1: CheckIntervalAndProbability
├── 检查是否达到最小间隔 (2500 ticks)
├── 生成随机数 (0.0 - 1.0)
└── 判断是否 <= 0.20 (20%概率)

Step 2: CheckAge
└── 检查 pawn.ageTracker.AgeBiologicalYears >= 10

Step 3: CheckPawnState
├── 检查 pawn.Awake() == true
├── 检查 pawn.Downed == false
└── 检查 pawn.InMentalState == false

Step 4: CheckCooldown
├── 获取 SylvieSeekPettingTracker
├── 检查 tracker.IsInCooldown(pawn)
└── 计算剩余冷却时间

Step 5: CheckIdleStatus
├── 获取 pawn.CurJob
├── 检查是否为关键任务 (IsCriticalJob)
└── 非关键任务可中断

Step 6: FindBestTarget (目标检查)
├── 获取同派系殖民者列表
├── 过滤符合条件的候选者
│   ├── IsValidPettingTarget 验证
│   ├── 距离检查 (<= 40格)
│   └── 好感度检查 (> 0)
└── 评分排序选择最佳目标

Step 7: JobDefCheck
└── 检查 SylvieDefNames.Job_SeekPettingDef != null
```

---

## 目标评分算法

```csharp
private float CalculateTargetScore(Pawn target, Pawn sylvie)
{
    float score = 0f;

    // 1. 好感度基础分 (直接加到分数)
    int opinion = sylvie.relations?.OpinionOf(target) ?? 0;
    score += opinion;

    // 2. 高好感度加成 (>40 获得 +100 优先级提升)
    if (opinion > HighOpinionThreshold)  // 40
    {
        score += 100f;
    }

    // 3. 目标心情加成 (>50% 获得 +50 分)
    float? targetMood = target.needs?.mood?.CurLevelPercentage;
    if (targetMood.HasValue && targetMood.Value > TargetMinMoodThreshold)  // 0.50f
    {
        score += 50f;
    }

    return score;
}
```

---

## 效果应用流程

```
ApplyPettingEffects()
├── ValidatePettingConditions()
│   ├── 检查 pawn != null
│   ├── 检查 TargetPawn != null
│   └── 检查 IsSylvieRace(pawn) == true
│
├── ApplyMoodThoughts()
│   ├── ApplySylvieMoodThought()
│   │   └── Thought_WasPetted (+10, 16小时)
│   └── ApplyTargetMoodThought()
│       ├── 计算目标对希尔薇的好感度
│       ├── DetermineThoughtStage()
│       │   ├── opinion > 40 → Stage 1 (+8)
│       │   └── opinion <= 40 → Stage 0 (+6)
│       └── 应用 Thought_PettedSomeone
│
├── UpdateRelationship()
│   ├── CalculateOpinionOffset()
│   │   ├── 基础值: +10
│   │   └── 希尔薇心情 < 30% → 额外 +5
│   ├── ApplySocialThought(TargetPawn, pawn, "Sylvie_PettedMe_Social")
│   └── ApplySocialThought(pawn, TargetPawn, "Sylvie_WasPetted_Social")
│
├── SendNotification()
│   └── 显示消息: "{0}主动寻求{1}的关爱..."
│
└── RecordCooldown()
    └── tracker.SetLastPettingTick(pawn, currentTick)
```

---

## 类依赖关系

```
JobGiver_SeekPetting
├── SylvieDefNames (种族检查、Def获取)
│   ├── IsSylvieRace()
│   ├── Job_SeekPettingDef
│   └── Thought_* 常量
├── SylvieValidationUtils (验证工具)
│   ├── IsValidPettingTarget()
│   ├── IsValidStateForPetting()
│   ├── MeetsAgeRequirement()
│   └── IsAvailableForPetting()
└── SylvieSeekPettingTracker (冷却检查)
    └── IsInCooldown()

JobDriver_SeekPetting
├── SylvieDefNames (ThoughtDef获取)
│   ├── Thought_WasPetted
│   ├── Thought_PettedSomeone
│   ├── Thought_PettedMe_Social
│   └── Thought_WasPetted_Social
└── SylvieSeekPettingTracker (记录冷却)
    └── SetLastPettingTick()

SylvieSeekPettingTracker
└── GameComponent (继承)
    └── ExposeData() (存档支持)
```

---

## 存档支持

```csharp
public class SylvieSeekPettingTracker : GameComponent
{
    private Dictionary<Pawn, int> lastPettingTicks = new Dictionary<Pawn, int>();
    private List<Pawn> pawnKeys = new List<Pawn>();      // Scribe 用
    private List<int> tickValues = new List<int>();      // Scribe 用

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(
            ref lastPettingTicks,
            "lastPettingTicks",
            LookMode.Reference,
            LookMode.Value,
            ref pawnKeys,
            ref tickValues
        );
    }
}
```
