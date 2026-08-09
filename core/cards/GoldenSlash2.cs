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
/// 必胜黄金连斩·二 — 0 cost. Deal 4 damage x2 random hits. Chain.
/// </summary>
[Pool(typeof(RinCardPool))]
public class GoldenSlash2 : GoldenSlashBase
{
    public GoldenSlash2() : base(0, TargetType.RandomEnemy) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(4m, ValueProp.Move)
    };

    private int _hitCount = 2;

    public override string PortraitPath => "GoldenSlash2.png".CardPortraitPath();
    public override string CustomPortraitPath => "GoldenSlash2.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "GoldenSlash2.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (int i = 0; i < _hitCount; i++)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .TargetingRandomOpponents(base.CombatState!, allowDuplicates: true)
                .Execute(choiceContext);
        }

        await TriggerChain(choiceContext);
    }

    protected override void OnUpgrade()
    {
        _hitCount = 3;
    }
}
