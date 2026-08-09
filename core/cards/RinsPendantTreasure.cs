using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 凛的吊坠·财宝 — 0 cost. Generate KingTreasure x4 (buff existing if hand has one).
/// </summary>
[Pool(typeof(RinCardPool))]
public class RinsPendantTreasure : CustomCardModel
{
    public RinsPendantTreasure() : base(1, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
    }

    // 王之财宝！ referenced in the description — show a card preview on hover.
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromCard<KingTreasure>() };

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { };

    public override string PortraitPath => "RinsPendantTreasure.png".CardPortraitPath();
    public override string CustomPortraitPath => "RinsPendantTreasure.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "RinsPendantTreasure.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (int i = 0; i < 4; i++)
            await KingTreasure.AddToHand(Owner);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
