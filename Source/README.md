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
│       ├── Animations/        # 动画定义
│       │   ├── AimingAnimation.xml   # 瞄准动画
│       │   ├── IngestAnimation.xml   # 进食动画
│       │   ├── CooldownAnimation.xml # 冷却动画
│       │   └── ResearchAnimation.xml # 研究动画
│       ├── Types/             # 类型定义
│       │   ├── EyeType.xml           # 眼睛类型
│       │   ├── MouthType.xml         # 嘴巴类型
│       │   ├── BrowType.xml          # 眉毛类型
│       │   ├── LidType.xml           # 眼睑类型
│       │   ├── LidOptionType.xml     # 眼睑选项类型
│       │   ├── EmotionType.xml       # 情绪类型
│       │   ├── HeadType.xml          # 头部类型
│       │   ├── SkinType.xml          # 皮肤类型
│       │   ├── EyeballType.xml       # 眼球类型
│       │   └── CooldownOverlayType.xml # 冷却叠加层类型
│       ├── Shapes/            # 形状扩展定义
│       │   ├── EyeShapeEx.xml        # 眼睛形状扩展（lookdown）
│       │   ├── EyeballShapeEx.xml    # 眼球形状扩展
│       │   ├── MouthShapeEx.xml      # 嘴巴形状扩展（eating1-3）
│       │   ├── BrowShapeEx.xml       # 眉毛形状扩展（aiming, confused）
│       │   ├── LidOptionShapeEx.xml  # 准星形状定义
│       │   └── CooldownShapeEx.xml   # 冷却动画形状定义
│       └── Sylvie_RaceFaceAdjustment.xml  # 面部调整
├── Patches/
│   └── Sylvie_Race_FacialAnimation_Patches.xml  # 动态表情补丁
├── Source/
│   ├── Core/
│   │   ├── HarmonyInit.cs           # Harmony 初始化
│   │   ├── SylvieDefNames.cs        # Def 名称常量
│   │   └── SylvieConstants.cs       # 全局常量定义
│   ├── Components/
│   │   ├── SylvieGameComponent.cs   # 游戏组件（状态管理）
│   │   ├── SylvieCooldownTracker.cs # 冷却状态跟踪组件
│   │   ├── SylvieCooldownOverlayComp.cs # 冷却叠加层渲染组件
│   │   └── SylvieCatEarComp.cs      # 猫耳渲染组件
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
│   │   ├── Patch_Stance_Warmup.cs   # 瞄准动画同步补丁
│   │   └── Patch_ResearchAnimation.cs # 研究动画与猫耳同步补丁
│   ├── SylvieRace.csproj      # 项目文件
│   └── SylvieRace.sln         # 解决方案
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
│               ├── Skins/
│               └── CooldownOverlay/  # 冷却动画叠加层贴图
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

### 1. SylvieDefNames（Def 名称常量类）

**文件位置**: `Source/Core/SylvieDefNames.cs`

集中管理所有 Def 名称，便于维护和避免拼写错误：

```csharp
// File: Source/Core/SylvieDefNames.cs
#nullable enable

public static class SylvieDefNames
{
    public const string Incident_ArrivalEvent = "Sylvie_ArrivalEvent";
    public const string PawnKind_Sylvie = "Sylvie_PawnKind";
    public const string Hediff_InitialTrauma = "SylvieRace_InitialTrauma";
    public const string Letter_OfferLetter = "Sylvie_OfferLetter";
    public const string Tattoo_ScarHead = "SylvieRace_ScarHead";
    public const string Tattoo_ScarBody = "SylvieRace_ScarBody";
    public const string Trader_ClothingTrader = "Sylvie_ClothingTrader";
    public const string Gene_SkinSheerWhite = "Skin_SheerWhite";
    public const string Gene_HairSnowWhite = "Hair_SnowWhite";
    
    /// <summary>
    /// 希尔薇种族的 ThingDef 名称。
    /// </summary>
    public const string Race_Sylvie = "Sylvie_Race";
    
    // 便捷属性
    public static HediffDef? Hediff_InitialTraumaDef => HediffDef.Named(Hediff_InitialTrauma);
    public static PawnKindDef? PawnKind_SylvieDef => PawnKindDef.Named(PawnKind_Sylvie);
    public static IncidentDef? Incident_ArrivalEventDef => IncidentDef.Named(Incident_ArrivalEvent);
    public static LetterDef? Letter_OfferLetterDef => DefDatabase<LetterDef>.GetNamed(Letter_OfferLetter, false);
    public static TraderKindDef? Trader_ClothingTraderDef => DefDatabase<TraderKindDef>.GetNamed(Trader_ClothingTrader, false);
    public static TattooDef? Tattoo_ScarHeadDef => DefDatabase<TattooDef>.GetNamed(Tattoo_ScarHead, false);
    public static TattooDef? Tattoo_ScarBodyDef => DefDatabase<TattooDef>.GetNamed(Tattoo_ScarBody, false);
    public static GeneDef? Gene_SkinSheerWhiteDef => DefDatabase<GeneDef>.GetNamed(Gene_SkinSheerWhite, false);
    public static GeneDef? Gene_HairSnowWhiteDef => DefDatabase<GeneDef>.GetNamed(Gene_HairSnowWhite, false);
    
    /// <summary>
    /// 检查指定的 Pawn 是否为希尔薇种族。
    /// </summary>
    public static bool IsSylvieRace(Pawn? pawn)
    {
        return pawn?.def?.defName == Race_Sylvie;
    }
    
    /// <summary>
    /// 检查指定的 ThingDef 是否为希尔薇种族。
    /// </summary>
    public static bool IsSylvieRace(ThingDef? raceDef)
    {
        return raceDef?.defName == Race_Sylvie;
    }
}
```

**种族判断辅助方法**：

- `IsSylvieRace(Pawn?)` - 检查 Pawn 是否为希尔薇种族，使用空值传播运算符安全处理 null
- `IsSylvieRace(ThingDef?)` - 检查 ThingDef 是否为希尔薇种族定义

这些方法统一种族判断逻辑，避免在代码中分散使用字符串比较（如 `pawn.def.defName == "Sylvie_Race"`），提高代码可维护性和类型安全性。

### 2. SylvieConstants（全局常量类）

**文件位置**: `Source/Core/SylvieConstants.cs`

集中管理所有魔法数字常量，提高代码可读性和可维护性：

```csharp
// File: Source/Core/SylvieConstants.cs
#nullable enable

public static class SylvieConstants
{
    #region 动画相关常量
    
    /// <summary>
    /// 默认动画帧持续时间（单位：ticks）。
    /// 30 ticks ≈ 0.5 秒（@ 60 TPS）
    /// </summary>
    public const int DefaultAnimationDuration = 30;
    
    /// <summary>
    /// 研究动画的帧数量。
    /// </summary>
    public const int ResearchAnimationFrameCount = 8;
    
    #endregion
    
    #region 时间间隔常量
    
    /// <summary>
    /// 游戏组件检查间隔（单位：ticks）。
    /// 2500 ticks ≈ 41.7 秒（@ 60 TPS）
    /// </summary>
    public const int CheckIntervalTicks = 2500;
    
    /// <summary>
    /// 初始事件延迟（单位：ticks）。
    /// 5000 ticks ≈ 83.3 秒（@ 60 TPS）
    /// </summary>
    public const int InitialEventDelayTicks = 5000;
    
    /// <summary>
    /// Hediff 添加延迟（单位：ticks）。
    /// 300000 ticks = 5 天（RimWorld 标准）
    /// </summary>
    public const int HediffDelayTicks = 300000;
    
    /// <summary>
    /// 护士服治疗间隔（单位：ticks）。
    /// 600 ticks = 10 秒（@ 60 TPS）
    /// </summary>
    public const int NurseHealIntervalTicks = 600;
    
    #endregion
    
    #region 渲染层级常量
    
    /// <summary>
    /// 基础渲染层级。
    /// </summary>
    public const float BaseRenderLayer = 61f;
    
    /// <summary>
    /// 猫耳渲染层级。
    /// 位于头发（62）之上，头盔（75+）之下。
    /// </summary>
    public const float CatEarRenderLayer = 74f;
    
    /// <summary>
    /// 汗液渲染层级。
    /// </summary>
    public const float SweatRenderLayer = 61.1f;
    
    /// <summary>
    /// 弹匣渲染层级。
    /// </summary>
    public const float MagazineRenderLayer = 61.2f;
    
    /// <summary>
    /// 子弹渲染层级。
    /// </summary>
    public const float BulletRenderLayer = 61.3f;
    
    #endregion
    
    #region 数值常量
    
    /// <summary>
    /// 默认持续时间除数。
    /// 用于计算动画进度等。
    /// </summary>
    public const int DefaultDurationDivisor = 30;
    
    /// <summary>
    /// 护士服治疗效果。
    /// 每次治疗恢复的数值。
    /// </summary>
    public const float NurseHealAmount = 0.01f;
    
    /// <summary>
    /// 护士服治疗阈值。
    /// 低于此值时开始治疗。
    /// </summary>
    public const float NurseHealThreshold = 0.99f;
    
    #endregion
}
```

