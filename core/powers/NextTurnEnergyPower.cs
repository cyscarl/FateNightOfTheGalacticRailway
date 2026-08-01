using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace FateNightOfTheGalacticRailway.Core.Powers;

/// <summary>
/// At start of player's next turn, grants energy equal to Amount, then removes itself.
/// Applied by Cooperation.
/// </summary>
public class NextTurnEnergyPower : RinPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.None;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != Owner || Amount <= 0) return;
        await PlayerCmd.GainEnergy(Amount, player);
        await PowerCmd.Remove(this);
    }
}
