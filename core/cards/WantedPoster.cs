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
/// 通缉令
/// </summary>
[Pool(typeof(RinCardPool))]
public class WantedPoster : CustomCardModel
{
    private decimal _vulnAmount = 1m;

    public WantedPoster() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { };

    public override string PortraitPath => "WantedPoster.png".CardPortraitPath();
    public override string CustomPortraitPath => "WantedPoster.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "WantedPoster.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await PowerCmd.Apply<MegaCrit.Sts2.Core.Models.Powers.VulnerablePower>(choiceContext, cardPlay.Target, _vulnAmount, Owner.Creature, this);
        await CardPileCmd.Draw(choiceContext, 1m, Owner, false);
    }

    protected override void OnUpgrade()
    {
        _vulnAmount = 2m;
    }
}
