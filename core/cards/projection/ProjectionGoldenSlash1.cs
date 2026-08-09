using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace FateNightOfTheGalacticRailway.Core.Cards.Projection;

/// <summary>（伪）必胜黄金连斩·一 — projection (weakened) copy of GoldenSlash1.
/// Simplified placeholder: no chain trigger (that logic lives in GoldenSlashBase).</summary>
[Pool(typeof(WeakenedCardPool))]
public class ProjectionGoldenSlash1 : ProjectionCardBase
{
    public ProjectionGoldenSlash1() : base(CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(4m, ValueProp.Move)
    };

    public override string PortraitPath => "GoldenSlash1.png".CardPortraitPath();
    public override string CustomPortraitPath => "GoldenSlash1.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "GoldenSlash1.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
    }
}
