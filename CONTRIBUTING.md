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

## 分支与发布

- **主分支**：`master`，日常直接提交
- **发布标记**：每次上传创意工坊后，在对应提交上打 annotated tag

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

## 已知的历史遗留

2026-09 之前的提交中，有约 10 条未遵循上述格式（集中在 v0.0.x 阶段）。因仓库已有外部 fork，**不做历史重写**，新提交请一律按本文件执行。
