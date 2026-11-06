# 贡献与开发约定

本文件说明 SylvieRace 的提交信息格式与发布流程。适用于本仓库的所有提交。

---

## 提交信息格式

采用 [Conventional Commits](https://www.conventionalcommits.org/)，**描述部分用中文**。

```
<type>(<scope>): <描述>
```

**type**（必填，小写）：

| type | 用途 |
|------|------|
| `feat` | 新增功能 |
| `fix` | 修复 bug |
| `docs` | 仅文档改动 |
| `refactor` | 重构（不改变外部行为） |
| `perf` | 性能优化 |
| `style` | 格式调整（不影响逻辑） |
| `build` | 构建配置、依赖变更 |
| `chore` | 杂项（版本号、资源更新等） |

**scope**（可选，中文模块名）：标明改动落在哪个模块。

**描述**：动词开头，说明"做了什么"，句末不加句号。

**PR 标题同样适用本格式**——squash 合并后，PR 标题就是 `master` 上的提交标题。

### 示例

```
feat(动画系统): 新增冷却动画系统
fix(生成): 修复新生成的希尔薇无头发问题
fix(事件系统): 修复商队触发但希尔薇不生成的问题
docs(About): 添加 PublishedFileId.txt 文件
chore: 更新 Sylvie 角色女性头像的东向和南向纹理贴图
```

### 正文（可选）

改动较复杂时，在标题下空一行写正文，说明 **根因 / 方案 / 影响范围**。参考 `fix(生成)` 那次提交的写法。

---

## 分支与 PR

- **主分支**：`master`，已开启分支保护——所有改动必须经 PR 合入，不能直接推送
- **分支命名**：`<type>/<短 slug>`，`type` 取上面的提交类型（如 `feat/seek-petting`、`fix/hair-generation`、`docs/pr-workflow`）
- **PR 标题**：与提交信息同格式 `<type>(<scope>): <中文描述>`；合并方式是 **squash**，PR 标题会成为 `master` 上唯一的提交标题，PR 正文成为提交正文
- **PR 正文**：按 `.github/pull_request_template.md` 填写（改动 / 根因与方案 / 验证 / 影响）
- **合并**：仅 squash，合并后自动删除分支
- **评审**：单人仓库不要求他人 approve，合并前自己过一遍模板里的自查项；有外部贡献者时再开启「至少 1 人 review」

### 一次改动的流程

```bash
git switch master && git pull --ff-only
git switch -c fix/hair-generation
# 改动、本地构建、自测

git add -A
git commit -m "fix(生成): 修复新生成的希尔薇无头发问题"
git push -u origin fix/hair-generation

gh pr create    # 网页创建时 PR 模板会自动填充；CLI 需补 --title/--body
gh pr merge --squash --delete-branch
```

### 发布

- **发布标记**：每次上传创意工坊后，在**已合入 `master` 的提交**上打 annotated tag（tag 不受分支保护限制）

```bash
git tag -a v1.0.5 <commit> -m "工坊发布 v1.0.5：修复新生成的希尔薇无头发问题"
git push origin v1.0.5
```

**版本号规则**：正式发布用 `vX.Y.Z`；若某版本还在收集反馈阶段，可先标 `vX.Y.Z-pre`，转正时去掉后缀。

> 版本号的唯一声明位置是 `README.md` 的 `**版本**` 行 —— `About/About.xml`、`.csproj` 中均无版本字段，改动时不要遗漏。

---

## 代码与资源约定

- **代码注释**：中文
- **日志输出**：英文（`Log.Message` 等不支持 UTF-8）
- **XML Def 文本**：英文，翻译通过 `Languages/ChineseSimplified/` 注入
- **构建产物**：不要提交 `Source/obj/`、`Source/bin/`（已被 `.gitignore` 排除）
- **必须保留**：`1.6/Assemblies/SylvieRace.dll` 与 `.pdb` 是创意工坊发布产物，**必须提交**

## 编译

```bash
cd Source
dotnet build --configuration Release
```

产物输出到 `1.6/Assemblies/`。

---

## 许可证

本项目**代码**以 [GPL-3.0-only](./LICENSE) 发布；`Textures/` 等美术资源不在该授权内，保留所有权利。

提交贡献（PR、补丁、翻译、贴图等）即表示你同意：

- 代码贡献以同一许可证（GPL-3.0-only）授权给本项目及下游使用者；
- 贴图等美术资源的贡献，默认授权本项目在仓库与模组发布中永久使用和分发，著作权仍归你；
- 你有权作出上述授权——贡献中不含从其他模组复制的代码或资源，也不含无权再许可的第三方素材。

新增源码文件请在文件开头保留 SPDX 标识：

```csharp
// SPDX-FileCopyrightText: <年份> <你的署名>
// SPDX-License-Identifier: GPL-3.0-only
```

---

## 已知的历史遗留

2026-09 之前的提交中，有约 10 条未遵循上述格式（集中在 v0.0.x 阶段）。因仓库已有外部 fork，**不做历史重写**，新提交请一律按本文件执行。
