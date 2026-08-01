using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.PotionPools;
using STS2RitsuLib.Interop.AutoRegistration;
using FateNightOfTheGalacticRailway.Core.Cards;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Potions;

/// <summary>开拓宝石 — Generate 3 Exhausting AhaStrike.</summary>
[Pool(typeof(RinPotionPool))]
[RegisterPotion(typeof(RinPotionPool))]
public class PioneerGemPotion : GemPotion
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    public override string CustomImagePath =>
        "FateNightOfTheGalacticRailway/images/potions/pioneer_gem_potion.png";
    public override string CustomOutlinePath =>
        "FateNightOfTheGalacticRailway/images/potions/pioneer_gem_potion_outline.png";

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        var state = Owner.Creature.CombatState;
        if (state == null) return;
        for (int i = 0; i < 3; i++)
        {
            var card = state.CreateCard<AhaStrike>(Owner);
            card.AddKeyword(CardKeyword.Exhaust);
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
        }
    }
}
