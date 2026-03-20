# XML配置结构概览

## 寻求抚摸系统相关XML

### 1. JobDef定义

**文件**: `Defs/Jobs/Sylvie_SeekPettingJobDefs.xml`

```xml
<JobDef>
  <defName>Sylvie_SeekPetting</defName>
  <driverClass>SylvieMod.JobDriver_SeekPetting</driverClass>
  <reportString>Sylvie_SeekPetting_JobReport</reportString>
  <casualInterruptible>true</casualInterruptible>
</JobDef>
```

### 2. ThoughtDef定义

**文件**: `Defs/Thoughts/Sylvie_SeekPettingThoughts.xml`

包含4个ThoughtDef：

| DefName | 类型 | 持续时间 | 效果 |
|---------|------|----------|------|
| Sylvie_WasPetted | Thought_Memory | 16小时 (0.67天) | +10心情 |
| Sylvie_PettedSomeone | Thought_Memory | 12小时 (0.5天) | +6/+8心情（2阶段） |
| Sylvie_PettedMe_Social | Thought_MemorySocial | 10天 | +10好感度 |
| Sylvie_WasPetted_Social | Thought_MemorySocial | 10天 | +10好感度 |

**详细配置**:

```xml
<!-- Sylvie_WasPetted - 希尔薇被抚摸的心情 -->
<ThoughtDef>
  <defName>Sylvie_WasPetted</defName>
  <thoughtClass>Thought_Memory</thoughtClass>
  <durationDays>0.67</durationDays> <!-- 16小时 -->
  <stackLimit>1</stackLimit>
  <stages>
    <li>
      <label>被温柔地抚摸了</label>
      <description>有人温柔地抚摸了我...</description>
      <baseMoodEffect>10</baseMoodEffect>
    </li>
  </stages>
</ThoughtDef>

<!-- Sylvie_PettedSomeone - 抚摸者的心情（2阶段） -->
<ThoughtDef>
  <defName>Sylvie_PettedSomeone</defName>
  <stages>
    <li> <!-- Stage 0: 普通关系 -->
      <label>抚摸了希尔薇</label>
      <baseMoodEffect>6</baseMoodEffect>
    </li>
    <li> <!-- Stage 1: 亲密关系（好感度>40） -->
      <label>抚摸了希尔薇（亲密）</label>
      <baseMoodEffect>8</baseMoodEffect>
    </li>
  </stages>
</ThoughtDef>

<!-- 社交关系ThoughtDef -->
<ThoughtDef>
  <defName>Sylvie_PettedMe_Social</defName>
  <thoughtClass>Thought_MemorySocial</thoughtClass>
  <durationDays>10</durationDays>
  <stackLimit>3</stackLimit>
  <baseOpinionOffset>10</baseOpinionOffset>
</ThoughtDef>
```

### 3. ThinkTree补丁

**文件**: `Patches/Sylvie_ThinkTreePatches.xml`

```xml
<Patch>
  <!-- 在 Humanlike ThinkTree 中添加 Sylvie 寻求抚摸行为 -->
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
</Patch>
```

**补丁说明**:
- 插入位置：Humanlike ThinkTree的第一个Idle行为之前
- 使用ThinkNode_ConditionalColonist限制只有殖民者执行
- 使用ThinkNode_Tagger标记为Idle行为
- JobGiver内部会进一步检查Sylvie种族和其他条件

### 4. 本地化文件

#### Keyed翻译

**文件**: `Languages/ChineseSimplified/Keyed/Sylvie_SeekPetting.xml`

```xml
<LanguageData>
  <!-- Job 报告文本 -->
  <Sylvie_SeekPetting_JobReport>向 {0} 寻求抚摸</Sylvie_SeekPetting_JobReport>
  
  <!-- 消息文本 -->
  <Sylvie_SeekPetting_Message>{0}主动寻求{1}的关爱，{1}温柔地抚摸了她。</Sylvie_SeekPetting_Message>
</LanguageData>
```

**文件**: `Languages/English/Keyed/Sylvie_SeekPetting.xml`

