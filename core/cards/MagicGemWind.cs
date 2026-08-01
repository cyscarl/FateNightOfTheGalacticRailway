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
/// 魔术宝石·风
/// </summary>
[Pool(typeof(RinCardPool))]
public class MagicGemWind : CustomCardModel
{
    private int _healAmount = 8;

    public MagicGemWind() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { };

    public override string PortraitPath => "MagicGemWind.png".CardPortraitPath();
    public override string CustomPortraitPath => "MagicGemWind.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "MagicGemWind.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Heal(Owner.Creature, _healAmount);
    }

    protected override void OnUpgrade()
    {
        _healAmount = 11;
    }
}
