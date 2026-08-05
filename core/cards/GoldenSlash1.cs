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
using MegaCrit.Sts2.Core.Nodes.CommonUi;
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

    /// <summary>
    /// Chain trigger: if there is no other GoldenSlash in hand, gain 1 random
    /// GoldenSlash variant (Ethereal + Exhaust). If this card is upgraded, the
    /// generated variant is also upgraded. All GoldenSlash cards cost +1 this turn.
    /// </summary>
    protected async Task TriggerChain(PlayerChoiceContext choiceContext)
    {
        if (Owner == null || CombatState == null) return;

        // Only trigger if no other GoldenSlash is already in hand.
        var handPile = PileType.Hand.GetPile(Owner);
        bool hasOtherInHand = handPile?.Cards.Any(c => c is GoldenSlashBase && c != this) == true;
        if (hasOtherInHand) return;

        // Increment the global tracker — all GoldenSlash costs +1 this turn.
        GoldenSlashTracker.Increment();

        // Generate a random variant using ICardScope.CreateCard<T>.
        var rng = Owner.RunState.Rng.CombatCardGeneration;
        var variantType = SlashVariants[rng.NextInt(SlashVariants.Length)];
        var method = typeof(ICardScope).GetMethod(nameof(ICardScope.CreateCard), new[] { typeof(Player) })!
            .MakeGenericMethod(variantType);
        var newCard = (CardModel)method.Invoke(CombatState, new object[] { Owner })!;

        // Upgraded card chains into upgraded variants.
        if (IsUpgraded)
            CardCmd.Upgrade(newCard, CardPreviewStyle.None);

        // Generated card gains Ethereal + Exhaust and the current chain cost.
        newCard.AddKeyword(CardKeyword.Ethereal);
        newCard.AddKeyword(CardKeyword.Exhaust);
        newCard.EnergyCost.AddThisTurn(GoldenSlashTracker.ExtraCost);

        await CardPileCmd.AddGeneratedCardToCombat(newCard, PileType.Hand, Owner);

        // Existing GoldenSlash cards in hand also cost +1 this turn.
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
