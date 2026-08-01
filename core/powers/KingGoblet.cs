using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace FateNightOfTheGalacticRailway.Core.Powers;

/// <summary>
/// Next turn: +2 energy, draw 2, then remove self. Applied by KingGoblet.
/// </summary>
public class KingGoblet : RinPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.None;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != Owner) return;
        await PlayerCmd.GainEnergy(2m, player);
        await CardPileCmd.Draw(choiceContext, 2m, player);
        await PowerCmd.Remove(this);
    }
}
