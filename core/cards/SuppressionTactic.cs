using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 压制战术 — Deal 3 AOE. Apply 1 Weak to all enemies.
/// </summary>
[Pool(typeof(RinCardPool))]
public class SuppressionTactic : CustomCardModel
{
    public SuppressionTactic() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(3m, ValueProp.Move)
    };

    public override string PortraitPath => "SuppressionTactic.png".CardPortraitPath();
    public override string CustomPortraitPath => "SuppressionTactic.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "SuppressionTactic.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var state = base.CombatState!;
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(state)
            .Execute(choiceContext);

        foreach (var enemy in state.HittableEnemies)
            await PowerCmd.Apply<WeakPower>(choiceContext, enemy, 1m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
