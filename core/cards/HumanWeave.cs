using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 由人编织 — 0 cost. Heal 3, Draw 1. Ethereal. Exhaust. Derived from WhyAreYouHere (level 2).
/// </summary>
[Pool(typeof(RinCardPool))]
public class HumanWeave : CustomCardModel
{
    public HumanWeave() : base(0, CardType.Skill, CardRarity.Event, TargetType.None)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => new[]
    {
        CardKeyword.Ethereal,
        CardKeyword.Exhaust,
    };

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { };

    private decimal _healAmount = 3m;

    public override string PortraitPath => "HumanWeave.png".CardPortraitPath();
    public override string CustomPortraitPath => "HumanWeave.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "HumanWeave.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Heal(Owner.Creature, _healAmount);
        await CardPileCmd.Draw(choiceContext, 1m, Owner);
    }

    protected override void OnUpgrade()
    {
        _healAmount = 5m;
    }
}
