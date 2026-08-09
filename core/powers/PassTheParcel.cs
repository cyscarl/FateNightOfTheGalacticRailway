using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace FateNightOfTheGalacticRailway.Core.Powers;

/// <summary>
/// Mark an enemy. When hit 3 times OR killed, deal 9 AOE to all enemies.
/// Applied by PassTheParcel card (upgraded: 12 damage; projection: 4 damage).
/// Multiple sources can coexist on the same target with independent counters.
/// </summary>
public class PassTheParcel : RinPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>Explosion damage (9 by default, 12 from an upgraded card, 4 from the projection).</summary>
    public decimal ExplodeDamage { get; set; } = 9m;

    // Multiple sources can coexist on the same target with independent counters.
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    private bool _triggered;

    /// <summary>
    /// Apply the mark to <paramref name="target"/> as ONE independent 3-hit counter per
    /// source. Vanilla debuff-doubling relics (不安之灯) multiply the applied amount; we
    /// reinterpret a doubled amount (e.g. 6) as TWO independent 3-hit counters (like
    /// casting the card twice) instead of one bigger counter. Extra instances are applied
    /// with a null cardSource so giver-multipliers don't fire on them again.
    /// </summary>
    public static async Task ApplyMark(PlayerChoiceContext ctx, Creature target, Creature applier, CardModel card, decimal explodeDamage)
    {
        var power = await PowerCmd.Apply<PassTheParcel>(ctx, target, 3m, applier, card);
        if (power == null) return;
        power.ExplodeDamage = explodeDamage;

        int extra = (int)(power.Amount / 3m) - 1;
        if (extra > 0)
        {
            power.SetAmount(3);
            for (int i = 0; i < extra; i++)
            {
                var second = await PowerCmd.Apply<PassTheParcel>(ctx, target, 3m, applier, null);
                if (second != null)
                {
                    second.SetAmount(3);
                    second.ExplodeDamage = explodeDamage;
                }
            }
        }
    }

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        await base.AfterApplied(applier, cardSource);
        // "被击杀" trigger: AfterDamageReceived is NOT called on the killing blow,
        // so we subscribe to the marked enemy's death to explode early too.
        if (Owner != null)
            Owner.Died += OnOwnerDied;
    }

    public override async Task AfterRemoved(Creature oldOwner)
    {
        await base.AfterRemoved(oldOwner);
        oldOwner.Died -= OnOwnerDied;
    }

    private void OnOwnerDied(Creature creature)
    {
        if (_triggered) return;
        TaskHelper.RunSafely(Explode(new ThrowingPlayerChoiceContext()));
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext ctx, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (_triggered || target != Owner || Amount <= 0) return;
        // Decrement counter (1 hit = 1 count). SetAmount avoids ShouldRemoveDueToAmount
        // removing the power before we explode.
        SetAmount(Amount - 1);
        if (Amount <= 0 || !Owner.IsAlive)
            await Explode(ctx);
    }

    private async Task Explode(PlayerChoiceContext ctx)
    {
        _triggered = true;
        if (Owner == null) return;
        var enemies = Owner.CombatState?.HittableEnemies;
        if (enemies == null) return;
        foreach (var enemy in enemies)
            await CreatureCmd.Damage(ctx, enemy, ExplodeDamage, ValueProp.Unpowered, Owner, null);
        await PowerCmd.Remove(this);
    }
}
