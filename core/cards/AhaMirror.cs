using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 阿哈哈哈镜 — Stun all non-elite enemies. In Elite/Boss rooms only summoned
/// minions are affected (the elite/boss and other non-summoned monsters are immune).
/// </summary>
[Pool(typeof(RinCardPool))]
public class AhaMirror : CustomCardModel
{
    public AhaMirror() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
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

        var roomType = CombatState.RunState.CurrentRoom.RoomType;
        bool eliteOrBossRoom = roomType == RoomType.Elite || roomType == RoomType.Boss;

        foreach (var enemy in CombatState.HittableEnemies)
        {
            // In Elite/Boss rooms, only summoned minions are stunned — the initial
            // encounter monsters (the elite/boss itself) are immune.
            if (eliteOrBossRoom && IsInitialEncounterMonster(enemy))
                continue;

            await CreatureCmd.Stun(enemy);
        }
    }

    /// <summary>Whether <paramref name="enemy"/> is one of the encounter's initial
    /// monsters (as opposed to a summon that spawned mid-combat).</summary>
    private static bool IsInitialEncounterMonster(Creature enemy)
    {
        var encounter = enemy.CombatState?.Encounter;
        return encounter?.MonstersWithSlots.Any(ms => ms.Item1.Creature == enemy) == true;
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
