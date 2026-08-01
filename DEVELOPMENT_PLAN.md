# Fate/Night of the Galactic Railway — 开发文档

> **注**: 本文件为开发过程记录。最终设计以 `E:\Project\STS2Mods\todo.md` 为准。

## 一、基础信息

| 项目 | 值 |
|------|-----|
| Mod ID | `FateNightOfTheGalacticRailway` |
| 中文名 | 命运·银河铁道之夜 |
| 英文名 | Fate/Night of the Galactic Railway |
| 角色 | 远坂凛 (TosakaRin) |
| Placeholder | Ironclad |
| 框架 | RitsuLib + BaseLib |
| 依赖 | BaseLib >= 3.3.0, STS2-RitsuLib >= 0.4.27 |
| 游戏版本 | >= 0.107.1 |
| 版本号 | 0.1.0 |

## 二、角色

- **类**: `TosakaRin : ModCharacterTemplate<RinCardPool, RinRelicPool, RinPotionPool>`
- **属性**: `[RegisterCharacter]`, `[RitsuLibOwnedBy("FateNightOfTheGalacticRailway")]`
- **初始 HP**: 70 | **初始金币**: 99 | **性别**: Feminine
- **初始药水栏**: 6 (via TosakaStyle `AfterObtained`)
- **自定义 AssetProfile**: 角色 UI、能量球场景、选人背景

## 三、初始遗物

**远坂流 (TosakaStyle)** — Starter
- 药水栏 +3
- 每场战斗首次玩家回合开始时，随机生成 2 个魔法宝石

## 四、Boss 遗物 (3 选 1)

| 类名 | 中文名 | 效果 |
|------|--------|------|
| FlySafely | 放心飞，开拓永相随！ | 每累计 10 张牌，阿哈打击伤害 +1 |
| Excalibur | 誓约胜利之剑 | 每累计 8 张牌，获得 1 能量 |
| EnumaElish | 天地乖离·开辟之星 | 每额外抽 1 张牌，生成 1 张王之财宝 |
| UnlimitedBladeWorks | 无限剑制 | 复制每回合前 2 张牌 (费用-1, 虚无, 消耗) |

## 五、特殊药水 (5 种)

| 类名 | 中文名 | 效果 |
|------|--------|------|
| EnergyGemPotion | 能量宝石 | 获得 2 点能量 |
| PioneerGemPotion | 开拓宝石 | 生成 3 张消耗的阿哈打击 |
| TreasureGemPotion | 财宝宝石 | 抽 3 张牌 |
| ProjectionGemPotion | 投影宝石 | 随机获得 2 张凛的卡牌 (费用 0, 虚无, 消耗) |
| ExcaliburGemPotion | 圣剑宝石 | 对所有敌人造成 15 点伤害 |

## 六、卡牌 (44 张 + 4 衍生)

### 攻击卡 (18 张)

| 类名 | 中文名 | 费用 | 稀有度 | 伤害 | 目标 | 关键词 | 升级 |
|------|--------|------|--------|------|------|--------|------|
| Cooperation | 配合无间 | 1 | Common | 6 | AnyEnemy | | 6→9 |
| FullAttack | 全力猛攻 | 1 | Common | 6 | AnyEnemy | | 6→9, 格挡 5→7 |
| SuppressionTactic | 压制战术 | 1 | Common | 3 | AllEnemies | | 3→5 |
| MagicGemFire | 魔术宝石·火 | 1 | Common | 6 | AnyEnemy | | 6→9 |
| MagicGemVoid | 魔术宝石·空 | 1 | Common | 3 | AllEnemies | | 3→5 |
| GaeBolg | 穿刺死棘之枪 | 2 | Uncommon | 18 | AnyEnemy | Retain | 18→22 |
| RuleBreaker | 万符必应破戒 | 1 | Rare | 6 | AnyEnemy | | 6→9 |
| AhaStrike | 阿哈打击！ | 0 | Common | 3 | AnyEnemy | | 3→6 |
| AhaSweep | 阿哈横扫！ | 1 | Uncommon | 3 | AllEnemies | | 3→5 |
| AhaSword | 阿哈之剑！ | 1 | Rare | 4 | AllEnemies | | 4→6 |
| Boring | 无聊！ | 1 | Uncommon | 4 | AnyEnemy | | 4→7 |
| CraneWingThree | 鹤翼三连 | 1 | Common | 9 | AnyEnemy | | 9→12 |
| ProjectionBegin | 投影，开始 | 1 | Common | 4 | AnyEnemy | | 4→6 |
| FantasyCollapse | 幻想崩坏 | 2 | Uncommon | 4 | AllEnemies | Innate | 4→7 |
| GoldenSlash1 | 必胜黄金连斩·一 | 0 | Uncommon | 6 | AnyEnemy | | 6→8 |
| GoldenSlash2 | 必胜黄金连斩·二 | 0 | Uncommon | 4 | Random | | 4→6 |
| GoldenSlash3 | 必胜黄金连斩·三 | 0 | Uncommon | 3 | AllEnemies | | 3→5 |
| ManaBurst | 魔力放出 | 40 | Rare | 50 | AllEnemies | Innate,Retain | 50→60 |

### 技能卡 (23 张)

