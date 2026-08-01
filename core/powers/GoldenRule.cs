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

namespace FateNightOfTheGalacticRailway.Core.Powers;

/// <summary>
/// Each time a card is played this turn, generate a KingTreasure.
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
        // Don't trigger on the GoldenRule card itself — only on subsequent cards
        if (cardPlay.Card is GoldenRule) return;
        await KingTreasure.AddToHand(player);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (Owner != null && participants.Contains(Owner))
            await PowerCmd.Remove(this);
    }
}
