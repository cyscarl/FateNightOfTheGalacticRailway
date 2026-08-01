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
/// 必胜黄金连斩·三 — 0 cost. Deal 3 AOE. Chain.
/// </summary>
[Pool(typeof(RinCardPool))]
public class GoldenSlash3 : GoldenSlashBase
{
    public GoldenSlash3() : base(0, TargetType.AllEnemies) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(3m, ValueProp.Move)
    };

    public override string PortraitPath => "GoldenSlash3.png".CardPortraitPath();
    public override string CustomPortraitPath => "GoldenSlash3.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "GoldenSlash3.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(base.CombatState!)
            .Execute(choiceContext);

        await TriggerChain(choiceContext);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
