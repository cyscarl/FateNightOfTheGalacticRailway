using System.Collections.Generic;
using System.Linq;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Unlocks;
using FateNightOfTheGalacticRailway.Core.Potions;

namespace FateNightOfTheGalacticRailway.Core.Characters;

public class RinPotionPool : CustomPotionPoolModel
{
    /// <summary>
    /// The 5 special gem potions only ever come from the TosakaStyle relic and the
    /// KingWine card — they are excluded from combat rewards and shops (both draw
    /// from the character potion pool via GetUnlockedPotions).
    /// </summary>
    public override IEnumerable<PotionModel> GetUnlockedPotions(UnlockState unlockState)
        => base.GetUnlockedPotions(unlockState)
            .Where(p => p is not (EnergyGemPotion or PioneerGemPotion or TreasureGemPotion
                or ProjectionGemPotion or ExcaliburGemPotion));
}