**设计目的**：
1. **消除魔法数字** - 将分散在代码中的数字常量集中管理，便于理解和修改
2. **提高可维护性** - 修改常量只需修改一处，避免遗漏
3. **文档化** - 每个常量都有详细的 XML 文档注释，说明其用途和计算依据
4. **分类组织** - 按功能区域分节（动画、时间、渲染、数值），便于查找

### 3. SylviePawnGenerator（Pawn 生成器）

**文件位置**: `Source/Pawns/SylviePawnGenerator.cs`

封装希尔薇 Pawn 生成逻辑：
- `GenerateSylvie(Faction)` - 生成希尔薇 Pawn（固定 19 岁，女性）
- `ConfigureName(Pawn)` - 设置名字（使用翻译键 `SylvieRace_FirstName`）
- `ConfigureGenes(Pawn)` - 设置基因（透白皮肤、雪白发色）
- `TryAddGene(Pawn, GeneDef, EndogeneCategory)` - 添加基因并移除冲突基因
- `ConfigureTraits(Pawn)` - 设置特性（清除所有特性并添加 Kind）
- `ConfigureTattoos(Pawn)` - 设置纹身（头部和身体疤痕）

### 4. SylvieHediffManager（Hediff 管理器）

**文件位置**: `Source/Hediffs/SylvieHediffManager.cs`

封装 Hediff 相关逻辑：
- `CalculateTriggerTick()` - 计算触发时间
- `TryTriggerHediff(Pawn)` - 触发 Hediff，返回 `bool` 表示是否成功触发
- `SendHediffLetter(Pawn)` - 发送信件通知

### 5. 护士服主动技能组件

**文件位置**: `Source/Hediffs/CompNurseHeal.cs`

**SylvieRace_CompProperties_NurseHeal**:
- `cooldownTicks` - 冷却时间（默认 5000 ticks = 2 游戏小时）
- `paralysisHediff` - 昏迷 Hediff 定义（XML 注入字段）

**SylvieRace_CompNurseHeal**:
- `CompGetWornGizmosExtra()` - 返回技能 Gizmo 按钮
- `TryUseAbility()` - 执行治疗逻辑
- `PostExposeData()` - 存档数据持久化（保存冷却状态）
- `IsOnCooldown` - 检查冷却状态
- `CooldownTicksRemaining` - 剩余冷却时间

**使用方式**:
- 穿着护士服后，装备栏显示"紧急治疗"技能按钮
- 点击后立即包扎所有未处理的伤口
- 触发 1 小时昏迷效果
- 约 2 游戏小时（5000 ticks）冷却时间

### 6. SylvieGameComponent（游戏组件）

**文件位置**: `Source/Components/SylvieGameComponent.cs`

负责状态管理和事件触发：
- `hasSylvieSpawned` - 希尔薇是否已生成
- `CheckForExistingSylvie()` - 检查殖民地是否已有希尔薇种族的殖民者
- `RegisterSylviePawn(Pawn)` - 注册希尔薇并安排 Hediff 触发
- `GameComponentTick()` - 定期检查事件触发

**防止重复生成机制**：
在触发事件前，会先检查殖民地是否已有希尔薇种族的殖民者（通过 `pawn.def.defName == SylvieRaceDefName` 判断，常量值为 `"Sylvie_Race"`）。如果已有，则设置 `hasSylvieSpawned = true` 并跳过事件触发。

**游戏组件常量**：
- `CheckInterval = SylvieConstants.CheckIntervalTicks` (2500 ticks) - 检查间隔（约 41 秒）
- `InitialEventTick = SylvieConstants.InitialEventDelayTicks` (5000 ticks) - 初始事件触发时间（约 83 秒）

### 7. SylvieCooldownTracker（冷却状态跟踪组件）

**文件位置**: `Source/Components/SylvieCooldownTracker.cs`

ThingComp（组件），用于跟踪远程武器冷却状态：

**核心属性**：
- `IsInRangedCooldown` - 检测是否处于远程武器冷却状态（排除连发和近战攻击）
- `CooldownProgress` - 冷却进度（0-1 浮点数）

**动画帧计算方法**：
- `GetSweatFrame()` - 根据冷却进度计算汗液动画帧（1-3）
  - 进度 0-33%：帧 1
  - 进度 33-66%：帧 2
  - 进度 66-100%：帧 3
- `GetBulletAnimationState()` - 计算子弹装填动画状态
  - 返回 `(insertFrame, bulletCount)` 元组
  - `insertFrame`：子弹投入帧（0-3，0 表示不显示）
  - `bulletCount`：已装填子弹数量（1-5）

**实现细节**：
```csharp
// File: Source/Components/SylvieCooldownTracker.cs
#nullable enable

public bool IsInRangedCooldown
{
    get
    {
        var stance = Pawn.stances?.curStance as Stance_Cooldown;
        if (stance == null) return false;
        
        Verb? verb = stance.verb;
        if (verb == null) return false;
        
        if (verb.state == VerbState.Bursting) return false;
        if (verb.verbProps.IsMeleeAttack) return false;
        
        return true;
    }
}
```

### 8. SylvieCooldownOverlayComp（冷却叠加层渲染组件）

**文件位置**: `Source/Components/SylvieCooldownOverlayComp.cs`

ThingComp（组件），在远程武器冷却期间渲染汗液、弹匣、子弹动画。

#### 渲染逻辑

**核心流程**：
1. 使用 `PostDraw` 方法在 Pawn 绘制后执行
2. 通过 `Matrix4x4.TRS` 进行缩放变换
3. **自动缩放**：使用 `CurLifeStage.headSizeFactor` 自动适配不同年龄的 pawn 大小（小孩 0.5/0.75，成年人 1.0）
4. **位置计算**：使用 `BaseHeadOffsetAt` 获取头部位置，组件偏移保持固定值（不缩放）
5. **智能朝向处理**：使用 `Graphic.MeshAt(rot)` 和 `Graphic.MatAt(rot)` 自动处理不同朝向

**渲染层级**：
- **Sweat 组件**：使用 `baseLayer = 61`（在胡子 60 和头发 62 之间）
- **其他组件**：保持原有默认层级

#### 朝向处理逻辑

| 朝向 | 行为 |
|------|------|
| South | 显示 south 贴图 |
| East | 显示 east 贴图 |
| West | 显示 east 贴图（自动翻转 via MeshAt） |
| North | 智能判断 - 有 north 贴图的组件显示，没有的不显示 |

**重要**：必须使用 `Graphic.MeshAt(rot)` 获取 mesh，它会自动处理 West 朝向的翻转。不能使用 `MeshPool.plane10` 配合 scale 负值来翻转 - 这样不会翻转 UV！

#### 渲染元素

1. **汗液动画**（3 帧）：`sweat1`, `sweat2`, `sweat3`
2. **弹匣贴图**：`magazine`（全程显示）
3. **子弹投入动画**（3 帧）：`bullet_insert1`, `bullet_insert2`, `bullet_insert3`
4. **子弹计数**（5 帧）：`bullet1` - `bullet5`

#### 缩放机制说明

RimWorld 原版使用 `headSizeFactor` 控制头部图形大小（小孩 0.5/0.75，成年人 1.0）。本组件的缩放策略：

| 元素 | 缩放方式 | 说明 |
|------|----------|------|
| 组件偏移 | **不缩放** | 保持固定值，确保组件相对于头部的位置不变 |
| 组件大小 | `headSizeFactor` 缩放 | 与头部图形大小同步 |
| 头部位置 | `BaseHeadOffsetAt` | 已包含 `sqrt(bodySizeFactor)` 缩放 |

这样无论 pawn 是小孩还是成年人，组件都能正确显示在头部，并保持相对位置不变。

#### 完整渲染代码示例

