using System;
using System.Reflection;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Runs;
using STS2RitsuLib.Interop.AutoRegistration;
using FateNightOfTheGalacticRailway.Core.Cards;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Potions;

/// <summary>投影宝石 — Get 2 random Rin cards with cost 0, Ethereal, Exhaust.</summary>
[Pool(typeof(RinPotionPool))]
[RegisterPotion(typeof(RinPotionPool))]
public class ProjectionGemPotion : GemPotion
{
    public override string CustomImagePath =>
        "FateNightOfTheGalacticRailway/images/potions/projection_gem_potion.png";
    public override string CustomOutlinePath =>
        "FateNightOfTheGalacticRailway/images/potions/projection_gem_potion_outline.png";

    private static readonly Type[] RinCards =
    {
        typeof(Cooperation), typeof(FullAttack), typeof(SuppressionTactic),
        typeof(MagicGemFire), typeof(MagicGemVoid), typeof(GaeBolg),
        typeof(RuleBreaker), typeof(AhaStrike), typeof(AhaSweep), typeof(AhaSword),
        typeof(Boring), typeof(CraneWingThree), typeof(ProjectionBegin),
        typeof(FantasyCollapse), typeof(GoldenSlash1), typeof(GoldenSlash2),
        typeof(GoldenSlash3), typeof(ManaBurst),
        typeof(RinsPendantPioneer), typeof(RinsPendantTreasure),
        typeof(RinsPendantProjection), typeof(RinsPendantSword),
        typeof(MagicGemWind), typeof(MagicGemEarth), typeof(MagicGemWater),
        typeof(EpicClayTablet), typeof(SimpleTrial), typeof(PerfectProjector),
        typeof(LawAka), typeof(WantedPoster), typeof(PassTheParcel),
        typeof(KingWine), typeof(FakeBook), typeof(MoralApproval),
        typeof(KingGoblet), typeof(FriendshipProof),
        typeof(AhaMirror), typeof(DeathTitanCloak), typeof(AhaSupport),
        typeof(GoldenRule), typeof(OpenLock), typeof(TrueEye),
        typeof(Avalon), typeof(WhyAreYouHere), typeof(RejuvenationSpecial),
    };

    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        var state = Owner.Creature.CombatState;
        if (state == null) return;
        var rng = Owner.RunState.Rng.CombatCardSelection;
        for (int i = 0; i < 2; i++)
        {
            var type = RinCards[rng.NextInt(RinCards.Length)];
            var method = typeof(ICardScope).GetMethod("CreateCard", new[] { typeof(Player) })!
                .MakeGenericMethod(type);
            var card = (CardModel)method.Invoke(state, new object[] { Owner })!;
            card.EnergyCost.SetUntilPlayed(0);
            card.AddKeyword(CardKeyword.Ethereal);
            card.AddKeyword(CardKeyword.Exhaust);
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
        }
    }
}
