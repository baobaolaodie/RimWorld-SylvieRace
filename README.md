# Sylvie Race | 希尔薇种族

一名满身伤痕的少女，你和她的故事即将开始。

## 功能介绍

### 种族特性
- **希尔薇种族**：基于 Humanoid Alien Races 框架的独特种族
- **专属发型**：凌乱短发
- **动态表情**：支持 Facial Animation 动态表情系统
- **独特背景**：专属背景故事

### 事件系统
- 游戏开始后约 1.4 游戏小时（5000 ticks），将自动触发奴隶商人事件
- 商人将带来希尔薇，你可以选择支付 100 白银收留她

### 服装系统
- **19 种专属服装**：包括旗袍、和服、女仆装、学生制服、婚纱等
- **轨道贸易商**：通过通讯台呼叫专用服装贸易商购买服装
- **种族限制**：服装仅限希尔薇种族穿着

### 动态表情
- 支持眼睛、眉毛、嘴巴等多种表情变化
- 根据心情、健康状况自动切换表情
- 支持眨眼、流泪等细节动画

## 前置依赖

| 模组 | Steam Workshop |
|------|----------------|
| Harmony | [链接](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077) |
| Humanoid Alien Races | [链接](https://steamcommunity.com/sharedfiles/filedetails/?id=839005762) |
| [NL] Facial Animation - WIP | [链接](https://steamcommunity.com/sharedfiles/filedetails/?id=1635901197) |
| [NL] Facial Animation - Experimentals | [链接](https://steamcommunity.com/sharedfiles/filedetails/?id=2952879982) |

**注意**：必须按正确顺序加载模组，本模组应加载在所有前置依赖之后。

## 使用指南

### 如何获得希尔薇
1. 开始新游戏或加载存档
2. 等待约 1.4 游戏小时（游戏内时间）
3. 奴隶商人事件将自动触发
4. 选择支付 100 白银收留希尔薇

### 如何购买服装
1. 建造通讯台
2. 选择殖民者操作通讯台
3. 选择"呼叫专用服装贸易商（免费）"
4. 与贸易商交易购买服装

## 兼容性

- **游戏版本**：RimWorld 1.6
- **DLC 兼容**：
  - ✅ Core
  - ✅ Royalty
  - ✅ Ideology
  - ✅ Biotech（支持儿童体型）
  - ✅ Anomaly

- **已知兼容**：
  - Humanoid Alien Races
  - Facial Animation 系列

- **可能不兼容**：
  - 其他修改同类型服装的模组
  - 其他种族模组（可能有服装冲突）

## 更新日志

### v0.0.2-pre (2026-03-03)
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
