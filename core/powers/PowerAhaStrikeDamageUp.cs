using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using FateNightOfTheGalacticRailway.Core.Cards;

namespace FateNightOfTheGalacticRailway.Core.Powers;

/// <summary>
/// Adds Amount to AhaStrike card damage. Applied by AhaSupport and AhaSword.
/// </summary>
public class PowerAhaStrikeDamageUp : RinPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        // Return the ADDITIVE increment, not the modified total — the game
        // does `num += returned` itself. Returning 0 for non-AhaStrike keeps
        // all other attacks unaffected.
        if (cardSource is AhaStrike)
            return Amount;
        return 0m;
    }
}
