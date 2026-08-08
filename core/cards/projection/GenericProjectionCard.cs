using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace FateNightOfTheGalacticRailway.Core.Cards.Projection;

/// <summary>
/// 伪卡牌 — generic projection fallback for cards that have no dedicated projection card
/// (e.g. base-game cards). 0 cost, draw 1, Exhaust, Rins Pendant art.
/// </summary>
[Pool(typeof(WeakenedCardPool))]
public class GenericProjectionCard : ProjectionCardBase
{
    public GenericProjectionCard() : base(CardType.Skill, CardRarity.Common, TargetType.None)
    {
    }

    public override string PortraitPath => "RinsPendant.png".CardPortraitPath();
    public override string CustomPortraitPath => "RinsPendant.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "RinsPendant.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner == null) return;
        await CardPileCmd.Draw(choiceContext, 1, Owner);
    }
}
