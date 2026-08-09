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
/// 阿哈来支持了！
/// </summary>
[Pool(typeof(RinCardPool))]
public class AhaSupport : CustomCardModel
{
    public AhaSupport() : base(0, CardType.Skill, CardRarity.Common, TargetType.None)
    {
    }

    // 阿哈打击！ referenced in the description — show a card preview on hover.
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromCard<AhaStrike>() };

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        // No canonical vars
    };

    public override string PortraitPath => "AhaSupport.png".CardPortraitPath();
    public override string CustomPortraitPath => "AhaSupport.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "AhaSupport.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // Base grants +2 AhaStrike damage this combat; upgraded grants +3.
        await PowerCmd.Apply<FateNightOfTheGalacticRailway.Core.Powers.PowerAhaStrikeDamageUp>(choiceContext, Owner.Creature, IsUpgraded ? 3m : 2m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}