| 类名 | 中文名 | 费用 | 稀有度 | 效果 | 升级 |
|------|--------|------|--------|------|------|
| RinsPendantPioneer | 凛的吊坠·开拓 | 0 | Rare | 每 2 阿哈打击→1 消耗阿哈打击 | 不变 |
| RinsPendantTreasure | 凛的吊坠·财宝 | 0 | Rare | 生成 4 张王之财宝 | 不变 |
| RinsPendantProjection | 凛的吊坠·投影 | 0 | Rare | 抽 1 牌费用 0 | 1→2 |
| RinsPendantSword | 凛的吊坠·圣剑 | 0 | Rare | 下 3 张必胜连斩费用 0 | 3→4 |
| MagicGemWind | 魔术宝石·风 | 1 | Common | 回复 8 HP | 8→11 |
| MagicGemEarth | 魔术宝石·地 | 1 | Common | 获得 10 格挡 | 10→13 |
| MagicGemWater | 魔术宝石·水 | 1 | Common | 目标 +2 额外伤害/击 | 2→3 |
| EpicClayTablet | 史诗泥板 | 1 | Uncommon | 抽 2 牌 | 2→3 |
| SimpleTrial | 完全简朴的试练 | 0 | Uncommon | 获得 1 能量 | 1→2 |
| PerfectProjector | 完美投影仪 | 1 | Uncommon | 复制上一张牌效果 | 不变 |
| LawAka | 炽天覆七重圆环 | 2 | Uncommon | +15 格挡, 反弹 50% 伤害 | 15→20 |
| WantedPoster | 通缉令 | 1 | Uncommon | 1 易伤 + 抽 1 牌 | 1→2 |
| PassTheParcel | 击鼓传花 | 1 | Uncommon | 标记, 3 击→9 AOE | 3→2 |
| KingWine | 王之陈酿 | 1 | Uncommon | 随机宝石药水 | 不变 |
| FakeBook | 伪臣之书 | 1 | Uncommon | +6 格挡 + 50% 当前格挡 | 6→8 |
| MoralApproval | 崇高道德的赞许 | 0 | Uncommon | +1 力量 | 1→2 |
| KingGoblet | 王之大杯 | 2 | Uncommon | 下回合 +2 能量 + 抽 2 | 不变 |
| FriendshipProof | 友谊的证明 | 0 | Uncommon | 硬币循环增益 | 不变 |
| AhaMirror | 阿哈哈哈镜 | 2 | Uncommon | 击晕非精英敌人 | 不变 |
| DeathTitanCloak | 死亡泰坦的隐身衣 | 0 | Uncommon | Innate, 抽牌堆消耗1+抽1 | 不变 |
| AhaSupport | 阿哈来支持了！ | 1 | Common | 阿哈打击伤害 +1 | +1→+2 |
| GoldenRule | 这就是黄金律！ | 1 | Common | Retain, 每牌→王之财宝 | 不变 |
| OpenLock | 打开门锁！ | 1 | Rare | 王之财宝 + 随机技能 | 不变 |
| TrueEye | 心眼（真） | 1 | Rare | 触发牌组 2 张牌效果 | 不变 |

### 能力卡 (3 张)

| 类名 | 中文名 | 费用 | 稀有度 | 效果 | 升级 |
|------|--------|------|--------|------|------|
| Avalon | 遗世独立的理想乡 | 2 | Rare | 回复 15 + 每牌回 1 | 15→20 |
| WhyAreYouHere | 你为什么会在这里 | 1 | Rare | 每回合奖励升级 | 不变 |
| RejuvenationSpecial | 重返青春的特调 | 3 | Rare | 额外回合 | 不变 |

### 衍生卡 (4 张, 不可从卡池获取)

| 类名 | 中文名 | 费用 | 效果 |
|------|--------|------|------|
| KingTreasure | 王之财宝！ | 1 | Retain, Exhaust, 3 AOE, 手牌唯一, 重复→+3 |
| DivineCreation | 由神创造 | 0 | Heal 3, Ethereal, Exhaust |
| HumanWeave | 由人编织 | 0 | Heal 3 + Draw 1, Ethereal, Exhaust |
| ReturnToEarth | 回归泥土 | 0 | Heal 3 + Draw 1 + 1 Energy, Ethereal, Exhaust |

## 七、Power (15 个)

| 类名 | 中文名 | 类型 | 效果 |
|------|--------|------|------|
| PowerAhaStrikeDamageUp | 成长 | Buff | 阿哈打击伤害 +Amount |
| AhaSwordTracker | 剑制计数 | Buff | 每 3 牌→+1 成长, 每回合重置 |
| AvalonRegen | 理想乡 | Buff | 每牌回 1 HP |
| ExtraTurn | 额外回合 | Buff | 回合结束后额外回合 |
| GoldenRule | 黄金律 | Buff | 每牌→王之财宝 |
| KingGoblet | 王之大杯 | Buff | 下回合 +2 能量 + 抽 2 |
| KingTreasureDamage | 财宝强化 | Buff | 王之财宝伤害增加 |
| LawAkaReflect | 英雄之盾 | Buff | 反弹 50% 伤害 |
| PassTheParcel | 击鼓传花 | Debuff | 受击计数→AOE 爆炸 |
| RuleBreakerMark | 触电 | Debuff | 每击额外伤害 (递归保护) |
| WantedPosterVulnerable | 通缉 | Debuff | 易伤效果 |
| WhyAreYouHere | 奇美拉 | Buff | 每回合奖励卡升级 |
| NextTurnEnergyPower | 下回合能量 | Buff | 下回合 +1 能量 |
| NextGoldenSlashFreePower | 圣剑加护 | Buff | 下 N 张必胜连斩 0 费 |
| RinsPendantPioneerPower | 开拓之力 | Buff | 每 2 阿哈打击→消耗阿哈打击 |

已合并: `RuleBreakerMark` 统一处理额外伤害 (原 WaterMark 已删除)

## 八、状态

- ✅ 全部 44 张主卡 + 4 衍生卡 + 2 基础卡实现
- ✅ 全部 15 个 Power 实现
- ✅ 5 个遗物 + 5 种药水实现
- ✅ 中英文完整本地化
- ✅ 全部卡图已就位
- ✅ 版本号 0.1.0
