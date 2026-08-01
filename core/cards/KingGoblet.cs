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
/// 王之大杯
/// </summary>
[Pool(typeof(RinCardPool))]
public class KingGoblet : CustomCardModel
{
    public KingGoblet() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        // No canonical vars
    };

    public override string PortraitPath => "KingGoblet.png".CardPortraitPath();
    public override string CustomPortraitPath => "KingGoblet.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "KingGoblet.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<FateNightOfTheGalacticRailway.Core.Powers.KingGoblet>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}
