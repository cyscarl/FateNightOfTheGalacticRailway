using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace FateNightOfTheGalacticRailway.Core.Powers;

/// <summary>
/// Target takes extra damage per hit this turn. Clears at end of enemy turn.
/// Uses recursion guard to prevent the extra damage from retriggering itself.
/// </summary>
public class RuleBreakerMark : RinPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    private bool _processing;

    public override async Task AfterDamageReceived(PlayerChoiceContext ctx, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (_processing) return;
        if (target != Owner || Amount <= 0 || result.UnblockedDamage <= 0m) return;
        // Skip already-unpowered damage — prevents cross-power loops with WaterMark etc.
        if (props == ValueProp.Unpowered) return;

        _processing = true;
        try
        {
            await CreatureCmd.Damage(ctx, target, Amount, ValueProp.Unpowered, dealer, cardSource);
        }
        finally
        {
            _processing = false;
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext ctx, CombatSide side, System.Collections.Generic.IEnumerable<Creature> participants)
    {
        if (Owner != null && participants.Contains(Owner))
            await PowerCmd.Remove(this);
    }
}
