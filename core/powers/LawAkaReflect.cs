using System;
using System.Collections.Generic;
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
/// Reflect 50% of unblocked damage back to attacker, cleared at end of turn.
/// Damage capped so enemy HP doesn't go below 1.
/// </summary>
public class LawAkaReflect : RinPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.None;

    public override async Task AfterDamageReceived(PlayerChoiceContext ctx, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner || dealer == null || result.UnblockedDamage <= 0m) return;
        decimal reflect = Math.Floor(result.UnblockedDamage * 0.5m);
        if (reflect <= 0) return;
        reflect = Math.Min(reflect, dealer.CurrentHp - 1m);
        if (reflect <= 0) return;
        await CreatureCmd.Damage(ctx, dealer, reflect, ValueProp.Unpowered, Owner, null);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext ctx, CombatSide side, IEnumerable<Creature> participants)
    {
        if (Owner != null && participants.Contains(Owner))
            await PowerCmd.Remove(this);
    }
}
