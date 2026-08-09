using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace FateNightOfTheGalacticRailway.Core.Cards.Projection;

/// <summary>（伪）必胜黄金连斩·三 — projection (weakened) copy of GoldenSlash3.
/// Simplified placeholder: no chain trigger (that logic lives in GoldenSlashBase).</summary>
[Pool(typeof(WeakenedCardPool))]
public class ProjectionGoldenSlash3 : ProjectionCardBase
{
    public ProjectionGoldenSlash3() : base(CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(2m, ValueProp.Move)
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
    }

    protected override void OnUpgrade()
    {
    }
}
