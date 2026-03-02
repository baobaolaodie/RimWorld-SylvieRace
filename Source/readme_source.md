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
│   ├── defs.xml               # 种族定义、事件定义
│   ├── clothes.xml            # 服装定义（19种）
│   ├── ClothingTrader.xml     # 贸易商定义
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

### 1. 种族定义 (defs.xml)

**文件位置**: `Defs/defs.xml`

**主要内容**:
- `AlienRace.ThingDef_AlienRace` - 希尔薇种族定义
- `PawnKindDef` - 希尔薇 PawnKind 定义
- `IncidentDef` - 奴隶商人事件定义
- `HairDef` - 专属发型定义
- `LetterDef` - 选择信件定义
- `BackstoryDef` - 背景故事定义

**种族特性配置**:
```xml
<alienRace>
  <generalSettings>
    <maleGenderProbability>0.0000000000001</maleGenderProbability>
    <disallowedTraits>
      <li><defName>Abrasive</defName></li>
      <li><defName>Greedy</defName></li>
    </disallowedTraits>
  </generalSettings>
</alienRace>
```

### 2. 事件系统

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

### 4. 服装系统

**服装定义**: `Defs/clothes.xml`
- 19 种专属服装
- 使用 `apparel.tags` 限制为希尔薇种族
- 使用 `tradeTags` 允许贸易商出售

**种族限制机制**:
```xml
<apparel>
  <tags>
    <li>SylvieApparel</li>
  </tags>
</apparel>
```

对应的 PawnKindDef:
```xml
<apparelTags>
  <li>SylvieApparel</li>
</apparelTags>
```

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
2. **GameComponent 自动注册**：无需手动注册，RimWorld 会自动实例化
3. **动态表情目录结构**：Defs 和 Patches 必须在 mod 根目录下，不能放在子目录
4. **动态表情依赖**：需要 Facial Animation WIP 模组作为前置
