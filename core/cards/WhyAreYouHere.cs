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
/// 你为什么会在这里
/// </summary>
[Pool(typeof(RinCardPool))]
public class WhyAreYouHere : CustomCardModel
{
    public WhyAreYouHere() : base(1, CardType.Power, CardRarity.Rare, TargetType.None)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        // No canonical vars
    };

    public override string PortraitPath => "WhyAreYouHere.png".CardPortraitPath();
    public override string CustomPortraitPath => "WhyAreYouHere.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "WhyAreYouHere.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<FateNightOfTheGalacticRailway.Core.Powers.WhyAreYouHere>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}
