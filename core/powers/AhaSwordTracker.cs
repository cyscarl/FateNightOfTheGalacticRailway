using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using FateNightOfTheGalacticRailway.Core.Cards;
using FateNightOfTheGalacticRailway.Core.Cards.Projection;

namespace FateNightOfTheGalacticRailway.Core.Powers;

/// <summary>
/// This-turn counter: every 3 cards played this turn → +1 to AhaStrike damage
/// permanently. The counter itself is removed at end of turn (this-turn only);
/// the PowerAhaStrikeDamageUp it grants persists for the whole combat.
/// Applied by AhaSword.
/// Counting starts only after the granting card resolves — its own play (or a
/// projected copy of it) does not count.
/// </summary>
public class AhaSwordTracker : RinPower
{
    public override PowerType Type => PowerType.Buff;
    // Counter stack so the running count (0 → Threshold) is visible on the icon.
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>Number of cards played per trigger (3 by default, 2 for an upgraded AhaSword).</summary>
    public int Threshold { get; set; } = 3;

    // Multiple sources can coexist with independent counters.
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner?.Creature != Owner) return;
        // Don't count the card that granted this power — its own play (or a
        // projected copy) doesn't advance the counter, only later cards do.
        if (cardPlay.Card is AhaSword or ProjectionAhaSword) return;
        await PowerCmd.ModifyAmount(context, this, 1m, Owner, null);
        if (Amount >= Threshold)
        {
            // Reset the counter in-place. Using SetAmount (not ModifyAmount) avoids
            // ShouldRemoveDueToAmount removing the tracker at 0, so it can keep
            // triggering repeatedly within the same turn.
            SetAmount(0);
            await PowerCmd.Apply<PowerAhaStrikeDamageUp>(context, Owner, 1m, Owner, null);
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        // The 3-card counter is this-turn only — remove it at end of the player's
        // turn so it starts fresh next turn.
        if (Owner != null && participants.Contains(Owner))
            await PowerCmd.Remove(this);
    }
}
