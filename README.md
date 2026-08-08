# Fate/Night of the Galactic Railway

[English](README.md) | [简体中文](README_zh.md)

A [Slay the Spire 2](https://store.steampowered.com/app/2868840/Slay_the_Spire_2/) mod inspired by the Fate universe, adding a new playable character — **Tohsaka Rin** (远坂凛) — along with her full card set, relics, and potions.

---

## Features

### Character: Tohsaka Rin

A young magus from a world beyond the stars, where magic and mystery still thrive. The sixth head of the Tohsaka family, a lineage of mages from Fuyuki City. Her combat style revolves around card generation, versatile gem potions, and powerful chain attacks.

- **Starter Relic**: Tosaka Style — +3 potion slots. At the start of each combat, generate 2 random gem potions.
- **Starting HP**: 70
- **Starting Gold**: 99

### Content

- **44 new cards** (18 Attack, 22 Skill, 3 Power + 4 Token cards)
- **5 new relics**
- **5 special gem potions**
- **Custom card pool, relic pool, and potion pool**

---

## Installation

### Prerequisites

- [Slay the Spire 2](https://store.steampowered.com/app/2868840/Slay_the_Spire_2/)
- [STS2-RitsuLib](https://steamcommunity.com/sharedfiles/filedetails/?id=3747602295) (Steam Workshop)
- [BaseLib](https://steamcommunity.com/sharedfiles/filedetails/?id=3748744118)

### Manual Installation

1. Download the latest release from [Releases](https://github.com/cyscarl/FateNightOfTheGalacticRailway/releases).
2. Extract the `FateNightOfTheGalacticRailway` folder into your Slay the Spire 2 mods directory （`<STS2 install dir>/mods/FateNightOfTheGalacticRailway/`):
   - `FateNightOfTheGalacticRailway.dll`
   - `FateNightOfTheGalacticRailway.pck`
   - `mod_manifest.json`
3. Ensure `STS2-RitsuLib` and `BaseLib` are also installed in the mods directory.
4. Launch the game and select Tohsaka Rin from the character select screen.

### For Developers

```bash
# Clone the repository
git clone https://github.com/cyscarl/FateNightOfTheGalacticRailway.git

# Open in Godot 4.5.1 Mono and build
dotnet build --configuration Debug
dotnet publish
```

---

## Credits

- **Development**: cyscarl
- **Art Assets**: Various sources
- **Special Thanks**: The Slay the Spire modding community

---

## License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.
