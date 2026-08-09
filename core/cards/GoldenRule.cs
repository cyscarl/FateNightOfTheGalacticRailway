using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 这就是黄金律！
/// </summary>
[Pool(typeof(RinCardPool))]
public class GoldenRule : CustomCardModel
{
    public GoldenRule() : base(1, CardType.Skill, CardRarity.Common, TargetType.None)
    {
    }

    // 王之财宝！ referenced in the description — show a card preview on hover.
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromCard<KingTreasure>() };

    public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { CardKeyword.Retain };
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        // No canonical vars
    };

    public override string PortraitPath => "GoldenRule.png".CardPortraitPath();
    public override string CustomPortraitPath => "GoldenRule.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "GoldenRule.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // GoldenRule is a Buff — vanilla debuff-doubling relics (不安之灯) don't affect it,
        // so no amount override is needed here.
        await PowerCmd.Apply<FateNightOfTheGalacticRailway.Core.Powers.GoldenRule>(choiceContext, Owner.Creature, IsUpgraded ? 2m : 1m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}