```csharp
// File: Source/Components/SylvieCooldownOverlayComp.cs
#nullable enable

public override void PostDraw()
{
    base.PostDraw();
    
    // 1. 种族检查
    if (!SylvieDefNames.IsSylvieRace(Pawn))
        return;
    
    // 2. 冷却状态检查
    var tracker = SylvieCooldownTracker.GetTracker(Pawn);
    if (tracker == null || !tracker.IsInRangedCooldown)
        return;
    
    Rot4 rot = Pawn.Rotation;
    
    // 3. 获取头部大小因子（Biotech DLC）
    float headSizeFactor = 1f;
    if (ModsConfig.BiotechActive && Pawn.ageTracker.CurLifeStage.headSizeFactor.HasValue)
    {
        headSizeFactor = Pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
    }
    
    // 4. 计算头部基础偏移（已包含 bodySizeFactor 的平方根缩放）
    Vector3 headOffset = Pawn.Drawer.renderer.BaseHeadOffsetAt(rot);
    
    // 5. 计算面部组件偏移（不缩放，保持固定值）
    Vector3 faceOffset = GetFaceDrawOffset();
    
    // 6. 计算最终绘制位置
    Vector3 drawPos = Pawn.DrawPos + headOffset + faceOffset;
    drawPos.y += 0.01f; // 层级微调
    
    // 7. 计算缩放矩阵（组件大小按 headSizeFactor 缩放）
    Vector3 drawScale = Vector3.one * headSizeFactor;
    Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, drawScale);
    
    // 8. 渲染汗液动画
    RenderSweat(rot, matrix, tracker);
    
    // 9. 渲染弹匣（North 朝向需检查是否有独立贴图）
    RenderMagazine(rot, matrix);
    
    // 10. 渲染子弹投入动画
    RenderBulletInsert(rot, matrix, tracker);
    
    // 11. 渲染子弹计数
    RenderBulletCount(rot, matrix, tracker);
}

/// <summary>
/// 渲染汗液动画 - 使用 baseLayer 61（在胡子和头发之间）
/// </summary>
private void RenderSweat(Rot4 rot, Matrix4x4 baseMatrix, SylvieCooldownTracker tracker)
{
    Graphic sweatGraphic = GetSweatGraphic(tracker.GetSweatFrame());
    if (sweatGraphic == null) return;
    
    // 使用 MeshAt 获取正确的 mesh（自动处理 West 朝向翻转）
    Mesh mesh = sweatGraphic.MeshAt(rot);
    Material mat = sweatGraphic.MatAt(rot);
    
    Graphics.DrawMesh(mesh, baseMatrix, mat, 0);
}

/// <summary>
/// 渲染弹匣 - North 朝向智能判断
/// </summary>
private void RenderMagazine(Rot4 rot, Matrix4x4 baseMatrix)
{
    // North 朝向且没有独立 north 贴图时不渲染
    if (rot == Rot4.North && !HasNorthTexture(magazineGraphic))
        return;
    
    Mesh mesh = magazineGraphic.MeshAt(rot);
    Material mat = magazineGraphic.MatAt(rot);
    
    Graphics.DrawMesh(mesh, baseMatrix, mat, 0);
}

/// <summary>
/// 智能判断是否有 north 贴图
/// 原理：比较 MatNorth 和 MatSouth 是否为同一材质
/// </summary>
private bool HasNorthTexture(Graphic graphic)
{
    return graphic.MatNorth != graphic.MatSouth;
}

private static readonly Vector3[] FaceOffsets = new Vector3[]
{
    new Vector3(0f, 1f, 0f),   // North
    new Vector3(0f, 1f, 0f),  // East
    new Vector3(0f, 1f, 0f),   // South
    new Vector3(0f, 1f, 0f)  // West
};

/// <summary>
/// 获取面部组件偏移 - 根据朝向返回对应偏移值
/// </summary>
private Vector3 GetFaceDrawOffset()
{
    Rot4 rot = Pawn.Rotation;
    return FaceOffsets[rot.AsInt];
}
```

#### 关键实现细节

```csharp
// File: Source/Components/SylvieCooldownOverlayComp.cs
#nullable enable

// 1. 获取 headSizeFactor（Biotech DLC 支持不同年龄段头部大小）
float headSizeFactor = 1f;
if (ModsConfig.BiotechActive && Pawn.ageTracker.CurLifeStage.headSizeFactor.HasValue)
{
    headSizeFactor = Pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
}

// 2. 获取头部基础偏移（已包含 bodySizeFactor 的平方根缩放）
Vector3 headOffset = Pawn.Drawer.renderer.BaseHeadOffsetAt(rot);

// 3. 计算面部组件偏移（不缩放，保持固定值）
Vector3 faceOffset = GetFaceDrawOffset();

// 4. 计算最终绘制位置
Vector3 drawPos = Pawn.DrawPos + headOffset + faceOffset;

// 5. 计算缩放矩阵（组件大小按 headSizeFactor 缩放）
Vector3 drawScale = Vector3.one * headSizeFactor;
Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, drawScale);

// 6. 使用 Graphic.MeshAt 获取 mesh（自动处理翻转）
Mesh mesh = graphic.MeshAt(rot);
Material mat = graphic.MatAt(rot);
```

#### 朝向处理实现细节

**核心原理**：
- 使用 `Graphic.MeshAt(rot)` 获取正确的 mesh（会自动处理 West 朝向的翻转）
- 使用 `Graphic.MatAt(rot)` 获取对应朝向的材质
- 通过比较 `MatNorth` 和 `MatSouth` 判断是否有独立的 north 贴图

**朝向行为表**：

| 朝向 | MeshAt 行为 | MatAt 行为 | 说明 |
|------|-------------|------------|------|
| South | 正常 mesh | 返回 south 材质 | 正面显示 |
| East | 正常 mesh | 返回 east 材质 | 右侧显示 |
| West | **翻转 mesh** | 返回 east 材质 | 左侧显示（自动翻转） |
| North | 正常 mesh | 返回 north 材质 | 背面显示 |

**错误做法（不要这样做）**：
```csharp
// File: Example - Wrong Approach
#nullable enable

// 错误：使用 MeshPool.plane10 配合 scale 负值
// 这样不会正确翻转 UV！
Vector3 wrongScale = new Vector3(-headSizeFactor, 1f, headSizeFactor);
Matrix4x4 wrongMatrix = Matrix4x4.TRS(drawPos, Quaternion.identity, wrongScale);
Graphics.DrawMesh(MeshPool.plane10, wrongMatrix, mat, 0); // UV 不会翻转！
```

**正确做法**：
```csharp
// File: Source/Components/SylvieCooldownOverlayComp.cs
#nullable enable

// 正确：使用 Graphic.MeshAt(rot)
Mesh mesh = graphic.MeshAt(rot);  // 自动处理 West 朝向翻转
Material mat = graphic.MatAt(rot);
Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, drawScale);
Graphics.DrawMesh(mesh, matrix, mat, 0);
```

### 9. IncidentWorker_SylvieTrader（事件处理器）

**文件位置**: `Source/Incidents/IncidentWorker_SylvieTrader.cs`

处理奴隶商人事件：
- 生成商队
- 使用 `SylviePawnGenerator` 生成希尔薇
- 发送选择信件
- **智能检测**：如果殖民地已有希尔薇种族的殖民者，事件不会触发

### 10. ChoiceLetter_SylvieOffer（信件类）

**文件位置**: `Source/Letters/ChoiceLetter_SylvieOffer.cs`

自定义选择信件：
- 显示购买选项（100 白银）
- 处理购买和拒绝逻辑
- 转移希尔薇到玩家派系

### 11. Patch_CommsConsole（通讯台补丁）

**文件位置**: `Source/Patches/Patch_CommsConsole.cs`

为通讯台添加呼叫特殊服装贸易商选项：
- `Postfix` - 在原有选项后添加"呼叫特殊服装贸易商"选项
- `SpawnSpecialTrader` - 生成服装贸易商飞船
- `IsTraderAlreadyInOrbit` - 检查是否已有同名贸易商在轨道上

### 12. SylvieCatEarComp（猫耳组件）

**文件位置**: `Source/Components/SylvieCatEarComp.cs`

ThingComp（组件），用于在研究动画期间渲染动态猫耳。

**核心功能**：
- `SetCurrentEarFrame(int frameIndex)` - 设置当前猫耳帧（0=猫耳1, 1=猫耳2）
- `SetShouldRender(bool render)` - 控制是否渲染猫耳
- `PostDraw()` - 在 Pawn 绘制后渲染猫耳贴图

**渲染逻辑**：
- 渲染层级：使用 `PawnRenderUtility.AltitudeForLayer(74)` 转换（位于头发 62 和头盔 75+ 之间）
- 使用 `Graphic.MeshAt(rot)` 和 `Graphic.MatAt(rot)` 处理朝向
- 支持 Biotech DLC 的 `headSizeFactor` 自动缩放
- 使用 `BaseHeadOffsetAt(rot)` 获取头部位置
- 种族检查使用 `SylvieDefNames.IsSylvieRace(Pawn)` 辅助方法

