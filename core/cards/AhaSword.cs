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
using FateNightOfTheGalacticRailway.Core.Powers;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 阿哈之剑！ — Deal 4 AOE. Every 3 cards this turn, AhaStrike damage +1 this combat.
/// </summary>
[Pool(typeof(RinCardPool))]
public class AhaSword : CustomCardModel
{
    public AhaSword() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
    }

    // 阿哈打击！ referenced in the description — show a card preview on hover.
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromCard<AhaStrike>() };

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(4m, ValueProp.Move)
    };

    public override string PortraitPath => "AhaSword.png".CardPortraitPath();
    public override string CustomPortraitPath => "AhaSword.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "AhaSword.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(base.CombatState!)
            .Execute(choiceContext);

        var tracker = await PowerCmd.Apply<AhaSwordTracker>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
        if (tracker != null)
        {
            // Start the counter at 0 — AhaSword's own play (or its projection) is not
            // counted, only cards played after it. Upgraded AhaSword triggers every 2
            // cards instead of every 3.
            tracker.SetAmount(0);
            tracker.Threshold = IsUpgraded ? 2 : 3;
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
