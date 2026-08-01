using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.PotionPools;
using STS2RitsuLib.Interop.AutoRegistration;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Potions;

/// <summary>能量宝石 — Gain 2 energy.</summary>
[Pool(typeof(RinPotionPool))]
[RegisterPotion(typeof(RinPotionPool))]
public class EnergyGemPotion : GemPotion
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    public override string CustomImagePath =>
        "FateNightOfTheGalacticRailway/images/potions/energy_gem_potion.png";
    public override string CustomOutlinePath =>
        "FateNightOfTheGalacticRailway/images/potions/energy_gem_potion_outline.png";

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        await PlayerCmd.GainEnergy(2m, Owner);
    }
}
