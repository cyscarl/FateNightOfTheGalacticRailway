using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using FateNightOfTheGalacticRailway.Core.Cards;
using FateNightOfTheGalacticRailway.Core.Cards.Projection;
// This power shares its class name with the granting card
// (FateNightOfTheGalacticRailway.Core.Cards.GoldenRule) — alias to refer to
// the card type unambiguously from within this class.
using GoldenRuleCard = FateNightOfTheGalacticRailway.Core.Cards.GoldenRule;

namespace FateNightOfTheGalacticRailway.Core.Powers;

/// <summary>
/// While active, each card the player plays generates a KingTreasure.
/// Counting only starts for cards played AFTER this power is granted:
/// cards played before the granting card, and the granting card's own play
/// (or a projected copy of it), do not count.
/// Hand limit: only 1 KingTreasure. Duplicates buff existing instead.
/// Removed at end of turn.
/// </summary>
public class GoldenRule : RinPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.None;

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (Owner == null) return;
        var player = Owner.Player;
        if (player == null || cardPlay.Card.Owner != player) return;
        // Don't count the card that granted this power — its own play (or a
        // projected copy of it) doesn't generate a treasure, only later cards do.
        if (cardPlay.Card is GoldenRuleCard or ProjectionGoldenRule) return;
        await KingTreasure.AddToHand(player);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (Owner != null && participants.Contains(Owner))
            await PowerCmd.Remove(this);
    }
}
