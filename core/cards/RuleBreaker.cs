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
/// 万符必应破戒
/// </summary>
[Pool(typeof(RinCardPool))]
public class RuleBreaker : CustomCardModel
{
    public RuleBreaker() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(6m, ValueProp.Move)
    };

    public override string PortraitPath => "RuleBreaker.png".CardPortraitPath();
    public override string CustomPortraitPath => "RuleBreaker.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "RuleBreaker.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        await PowerCmd.Apply<Powers.RuleBreakerMark>(choiceContext, cardPlay.Target, 3m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
