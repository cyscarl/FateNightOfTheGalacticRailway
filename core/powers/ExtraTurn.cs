using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace FateNightOfTheGalacticRailway.Core.Powers;

/// <summary>
/// Grants an extra turn, then removes itself. Applied by RejuvenationSpecial.
/// </summary>
public class ExtraTurn : RinPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.None;

    // Multiple extra turns can coexist; each granted turn consumes one instance.
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override bool ShouldTakeExtraTurn(Player player)
    {
        return player?.Creature == Owner && Amount > 0;
    }

    public override async Task AfterTakingExtraTurn(Player player)
    {
        if (player?.Creature != Owner) return;
        await PowerCmd.Remove(this);
    }
}
