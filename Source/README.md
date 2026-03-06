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
│       ├── EyeType.xml         # 眼睛类型
│       ├── MouthType.xml       # 嘴巴类型
│       ├── BrowType.xml        # 眉毛类型
│       ├── LidType.xml         # 眼睑类型
│       ├── LidOptionType.xml   # 眼睑选项
│       ├── EmotionType.xml     # 情绪类型
│       ├── HeadType.xml        # 头部类型
│       ├── SkinType.xml        # 皮肤类型
│       └── Sylvie_RaceFaceAdjustment.xml  # 面部调整
├── Patches/
│   └── Sylvie_Race_FacialAnimation_Patches.xml  # 动态表情补丁
├── Source/
│   ├── Core/
│   │   ├── HarmonyInit.cs           # Harmony 初始化
│   │   └── SylvieDefNames.cs        # Def 名称常量
│   ├── Components/
│   │   └── SylvieGameComponent.cs   # 游戏组件（状态管理）
│   ├── Incidents/
│   │   └── IncidentWorker_SylvieTrader.cs  # 事件处理器
│   ├── Pawns/
│   │   └── SylviePawnGenerator.cs   # 希尔薇生成逻辑
│   ├── Letters/
│   │   └── ChoiceLetter_SylvieOffer.cs  # 信件类
│   ├── Hediffs/
│   │   └── SylvieHediffManager.cs   # Hediff 管理逻辑
│   ├── Patches/
│   │   └── Patch_CommsConsole.cs    # 通讯台补丁
│   ├── SylvieRace.csproj      # 项目文件
│   ├── SylvieRace.sln         # 解决方案
│   └── AssemblyInfo.cs        # 程序集信息
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
│               └── Skins/
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

### 1. SylvieDefNames（常量类）

**文件位置**: `Source/Core/SylvieDefNames.cs`

集中管理所有 Def 名称，便于维护和避免拼写错误：

```csharp
public static class SylvieDefNames
{
    public const string Incident_ArrivalEvent = "Sylvie_ArrivalEvent";
    public const string PawnKind_Sylvie = "Sylvie_PawnKind";
    public const string Hediff_InitialTrauma = "SylvieRace_InitialTrauma";
    // ... 更多常量
    
    // 便捷属性
    public static HediffDef? Hediff_InitialTraumaDef => HediffDef.Named(Hediff_InitialTrauma);
}
```

### 2. SylviePawnGenerator（Pawn 生成器）

**文件位置**: `Source/Pawns/SylviePawnGenerator.cs`

封装希尔薇 Pawn 生成逻辑：
- `GenerateSylvie(Faction)` - 生成希尔薇 Pawn
- `ConfigureName(Pawn)` - 设置名字
- `ConfigureGenes(Pawn)` - 设置基因（皮肤、发色）
- `ConfigureTraits(Pawn)` - 设置特性
- `ConfigureTattoos(Pawn)` - 设置纹身

### 3. SylvieHediffManager（Hediff 管理器）

**文件位置**: `Source/Hediffs/SylvieHediffManager.cs`

封装 Hediff 相关逻辑：
- `CalculateTriggerTick()` - 计算触发时间
- `TryTriggerHediff(Pawn)` - 触发 Hediff
- `SendHediffLetter(Pawn)` - 发送信件通知

### 4. 护士服主动技能组件

**文件位置**: `Source/Hediffs/SylvieHediffManager.cs`

**SylvieRace_CompProperties_NurseHeal**:
- `cooldownTicks` - 冷却时间（默认 5000 ticks = 2 小时）
- `paralysisHediff` - 昏迷 Hediff 定义

**SylvieRace_CompNurseHeal**:
- `CompGetWornGizmosExtra()` - 返回技能 Gizmo 按钮
- `TryUseAbility()` - 执行治疗逻辑
- `IsOnCooldown` - 检查冷却状态
- `CooldownTicksRemaining` - 剩余冷却时间

