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
/// 阿哈来支持了！
/// </summary>
[Pool(typeof(RinCardPool))]
public class AhaSupport : CustomCardModel
{
    public AhaSupport() : base(1, CardType.Skill, CardRarity.Common, TargetType.None)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        // No canonical vars
    };

    public override string PortraitPath => "AhaSupport.png".CardPortraitPath();
    public override string CustomPortraitPath => "AhaSupport.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "AhaSupport.png".CardPortraitPath();

    private decimal _amount = 1m;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<FateNightOfTheGalacticRailway.Core.Powers.PowerAhaStrikeDamageUp>(choiceContext, Owner.Creature, _amount, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        _amount = 2m;
    }
}
