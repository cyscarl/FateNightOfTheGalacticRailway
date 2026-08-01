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
/// 重返青春的特调
/// </summary>
[Pool(typeof(RinCardPool))]
public class RejuvenationSpecial : CustomCardModel
{
    public RejuvenationSpecial() : base(3, CardType.Power, CardRarity.Rare, TargetType.None)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        // No canonical vars
    };

    public override string PortraitPath => "RejuvenationSpecial.png".CardPortraitPath();
    public override string CustomPortraitPath => "RejuvenationSpecial.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "RejuvenationSpecial.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<FateNightOfTheGalacticRailway.Core.Powers.ExtraTurn>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}
