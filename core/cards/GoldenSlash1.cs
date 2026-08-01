using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// Shared base for all GoldenSlash cards. Handles chain generation and
/// per-turn cost scaling via GoldenSlashTracker.
/// </summary>
public abstract class GoldenSlashBase : CustomCardModel
{
    private static readonly Type[] SlashVariants = { typeof(GoldenSlash1), typeof(GoldenSlash2), typeof(GoldenSlash3) };

    protected GoldenSlashBase(int cost, TargetType target) : base(cost, CardType.Attack, CardRarity.Uncommon, target) { }

    /// <summary>Apply current tracker cost when a GoldenSlash enters hand.</summary>
    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card == this && GoldenSlashTracker.ExtraCost > 0)
        {
            EnergyCost.AddThisTurn(GoldenSlashTracker.ExtraCost);
        }
        await base.AfterCardDrawn(choiceContext, card, fromHandDraw);
    }

    /// <summary>Apply current tracker cost when a generated GoldenSlash enters hand.</summary>
    public override async Task AfterCardEnteredCombat(CardModel card)
    {
        if (card == this && IsClone && GoldenSlashTracker.ExtraCost > 0)
        {
            EnergyCost.AddThisTurn(GoldenSlashTracker.ExtraCost);
        }
        await base.AfterCardEnteredCombat(card);
    }

    /// <summary>Generate a random variant, increment the global counter.</summary>
    protected async Task TriggerChain(PlayerChoiceContext choiceContext)
    {
        if (Owner == null || CombatState == null) return;

        // Increment the global tracker — this makes all future GoldenSlash costs +1
        GoldenSlashTracker.Increment();

        // Generate a random variant with Ethereal + Exhaust using ICardScope.CreateCard<T>
        var rng = Owner.RunState.Rng.CombatCardGeneration;
        var variantType = SlashVariants[rng.NextInt(SlashVariants.Length)];
        var method = typeof(ICardScope).GetMethod(nameof(ICardScope.CreateCard), new[] { typeof(Player) })!
            .MakeGenericMethod(variantType);
        var newCard = (CardModel)method.Invoke(CombatState, new object[] { Owner })!;
        newCard.AddKeyword(CardKeyword.Ethereal);
        newCard.AddKeyword(CardKeyword.Exhaust);
        newCard.EnergyCost.AddThisTurn(GoldenSlashTracker.ExtraCost);

        await CardPileCmd.AddGeneratedCardToCombat(newCard, PileType.Hand, Owner);

        // Also update existing GoldenSlash cards in hand
        var handPile = PileType.Hand.GetPile(Owner);
        if (handPile != null)
        {
            foreach (var card in handPile.Cards)
            {
                if (card is GoldenSlashBase && card != this && card != newCard)
                    card.EnergyCost.AddThisTurn(1);
            }
        }
    }
}

// ── Variant 1: single target 6 damage ────────────────────────────────

[Pool(typeof(RinCardPool))]
public class GoldenSlash1 : GoldenSlashBase
{
    public GoldenSlash1() : base(0, TargetType.AnyEnemy) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(6m, ValueProp.Move)
    };

    public override string PortraitPath => "GoldenSlash1.png".CardPortraitPath();
    public override string CustomPortraitPath => "GoldenSlash1.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "GoldenSlash1.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        await TriggerChain(choiceContext);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
