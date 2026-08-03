using System.Collections.Generic;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Characters;

namespace FateNightOfTheGalacticRailway.Core.Characters;

[RitsuLibOwnedBy("FateNightOfTheGalacticRailway")]
[RegisterCharacter]
public class TosakaRin : ModCharacterTemplate<RinCardPool, RinRelicPool, RinPotionPool>
{
    public static readonly Color Color = new Color(0.8f, 0.13f, 0.13f, 1f);

    public override string PlaceholderCharacterId => "ironclad";
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 70;
    public override int StartingGold => 99;
    public override Color NameColor => Color;
    public override float AttackAnimDelay => 0f;
    public override float CastAnimDelay => 0f;
    public override List<string> GetArchitectAttackVfx() => [
        "vfx/vfx_attack_slash", "vfx/vfx_attack_slash",
        "vfx/vfx_attack_slash", "vfx/vfx_attack_slash",
        "vfx/vfx_attack_slash"
    ];

    /// <summary>
    /// Override asset profile to point to mod-relative paths under FateNightOfTheGalacticRailway/.
    /// </summary>
    public override CharacterAssetProfile AssetProfile => new(
        Scenes: new CharacterSceneAssetSet(
            EnergyCounterPath: "res://FateNightOfTheGalacticRailway/scenes/tosaka_rin/energy_counter.tscn",
            VisualsPath: "res://FateNightOfTheGalacticRailway/scenes/tosaka_rin/character_visuals.tscn",
            MerchantAnimPath: "res://FateNightOfTheGalacticRailway/scenes/tosaka_rin/character_merchant.tscn",
            RestSiteAnimPath: "res://FateNightOfTheGalacticRailway/scenes/tosaka_rin/character_rest_site.tscn"
        ),
        Ui: new CharacterUiAssetSet(
            IconPath: "res://FateNightOfTheGalacticRailway/scenes/tosaka_rin/character_icon.tscn",
            IconTexturePath: "res://FateNightOfTheGalacticRailway/images/charui/tosaka_rin/character_icon.png",
            CharacterSelectIconPath: "res://FateNightOfTheGalacticRailway/images/charui/tosaka_rin/char_select.png",
            CharacterSelectLockedIconPath: "res://FateNightOfTheGalacticRailway/images/charui/tosaka_rin/char_select_locked.png",
            MapMarkerPath: "res://FateNightOfTheGalacticRailway/images/charui/tosaka_rin/map_marker.png",
            CharacterSelectBgPath: "res://FateNightOfTheGalacticRailway/images/charui/tosaka_rin/select_bg.png"
        ),
        Multiplayer: new CharacterMultiplayerAssetSet(
            ArmPointingTexturePath: "res://FateNightOfTheGalacticRailway/images/charui/tosaka_rin/hand_pointer.png",
            ArmRockTexturePath: "res://FateNightOfTheGalacticRailway/images/charui/tosaka_rin/hand_rock.png",
            ArmPaperTexturePath: "res://FateNightOfTheGalacticRailway/images/charui/tosaka_rin/hand_paper.png",
            ArmScissorsTexturePath: "res://FateNightOfTheGalacticRailway/images/charui/tosaka_rin/hand_scissors.png"
        )
    );
}
