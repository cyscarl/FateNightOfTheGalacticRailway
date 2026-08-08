using System;
using System.Reflection;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace FateNightOfTheGalacticRailway.Core.Cards.Projection;

/// <summary>
/// Generic helpers for generating projection cards from any source card
/// (deck / hand / fully random) into a player's hand.
/// </summary>
public static class ProjectionUtil
{
    /// <summary>
    /// Create the projection card for <paramref name="original"/> (its dedicated
    /// "（伪）X" card, or the generic 伪卡牌 when none exists), without placing it in hand.
    /// </summary>
    public static CardModel CreateProjectionCard(CardModel original, ICombatState combatState, Player owner)
    {
        Type projectionType = ProjectionRegistry.GetProjectionType(original.GetType());
        var createMethod = typeof(ICardScope).GetMethod(nameof(ICardScope.CreateCard), new[] { typeof(Player) })!
            .MakeGenericMethod(projectionType);
        return (CardModel)createMethod.Invoke(combatState, new object[] { owner })!;
    }

    /// <summary>
    /// Create the projection card for <paramref name="original"/> and add it to
    /// <paramref name="owner"/>'s hand.
    /// </summary>
    public static async Task AddProjectionToHand(PlayerChoiceContext choiceContext,
        CardModel original, ICombatState combatState, Player owner)
    {
        if (original == null || owner == null || combatState == null) return;
        CardModel projection = CreateProjectionCard(original, combatState, owner);
        await CardPileCmd.AddGeneratedCardToCombat(projection, PileType.Hand, owner);
    }
}
