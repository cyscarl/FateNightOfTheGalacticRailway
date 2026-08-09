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
/// 凛的吊坠・圣剑 — 0 cost. Next 3 GoldenSlash cards cost 0.
/// </summary>
[Pool(typeof(RinCardPool))]
public class RinsPendantSword : CustomCardModel
{
    public RinsPendantSword() : base(1, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { };

    public override string PortraitPath => "RinsPendantSword.png".CardPortraitPath();
    public override string CustomPortraitPath => "RinsPendantSword.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "RinsPendantSword.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<NextGoldenSlashFreePower>(
            choiceContext, Owner.Creature, 3m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