### 13. Patch_ResearchAnimation（研究动画补丁）

**文件位置**: `Source/Patches/Patch_ResearchAnimation.cs`

Harmony 补丁，实现研究动画与猫耳的同步。

**核心组件**：

#### Patch_FaceAnimation_GetCurrentFrame_Research
拦截 FA 的 `GetCurrentFrame` 方法，处理 `Sylvie_ResearchAnimation` 的帧计算：

```csharp
// File: Source/Patches/Patch_ResearchAnimation.cs
#nullable enable

[HarmonyPatch(typeof(FaceAnimation), nameof(FaceAnimation.GetCurrentFrame))]
public static class Patch_FaceAnimation_GetCurrentFrame_Research
{
    private static Dictionary<FaceAnimation, Pawn> animationToPawn = new Dictionary<FaceAnimation, Pawn>();
    private static FieldInfo? startTickField;
    private static FieldInfo? animationFramesField;
    
    private static int GetStartTick(FaceAnimation animation)
    {
        if (startTickField == null)
        {
            startTickField = typeof(FaceAnimation).GetField("startTick", 
                BindingFlags.NonPublic | BindingFlags.Instance);
        }
        return (int?)startTickField?.GetValue(animation) ?? 0;
    }
    
    private static List<FaceAnimationDef.AnimationFrame>? GetAnimationFrames(FaceAnimationDef animationDef)
    {
        if (animationFramesField == null)
        {
            animationFramesField = typeof(FaceAnimationDef).GetField("animationFrames", 
                BindingFlags.NonPublic | BindingFlags.Instance);
        }
        return animationFramesField?.GetValue(animationDef) as List<FaceAnimationDef.AnimationFrame>;
    }
    
    public static bool Prefix(FaceAnimation __instance, int tickGame, 
                            ref FaceAnimationDef.AnimationFrame? __result)
    {
        // 只处理 Sylvie_ResearchAnimation（使用空值传播防止 NullReferenceException）
        if (__instance?.animationDef?.defName != "Sylvie_ResearchAnimation") 
            return true;
        
        // 检查动画到 Pawn 的映射是否存在
        if (!animationToPawn.TryGetValue(__instance, out var pawn) || pawn == null)
            return true;
        
        // 只处理希尔薇种族
        if (!SylvieDefNames.IsSylvieRace(pawn))
            return true;
        
        // 获取动画帧列表（已根据 duration 展开）
        var frames = __instance.animationDef.GetSequentialAnimationFrames();
        if (frames == null || frames.Count == 0) return true;
        
        // 使用反射获取 startTick
        int startTick = GetStartTick(__instance);
        int elapsedTicks = tickGame - startTick;
        
        // 确保 elapsedTicks 非负
        if (elapsedTicks < 0) elapsedTicks = 0;
        
        // 计算循环索引
        int frameCount = frames.Count;
        if (frameCount <= 0) return true;
        
        int frameIndex = elapsedTicks % frameCount;
        
        // 最终安全检查
        if (frameIndex < 0 || frameIndex >= frameCount)
            frameIndex = 0;
        
        __result = frames[frameIndex];
        
        // 根据原始动画帧索引设置猫耳
        var catEarComp = pawn.GetComp<SylvieCatEarComp>();
        if (catEarComp != null)
        {
            // 使用反射获取原始动画帧列表，从中读取 duration
            var originalFrames = GetAnimationFrames(__instance.animationDef);
            if (originalFrames != null && originalFrames.Count > 0)
            {
                int durationPerFrame = originalFrames[0].duration;  // 从 XML 读取 duration
                // 使用默认值防止除零
                if (durationPerFrame <= 0) durationPerFrame = SylvieConstants.DefaultAnimationDuration;
                
                int originalFrameIndex = frameIndex / durationPerFrame;
                int originalFrameCount = originalFrames.Count;
                if (originalFrameIndex >= originalFrameCount) 
                    originalFrameIndex = originalFrameCount - 1;
                
                int earFrame = (originalFrameIndex == 0 || originalFrameIndex == 2 || originalFrameIndex == 4) ? 0 : 1;
                catEarComp.SetCurrentEarFrame(earFrame);
                catEarComp.SetShouldRender(true);
            }
        }
        
        return false;  // 跳过原始方法
    }
}
```

**关键空值检查点**：

1. **动画定义检查**：`__instance?.animationDef?.defName` - 使用空值传播运算符防止 NullReferenceException
2. **Pawn 映射检查**：`animationToPawn.TryGetValue(__instance, out var pawn)` - 确保动画已注册到 Pawn
3. **种族检查**：`SylvieDefNames.IsSylvieRace(pawn)` - 使用辅助方法进行类型安全的判断
4. **帧列表检查**：`frames == null || frames.Count == 0` - 确保动画帧数据有效
5. **边界检查**：`frameIndex < 0 || frameIndex >= frameCount` - 防止数组越界
6. **组件检查**：`catEarComp != null` - 确保猫耳组件存在

**猫耳帧映射**：
| 原始动画帧 | 持续时间 | 猫耳帧 | 说明 |
|------------|----------|--------|------|
| 0 | 30 ticks | 0（猫耳1）| 第1帧显示猫耳1 |
| 1 | 30 ticks | 1（猫耳2）| 第2帧显示猫耳2 |
| 2 | 30 ticks | 0（猫耳1）| 第3帧显示猫耳1 |
| 3 | 30 ticks | 1（猫耳2）| 第4帧显示猫耳2 |
| 4 | 30 ticks | 0（猫耳1）| 第5帧显示猫耳1 |
| 5 | 30 ticks | 1（猫耳2）| 第6帧显示猫耳2 |
| 6 | 30 ticks | 1（猫耳2）| 第7帧显示猫耳2 |
| 7 | 30 ticks | 1（猫耳2）| 第8帧显示猫耳2 |

#### Patch_FacialAnimationControllerComp_CompTick
在 `CompTick` 中检测研究状态变化，控制猫耳显示/隐藏：

```csharp
// File: Source/Patches/Patch_ResearchAnimation.cs
#nullable enable

[HarmonyPatch(typeof(FacialAnimationControllerComp), nameof(FacialAnimationControllerComp.CompTick))]
public static class Patch_FacialAnimationControllerComp_CompTick
{
    public static void Postfix(FacialAnimationControllerComp __instance)
    {
        Pawn? pawn = __instance.parent as Pawn;
        if (pawn == null) return;
        if (!SylvieDefNames.IsSylvieRace(pawn)) return;
        
        var catEarComp = pawn.GetComp<SylvieCatEarComp>();
        if (catEarComp == null) return;
        
        // 检查当前是否在研究
        bool isResearchNow = pawn.CurJobDef != null && pawn.CurJobDef.defName == "Research";
        
        // 如果刚刚停止研究，隐藏猫耳
        if (wasResearchBefore && !isResearchNow)
        {
            catEarComp.SetShouldRender(false);
        }
    }
}
```

**空值检查模式说明**：

补丁中使用了防御性编程的空值检查模式来修复 CS8602 警告：

```csharp
// File: Source/Patches/Patch_ResearchAnimation.cs
#nullable enable

// 模式1：早期返回（Guard Clause）
Pawn? pawn = __instance.parent as Pawn;
if (pawn == null) return;

// 模式2：使用辅助方法进行类型安全的判断
if (!SylvieDefNames.IsSylvieRace(pawn)) return;

// 模式3：组件获取检查
var catEarComp = pawn.GetComp<SylvieCatEarComp>();
if (catEarComp == null) return;
```

这种模式的优点：
1. **消除 CS8602 警告** - 编译器能够识别 null 检查后的变量为非空
2. **提高代码可读性** - 每个检查点都有明确的意图
3. **避免深层嵌套** - 使用早期返回代替嵌套的 if 语句
4. **类型安全** - `IsSylvieRace` 辅助方法封装了种族判断逻辑

#### Patch_FacialAnimationControllerComp_InitializeIfNeed_Research
在动画初始化时注册动画到 Pawn 的映射：

```csharp
// File: Source/Patches/Patch_ResearchAnimation.cs
#nullable enable

[HarmonyPatch(typeof(FacialAnimationControllerComp), "InitializeIfNeed")]
public static class Patch_FacialAnimationControllerComp_InitializeIfNeed_Research
{
    public static void Postfix(FacialAnimationControllerComp __instance, 
                               Pawn ___pawn, 
                               Dictionary<string, List<FaceAnimation>> ___animationDict)
    {
        if (!SylvieDefNames.IsSylvieRace(___pawn)) return;
        
        foreach (var kvp in ___animationDict)
        {
            foreach (var animation in kvp.Value)
            {
                if (animation.animationDef.defName == "Sylvie_ResearchAnimation")
                {
                    Patch_FaceAnimation_GetCurrentFrame_Research.RegisterAnimation(animation, ___pawn);
                }
            }
        }
    }
}
```

