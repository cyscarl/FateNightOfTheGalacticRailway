using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 友谊的证明 — Coin flip: heads → random buff (Draw 1 / +1 Energy / 5 Block), repeat until tails.
/// </summary>
[Pool(typeof(RinCardPool))]
public class FriendshipProof : CustomCardModel
{
    public FriendshipProof() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
    };

    public override string PortraitPath => "FriendshipProof.png".CardPortraitPath();
    public override string CustomPortraitPath => "FriendshipProof.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "FriendshipProof.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner == null) return;

        var rng = Owner.RunState.Rng.CombatCardSelection;

        while (rng.NextDouble() < 0.5) // heads — keep going
        {
            int roll = rng.NextInt(3);
            switch (roll)
            {
                case 0:
                    await CardPileCmd.Draw(choiceContext, 1m, Owner);
                    break;
                case 1:
                    await PlayerCmd.GainEnergy(1m, Owner);
                    break;
                case 2:
                    await CreatureCmd.GainBlock(Owner.Creature, 5m, ValueProp.Unpowered, null);
                    break;
            }
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
