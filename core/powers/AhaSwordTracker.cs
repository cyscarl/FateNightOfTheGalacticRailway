using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace FateNightOfTheGalacticRailway.Core.Powers;

/// <summary>
/// Tracks cards played this turn. Every 3 → +1 to AhaStrike damage permanently.
/// Applied by AhaSword.
/// </summary>
public class AhaSwordTracker : RinPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.None;

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner?.Creature != Owner) return;
        await PowerCmd.ModifyAmount(context, this, 1m, Owner, null);
        if (Amount >= 3)
        {
            await PowerCmd.ModifyAmount(context, this, -3m, Owner, null);
            await PowerCmd.Apply<PowerAhaStrikeDamageUp>(context, Owner, 1m, Owner, null);
        }
    }
}