**重要技术细节**：

1. **FA 原生帧计算行为**：
   - `GetSequentialAnimationFrames()` 会根据每帧的 `duration` 展开帧列表
   - 例如：8帧，每帧 duration=30，展开后共 240 帧（8 × 30）
   - FA 原生的 `GetCurrentFrame` 使用 `tickGame - startTick` 直接索引展开后的列表
   - 这意味着同一原始帧会持续 `duration` 个 tick

2. **猫耳同步机制**：
   - 获取展开后的帧索引：`frameIndex = elapsedTicks % frameCount`
   - 使用反射获取原始动画帧列表：`GetAnimationFrames(animationDef)`
   - 从原始帧读取 duration：`durationPerFrame = originalFrames[0].duration`
   - 计算原始帧索引：`originalFrameIndex = frameIndex / durationPerFrame`
   - 根据原始帧索引设置猫耳：`earFrame = (originalFrameIndex == 0 || originalFrameIndex == 2 || originalFrameIndex == 4) ? 0 : 1`

2. **反射获取 startTick**：
   - `startTick` 是 `FaceAnimation` 的私有字段
   - 使用反射获取：`typeof(FaceAnimation).GetField("startTick", BindingFlags.NonPublic | BindingFlags.Instance)`

3. **空值检查**：
   - `__instance?.animationDef?.defName` - 防止空引用
   - `animationToPawn.TryGetValue` - 确保字典中有映射
   - `frames == null || frames.Count == 0` - 确保帧列表有效

4. **边界检查**：
   - `if (elapsedTicks < 0) elapsedTicks = 0` - 防止负数索引
   - `if (frameIndex < 0 || frameIndex >= frameCount) frameIndex = 0` - 最终安全检查

3. **动画定义** (`Defs/FacialAnimation/Animations/ResearchAnimation.xml`):

```xml
<FacialAnimation.FaceAnimationDef>
  <defName>Sylvie_ResearchAnimation</defName>
  <animationFrames>
    <!-- 8帧循环动画 -->
    <li>
      <duration>10</duration>
      <browShapeDef>normal</browShapeDef>
      <mouthShapeDef>w_mouth</mouthShapeDef>
      <eyeballShapeDef>white_eye</eyeballShapeDef>
    </li>
    <!-- ... 共8帧 ... -->
    <li>
      <duration>10</duration>
      <browShapeDef>normal</browShapeDef>
      <mouthShapeDef>w_mouth</mouthShapeDef>
      <eyeballShapeDef>circle_eye_down</eyeballShapeDef>
      <emotionShapeDef>gloomy</emotionShapeDef>
    </li>
  </animationFrames>
  <targetJobs>
    <li>Research</li>
  </targetJobs>
  <priority>10003</priority>
</FacialAnimation.FaceAnimationDef>
```

**眼睛形状定义** (`Defs/FacialAnimation/Shapes/EyeShapeEx_Research.xml`):

```xml
<!-- 白眼球形状 -->
<FacialAnimation.EyeballShapeDef>
  <defName>white_eye</defName>
  <label>white eye</label>
</FacialAnimation.EyeballShapeDef>

<!-- 右眼眨眼 -->
<FacialAnimation.EyeballShapeDef>
  <defName>white_eye_right</defName>
  <label>white eye right</label>
</FacialAnimation.EyeballShapeDef>

<!-- 左眼眨眼 -->
<FacialAnimation.EyeballShapeDef>
  <defName>white_eye_left</defName>
  <label>white eye left</label>
</FacialAnimation.EyeballShapeDef>

<!-- 圈圈眼 -->
<FacialAnimation.EyeballShapeDef>
  <defName>circle_eye</defName>
  <label>circle eye</label>
</FacialAnimation.EyeballShapeDef>

<!-- 向下圈圈眼 -->
<FacialAnimation.EyeballShapeDef>
  <defName>circle_eye_down</defName>
  <label>circle eye down</label>
</FacialAnimation.EyeballShapeDef>
```

**注意事项**：
- `EyeballShapeDef` **不能**指定 `eyeballType` 字段（该字段不存在）
- 动画定义（`FaceAnimationDef`）**不应**添加 `raceName` 限制
- TypeDef（如 `EyeballTypeDef`）**需要**添加 `raceName` 限制
- 如果动画有 `raceName` 限制而 TypeDef 也有，会导致 FA 框架匹配冲突

### 14. Patch_Stance_Warmup（瞄准动画同步补丁）

**文件位置**: `Source/Patches/Patch_Stance_Warmup.cs`

Harmony 补丁，拦截 Facial Animation 的 `GetCurrentFrame` 方法，实现瞄准动画同步：

**核心功能**：
- `SylvieAimingTracker` - ThingComp（组件），缓存 Pawn 引用
- `Patch_Pawn_SpawnSetup` - 为希尔薇种族 Pawn 添加跟踪器组件
- `Patch_FaceAnimation_GetCurrentFrame` - 拦截动画帧计算
- `Patch_FacialAnimationControllerComp_InitializeIfNeed` - 注册动画到 Pawn 的映射

**动画帧逻辑**：
- `Stance_Warmup` 状态：基于 warmup 进度计算帧（帧0 → 帧1 → 帧2）
- `VerbState.Bursting` 状态：显示最后一帧（帧2）
- `Stance_Cooldown` 状态：返回 `GetCooldownFrame()` 构造的冷却帧（包含 `browShapeDef: confused` 和 `eyeballShapeDef: lookdown`）
- 其他状态：返回 `true` 让原始方法执行

**冷却帧构造**：
```csharp
// File: Source/Patches/Patch_Stance_Warmup.cs
#nullable enable

private static FaceAnimationDef.AnimationFrame GetCooldownFrame()
{
    if (cachedCooldownFrame == null)
    {
        cachedCooldownFrame = new FaceAnimationDef.AnimationFrame
        {
            duration = 30,
            browShapeDef = ConfusedBrowDef,      // 困惑眉毛
            eyeballShapeDef = LookdownEyeballDef  // 向下看的眼球
        };
    }
    return cachedCooldownFrame;
}
```

**关键 API**：
```csharp
// File: Source/Patches/Patch_Stance_Warmup.cs
#nullable enable

// 武器词条
Verb.verbProps.warmupTime           // 瞄准时间（秒）

// Pawn 属性
pawn.GetStatValue(StatDefOf.AimingDelayFactor)   // 瞄准时间乘数

// Stance_Warmup 状态
warmup.ticksLeft    // 剩余瞄准 ticks
warmup.verb         // 当前使用的 Verb
```

### 15. 初始健康状态系统 (Hediffs/)

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

### 16. 心情效果系统 (Thoughts/)

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
- `SylvieCooldownTracker` - 只负责冷却状态跟踪
- `SylvieCooldownOverlayComp` - 只负责冷却叠加层渲染

### 开闭原则
- 使用 `SylvieDefNames` 常量类，添加新 Def 只需修改一处

### 依赖倒置原则
- 高层模块（`SylvieGameComponent`）依赖低层模块（`SylvieHediffManager`）

### 常量提取模式

**原则**：将代码中的魔法数字提取到集中管理的常量类中。

**示例**：
```csharp
// File: Example - Before Refactoring
#nullable enable

// 重构前（魔法数字分散在代码中）
if (durationPerFrame <= 0) durationPerFrame = 30;  // 30 是什么含义？

// File: Example - After Refactoring
#nullable enable

// 重构后（使用命名常量）
if (durationPerFrame <= 0) durationPerFrame = SylvieConstants.DefaultAnimationDuration;
```

**好处**：
1. 提高代码可读性 - 常量名称说明其用途
2. 便于维护 - 修改只需改一处
3. 避免错误 - 防止不同地方使用不同值

### 辅助方法封装模式

**原则**：将重复的判断逻辑封装为辅助方法。

**示例**：
```csharp
// File: Example - Helper Method Pattern
#nullable enable

// 重构前（分散的字符串比较）
if (pawn?.def?.defName == "Sylvie_Race") { ... }

// 重构后（使用辅助方法）
if (SylvieDefNames.IsSylvieRace(pawn)) { ... }
```

**好处**：
1. 统一种族判断逻辑
2. 类型安全 - 编译器检查参数类型
3. 可空安全 - 辅助方法内部处理 null 检查
4. 便于重构 - 修改判断逻辑只需改一处

### 可空性处理最佳实践

项目启用了 `#nullable enable`，遵循以下模式：

