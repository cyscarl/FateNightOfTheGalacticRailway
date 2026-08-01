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
/// 伪臣之书
/// </summary>
[Pool(typeof(RinCardPool))]
public class FakeBook : CustomCardModel
{
    public FakeBook() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new BlockVar(6m, ValueProp.Move)
    };

    public override string PortraitPath => "FakeBook.png".CardPortraitPath();
    public override string CustomPortraitPath => "FakeBook.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "FakeBook.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, base.DynamicVars.Block, null);
        // Gain extra block equal to 50% of current block
        var extra = (decimal)System.Math.Floor(Owner.Creature.Block * 0.5);
        if (extra > 0)
            await CreatureCmd.GainBlock(Owner.Creature, extra, ValueProp.Unpowered, null);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Block.UpgradeValueBy(2m);
    }
}
