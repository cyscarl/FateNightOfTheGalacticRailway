using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 阿哈哈哈镜 — Stun all non-elite enemies (skips in Elite/Boss rooms).
/// </summary>
[Pool(typeof(RinCardPool))]
public class AhaMirror : CustomCardModel
{
    public AhaMirror() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
    };

    public override string PortraitPath => "AhaMirror.png".CardPortraitPath();
    public override string CustomPortraitPath => "AhaMirror.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "AhaMirror.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState == null) return;

        // Only stun in normal monster rooms, not Elite or Boss
        var roomType = CombatState.RunState.CurrentRoom.RoomType;
        if (roomType == RoomType.Elite || roomType == RoomType.Boss) return;

        foreach (var enemy in CombatState.HittableEnemies)
            await CreatureCmd.Stun(enemy);
    }

    protected override void OnUpgrade()
    {
    }
}