**1. 早期返回模式（Guard Clause）**
```csharp
// File: Source/Patches/Patch_ResearchAnimation.cs
#nullable enable

Pawn? pawn = __instance.parent as Pawn;
if (pawn == null) return;
if (!SylvieDefNames.IsSylvieRace(pawn)) return;
```

**2. 空值传播运算符**
```csharp
// File: Source/Patches/Patch_ResearchAnimation.cs
#nullable enable

// 安全地访问可能为 null 的属性链
if (__instance?.animationDef?.defName != "Sylvie_ResearchAnimation") 
    return true;
```

**3. TryGetValue 模式**
```csharp
// File: Source/Patches/Patch_ResearchAnimation.cs
#nullable enable

if (!animationToPawn.TryGetValue(__instance, out var pawn) || pawn == null)
    return true;
```

**4. XML 注入字段标记**
```csharp
// File: Source/Hediffs/SylvieHediffManager.cs
#nullable enable

public class SylvieRace_CompProperties_NurseHeal : CompProperties
{
    public HediffDef paralysisHediff = null!;  // XML 注入字段
}
```

**5. 可空返回值**
```csharp
// File: Source/Core/SylvieDefNames.cs
#nullable enable

public static HediffDef? Hediff_InitialTraumaDef => 
    HediffDef.Named(Hediff_InitialTrauma);
```

## 编译配置

**项目文件**: `Source/SylvieRace.csproj`
- 目标框架：.NET Framework 4.7.2
- 输出路径：`..\1.6\Assemblies\`
- 输出文件：`SylvieRace.dll`
- 根命名空间：`SylvieMod`

**DLL 引用路径**:
游戏 DLL 文件位于工作区的 `GameDll/` 目录：
- `0Harmony.dll` - Harmony 补丁框架
- `Assembly-CSharp.dll` - RimWorld 核心程序集
- `UnityEngine.CoreModule.dll` - Unity 核心模块

**模组 DLL 引用**:
- `FacialAnimation.dll` - [NL] Facial Animation 模组程序集（用于瞄准动画系统）

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
10. **动画 raceName 使用方式（重要！）**：
    - **TypeDef 需要添加 `raceName` 限制**（如 EyeType、MouthType、CooldownOverlayType 等）
    - **动画定义（FaceAnimationDef）不应添加 `raceName` 限制**
    - 如果动画有 `raceName` 限制，而 TypeDef 也有 `raceName` 限制，会导致 FA 框架匹配冲突，引发 NullReferenceException
    - 正确做法：TypeDef 有 raceName，动画无 raceName，让 FA 的默认动画可以正常应用

## 故障排查

### LidOption 组件渲染问题

#### 问题描述
生成 Sylvie pawn 时，有概率无法呈现任何 LidOption 组件（包括 tear 和 crosshair）。

#### 根本原因
Sylvie 定义了两个 LidOptionTypeDef：
1. `Sylvie_LidOptionNormal` - 用于正常的 LidOption 效果（tear, crosshair）
2. `Sylvie_CooldownOverlay` - 用于冷却动画叠加层

当 pawn 生成时，FA 框架的 `SetRandomFaceType()` 会随机选择这两个 TypeDef 中的一个。如果选中了 `Sylvie_CooldownOverlay`，由于它缺少 `normal` 贴图，导致 LidOption 组件初始化失败。

#### 解决方案
在 `CooldownOverlayType.xml` 中给 `Sylvie_CooldownOverlay` 添加 `<probability>0</probability>`，这样它不会被随机选中，但仍然可以通过代码直接使用（`SylvieCooldownOverlayComp` 不受影响）。

#### 代码示例
```xml
<FacialAnimation.LidOptionTypeDef>
  <defName>Sylvie_CooldownOverlay</defName>
  <texPath>Things/Pawn/Sylvie/CooldownOverlay</texPath>
  <raceName>Sylvie_Race</raceName>
  <probability>0</probability>  <!-- 防止被随机选中 -->
</FacialAnimation.LidOptionTypeDef>
```

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
├── BrowShapeEx.xml      # 瞄准眉毛形状定义（aiming, confused）
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

## 进食动画系统

### 功能概述
进食时显示专属动态表情动画，包括：
- 眼睛向下看（lookdown shape）
- 嘴巴咀嚼动画（open → eating1 → eating2 → eating3）
- 头部移动模拟（headOffset 动画）

### 文件结构
```
Defs/FacialAnimation/
├── MouthShapeEx.xml     # 进食嘴巴形状定义（eating1, eating2, eating3）
└── IngestAnimation.xml  # 进食动画定义

Textures/Things/Pawn/Sylvie/Mouth/Normal/Unisex/
├── eating1_south.png    # 进食嘴巴帧1（正面）
├── eating1_east.png     # 进食嘴巴帧1（侧面）
├── eating2_south.png    # 进食嘴巴帧2（正面）
├── eating2_east.png     # 进食嘴巴帧2（侧面）
├── eating3_south.png    # 进食嘴巴帧3（正面）
└── eating3_east.png     # 进食嘴巴帧3（侧面）
```

### 动画帧序列
| 帧 | duration | headOffset | 眼睛 | 嘴巴 | 说明 |
|----|----------|------------|------|------|------|
| 1 | 30 | (0.0,0,0.0025) | lookdown | open | 张嘴准备 |
| 2 | 20 | (0.0,0,0.005) | lookdown | eating1 | 抬头进食 |
| 3 | 20 | (0.0,0,0) | lookdown | eating2 | 低头咀嚼 |
| 4 | 20 | (0.0,0,0) | lookdown | eating3 | 咀嚼完成 |

### 关键配置
```xml
<FacialAnimation.FaceAnimationDef>
  <defName>Sylvie_IngestAnimation</defName>
  <animationFrames>
    <li>
      <duration>30</duration>
      <headOffset>(0.0,0,0.0025)</headOffset>
      <eyeballShapeDef>lookdown</eyeballShapeDef>
      <mouthShapeDef>open</mouthShapeDef>
    </li>
    <!-- ... 其他帧 ... -->
  </animationFrames>
  <loopIntervalMin>0</loopIntervalMin>
  <loopIntervalMax>30</loopIntervalMax>
  <targetJobs>
    <li>Ingest</li>
  </targetJobs>
  <priority>11</priority>
  <applyWhenStandingOnly>true</applyWhenStandingOnly>
</FacialAnimation.FaceAnimationDef>
```

### headOffset 说明
`headOffset` 用于模拟进食时头部的移动：
- `(0.0, 0, 0.0025)` - 头部轻微抬起（准备进食）
- `(0.0, 0, 0.005)` - 头部进一步抬起（进食中）
- `(0.0, 0, 0)` - 头部回到正常位置（咀嚼）

### priority 设置
- FA 默认 Ingest 动画 priority 为 10
- Sylvie 进食动画 priority 设置为 11，确保覆盖默认动画

## 冷却动画系统

### 功能概述
远程武器冷却期间显示完整的装填动画，包括：
- 困惑眉毛（全程）
- 向下看的眼睛（冷却期间替换眼球类型）
- 汗液动画（3帧，根据冷却进度）
- 弹匣显示（全程）
- 子弹装填动画（投入1→投入2→投入3→N颗子弹，循环到5颗）

### 系统架构

```
┌─────────────────────────────────────────────────────────────┐
│                    冷却动画系统架构                          │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌─────────────────────┐    ┌─────────────────────────────┐ │
│  │ SylvieCooldownTracker│    │ SylvieCooldownOverlayComp  │ │
│  │   (ThingComp)        │    │   (ThingComp)              │ │
│  ├─────────────────────┤    ├─────────────────────────────┤ │
│  │ - IsInRangedCooldown │    │ - PostDraw()               │ │
│  │ - CooldownProgress   │    │ - 渲染汗液/弹匣/子弹        │ │
│  │ - GetSweatFrame()    │    │                            │ │
│  │ - GetBulletAnimation │    │                            │ │
│  └─────────────────────┘    └─────────────────────────────┘ │
│            │                              │                 │
│            └──────────────┬───────────────┘                 │
│                           │                                 │
│            ┌──────────────▼───────────────┐                 │
│            │  Patch_FaceAnimation_GetCurrentFrame │         │
│            │      (Harmony Patch)         │                 │
│            ├──────────────────────────────┤                 │
│            │ - 拦截 FA 的 GetCurrentFrame  │                 │
│            │ - 冷却期间返回冷却帧           │                 │
│            │ - browShapeDef: confused     │                 │
│            │ - eyeballShapeDef: lookdown  │                 │
│            └──────────────────────────────┘                 │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### 核心组件

