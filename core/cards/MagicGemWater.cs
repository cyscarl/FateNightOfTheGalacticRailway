using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using FateNightOfTheGalacticRailway.Core.Characters;
using FateNightOfTheGalacticRailway.Core.Powers;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 魔术宝石·水 — Target takes +2 extra damage per hit this turn. Upgrades to +3.
/// Uses shared RuleBreakerMark power with variable amount.
/// </summary>
[Pool(typeof(RinCardPool))]
public class MagicGemWater : CustomCardModel
{
    private decimal _extraDamage = 2m;

    public MagicGemWater() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { };

    public override string PortraitPath => "MagicGemWater.png".CardPortraitPath();
    public override string CustomPortraitPath => "MagicGemWater.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "MagicGemWater.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await PowerCmd.Apply<RuleBreakerMark>(choiceContext, cardPlay.Target, _extraDamage, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        _extraDamage = 3m;
    }
}
