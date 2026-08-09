using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using FateNightOfTheGalacticRailway.Core.Cards;
using FateNightOfTheGalacticRailway.Core.Cards.Projection;

namespace FateNightOfTheGalacticRailway.Core.Powers;

/// <summary>
/// Heal 1 HP per card played this combat. Applied by Avalon.
/// Healing starts only after the granting card resolves — its own play (or a
/// projected copy of it) does not heal.
/// </summary>
public class AvalonRegen : RinPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner?.Creature != Owner || Amount <= 0) return;
        // Don't heal for the card that granted this power — its own play (or a
        // projected copy) doesn't trigger the heal, only later cards do.
        if (cardPlay.Card is Avalon or ProjectionAvalon) return;
        await CreatureCmd.Heal(Owner, Amount);
    }
}
