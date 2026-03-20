# 文档更新指导

**版本**: 2026-03-20  
**目标**: 全面完善三个 README 文档

---

## 文档职责分离

| 文档 | 面向读者 | 包含内容 | 禁止内容 |
|------|----------|----------|----------|
| `SylvieRace/README.md` | 最终用户 | 功能介绍、使用指南、兼容性、更新日志 | 技术实现细节、代码示例 |
| `SylvieRace/Source/README.md` | 开发者 | 项目结构、核心组件、代码说明、架构设计 | 更新日志、用户指南 |
| `README.md` (工作区根目录) | 项目索引 | 目录结构、项目列表、总体更新日志 | 详细功能说明 |

---

## 更新任务分配

### 任务 1: 更新 SylvieRace/README.md (用户文档)

**Subagent 任务描述**:
- **文档路径**: `d:\LongYinHaHa\VSCode\Rimworld\SylvieRace\README.md`
- **目标读者**: 最终用户（玩家）
- **更新重点**:
  1. 验证"寻求抚摸系统"章节的所有数值与代码一致
  2. 确保7步检查流程描述清晰准确
  3. 验证心情效果数值（+10/+6/+8）和持续时间（16小时/12小时）
  4. 验证社交关系效果（+10，低心情额外+5）
  5. 检查更新日志 v0.0.6-pre 条目完整性
  6. 确保术语统一（"寻求抚摸"、"希尔薇"、"殖民者"）

**必须遵循的写作规范**:
- 使用 crafting-effective-readmes skill 获取写作指导
- 选择"用户导向"模板
- 语言：中文
- 数值必须与代码完全一致

### 任务 2: 更新 SylvieRace/Source/README.md (开发者文档)

**Subagent 任务描述**:
- **文档路径**: `d:\LongYinHaHa\VSCode\Rimworld\SylvieRace\Source\README.md`
- **目标读者**: Mod 开发者
- **更新重点**:
  1. 验证"寻求抚摸系统"章节的技术描述准确性
  2. 检查代码示例与实现代码一致
  3. 验证核心类职责表中的描述
  4. 检查架构图中是否包含所有相关类
  5. 验证常量定义与代码一致
  6. 确保依赖关系图准确

**必须遵循的写作规范**:
- 使用 crafting-effective-readmes skill 获取写作指导
- 选择"Internal"模板
- 语言：中文
- 代码示例必须可复制粘贴使用

### 任务 3: 更新工作区根目录 README.md (项目索引)

**Subagent 任务描述**:
- **文档路径**: `d:\LongYinHaHa\VSCode\Rimworld\README.md`
- **目标读者**: 项目浏览者、新开发者
- **更新重点**:
  1. 验证 SylvieRace 项目描述中的功能列表完整性
  2. 确保"寻求抚摸系统"已列出
  3. 检查更新日志摘要中 v0.0.6-pre 条目
  4. 验证与其他文档的交叉引用

**必须遵循的写作规范**:
- 使用 crafting-effective-readmes skill 获取写作指导
- 选择"Internal"模板
- 语言：中文

---

## 数值验证清单

### 必须在所有文档中保持一致的数值

| 数值项 | 值 | 代码位置 | 用户文档 | 开发者文档 | 工作区文档 |
|--------|-----|----------|----------|------------|------------|
| 检查间隔 | 2500 ticks (1小时) | JobGiver_SeekPetting.cs:18 | ✅ | ✅ | - |
| 触发概率 | 20% | JobGiver_SeekPetting.cs:19 | ✅ | ✅ | - |
| 搜索距离 | 40 格 | JobGiver_SeekPetting.cs:20 | ✅ | ✅ | - |
| 最小年龄 | 10 岁 | JobGiver_SeekPetting.cs:21 | ✅ | ✅ | - |
| 高好感度阈值 | 40 | JobGiver_SeekPetting.cs:22 | ✅ | ✅ | - |
| 目标心情阈值 | 50% | JobGiver_SeekPetting.cs:23 | ✅ | ✅ | - |
| 基础关系加成 | +10 | JobDriver_SeekPetting.cs:20 | ✅ | ✅ | - |
| 低心情额外加成 | +5 | JobDriver_SeekPetting.cs:25 | ✅ | ✅ | - |
| 低心情阈值 | 30% | JobDriver_SeekPetting.cs:30 | ✅ | ✅ | - |
| 亲密关系阈值 | 40 | JobDriver_SeekPetting.cs:35 | ✅ | ✅ | - |
| 冷却时间 | 15000 ticks (6小时) | SylvieSeekPettingTracker.cs:21 | ✅ | ✅ | - |
| 希尔薇心情值 | +10 | ThoughtDef | ✅ | ✅ | - |
| 希尔薇心情持续 | 16小时 (0.67天) | ThoughtDef | ✅ | ✅ | - |
| 抚摸者心情值(普通) | +6 | ThoughtDef | ✅ | ✅ | - |
| 抚摸者心情值(亲密) | +8 | ThoughtDef | ✅ | ✅ | - |
| 抚摸者心情持续 | 12小时 (0.5天) | ThoughtDef | ✅ | ✅ | - |
| 社交关系持续 | 10天 | ThoughtDef | - | ✅ | - |

