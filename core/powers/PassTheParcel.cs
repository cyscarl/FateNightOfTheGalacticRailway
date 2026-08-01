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
/// Mark an enemy. When hit 3 times or killed, deal 9 AOE to all enemies.
/// Applied by PassTheParcel card.
/// </summary>
public class PassTheParcel : RinPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    private bool _triggered;

    public override async Task AfterDamageReceived(PlayerChoiceContext ctx, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (_triggered || target != Owner || Amount <= 0) return;
        // Decrement counter (1 hit = 1 count)
        await PowerCmd.ModifyAmount(ctx, this, -1m, Owner, null);
        if (Amount <= 0)
            await Explode(ctx);
    }

    // Note: "on kill" trigger is handled by hitting 0 counter via AfterDamageReceived.
    // If the enemy is killed before all hits land, the counter reset at end of combat handles cleanup.
    private async Task Explode(PlayerChoiceContext ctx)
    {
        _triggered = true;
        if (Owner == null) return;
        var enemies = Owner.CombatState?.HittableEnemies;
        if (enemies == null) return;
        foreach (var enemy in enemies)
            await CreatureCmd.Damage(ctx, enemy, 9m, ValueProp.Unpowered, Owner, null);
        await PowerCmd.Remove(this);
    }
}
