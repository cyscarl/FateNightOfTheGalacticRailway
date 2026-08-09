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
/// 完全简朴的试练
/// </summary>
[Pool(typeof(RinCardPool))]
public class SimpleTrial : CustomCardModel
{
    public SimpleTrial() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        // No canonical vars
    };

    public override string PortraitPath => "SimpleTrial.png".CardPortraitPath();
    public override string CustomPortraitPath => "SimpleTrial.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "SimpleTrial.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(IsUpgraded ? 2m : 1m, Owner);
    }

    protected override void OnUpgrade()
    {
    }
}
