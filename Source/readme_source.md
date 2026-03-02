# SylvieRace 开发者文档

本文档面向开发者，介绍项目结构和技术实现细节。

## 项目结构

```
SylvieRace/
├── About/
│   └── About.xml              # 模组元数据
├── Assemblies/
│   └── SylvieRace.dll         # 编译输出
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
│   │   ├── Apparel_Pants.xml      # 下装类 (2件)
│   │   ├── Apparel_Special.xml    # 特殊服装 (3件)
│   │   └── Apparel_Headwear.xml   # 头饰 (1件)
│   ├── Hair/
│   │   └── Sylvie_Hair.xml        # 发型定义
│   ├── Incidents/
│   │   └── Sylvie_Incident.xml    # 事件定义
│   ├── Letters/
│   │   └── Sylvie_Letter.xml      # 信件定义
│   ├── Backstories/
│   │   └── Sylvie_Backstory.xml   # 背景故事
│   ├── Traders/
│   │   └── ClothingTrader.xml     # 贸易商定义
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
│   ├── SylvieRace.csproj      # 项目文件
│   ├── SylvieRace.sln         # 解决方案
│   ├── HarmonyInit.cs         # Harmony 初始化
│   ├── SylvieGameComponent.cs # 游戏组件（事件触发）
│   ├── IncidentWorker_SylvieTrader.cs  # 事件处理器
│   ├── Patch_CommsConsole.cs  # 通讯台补丁
│   └── AssemblyInfo.cs        # 程序集信息
├── Textures/
│   └── Things/
│       ├── Clothes/           # 服装贴图
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
└── README.md                  # 用户文档
```

## 核心组件

### 1. 种族定义 (Races/Sylvie_Race.xml)

**文件位置**: `Defs/Races/Sylvie_Race.xml`

**主要内容**:
- `AlienRace.ThingDef_AlienRace` - 希尔薇种族定义

**种族特性配置**:
```xml
<alienRace>
  <generalSettings>
    <maleGenderProbability>0.0000000000001</maleGenderProbability>
  </generalSettings>
</alienRace>
```

**注意**: 特性配置已移至 C# 代码中处理，生成时强制清空所有特性并赋予善良特性。

### 2. PawnKind 定义 (PawnKinds/Sylvie_PawnKind.xml)

**文件位置**: `Defs/PawnKinds/Sylvie_PawnKind.xml`

**主要内容**:
- `PawnKindDef` - 希尔薇 PawnKind 定义
- `apparelTags` - 服装标签匹配

### 3. 事件系统 (Incidents/Sylvie_Incident.xml)

**文件位置**: `Defs/Incidents/Sylvie_Incident.xml`

**触发机制**: `SylvieGameComponent.GameComponentTick()`
- 每 2500 ticks 检查一次
- 游戏时间 >= 5000 ticks 时触发
- 需要有玩家殖民地和自由殖民者

**事件处理器**: `IncidentWorker_SylvieTrader`
- 生成奴隶商队
- 商队携带希尔薇（作为囚犯）
- 显示选择信件，支付 100 白银收留

### 3. 贸易商系统

**通讯台补丁**: `Patch_CommsConsole`
- 添加"呼叫专用服装贸易商"选项
- 调用费用：免费

**贸易商定义**: `ClothingTrader.xml`
- `TraderKindDef` - 贸易商类型
- `StockGenerator_Tag` - 使用 `SylvieClothesTag` 筛选商品

### 4. 服装系统 (Apparel/)

**文件位置**: `Defs/Apparel/`

**文件分类**:
- `Apparel_Dresses.xml` - 连衣裙类 (Purpledress, Bluedress, floraldress)
- `Apparel_Outfits.xml` - 套装类 (Finally, Fineclothing, Kimono, maidoutfit, Replacedmaid, Studentuniform, Suits, Elegantclothing, cheongsam, SprinFestivalWedding)
- `Apparel_Pants.xml` - 下装类 (ElegantclothingPant, StudentuniformPant)
- `Apparel_Special.xml` - 特殊服装 (Bandaid, Swimsuit, Shawl)
- `Apparel_Headwear.xml` - 头饰 (SpringFestivalHeadwear)

**服装定义特点**:
- 19 种专属服装
- 使用 `apparel.tags` 限制为希尔薇种族
- 使用 `tradeTags` 允许贸易商出售
- 使用 `thingCategories` 允许储存区识别
- **禁用制作配方**：服装无法在缝纫台制作

**完整服装定义示例**:
```xml
<ThingDef ParentName="ApparelBase">
  <defName>Purpledress</defName>
  <label>紫色连衣裙</label>
  <description>紫色连衣裙。</description>
  <recipeMaker Inherit="False" IsNull="True" />
  <techLevel>Medieval</techLevel>
  <tradeability>All</tradeability>
  <thingCategories>
    <li>SylvieApparel</li>
  </thingCategories>
  <apparel>
    <tags>
      <li>SylvieApparel</li>
    </tags>
    <defaultOutfitTags>
      <li>Worker</li>
    </defaultOutfitTags>
    <countsAsClothingForNudity>true</countsAsClothingForNudity>
    <developmentalStageFilter>Child, Adult</developmentalStageFilter>
    <canBeDesiredForIdeo>false</canBeDesiredForIdeo>
  </apparel>
  <tradeTags>
    <li>SylvieClothesTag</li>
  </tradeTags>
</ThingDef>
```

