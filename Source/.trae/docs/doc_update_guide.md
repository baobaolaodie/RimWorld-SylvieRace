# 文档更新指导

## 概述

本文档为Subagent提供更新三个README文件的详细指导。

## 文档职责分离

| 文档 | 面向读者 | 应包含 | 禁止包含 |
|------|----------|--------|----------|
| SylvieRace/README.md | 用户/玩家 | 功能介绍、使用指南、更新日志 | 代码实现细节 |
| SylvieRace/Source/README.md | 开发者 | 项目结构、核心组件、代码架构 | 更新日志 |
| 工作区根目录 README.md | 项目索引 | 目录结构、项目简介、更新摘要 | 详细功能说明 |

## 更新任务分配

### 任务1: 更新 SylvieRace/README.md（用户文档）

**目标读者**: 使用Mod的玩家

**必须完成**:
1. 验证"寻求抚摸系统"章节描述准确性
2. 确保触发条件描述与代码一致（7步检查）
3. 确保数值参数准确（20%概率、6小时冷却、+10/+6/+8心情等）
4. 验证v0.0.6-pre更新日志完整性

**检查清单**:
- [ ] 功能概述准确
- [ ] 触发条件完整（7步检查）
- [ ] 目标选择逻辑正确
- [ ] 评分机制描述准确
- [ ] 效果描述正确（心情、社交关系）
- [ ] 数值参数与代码一致
- [ ] 更新日志包含v0.0.6-pre条目

**参考文件**:
- `.trae/docs/requirements.md` - 功能需求
- `.trae/docs/architecture.md` - 系统架构
- `Source/Jobs/JobGiver_SeekPetting.cs` - 触发条件
- `Source/Jobs/JobDriver_SeekPetting.cs` - 效果应用

---

### 任务2: 更新 SylvieRace/Source/README.md（开发者文档）

**目标读者**: Mod开发者

**必须完成**:
1. 更新目录结构（添加Jobs目录）
2. 验证"寻求抚摸系统"章节代码片段准确性
3. 确保架构图反映新组件
4. 添加核心组件详细说明

**检查清单**:
- [ ] 目录结构包含Jobs目录
- [ ] 核心类职责表格包含新组件
- [ ] 寻求抚摸系统章节代码片段与实现一致
- [ ] 架构图包含JobGiver/JobDriver/Tracker
- [ ] 常量定义准确
- [ ] 依赖关系描述正确

**参考文件**:
- `.trae/docs/code_structure.md` - 代码结构
- `.trae/docs/architecture.md` - 系统架构
- `.trae/docs/xml_structure.md` - XML配置
- `Source/Jobs/*.cs` - 实现代码
- `Source/Components/SylvieSeekPettingTracker.cs` - 冷却跟踪

---

### 任务3: 更新工作区根目录 README.md（项目索引）

**目标读者**: 项目浏览者

**必须完成**:
1. 验证SylvieRace项目描述准确性
2. 确保更新日志摘要包含v0.0.6-pre关键信息
3. 保持与其他文档的一致性

**检查清单**:
- [ ] SylvieRace项目描述准确
- [ ] 功能列表完整（22种服装、5种动画、寻求抚摸等）
- [ ] 版本号正确（v0.0.6-pre）
- [ ] 更新日志摘要包含代码架构重构
- [ ] 更新日志摘要包含寻求抚摸功能
- [ ] 与其他文档描述一致

**参考文件**:
- `.trae/docs/current_docs_summary.md` - 现有文档概览
- `SylvieRace/README.md` - 用户文档
- `SylvieRace/Source/README.md` - 开发者文档

---

## 一致性检查点

### 版本号
三个文档中的版本号必须一致：
- 当前版本: v0.0.6-pre
- 游戏版本: RimWorld 1.6

### 功能描述
以下描述必须在三个文档中保持一致：

1. **触发条件**:
   - 每小时20%概率检查
   - 年龄≥10岁
   - 6小时冷却
   - 40格范围

2. **效果数值**:
   - 希尔薇心情: +10，持续16小时
   - 抚摸者心情: +6/+8，持续12小时
   - 社交关系: +10（低心情额外+5）

3. **术语统一**:
   - 使用"寻求抚摸"而非"求抚摸"
   - 使用"殖民者"而非"角色"
   - 使用"好感度"而非"关系值"

### 代码引用
Source/README.md中的代码片段必须与实际代码一致：
- 类名正确
- 方法名正确
- 常量值正确
- 文件路径正确

---

## Subagent任务模板

### 通用要求

**每个Subagent必须**:
1. **首先调用 `crafting-effective-readmes` skill** - 获取README写作指导
2. 阅读 `.trae/docs/doc_update_guide.md` - 了解任务要求
3. 阅读分配的详细参考文件
4. 使用SearchReplace工具修改文档
5. 验证修改质量

### 任务描述模板

```yaml
description: "更新 <文档路径>"
query: "请完善以下文档：

**任务背景：**
- 文档路径：<路径>
- 目标读者：<用户/开发者/索引>
- 更新原因：配合v0.0.6-pre寻求抚摸功能

**当前状态：**
<当前内容摘要>

**需要完成：**
1. <具体任务1>
2. <具体任务2>
3. <具体任务3>

**参考信息：**
- 需求文档：.trae/docs/requirements.md
- 架构文档：.trae/docs/architecture.md
- 代码结构：.trae/docs/code_structure.md
- 现有概览：.trae/docs/current_docs_summary.md

**⚠️ 强制要求：**
**本文档是README文件，你必须在修改前先调用 `crafting-effective-readmes` skill 获取写作指导，然后严格按照指导进行文档更新。这是强制要求，不可跳过。**

**执行步骤：**
1. **首先调用 `crafting-effective-readmes` skill** - 获取README写作指导
2. **阅读参考文件** - 了解功能实现细节
3. **分析当前文档** - 识别需要改进的地方
4. **执行文档修改** - 使用SearchReplace工具
5. **验证修改质量** - 确保符合写作指导要求

**输出要求：**
- 使用SearchReplace工具修改
- 保持与现有风格一致
- 确保内容准确对应代码实现
- **必须在开始修改前调用 `crafting-effective-readmes` skill**"
subagent_type: "rimworld-mod-developer"
response_language: "中文"
```

---

## 质量检查清单

### 修改前检查
- [ ] 已阅读所有参考文件
- [ ] 已调用crafting-effective-readmes skill
- [ ] 理解功能实现细节

### 修改中检查
- [ ] 保持文档原有风格
- [ ] 使用准确的术语
- [ ] 数值与代码一致
- [ ] 不添加未实现的功能

### 修改后检查
- [ ] 版本号一致
- [ ] 无拼写错误
- [ ] 格式正确
- [ ] 链接有效（如有）

---

## 重要提醒

1. **严禁跳过crafting-effective-readmes skill调用** - 这是强制要求
2. **所有结论必须有据可查** - 基于实际代码，不凭经验猜测
3. **保持术语一致性** - 三个文档使用统一术语
4. **数值必须准确** - 与代码中的常量完全一致
5. **不添加未经验证的信息** - 只描述已实现的功能
