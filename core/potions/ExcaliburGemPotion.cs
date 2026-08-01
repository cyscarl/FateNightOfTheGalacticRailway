using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Potions;

/// <summary>圣剑宝石 — Deal 15 damage to all enemies.</summary>
[Pool(typeof(RinPotionPool))]
[RegisterPotion(typeof(RinPotionPool))]
public class ExcaliburGemPotion : GemPotion
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.None;

    public override string CustomImagePath =>
        "FateNightOfTheGalacticRailway/images/potions/excalibur_gem_potion.png";
    public override string CustomOutlinePath =>
        "FateNightOfTheGalacticRailway/images/potions/excalibur_gem_potion_outline.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(15m, ValueProp.Unpowered)
    };

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        var cs = Owner.Creature.CombatState;
        if (cs == null) return;
        foreach (var enemy in cs.HittableEnemies)
            await CreatureCmd.Damage(choiceContext, enemy, base.DynamicVars.Damage.BaseValue, ValueProp.Unpowered, Owner.Creature, null);
    }
}
