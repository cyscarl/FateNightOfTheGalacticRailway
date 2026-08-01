using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace FateNightOfTheGalacticRailway.Core.Powers;

/// <summary>
/// Heal 1 HP per card played this combat. Applied by Avalon.
/// </summary>
public class AvalonRegen : RinPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner?.Creature != Owner || Amount <= 0) return;
        await CreatureCmd.Heal(Owner, Amount);
    }
}
