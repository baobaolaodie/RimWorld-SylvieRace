# Sylvie Race | 希尔薇种族

一名满身伤痕的少女，你和她的故事即将开始。

**版本**: v1.0.0-pre  
**游戏版本**: RimWorld 1.6  

---

## 功能概览

### 种族特性
- **希尔薇种族**: 基于 Humanoid Alien Races 框架的独特种族
- **专属名字**: 所有希尔薇角色名字统一为"Sylvie"/"希尔薇" + 随机姓氏
- **专属发型**: 凌乱短发，种族独有
- **动态表情**: 完整支持 Facial Animation 动态表情系统
- **独特背景**: 专属背景故事与纹身系统

### 事件系统
游戏开始后约 1.4 游戏小时（5000 ticks），自动触发奴隶商人事件：
- 商人带来希尔薇（作为囚犯）
- 支付 100 白银即可收留她
- 事件仅触发一次，确保希尔薇的独特性

### 服装系统
**19 种专属服装**，只能通过轨道贸易商购买：

| 类别 | 服装 |
|------|------|
| 连衣裙 | 紫色连衣裙、蓝色连衣裙、碎花连衣裙 |
| 套装 | 和服、旗袍、女仆装、重装女仆、学生制服、西装、优雅套装、春节婚纱 |
| 下装 | 优雅裤装、学生裤装 |
| 特殊 | 创可贴、泳装、披肩 |
| 头饰 | 春节头饰 |

**种族限制**: 所有服装仅限希尔薇种族穿着

### 动态表情系统
- 眼睛、眉毛、嘴巴等多种表情变化
- 根据心情、健康状况自动切换
- 支持眨眼、流泪等细节动画
- 需要 Facial Animation WIP + Experimentals 前置

### 纹身系统
- **面部伤痕**: 过去经历留下的面部疤痕
- **身体伤痕**: 覆盖身体的伤痕，诉说着过往
- 希尔薇生成时自动获得专属纹身

---

## 前置依赖

| 模组 | 用途 | Steam 链接 |
|------|------|------------|
| Harmony | 必需 | [2009463077](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077) |
| Humanoid Alien Races | 种族框架 | [839005762](https://steamcommunity.com/sharedfiles/filedetails/?id=839005762) |
| [NL] Facial Animation - WIP | 动态表情基础 | [1635901197](https://steamcommunity.com/sharedfiles/filedetails/?id=1635901197) |
| [NL] Facial Animation - Experimentals | 动态表情扩展 | [2952879982](https://steamcommunity.com/sharedfiles/filedetails/?id=2952879982) |

**加载顺序**: Harmony → HAR → Facial Animation → SylvieRace

---

## 使用指南

### 如何获得希尔薇
1. 开始新游戏或加载存档
2. 等待约 1.4 游戏小时
3. 奴隶商人事件自动触发
4. 选择支付 100 白银收留希尔薇

### 如何购买服装
1. 建造通讯台
2. 选择殖民者操作通讯台
3. 选择"呼叫专用服装贸易商（免费）"
4. 与贸易商交易购买服装

### 角色定制
在角色编辑界面可为希尔薇：
- 更换发型（仅限希尔薇专属发型）
- 选择纹身样式（面部/身体伤痕）
- 查看动态表情效果

---

## 兼容性

### DLC 支持
- ✅ Core
- ✅ Royalty
- ✅ Ideology
- ✅ Biotech（支持儿童体型）
- ✅ Anomaly

### 已知兼容
- Humanoid Alien Races
- Facial Animation 系列
- 大多数种族模组（无服装冲突时）

### 可能冲突
- 修改同类型服装的模组
- 强制修改所有种族服装穿着规则的模组

---

## 技术信息

### 命名规范
- 所有 Def 名使用 `SylvieRace_` 前缀
- XML 中使用英文，翻译通过 Languages 注入
- 服装标签: `SylvieRace_Apparel`

### 文件结构
```
SylvieRace/
├── Defs/
│   ├── Races/           # 种族定义
│   ├── Apparel/         # 服装定义
│   ├── FacialAnimation/ # 动态表情
│   └── ...
├── Source/              # C# 源码
├── Textures/            # 贴图资源
└── Languages/           # 翻译文件
```

---

## 更新日志

### v0.0.3-pre (2026-03-03)
- 修复翻译系统，将语言目录从 SimplifiedChinese 重命名为 ChineseSimplified
- 修复名字翻译，FirstName 和 Nick 都正确显示为"Sylvie"/"希尔薇"
- 添加 AlienRace.ThingDef_AlienRace 翻译目录，修复种族名称显示
- 添加希尔薇种族专属名字系统
- 所有希尔薇角色名字统一为"Sylvie"/"希尔薇" + 随机姓氏
- 支持中英文翻译
- 添加纹身系统（面部伤痕、身体伤痕）
- 希尔薇生成时自动获得专属纹身
- 重构所有 defName 命名，添加 SylvieRace_ 前缀避免冲突
- 添加完整的中英文翻译支持
- 将 Defs 中的中文文本改为英文，符合 modding 规范
- 重命名多个定义以符合命名规范
- 验证希尔薇只能通过事件生成，不会随机出现

### v0.0.2 (2026-03-03)
- 添加前置依赖 [NL] Facial Animation - Experimentals
- 尝试修复 XML 编码损坏问题（PowerShell Set-Content 导致 UTF-8 编码错误）
- 尝试修复贸易商不出售服装的问题（tradeability 从 Sellable 改为 All）
- 尝试修复 SpringFestivalHeadwear 重复 thingCategories 错误
- 尝试添加自定义服装类别"希尔薇衣服"（储存区筛选）
- 尝试禁用服装制作配方，服装只能通过贸易商获得
- 尝试修复服装无法被储存区识别的问题（添加 thingCategories）
- 重构 Defs 文件结构，按类型分离到独立文件
- 补充服装定义中缺失的标准字段
- 移除种族定义中冗余的特性屏蔽配置

### v0.0.1 (2026-03-02)
- 初始预发布版本
- 添加希尔薇种族定义
- 实现奴隶商人事件系统
- 添加 19 种专属服装
- 实现轨道服装贸易商
- 添加动态表情支持
- 修复动态表情无法加载的问题（目录结构重组）
- 修复服装生成到其他种族的问题（添加 apparel.tags 限制）

---

## 开发者信息

技术实现细节参见 [Source/README.md](Source/README.md)

---

*"Every scar tells a story."*
