using System.Collections.Generic;
using System.Linq;
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
/// 凛的吊坠・投影 — 0 cost. Draw 1 card, its next play costs 0.
/// </summary>
[Pool(typeof(RinCardPool))]
public class RinsPendantProjection : CustomCardModel
{
    public RinsPendantProjection() : base(1, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { };

    public override string PortraitPath => "RinsPendantProjection.png".CardPortraitPath();
    public override string CustomPortraitPath => "RinsPendantProjection.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "RinsPendantProjection.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var drawn = await CardPileCmd.Draw(choiceContext, 1m, Owner);
        foreach (var card in drawn)
            card.EnergyCost.SetUntilPlayed(0);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
