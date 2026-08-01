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

/// <summary>财宝宝石 — Draw 3 cards.</summary>
[Pool(typeof(RinPotionPool))]
[RegisterPotion(typeof(RinPotionPool))]
public class TreasureGemPotion : GemPotion
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    public override string CustomImagePath =>
        "FateNightOfTheGalacticRailway/images/potions/treasure_gem_potion.png";
    public override string CustomOutlinePath =>
        "FateNightOfTheGalacticRailway/images/potions/treasure_gem_potion_outline.png";

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        await CardPileCmd.Draw(choiceContext, 3m, Owner);
    }
}
