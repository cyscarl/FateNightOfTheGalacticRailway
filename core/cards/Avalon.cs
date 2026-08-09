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
/// 遗世独立的理想乡
/// </summary>
[Pool(typeof(RinCardPool))]
public class Avalon : CustomCardModel
{
    public Avalon() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        // No canonical vars
    };

    public override string PortraitPath => "Avalon.png".CardPortraitPath();
    public override string CustomPortraitPath => "Avalon.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "Avalon.png".CardPortraitPath();

    private decimal _initialHeal = 15m;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Heal(Owner.Creature, _initialHeal);
        await PowerCmd.Apply<FateNightOfTheGalacticRailway.Core.Powers.AvalonRegen>(choiceContext, Owner.Creature, IsUpgraded ? 2m : 1m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        _initialHeal = 20m;
    }
}