**使用方式**:
- 穿着护士服后，装备栏显示"紧急治疗"技能按钮
- 点击后立即包扎所有未处理的伤口
- 触发 1 小时昏迷效果
- 2 小时冷却时间

### 5. SylvieGameComponent（游戏组件）

**文件位置**: `Source/Components/SylvieGameComponent.cs`

负责状态管理和事件触发：
- `hasSylvieSpawned` - 希尔薇是否已生成
- `RegisterSylviePawn(Pawn)` - 注册希尔薇并安排 Hediff 触发
- `GameComponentTick()` - 定期检查事件触发

### 6. IncidentWorker_SylvieTrader（事件处理器）

**文件位置**: `Source/Incidents/IncidentWorker_SylvieTrader.cs`

处理奴隶商人事件：
- 生成商队
- 使用 `SylviePawnGenerator` 生成希尔薇
- 发送选择信件

### 7. ChoiceLetter_SylvieOffer（信件类）

**文件位置**: `Source/Letters/ChoiceLetter_SylvieOffer.cs`

自定义选择信件：
- 显示购买选项（100 白银）
- 处理购买和拒绝逻辑
- 转移希尔薇到玩家派系

### 8. Patch_CommsConsole（通讯台补丁）

**文件位置**: `Source/Patches/Patch_CommsConsole.cs`

添加呼叫服装贸易商选项。

### 9. 初始健康状态系统 (Hediffs/)

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

### 10. 心情效果系统 (Thoughts/)

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

## 代码架构原则

### 单一职责原则
每个类只负责一件事：
- `SylviePawnGenerator` - 只负责生成 Pawn
- `SylvieHediffManager` - 只负责 Hediff 管理
- `SylvieGameComponent` - 只负责状态管理

### 开闭原则
- 使用 `SylvieDefNames` 常量类，添加新 Def 只需修改一处

### 依赖倒置原则
- 高层模块（`SylvieGameComponent`）依赖低层模块（`SylvieHediffManager`）

## 编译配置

**项目文件**: `Source/SylvieRace.csproj`
- 目标框架：.NET Framework 4.8
- 输出路径：`..\1.6\Assemblies\`
- 输出文件：`SylvieRace.dll`

**DLL 引用路径**:
游戏 DLL 文件位于工作区的 `GameDll/` 目录：
- `0Harmony.dll` - Harmony 补丁框架
- `Assembly-CSharp.dll` - RimWorld 核心程序集
- `UnityEngine.CoreModule.dll` - Unity 核心模块

**编译命令**:
```bash
cd Source
dotnet build --configuration Release
```

## 可空性处理

项目启用了 `#nullable enable`，对于通过 XML 配置注入的字段（如 `CompProperties` 中的 `HediffDef`），使用 `= null!` 标记：

```csharp
public class SylvieRace_CompProperties_NurseHeal : CompProperties
{
    public int cooldownTicks = 5000;
    public HediffDef paralysisHediff = null!;  // XML 注入字段
}
```

这告诉编译器该字段会在运行时由 RimWorld 的 XML 反序列化系统赋值。

## 注意事项

1. **服装种族限制**：使用 `apparel.tags` + `PawnKindDef.apparelTags` 机制
2. **服装不可制作**：使用 `ApparelBase` + `recipeMaker IsNull="True"` 禁用缝纫台配方
3. **服装储存区识别**：使用自定义 `thingCategories: SylvieRace_Apparel`
4. **GameComponent 自动注册**：无需手动注册，RimWorld 会自动实例化
5. **动态表情目录结构**：Defs 和 Patches 必须在 mod 根目录下
6. **defName 命名规范**：所有 defName 使用 `SylvieRace_` 或 `Sylvie_` 前缀
7. **翻译系统**：Defs 中使用英文，中文翻译通过 Languages 目录注入
8. **希尔薇唯一生成**：只能通过 `Sylvie_ArrivalEvent` 事件生成