---

## 术语统一规范

### 中文术语

| 术语 | 使用场景 | 说明 |
|------|----------|------|
| 寻求抚摸 | 功能名称 | 统一使用此术语，不使用"求抚摸"或其他变体 |
| 希尔薇 | 角色/种族名称 | 中文文档使用，英文代码使用 Sylvie |
| 殖民者 | 目标对象 | 指代被寻求抚摸的其他角色 |
| 抚摸者 | 执行者 | 指代执行抚摸动作的殖民者 |
| 好感度 | 社交关系 | 指代 opinion 值 |
| 心情 | mood | 指代心情值 |

### 英文术语（开发者文档）

| 术语 | 说明 |
|------|------|
| Seek Petting | 功能英文名 |
| Sylvie | 种族/角色名 |
| Colonist | 殖民者 |
| Opinion | 好感度 |
| Mood | 心情 |

---

## 更新日志格式规范

### 日期格式

```
yyyy-MM-dd
```

示例: `2026-03-20`

### 版本号格式

```
vx.x.x-pre  (预发布版)
vx.x.x      (正式版)
```

示例: `v0.0.6-pre`

### 条目格式

```markdown
### vx.x.x-pre (yyyy-MM-dd)
- **功能/修改标题**：
  - 详细描述 1
  - 详细描述 2
  - [查看完整更新日志](../路径)
```

---

## 质量检查清单

### 更新前检查

- [ ] 已阅读 requirements.md 了解功能需求
- [ ] 已阅读 architecture.md 了解技术实现
- [ ] 已阅读 current_docs_summary.md 了解现有文档状态

### 更新中检查

- [ ] 已调用 crafting-effective-readmes skill 获取写作指导
- [ ] 所有数值与代码完全一致
- [ ] 术语使用统一
- [ ] 代码示例准确无误
- [ ] 链接有效

### 更新后检查

- [ ] 文档与代码一致性验证
- [ ] 与其他文档的交叉引用验证
- [ ] 更新日志格式正确
- [ ] 无拼写错误
- [ ] 无语法错误

---

## 参考文件位置

### 规划文件

- `d:\LongYinHaHa\VSCode\Rimworld\SylvieRace\.trae\docs\requirements.md`
- `d:\LongYinHaHa\VSCode\Rimworld\SylvieRace\.trae\docs\current_docs_summary.md`
- `d:\LongYinHaHa\VSCode\Rimworld\SylvieRace\.trae\docs\architecture.md`
- `d:\LongYinHaHa\VSCode\Rimworld\SylvieRace\.trae\docs\doc_update_guide.md` (本文件)

### 源代码文件

- `d:\LongYinHaHa\VSCode\Rimworld\SylvieRace\Source\Jobs\JobGiver_SeekPetting.cs`
- `d:\LongYinHaHa\VSCode\Rimworld\SylvieRace\Source\Jobs\JobDriver_SeekPetting.cs`
- `d:\LongYinHaHa\VSCode\Rimworld\SylvieRace\Source\Components\SylvieSeekPettingTracker.cs`
- `d:\LongYinHaHa\VSCode\Rimworld\SylvieRace\Source\Utils\SylvieValidationUtils.cs`
- `d:\LongYinHaHa\VSCode\Rimworld\SylvieRace\Source\Core\SylvieDefNames.cs`

### XML 定义文件

- `d:\LongYinHaHa\VSCode\Rimworld\SylvieRace\Defs\Thoughts\Sylvie_SeekPettingThoughts.xml`
- `d:\LongYinHaHa\VSCode\Rimworld\SylvieRace\Defs\Jobs\Sylvie_SeekPettingJobDefs.xml`
- `d:\LongYinHaHa\VSCode\Rimworld\SylvieRace\Patches\Sylvie_ThinkTreePatches.xml`

---

## Subagent 调用模板

```yaml
description: "更新 <文档路径>"
query: |
  请更新以下文档：

  **任务背景**:
  - 文档路径: <路径>
  - 目标读者: <用户/开发者>
  - 更新目标: 完善"寻求抚摸"功能描述

  **必须执行的步骤**:
  1. **首先调用 crafting-effective-readmes skill** - 获取 README 写作指导
  2. 阅读 `.trae/docs/requirements.md` - 了解功能需求
  3. 阅读 `.trae/docs/architecture.md` - 了解技术实现
  4. 阅读当前文档 - 了解现有内容
  5. 按照写作指导更新文档
  6. 验证所有数值与代码一致

  **关键约束**:
  - 所有数值必须与代码完全一致
  - 术语必须统一（"寻求抚摸"、"希尔薇"）
  - 更新日志日期格式: yyyy-MM-dd

  **输出要求**:
  - 使用 SearchReplace 工具修改
  - 保持与现有风格一致
  - 确保内容准确对应代码实现

  **⚠️ 强制要求**:
  本文档是 README 文件，你必须在修改前先调用 `crafting-effective-readmes` skill 获取写作指导，然后严格按照指导进行文档更新。这是强制要求，不可跳过。
subagent_type: "rimworld-mod-developer"
response_language: "中文"
```
