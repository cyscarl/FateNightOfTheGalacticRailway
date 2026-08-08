using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Keywords;
using FateNightOfTheGalacticRailway.Core;
using FateNightOfTheGalacticRailway.Core.Cards.Projection;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 投影，开始 — Deal 4 damage. Randomly pick a card from your deck and put its
/// "（伪）" projection card into your hand.
/// </summary>
[Pool(typeof(RinCardPool))]
public class ProjectionBegin : CustomCardModel
{
    public ProjectionBegin() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    // 投影 — carries the keyword so the effect text is hoverable.
    public override IEnumerable<CardKeyword> CanonicalKeywords => new[]
    {
        ProjectionKeywords.Projection.GetModCardKeyword()
    };

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(4m, ValueProp.Move)
    };

    public override string PortraitPath => "ProjectionBegin.png".CardPortraitPath();
    public override string CustomPortraitPath => "ProjectionBegin.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "ProjectionBegin.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        if (Owner == null || CombatState == null) return;

        // Randomly pick one card that exists in the player's deck, then add its
        // projection card to hand (generic ProjectionUtil, reusable from any source).
        var candidates = Owner.Deck.Cards.ToList();
        if (candidates.Count == 0) return;

        int idx = Owner.RunState.Rng.CombatCardSelection.NextInt(candidates.Count);
        await ProjectionUtil.AddProjectionToHand(choiceContext, candidates[idx], CombatState, Owner);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