#### SylvieCooldownTracker
**文件位置**: `Source/Components/SylvieCooldownTracker.cs`

ThingComp（组件），用于跟踪冷却状态和计算动画帧：
- `IsInRangedCooldown` - 是否处于远程武器冷却状态
- `CooldownProgress` - 冷却进度（0-1）
- `GetSweatFrame()` - 获取当前汗液帧（1-3）
- `GetBulletAnimationState()` - 获取子弹动画状态（投入帧，子弹数量）

#### SylvieCooldownOverlayComp
**文件位置**: `Source/Components/SylvieCooldownOverlayComp.cs`

ThingComp（组件），用于渲染冷却期间的叠加层：
- 汗液贴图（3帧）
- 弹匣贴图
- 子弹投入贴图（3帧）
- 子弹计数贴图（5帧）

**渲染流程**：
```
PostDraw() 
  → 检查种族和冷却状态
  → 获取 headSizeFactor（Biotech DLC）
  → BaseHeadOffsetAt() 获取头部位置（已包含 sqrt(bodySizeFactor) 缩放）
  → GetFaceDrawOffset() 获取固定组件偏移（不缩放）
  → 计算 drawPos = Pawn.DrawPos + headOffset + faceOffset
  → 计算 drawScale = Vector3.one * headSizeFactor（组件大小缩放）
  → 按顺序渲染：汗液(baseLayer=61) → 弹匣 → 子弹投入 → 子弹计数
  → 使用 Matrix4x4.TRS(drawPos, Quaternion.identity, drawScale) 进行变换
  → Graphics.DrawMesh 绘制
```

**缩放机制要点**：
- **组件偏移**：保持固定值，不随 `headSizeFactor` 缩放
- **组件大小**：按 `headSizeFactor` 缩放，与头部图形同步
- **头部位置**：`BaseHeadOffsetAt` 已包含 `sqrt(bodySizeFactor)` 缩放

### 渲染流程详解

```
┌────────────────────────────────────────┐
│           PostDraw 渲染流程            │
├────────────────────────────────────────┤
│                                        │
│  1. 检查 Pawn.def.defName == "Sylvie_Race" │
│     └─ 不匹配则直接返回                │
│                                        │
│  2. 获取 SylvieCooldownTracker         │
│     └─ 检查 IsInRangedCooldown         │
│                                        │
│  3. 获取 Pawn 朝向 rot = Pawn.Rotation │
│                                        │
│  4. 获取头部大小因子                   │
│     headSizeFactor = CurLifeStage.headSizeFactor ?? 1.0 │
│     (Biotech DLC 支持不同年龄段头部大小)  │
│     小孩: 0.5/0.75, 成年人: 1.0        │
│                                        │
│  5. 计算头部基础偏移                   │
│     headOffset = BaseHeadOffsetAt(rot) │
│     (已包含 sqrt(bodySizeFactor) 缩放)   │
│                                        │
│  6. 计算面部组件偏移（固定值，不缩放）   │
│     faceOffset = GetFaceDrawOffset()   │
│     (保持固定，确保相对位置不变)         │
│                                        │
│  7. 计算最终绘制位置                   │
│     drawPos = Pawn.DrawPos + headOffset + faceOffset    │
│     drawPos.y += 0.01f (层级调整)      │
│                                        │
│  8. 计算缩放矩阵（组件大小缩放）         │
│     drawScale = Vector3.one * headSizeFactor            │
│     matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, drawScale)│
│                                        │
│  9. 渲染汗液 (3帧循环, baseLayer=61)   │
│     Mesh mesh = graphic.MeshAt(rot)    │
│     Material mat = graphic.MatAt(rot)  │
│     Graphics.DrawMesh(mesh, matrix, mat, 0)               │
│                                        │
│  10. 渲染弹匣 (全程显示，North需检查)  │
│     if (rot == North && !HasNorthTexture) skip            │
│                                        │
│  11. 渲染子弹投入动画 (3帧)            │
│                                        │
│  12. 渲染子弹计数 (1-5颗)              │
│                                        │
└────────────────────────────────────────┘
```

**缩放机制详解**：

| 步骤 | 元素 | 处理方式 | 缩放因子 | 说明 |
|------|------|----------|----------|------|
| 5 | 头部位置 | `BaseHeadOffsetAt(rot)` | `sqrt(bodySizeFactor)` | RimWorld 原版已处理 |
| 6 | 组件偏移 | `GetFaceDrawOffset()` | **无缩放** | 保持固定值 |
| 8 | 组件大小 | `Matrix4x4.TRS` | `headSizeFactor` | 与头部图形同步 |

**关键区别**：
- **组件偏移不缩放**：确保组件相对于头部的相对位置保持不变
- **组件大小缩放**：确保组件与头部图形大小同步变化
- **头部位置已缩放**：`BaseHeadOffsetAt` 返回的位置已经考虑了身体大小

### 朝向处理实现细节

**核心原理**：
- 使用 `Graphic.MeshAt(rot)` 获取正确的 mesh（会自动处理 West 朝向的翻转）
- 使用 `Graphic.MatAt(rot)` 获取对应朝向的材质
- 通过比较 `MatNorth` 和 `MatSouth` 判断是否有独立的 north 贴图

**代码实现**：
```csharp
// File: Source/Components/SylvieCooldownOverlayComp.cs
#nullable enable

// 获取 Pawn 当前朝向
Rot4 rot = Pawn.Rotation;

// 获取头部大小因子（Biotech DLC 支持不同年龄段）
float headSizeFactor = 1f;
if (ModsConfig.BiotechActive && Pawn.ageTracker.CurLifeStage.headSizeFactor.HasValue)
{
    headSizeFactor = Pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
}

// 计算头部基础偏移（已包含 bodySizeFactor 的平方根缩放）
Vector3 headOffset = Pawn.Drawer.renderer.BaseHeadOffsetAt(rot);

// 计算面部组件偏移（固定值，不缩放）
Vector3 faceOffset = GetFaceDrawOffset();

// 计算最终绘制位置
Vector3 drawPos = Pawn.DrawPos + headOffset + faceOffset;
drawPos.y += 0.01f; // 层级调整

// 使用 Graphic 的方法获取对应朝向的 mesh 和 material
Mesh mesh = graphic.MeshAt(rot);
Material mat = graphic.MatAt(rot);

// 计算缩放矩阵（组件大小按 headSizeFactor 缩放）
Vector3 drawScale = Vector3.one * headSizeFactor;
Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, drawScale);

// 渲染
Graphics.DrawMesh(mesh, matrix, mat, 0);

// 智能判断是否有 north 贴图（用于决定是否渲染 North 朝向）
private bool HasNorthTexture(Graphic graphic)
{
    return graphic.MatNorth != graphic.MatSouth;
}
```

**朝向行为表**：

| 朝向 | MeshAt 行为 | MatAt 行为 | 说明 |
|------|-------------|------------|------|
| South | 正常 mesh | 返回 south 材质 | 正面显示 |
| East | 正常 mesh | 返回 east 材质 | 右侧显示 |
| West | **翻转 mesh** | 返回 east 材质 | 左侧显示（自动翻转） |
| North | 正常 mesh | 返回 north 材质 | 背面显示 |

**重要警告**：

**错误做法（不要这样做）**：
```csharp
// File: Example - Wrong Approach
#nullable enable

// 错误：使用 MeshPool.plane10 配合 scale 负值来翻转
// 这样不会正确翻转 UV，导致贴图显示错误！
Vector3 wrongScale = new Vector3(-headSizeFactor, 1f, headSizeFactor);
Matrix4x4 wrongMatrix = Matrix4x4.TRS(drawPos, Quaternion.identity, wrongScale);
Graphics.DrawMesh(MeshPool.plane10, wrongMatrix, mat, 0);
```

**正确做法**：
```csharp
// File: Source/Components/SylvieCooldownOverlayComp.cs
#nullable enable

// 正确：使用 Graphic.MeshAt(rot) 获取 mesh
// 它会自动处理 West 朝向的翻转，包括 UV 翻转
Mesh mesh = graphic.MeshAt(rot);
Material mat = graphic.MatAt(rot);
Vector3 drawScale = Vector3.one * headSizeFactor;
Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, drawScale);
Graphics.DrawMesh(mesh, matrix, mat, 0);
```

**North 贴图智能判断**：

某些组件可能没有独立的 north 贴图（使用 south 贴图作为默认）。通过比较 `MatNorth` 和 `MatSouth` 可以判断：

