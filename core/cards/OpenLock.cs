using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 打开门锁！ — Generate 1 KingTreasure + 1 random Rin Skill with cost 0 and Exhaust.
/// </summary>
[Pool(typeof(RinCardPool))]
public class OpenLock : CustomCardModel
{
    private static readonly Type[] RinSkillTypes =
    {
        typeof(RinsPendantPioneer), typeof(RinsPendantTreasure),
        typeof(RinsPendantProjection), typeof(RinsPendantSword),
        typeof(MagicGemWind), typeof(MagicGemEarth), typeof(MagicGemWater),
        typeof(EpicClayTablet), typeof(SimpleTrial), typeof(PerfectProjector),
        typeof(LawAka), typeof(WantedPoster), typeof(PassTheParcel),
        typeof(KingWine), typeof(FakeBook), typeof(MoralApproval),
        typeof(KingGoblet), typeof(FriendshipProof), typeof(AhaMirror),
        typeof(DeathTitanCloak), typeof(AhaSupport), typeof(GoldenRule),
        typeof(TrueEye),
    };

    public OpenLock() : base(1, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { };

    public override string PortraitPath => "OpenLock.png".CardPortraitPath();
    public override string CustomPortraitPath => "OpenLock.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "OpenLock.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner == null) return;
        var state = base.CombatState!;

        // 1. Generate KingTreasure
        await KingTreasure.AddToHand(Owner);

        // 2. Generate random Rin Skill card with cost 0 + Exhaust
        var rng = Owner.RunState.Rng.CombatCardGeneration;
        var skillType = RinSkillTypes[rng.NextInt(RinSkillTypes.Length)];
        var createCardMethod = typeof(ICardScope).GetMethod(nameof(ICardScope.CreateCard), new[] { typeof(Player) })!
            .MakeGenericMethod(skillType);
        var card = (CardModel)createCardMethod.Invoke(state, new object[] { Owner })!;
        card.EnergyCost.SetUntilPlayed(0);
        card.AddKeyword(CardKeyword.Exhaust);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
    }

    protected override void OnUpgrade() { }
}
