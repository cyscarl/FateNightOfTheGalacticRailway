using System;
using System.Reflection;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using FateNightOfTheGalacticRailway.Core.Cards;
using FateNightOfTheGalacticRailway.Core.Cards.Projection;
// This power shares its class name with the granting card
// (FateNightOfTheGalacticRailway.Core.Cards.WhyAreYouHere) — alias to refer to
// the card type unambiguously from within this class.
using WhyAreYouHereCard = FateNightOfTheGalacticRailway.Core.Cards.WhyAreYouHere;

namespace FateNightOfTheGalacticRailway.Core.Powers;

/// <summary>
/// Tracks cards played. At turn start, generates escalating reward card:
///   Lv1 (0-9): DivineCreation (heal 3)
///   Lv2 (10-19): HumanWeave (heal 3 + draw 1)
///   Lv3 (20+): ReturnToEarth (heal 3 + draw 1 + energy 1)
/// Counting starts only after the granting card resolves — its own play (or a
/// projected copy of it) does not count.
/// </summary>
public class WhyAreYouHere : RinPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    private int Level => Amount >= 20 ? 3 : Amount >= 10 ? 2 : 1;

    private static Type CardForLevel(int level) => level switch
    {
        3 => typeof(ReturnToEarth),
        2 => typeof(HumanWeave),
        _ => typeof(DivineCreation),
    };

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner?.Creature != Owner) return;
        // Don't count the card that granted this power — its own play (or a
        // projected copy) doesn't advance the reward, only later cards do.
        if (cardPlay.Card is WhyAreYouHereCard or ProjectionWhyAreYouHere) return;
        if (Amount >= 30) return; // Stop counting at level 3
        await PowerCmd.ModifyAmount(context, this, 1m, Owner, null);
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != Owner) return;
        await GenerateRewardCard(choiceContext, player);
    }

    /// <summary>
    /// Add the reward card for the current level to the player's hand. Used at turn
    /// start, and when a duplicate WhyAreYouHere card is played (the power is unique,
    /// so re-applying triggers the reward instead of stacking).
    /// </summary>
    public async Task GenerateRewardCard(PlayerChoiceContext choiceContext, Player player)
    {
        var state = Owner?.CombatState;
        if (state == null) return;

        var cardType = CardForLevel(Level);
        var method = typeof(ICardScope).GetMethod(nameof(ICardScope.CreateCard), new[] { typeof(Player) })!
            .MakeGenericMethod(cardType);
        var card = (CardModel)method.Invoke(state, new object[] { player })!;
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, player);
    }
}
