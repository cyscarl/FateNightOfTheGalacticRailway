using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace FateNightOfTheGalacticRailway.Core.Cards.Projection;

/// <summary>（伪）必胜黄金连斩·二 — projection (weakened) copy of GoldenSlash2.
/// Simplified placeholder: no chain trigger (that logic lives in GoldenSlashBase).</summary>
[Pool(typeof(WeakenedCardPool))]
public class ProjectionGoldenSlash2 : ProjectionCardBase
{
    public ProjectionGoldenSlash2() : base(CardType.Attack, CardRarity.Uncommon, TargetType.RandomEnemy) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(2m, ValueProp.Move)
    };

    public override string PortraitPath => "GoldenSlash2.png".CardPortraitPath();
    public override string CustomPortraitPath => "GoldenSlash2.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "GoldenSlash2.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (int i = 0; i < 2; i++)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .TargetingRandomOpponents(base.CombatState!, allowDuplicates: true)
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
    }
}
