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

    private decimal _hitCount = 3m;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await PowerCmd.Apply<FateNightOfTheGalacticRailway.Core.Powers.PassTheParcel>(choiceContext, cardPlay.Target, _hitCount, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        _hitCount = 2m;
    }
}
