using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 阿哈横扫！ — Deal 3 AOE. Create 2 Exhausting AhaStrike.
/// </summary>
[Pool(typeof(RinCardPool))]
public class AhaSweep : CustomCardModel
{
    public AhaSweep() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(3m, ValueProp.Move)
    };

    public override string PortraitPath => "AhaSweep.png".CardPortraitPath();
    public override string CustomPortraitPath => "AhaSweep.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "AhaSweep.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(base.CombatState!)
            .Execute(choiceContext);

        // Create 2 Exhausting AhaStrike copies
        for (int i = 0; i < 2; i++)
        {
            var strike = base.CombatState!.CreateCard<AhaStrike>(Owner);
            strike.AddKeyword(CardKeyword.Exhaust);
            await CardPileCmd.AddGeneratedCardToCombat(strike, PileType.Hand, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
