using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Keywords;
using FateNightOfTheGalacticRailway.Core;
using FateNightOfTheGalacticRailway.Core.Cards.Projection;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 幻想崩坏 — Innate. Return to hand after play. Deal 4 AOE.
/// While in hand, playing an Ethereal or Exhaust card sets this card's next cost to 0.
/// </summary>
[Pool(typeof(RinCardPool))]
public class FantasyCollapse : CustomCardModel
{
    public FantasyCollapse() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
    }

    // 固有 + 投影 — carries 投影 so the keyword in the effect text is hoverable.
    public override IEnumerable<CardKeyword> CanonicalKeywords => new[]
    {
        CardKeyword.Innate,
        ProjectionKeywords.Projection.GetModCardKeyword()
    };

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(4m, ValueProp.Move)
    };

    public override string PortraitPath => "FantasyCollapse.png".CardPortraitPath();
    public override string CustomPortraitPath => "FantasyCollapse.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "FantasyCollapse.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(base.CombatState!)
            .Execute(choiceContext);

        // Return to hand; AfterCardPlayed cleanup resets cost to normal
        await CardPileCmd.Add(this, PileType.Hand);
    }

    /// <summary>
    /// While this card is in hand, if any actual projection card (伪卡) is played, set
    /// this card's cost to 0 until played. We check the projection card TYPE rather than
    /// the 投影 keyword — cards like 投影，开始 carry the keyword in their text for the
    /// hover tip but are not themselves projection cards.
    /// </summary>
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await base.AfterCardPlayed(choiceContext, cardPlay);
        if (Pile?.Type != PileType.Hand) return;
        if (cardPlay.Card.Owner != Owner) return;
        if (cardPlay.Card == this) return;

        if (cardPlay.Card is ProjectionCardBase)
        {
            EnergyCost.SetUntilPlayed(0);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