```csharp
// File: Source/Components/SylvieCooldownOverlayComp.cs
#nullable enable

private bool HasNorthTexture(Graphic graphic)
{
    // 如果 MatNorth 和 MatSouth 是同一材质，说明没有独立的 north 贴图
    return graphic.MatNorth != graphic.MatSouth;
}

// 使用示例
if (rot == Rot4.North && !HasNorthTexture(magazineGraphic))
{
    // North 朝向且没有独立 north 贴图，跳过渲染
    return;
}
```

### 与 Facial Animation 集成

冷却动画通过 `Patch_FaceAnimation_GetCurrentFrame` 补丁与 FA 系统集成：

```csharp
// File: Source/Patches/Patch_Stance_Warmup.cs
#nullable enable

[HarmonyPatch(typeof(FaceAnimation), nameof(FaceAnimation.GetCurrentFrame))]
public static class Patch_FaceAnimation_GetCurrentFrame
{
    public static bool Prefix(FaceAnimation __instance, int tickGame, 
                            ref FaceAnimationDef.AnimationFrame? __result)
    {
        // 只处理 Sylvie_AimingAnimation
        if (__instance.animationDef.defName != "Sylvie_AimingAnimation") 
            return true;
        
        // ... 瞄准帧计算逻辑 ...
        
        // 冷却期间返回冷却帧
        else if (curStance is Stance_Cooldown cooldown)
        {
            __result = GetCooldownFrame();
            return false;  // 跳过原始方法
        }
        
        return true;
    }
    
    private static FaceAnimationDef.AnimationFrame GetCooldownFrame()
    {
        return new FaceAnimationDef.AnimationFrame
        {
            duration = 30,
            browShapeDef = ConfusedBrowDef,      // 困惑眉毛
            eyeballShapeDef = LookdownEyeballDef  // 向下看的眼球
        };
    }
}
```

**关键点**：
- 通过 `eyeballShapeDef` 替换眼球形状为 `lookdown`
- 通过 `browShapeDef` 设置眉毛为 `confused`
- Prefix 返回 `false` 时跳过原始方法，直接使用 `__result`

### 子弹装填动画逻辑

```
冷却进度 → 动画序列
0-20%    → 投入1 → 投入2 → 投入3 → 1颗
20-40%   → 投入1 → 投入2 → 投入3 → 2颗
40-60%   → 投入1 → 投入2 → 投入3 → 3颗
60-80%   → 投入1 → 投入2 → 投入3 → 4颗
80-100%  → 投入1 → 投入2 → 投入3 → 5颗
```

### 贴图资产

| 资产 | South | East |
|-----|-------|------|
| 困惑眉毛 | confused_south.png | confused_east.png |
| 向下看的眼睛 | lookdown_south.png | lookdown_east.png |
| 汗液1 | sweat1_south.png | sweat1_east.png |
| 汗液2 | sweat2_south.png | sweat2_east.png |
| 汗液3 | sweat3_south.png | sweat3_east.png |
| 弹匣 | magazine_south.png | magazine_east.png |
| 子弹投入1 | bullet_insert1_south.png | bullet_insert1_east.png |
| 子弹投入2 | bullet_insert2_south.png | bullet_insert2_east.png |
| 子弹投入3 | bullet_insert3_south.png | bullet_insert3_east.png |
| 一颗子弹 | bullet1_south.png | bullet1_east.png |
| 两颗子弹 | bullet2_south.png | bullet2_east.png |
| 三颗子弹 | bullet3_south.png | bullet3_east.png |
| 四颗子弹 | bullet4_south.png | bullet4_east.png |
| 五颗子弹 | bullet5_south.png | bullet5_east.png |

### XML 定义

**冷却动画定义** (`CooldownAnimation.xml`):
```xml
<FacialAnimation.FaceAnimationDef>
  <defName>Sylvie_CooldownAnimation</defName>
  <animationFrames>
    <li>
      <duration>30</duration>
      <browShapeDef>confused</browShapeDef>
    </li>
  </animationFrames>
  <targetJobs>
    <li>AttackStatic</li>
  </targetJobs>
  <priority>10200</priority>
</FacialAnimation.FaceAnimationDef>
```

**眉毛形状定义** (`BrowShapeEx.xml`):
```xml
<FacialAnimation.BrowShapeDef>
  <defName>confused</defName>
</FacialAnimation.BrowShapeDef>
```

**眼球形状定义** (`EyeShapeEx.xml`):
```xml
<FacialAnimation.EyeballShapeDef>
  <defName>lookdown</defName>
  <label>lookdown</label>
</FacialAnimation.EyeballShapeDef>
```

**冷却叠加层形状定义** (`CooldownShapeEx.xml`):
```xml
<!-- 汗液、弹匣、子弹等 LidOptionShapeDef 定义 -->
<FacialAnimation.LidOptionShapeDef>
  <defName>sweat1</defName>
  <raceName>Sylvie_Race</raceName>  <!-- TypeDef 需要 raceName 限制 -->
</FacialAnimation.LidOptionShapeDef>
<!-- ... sweat2, sweat3, magazine, bullet_insert1-3, bullet1-5 -->
```

**冷却叠加层类型定义** (`CooldownOverlayType.xml`):
```xml
<FacialAnimation.CooldownOverlayTypeDef>
  <defName>Sylvie_CooldownOverlay</defName>
  <raceName>Sylvie_Race</raceName>  <!-- TypeDef 需要 raceName 限制 -->
  <overlayShapeDefs>
    <li>sweat1</li>
    <li>sweat2</li>
    <li>sweat3</li>
    <!-- ... 其他形状 -->
  </overlayShapeDefs>
</FacialAnimation.CooldownOverlayTypeDef>
```

**重要提示**：
- `LidOptionShapeDef` 和 `CooldownOverlayTypeDef` 需要添加 `<raceName>Sylvie_Race</raceName>`
- `FaceAnimationDef`（如 `Sylvie_CooldownAnimation`）**不应添加 raceName**，否则会导致 FA 框架匹配冲突

## 瞄准动画同步系统

### 技术实现
由于 Facial Animation 的动画选择基于 Job，而 `AttackStatic` Job 在整个射击过程中都存在（warmup + cooldown + burst），动画的 `startTick` 是在 Job 开始时设置的。因此使用 Harmony 补丁直接检查 `Stance_Warmup` 状态来实现动态动画同步。

### 核心组件

#### SylvieAimingTracker
**文件位置**: `Source/Patches/Patch_Stance_Warmup.cs`

ThingComp（组件），用于缓存 Pawn 引用和静态字典查找：
- `Pawn` - 获取缓存的 Pawn 引用
- `GetTracker(Pawn)` - 静态方法，从字典获取或创建跟踪器

#### Harmony 补丁
**文件位置**: `Source/Patches/Patch_Stance_Warmup.cs`

- `Patch_Pawn_SpawnSetup` - 为 Sylvie 种族 Pawn 添加跟踪器组件
- `Patch_FaceAnimation_GetCurrentFrame` - 使用 Prefix 拦截 Facial Animation 的帧计算
  - `Stance_Warmup` 状态：基于 warmup 进度计算帧（帧0 → 帧1 → 帧2）
  - `VerbState.Bursting` 状态：显示最后一帧（帧2）
  - `Stance_Cooldown` 状态：返回 `GetCooldownFrame()` 构造的冷却帧（包含困惑眉毛和向下看的眼球）
  - 其他状态：返回 `true` 让原始方法执行
  - **注意**：Prefix 返回 `false` 时会跳过原始方法，直接返回 `__result`

**冷却帧构造**：
```csharp
// File: Source/Patches/Patch_Stance_Warmup.cs
#nullable enable

private static FaceAnimationDef.AnimationFrame GetCooldownFrame()
{
    if (cachedCooldownFrame == null)
    {
        cachedCooldownFrame = new FaceAnimationDef.AnimationFrame
        {
            duration = 30,
            browShapeDef = ConfusedBrowDef,      // 困惑眉毛
            eyeballShapeDef = LookdownEyeballDef  // 向下看的眼球
        };
    }
    return cachedCooldownFrame;
}
```
- `Patch_FacialAnimationControllerComp_InitializeIfNeed` - 注册动画到 Pawn 的映射

### 动画帧边界处理
当计算的帧索引超出范围时，代码会进行边界检查：
- 如果 `frameIndex >= totalFrames`，设置为最后一帧
- 如果 `frameIndex < 0`，设置为第一帧

### 连发状态处理
在 `Stance_Cooldown` 状态下，如果武器处于 `VerbState.Bursting` 状态，
则继续显示最后一帧动画，直到连发结束。

### 瞄准时间计算
```
总瞄准时间 = 武器.warmupTime × Pawn.AimingDelayFactor
```

### 关键 API
```csharp
// File: Source/Patches/Patch_Stance_Warmup.cs
#nullable enable

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
