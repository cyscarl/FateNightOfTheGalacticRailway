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

namespace FateNightOfTheGalacticRailway.Core.Powers;

/// <summary>
/// Tracks cards played. At turn start, generates escalating reward card:
///   Lv1 (0-9): DivineCreation (heal 3)
///   Lv2 (10-19): HumanWeave (heal 3 + draw 1)
///   Lv3 (20+): ReturnToEarth (heal 3 + draw 1 + energy 1)
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
        if (Amount >= 30) return; // Stop counting at level 3
        await PowerCmd.ModifyAmount(context, this, 1m, Owner, null);
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != Owner) return;

        var state = Owner?.CombatState;
        if (state == null) return;

        var cardType = CardForLevel(Level);
        var method = typeof(ICardScope).GetMethod(nameof(ICardScope.CreateCard), new[] { typeof(Player) })!
            .MakeGenericMethod(cardType);
        var card = (CardModel)method.Invoke(state, new object[] { player })!;
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, player);
    }
}
