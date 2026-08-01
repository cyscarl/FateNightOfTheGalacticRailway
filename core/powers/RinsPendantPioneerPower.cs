using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using FateNightOfTheGalacticRailway.Core.Cards;

namespace FateNightOfTheGalacticRailway.Core.Powers;

/// <summary>
/// Every 2 AhaStrike plays → generate 1 Exhausting AhaStrike.
/// Cleared at end of player's turn.
/// </summary>
public class RinsPendantPioneerPower : RinPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.None;

    private int _ahaCount;

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner?.Creature != Owner) return;
        if (cardPlay.Card is not AhaStrike) return;

        _ahaCount++;
        if (_ahaCount % 2 == 0)
        {
            var state = Owner?.CombatState;
            if (state == null || Owner == null) return;
            var card = state.CreateCard<AhaStrike>(Owner.Player!);
            card.AddKeyword(CardKeyword.Exhaust);
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner.Player);
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext ctx, CombatSide side, IEnumerable<Creature> participants)
    {
        if (Owner != null && participants.Contains(Owner))
            await PowerCmd.Remove(this);
    }
}
