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
/// 击鼓传花
/// </summary>
[Pool(typeof(RinCardPool))]
public class PassTheParcel : CustomCardModel
{
    public PassTheParcel() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        // No canonical vars
    };

    public override string PortraitPath => "PassTheParcel.png".CardPortraitPath();
    public override string CustomPortraitPath => "PassTheParcel.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "PassTheParcel.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        // One independent 3-hit counter per source. If a debuff-doubling relic (不安之灯)
        // triggers, ApplyMark splits it into two independent counters instead of one bigger one.
        await FateNightOfTheGalacticRailway.Core.Powers.PassTheParcel.ApplyMark(
            choiceContext, cardPlay.Target, Owner.Creature, this, IsUpgraded ? 12m : 9m);
    }

    protected override void OnUpgrade()
    {
    }
}