**关键字段说明**:
| 字段 | 说明 |
|------|------|
| `thingCategories` | 储存区识别必需（SylvieApparel - 希尔薇衣服） |
| `techLevel` | 科技等级 |
| `tradeability` | 交易性控制（All = 可买可卖） |
| `defaultOutfitTags` | 默认装备方案标签 |
| `countsAsClothingForNudity` | 是否算作服装（影响裸体判定） |
| `developmentalStageFilter` | 年龄阶段过滤 |
| `canBeDesiredForIdeo` | 是否可被文化需求 |

**编码注意事项**:
- XML 文件必须使用 UTF-8 编码
- 避免使用 PowerShell `Set-Content` 命令修改 XML 文件（会导致编码损坏）
- 推荐使用 Write 工具或支持 UTF-8 的编辑器修改文件

### 6. 物品类别系统 (ThingCategories/)

**文件位置**: `Defs/ThingCategories/Sylvie_ThingCategories.xml`

**类别定义**:
```xml
<ThingCategoryDef>
  <defName>SylvieApparel</defName>
  <label>希尔薇衣服</label>
  <parent>Apparel</parent>
</ThingCategoryDef>
```

**类别层级**:
```
Apparel
└── 希尔薇衣服 (SylvieApparel)
    └── 所有 SylvieRace 服装
```

**用途**: 在储存区筛选时，可以快速选择"希尔薇衣服"类别来筛选所有 SylvieRace 专属服装。

### 5. 动态表情系统

**文件位置**: `Defs/FacialAnimation/`

**表情定义类型**:
- `EyeType.xml` - 眼睛类型（EyeballTypeDef）
- `MouthType.xml` - 嘴巴类型（MouthTypeDef）
- `BrowType.xml` - 眉毛类型（BrowTypeDef）
- `LidType.xml` - 眼睑类型（LidTypeDef）
- `LidOptionType.xml` - 眼睑选项（LidOptionTypeDef）
- `EmotionType.xml` - 情绪类型（EmotionTypeDef）
- `HeadType.xml` - 头部类型（HeadTypeDef）
- `SkinType.xml` - 皮肤类型（SkinTypeDef）
- `Sylvie_RaceFaceAdjustment.xml` - 面部调整（FaceAdjustmentDef）

**种族补丁**: `Patches/Sylvie_Race_FacialAnimation_Patches.xml`
- 为 `Sylvie_Race` 添加动态表情组件
- 组件列表：
  - `DrawFaceGraphicsComp` - 绘制面部图形
  - `HeadControllerComp` - 头部控制器
  - `EyeballControllerComp` - 眼球控制器
  - `LidControllerComp` - 眼睑控制器
  - `BrowControllerComp` - 眉毛控制器
  - `MouthControllerComp` - 嘴巴控制器
  - `SkinControllerComp` - 皮肤控制器
  - `FacialAnimationControllerComp` - 表情动画控制器
  - （Experimentals）`LidOptionControllerComp` - 眼睑选项控制器
  - （Experimentals）`EmotionControllerComp` - 情绪控制器

**重要说明**:
- 动态表情 Defs 和 Patches 必须放在 mod 根目录的 `Defs/` 和 `Patches/` 文件夹下
- RimWorld 只会扫描根目录下的这些文件夹，子目录不会被加载

## 代码文件说明

### HarmonyInit.cs
```csharp
[StaticConstructorOnStartup]
public static class HarmonyInit
{
  static HarmonyInit() => new Harmony("com.sylvie.specialtrader").PatchAll();
}
```
- Harmony 补丁初始化
- 补丁 ID: `com.sylvie.specialtrader`

### SylvieGameComponent.cs
- 继承自 `GameComponent`
- 自动注册机制：RimWorld 的 `Game.FillComponents()` 会自动实例化所有 `GameComponent` 子类
- 负责事件触发逻辑

### IncidentWorker_SylvieTrader.cs
- 继承自 `IncidentWorker`
- 处理奴隶商人事件的生成逻辑
- 创建商队和希尔薇

### Patch_CommsConsole.cs
- Harmony Postfix 补丁
- 目标方法：`Building_CommsConsole.GetFloatMenuOptions`
- 添加呼叫服装贸易商选项

## 编译配置

**项目文件**: `Source/SylvieRace.csproj`
- 目标框架：.NET Framework 4.8
- 输出路径：`..\Assemblies\`
- 输出文件：`SylvieRace.dll`

**引用程序集**:
- 0Harmony.dll
- Assembly-CSharp.dll
- UnityEngine.CoreModule.dll

## 注意事项

1. **服装种族限制**：使用 `apparel.tags` + `PawnKindDef.apparelTags` 机制
2. **服装不可制作**：使用 `ApparelBase` + `recipeMaker IsNull="True"` 禁用缝纫台配方
3. **服装储存区识别**：使用自定义 `thingCategories: SylvieApparel`（希尔薇衣服）
4. **头饰类别继承**：`HatBase` 已包含 `Headgear`，子类需使用 `Inherit="False"` 覆盖
5. **特性配置**：C# 代码中强制设置特性，XML 中的 `disallowedTraits` 已移除
6. **GameComponent 自动注册**：无需手动注册，RimWorld 会自动实例化
7. **动态表情目录结构**：Defs 和 Patches 必须在 mod 根目录下，不能放在子目录
8. **动态表情依赖**：需要 Facial Animation WIP 和 Facial Animation Experimentals 模组作为前置
