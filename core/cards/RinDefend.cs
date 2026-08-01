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
/// 防御
/// </summary>
[Pool(typeof(RinCardPool))]
public class RinDefend : CustomCardModel
{
    public RinDefend() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new BlockVar(5m, ValueProp.Move)
    };

    public override string PortraitPath => "RinDefend.png".CardPortraitPath();
    public override string CustomPortraitPath => "RinDefend.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "RinDefend.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, base.DynamicVars.Block, null);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Block.UpgradeValueBy(3m);
    }
}
