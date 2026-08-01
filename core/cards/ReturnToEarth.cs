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
/// 回归泥土 — 0 cost. Heal 3, Draw 1, Gain 1 Energy. Ethereal. Exhaust.
/// Derived from WhyAreYouHere (level 3).
/// </summary>
[Pool(typeof(RinCardPool))]
public class ReturnToEarth : CustomCardModel
{
    public ReturnToEarth() : base(0, CardType.Skill, CardRarity.Event, TargetType.None)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => new[]
    {
        CardKeyword.Ethereal,
        CardKeyword.Exhaust,
    };

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { };

    public override string PortraitPath => "ReturnToEarth.png".CardPortraitPath();
    public override string CustomPortraitPath => "ReturnToEarth.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "ReturnToEarth.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Heal(Owner.Creature, 3m);
        await CardPileCmd.Draw(choiceContext, 1m, Owner);
        await PlayerCmd.GainEnergy(1m, Owner);
    }

    protected override void OnUpgrade() { }
}
