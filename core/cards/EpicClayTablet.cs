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
/// 史诗泥板
/// </summary>
[Pool(typeof(RinCardPool))]
public class EpicClayTablet : CustomCardModel
{
    public EpicClayTablet() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        // No canonical vars
    };

    private decimal _drawCount = 2m;

    public override string PortraitPath => "EpicClayTablet.png".CardPortraitPath();
    public override string CustomPortraitPath => "EpicClayTablet.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "EpicClayTablet.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, _drawCount, Owner, false);
    }

    protected override void OnUpgrade()
    {
        _drawCount = 3m;
    }
}
