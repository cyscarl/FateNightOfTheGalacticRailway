using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 王之财宝！ — 1 cost. Retain. Exhaust. Deal 3 AOE.
/// Hand limit: only 1 copy. Duplicates upgrade existing instead (infinite scaling, +3 per dupe).
/// Derived card — not in starter pool, not in rewards.
/// </summary>
[Pool(typeof(RinCardPool))]
public class KingTreasure : CustomCardModel
{
    public KingTreasure() : base(1, CardType.Attack, CardRarity.Event, TargetType.AllEnemies)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => new[]
    {
        CardKeyword.Retain,
        CardKeyword.Exhaust,
    };

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(3m, ValueProp.Move)
    };

    public override string PortraitPath => "KingTreasure.png".CardPortraitPath();
    public override string CustomPortraitPath => "KingTreasure.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "KingTreasure.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(base.CombatState!)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
    }

    /// <summary>
    /// Add a KingTreasure to hand. If one already exists, upgrade it (+3 dmg)
    /// instead of adding a duplicate. Uses CardCmd.Upgrade for proper UI refresh.
    /// </summary>
    public static async Task AddToHand(Player owner)
    {
        var state = owner.Creature.CombatState;
        if (state == null) return;

        // Re-query hand each call — essential for sequential batch generation
        var existing = PileType.Hand.GetPile(owner)?.Cards.OfType<KingTreasure>().FirstOrDefault();
        if (existing != null)
        {
            existing.DynamicVars.Damage.UpgradeValueBy(3m);
        }
        else
        {
            var card = state.CreateCard<KingTreasure>(owner);
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, owner);
        }
    }
}
