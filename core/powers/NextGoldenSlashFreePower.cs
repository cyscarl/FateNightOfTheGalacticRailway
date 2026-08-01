using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using FateNightOfTheGalacticRailway.Core.Cards;

namespace FateNightOfTheGalacticRailway.Core.Powers;

/// <summary>
/// Makes the next N GoldenSlash cards cost 0. N = initial Amount.
/// Removed when depleted.
/// </summary>
public class NextGoldenSlashFreePower : RinPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card.Owner?.Creature != Owner) return false;
        if (card is not GoldenSlashBase) return false;
        if (Amount <= 0) return false;

        modifiedCost = 0m;
        return true;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (Amount <= 0) return;
        if (cardPlay.Card is GoldenSlashBase && cardPlay.Card.Owner?.Creature == Owner)
        {
            await PowerCmd.ModifyAmount(context, this, -1m, Owner, null);
            if (Amount <= 0)
                await PowerCmd.Remove(this);
        }
    }
}
