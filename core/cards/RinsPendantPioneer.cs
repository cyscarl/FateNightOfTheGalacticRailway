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
using FateNightOfTheGalacticRailway.Core.Powers;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 凛的吊坠・开拓 — 0 cost. This turn, every 2 AhaStrike → generate 1 Exhausting AhaStrike.
/// </summary>
[Pool(typeof(RinCardPool))]
public class RinsPendantPioneer : CustomCardModel
{
    public RinsPendantPioneer() : base(1, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
    }

    // 阿哈打击！ referenced in the description — show a card preview on hover.
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromCard<AhaStrike>() };

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { };

    public override string PortraitPath => "RinsPendantPioneer.png".CardPortraitPath();
    public override string CustomPortraitPath => "RinsPendantPioneer.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "RinsPendantPioneer.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<RinsPendantPioneerPower>(
            choiceContext, Owner.Creature, 1m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