```xml
<LanguageData>
  <Sylvie_SeekPetting_JobReport>seeking petting from {0}</Sylvie_SeekPetting_JobReport>
  <Sylvie_SeekPetting_Message>{0} sought affection from {1}, who gently petted her.</Sylvie_SeekPetting_Message>
</LanguageData>
```

#### DefInjected翻译

**文件**: `Languages/ChineseSimplified/DefInjected/ThoughtDef/Sylvie_SeekPettingThoughts.xml`

```xml
<LanguageData>
  <!-- 被抚摸的心情 -->
  <Sylvie_WasPetted.label>被温柔地抚摸了</Sylvie_WasPetted.label>
  <Sylvie_WasPetted.description>有人温柔地抚摸了我，感觉被关爱和接纳，内心充满了温暖。</Sylvie_WasPetted.description>
  
  <!-- 抚摸别人的心情 -->
  <Sylvie_PettedSomeone.label>抚摸了希尔薇</Sylvie_PettedSomeone.label>
  <Sylvie_PettedSomeone.description>希尔薇主动来找我，让我抚摸她。她看起来那么需要关爱，能给她带来安慰让我感觉很温暖。</Sylvie_PettedSomeone.description>
  <Sylvie_PettedSomeone.stages.1.label>抚摸了希尔薇（亲密）</Sylvie_PettedSomeone.stages.1.label>
  <Sylvie_PettedSomeone.stages.1.description>希尔薇信任地让我抚摸她，我们之间有着特别的羁绊。能照顾她让我感到很幸福。</Sylvie_PettedSomeone.stages.1.description>
  
  <!-- 社交关系 -->
  <Sylvie_PettedMe_Social.label>抚摸了希尔薇</Sylvie_PettedMe_Social.label>
  <Sylvie_WasPetted_Social.label>被温柔地抚摸了</Sylvie_WasPetted_Social.label>
</LanguageData>
```

## XML配置统计

### 寻求抚摸系统XML文件

| 文件路径 | 用途 | 行数 |
|----------|------|------|
| Defs/Jobs/Sylvie_SeekPettingJobDefs.xml | JobDef定义 | ~9 |
| Defs/Thoughts/Sylvie_SeekPettingThoughts.xml | ThoughtDef定义 | ~65 |
| Patches/Sylvie_ThinkTreePatches.xml | ThinkTree补丁 | ~25 |
| Languages/ChineseSimplified/Keyed/Sylvie_SeekPetting.xml | 中文Keyed | ~8 |
| Languages/English/Keyed/Sylvie_SeekPetting.xml | 英文Keyed | ~7 |
| Languages/ChineseSimplified/DefInjected/ThoughtDef/Sylvie_SeekPettingThoughts.xml | 中文DefInjected | ~18 |
| Languages/English/DefInjected/ThoughtDef/Sylvie_SeekPettingThoughts.xml | 英文DefInjected | ~18 |

### Def名称汇总

| Def类型 | DefName | 用途 |
|---------|---------|------|
| JobDef | Sylvie_SeekPetting | 寻求抚摸工作定义 |
| ThoughtDef | Sylvie_WasPetted | 希尔薇被抚摸心情 |
| ThoughtDef | Sylvie_PettedSomeone | 抚摸者心情 |
| ThoughtDef | Sylvie_PettedMe_Social | 社交关系（殖民者对希尔薇） |
| ThoughtDef | Sylvie_WasPetted_Social | 社交关系（希尔薇对殖民者） |

## XML与代码对应关系

| XML DefName | 代码引用位置 | 用途 |
|-------------|--------------|------|
| Sylvie_SeekPetting | SylvieDefNames.Job_SeekPetting | JobGiver返回Job |
| Sylvie_WasPetted | SylvieDefNames.Thought_WasPetted | 应用希尔薇心情 |
| Sylvie_PettedSomeone | SylvieDefNames.Thought_PettedSomeone | 应用抚摸者心情 |
| Sylvie_PettedMe_Social | SylvieDefNames.Thought_PettedMe_Social | 社交关系更新 |
| Sylvie_WasPetted_Social | SylvieDefNames.Thought_WasPetted_Social | 社交关系更新 |
