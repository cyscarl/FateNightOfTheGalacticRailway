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
/// 由神创造 — 0 cost. Heal 3. Ethereal. Exhaust. Derived from WhyAreYouHere (level 1).
/// </summary>
[Pool(typeof(RinCardPool))]
public class DivineCreation : CustomCardModel
{
    public DivineCreation() : base(0, CardType.Skill, CardRarity.Event, TargetType.None)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => new[]
    {
        CardKeyword.Ethereal,
        CardKeyword.Exhaust,
    };

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { };

    private decimal _healAmount = 3m;

    public override string PortraitPath => "DivineCreation.png".CardPortraitPath();
    public override string CustomPortraitPath => "DivineCreation.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "DivineCreation.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Heal(Owner.Creature, _healAmount);
    }

    protected override void OnUpgrade()
    {
        _healAmount = 5m;
    }
}
