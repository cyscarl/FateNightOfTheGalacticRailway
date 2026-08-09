using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace FateNightOfTheGalacticRailway.Core.Powers;

/// <summary>
/// Reflect 50% of unblocked damage back to attacker, cleared at the START of the
/// owner's NEXT turn — so it stays active through the enemy turn and reflects the
/// damage the owner takes there.
/// Damage capped so enemy HP doesn't go below 1.
/// </summary>
public class LawAkaReflect : RinPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.None;

    // Multiple sources can coexist and each reflects separately.
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override async Task AfterDamageReceived(PlayerChoiceContext ctx, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner || dealer == null) return;

        // 反伤按「受击前（格挡减少前）」的格挡值计算：
        // 当前剩余格挡 + 本次被格挡掉的伤害 = 本次受击前的格挡值。
        decimal blockBeforeHit = target.Block + result.BlockedDamage;
        if (blockBeforeHit <= 0m) return;

        decimal reflect = Math.Floor(blockBeforeHit * 0.5m);
        if (reflect <= 0) return;
        // 该伤害不致命：不让敌人生命低于 1。
        reflect = Math.Min(reflect, dealer.CurrentHp - 1m);
        if (reflect <= 0) return;
        await CreatureCmd.Damage(ctx, dealer, reflect, ValueProp.Unpowered, Owner, null);
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext ctx, Player player)
    {
        if (player.Creature != Owner) return;
        await PowerCmd.Remove(this);
    }
}
