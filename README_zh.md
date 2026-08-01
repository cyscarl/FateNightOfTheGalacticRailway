# 命运/银河铁道之夜

[English](README.md) | [简体中文](README_zh.md)

基于 Fate 系列题材的 [Slay the Spire 2](https://store.steampowered.com/app/2868840/Slay_the_Spire_2/) 角色 Mod，新增可玩角色——**远坂凛**——包含完整的卡牌组、遗物和药水。

---

## 特性

### 角色：远坂凛

来自宇宙之外，某个存在着魔法与魔术的世界，不断精进自我的少女魔术师——冬木市魔术世家，远坂家第六代当家。

- **初始遗物**：远坂流 — 药水栏+3。每场战斗开始时，随机生成2个魔法宝石（特殊药水）。
- **初始生命**：70
- **初始金币**：99

### 内容

- **44 张新卡牌**（18 攻击卡 + 22 技能卡 + 3 能力卡 + 4 衍生卡）
- **5 个新遗物**
- **5 种特殊宝石药水**
- **自定义卡牌池、遗物池、药水池**

---

## 安装

### 前置依赖

- [Slay the Spire 2](https://store.steampowered.com/app/2868840/Slay_the_Spire_2/)
- [STS2-RitsuLib](https://steamcommunity.com/sharedfiles/filedetails/?id=3747602295)（Steam 创意工坊）
- [BaseLib](https://steamcommunity.com/sharedfiles/filedetails/?id=3747602295)

### 手动安装

1. 从 [Releases](https://github.com/your-repo/fatenightofthegalacticrailway/releases) 下载最新版本。
2. 将 `FateNightOfTheGalacticRailway` 文件夹解压到 Slay the Spire 2 的 mods 目录：
   - Windows：`%USERPROFILE%/AppData/LocalLow/Mega Crit/SlayTheSpire2/mods/`
   - 或：`<游戏安装目录>/mods/`
3. 确保 `STS2-RitsuLib` 和 `BaseLib` 也安装在 mods 目录中。
4. 启动游戏，在角色选择界面选择远坂凛。

### 开发者安装

```bash
git clone https://github.com/your-repo/fatenightofthegalacticrailway.git
cd FateNightOfTheGalacticRailway
dotnet build --configuration Debug
dotnet publish
```

---

## 鸣谢

- **开发**：独立 Mod 开发者
- **美术资源**：多个来源
- **特别感谢**：Slay the Spire 社区

---

## 许可

本项目基于 MIT 许可协议——详见 [LICENSE](LICENSE) 文件。
