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
│       ├── MouthShapeEx.xml    # 瞄准嘴巴形状定义
│       ├── BrowType.xml        # 眉毛类型
│       ├── BrowShapeEx.xml     # 瞄准眉毛形状定义
│       ├── LidType.xml         # 眼睑类型
│       ├── LidOptionType.xml   # 眼睑选项类型
│       ├── LidOptionShapeEx.xml # 准星形状定义
│       ├── EmotionType.xml     # 情绪类型
│       ├── HeadType.xml        # 头部类型
│       ├── SkinType.xml        # 皮肤类型
│       ├── AimingAnimation.xml # 瞄准动画定义
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
│   │   ├── Patch_CommsConsole.cs    # 通讯台补丁
│   │   └── Patch_Stance_Warmup.cs   # 瞄准动画同步补丁
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
- `CheckForExistingSylvie()` - 检查殖民地是否已有希尔薇种族的殖民者
- `RegisterSylviePawn(Pawn)` - 注册希尔薇并安排 Hediff 触发
- `GameComponentTick()` - 定期检查事件触发

**防止重复生成机制**：
在触发事件前，会先检查殖民地是否已有希尔薇种族的殖民者（通过 `pawn.def.defName == "Sylvie_Race"` 判断）。如果已有，则设置 `hasSylvieSpawned = true` 并跳过事件触发。

### 6. IncidentWorker_SylvieTrader（事件处理器）

**文件位置**: `Source/Incidents/IncidentWorker_SylvieTrader.cs`

处理奴隶商人事件：
- 生成商队
- 使用 `SylviePawnGenerator` 生成希尔薇
- 发送选择信件
- **智能检测**：如果殖民地已有希尔薇种族的殖民者，事件不会触发

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

1. **服装种族限制**：使用 `apparel.tags` + `PawnKindDef.apparelTags` 机制，同时使用 `apparel.raceRestriction` 限制非希尔薇种族穿戴
2. **服装不可制作**：使用 `ApparelBase` + `recipeMaker IsNull="True"` 禁用缝纫台配方
3. **服装储存区识别**：使用自定义 `thingCategories: SylvieRace_Apparel`
4. **GameComponent 自动注册**：无需手动注册，RimWorld 会自动实例化
5. **动态表情目录结构**：Defs 和 Patches 必须在 mod 根目录下
6. **defName 命名规范**：所有 defName 使用 `SylvieRace_` 或 `Sylvie_` 前缀
7. **翻译系统**：Defs 中使用英文，中文翻译通过 Languages 目录注入
8. **希尔薇唯一生成**：只能通过 `Sylvie_ArrivalEvent` 事件生成
9. **服装层级配置**：泳装和创口贴使用 Belt 层（配件层），可与其他服装同时穿戴

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
      <li>SylvieRace_Bandaid</li>
      <li>SylvieRace_Swimsuit</li>
      <!-- 其他服装 defName -->
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

## 瞄准动画系统

### 技术实现
瞄准动画通过 Facial Animation 的 ShapeDef 实现，贴图整合到现有的 Normal 目录中。

### 重要原则
**不要创建新的 TypeDef！** 每个种族只需要一个 BrowTypeDef、一个 MouthTypeDef 和一个 LidOptionTypeDef。新形状的贴图应该添加到现有的 Normal 目录中。

### 文件结构
```
Defs/FacialAnimation/
├── BrowShapeEx.xml      # 瞄准眉毛形状定义（只定义形状，不定义贴图路径）
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

## 瞄准动画同步系统

### 技术实现
由于 Facial Animation 的动画选择基于 Job，而 `AttackStatic` Job 在整个射击过程中都存在（warmup + cooldown + burst），动画的 `startTick` 是在 Job 开始时设置的。因此使用 Harmony 补丁直接检查 `Stance_Warmup` 状态来实现动态动画同步。

### 核心组件

#### SylvieAimingTracker
**文件位置**: `Source/Patches/Patch_Stance_Warmup.cs`

简化的组件，仅用于缓存 Pawn 引用和静态字典查找。

#### Harmony 补丁
**文件位置**: `Source/Patches/Patch_Stance_Warmup.cs`

- `Patch_Pawn_SpawnSetup` - 为 Sylvie 种族 Pawn 添加跟踪器组件
- `Patch_FaceAnimation_GetCurrentFrame` - 拦截 Facial Animation 的帧计算
  - `Stance_Warmup` 状态：基于 warmup 进度计算帧（帧0 → 帧1 → 帧2）
  - `Stance_Cooldown` 状态：只显示第一帧（帧0）
  - 其他状态：返回原始逻辑
- `Patch_FacialAnimationControllerComp_InitializeIfNeed` - 注册动画到 Pawn 的映射

### 瞄准时间计算
```
总瞄准时间 = 武器.warmupTime × Pawn.AimingDelayFactor
```

### 关键 API
```csharp
// 武器词条
Verb.verbProps.warmupTime           // 瞄准时间（秒）

// Pawn 属性
pawn.GetStatValue(StatDefOf.AimingDelayFactor)   // 瞄准时间乘数

// Stance_Warmup 状态
warmup.ticksLeft    // 剩余瞄准 ticks
warmup.verb         // 当前使用的 Verb
```

### 动画帧计算
假设动画有 3 帧，总瞄准时间为 T ticks：
- `elapsedTicks = totalWarmupTicks - warmup.ticksLeft`
- `progress = elapsedTicks / totalWarmupTicks`
- `frameIndex = (int)(progress * 3)`
- 帧1：进度 0% - 33%
- 帧2：进度 33% - 66%
- 帧3：进度 66% - 100%（射击发生）
