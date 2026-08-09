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
using FateNightOfTheGalacticRailway.Core.Cards.Projection;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 心眼（真） — Randomly trigger the effects of 2 cards in your deck by auto-playing
/// their corresponding "（伪）" projection cards (no cost, ignores play conditions).
/// Originals stay in place.
/// </summary>
[Pool(typeof(RinCardPool))]
public class TrueEye : CustomCardModel
{
    public TrueEye() : base(1, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { };

    public override string PortraitPath => "TrueEye.png".CardPortraitPath();
    public override string CustomPortraitPath => "TrueEye.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "TrueEye.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner == null || CombatState == null) return;

        // Cards in the deck (draw + discard + hand, excluding this card).
        var deck = CardPile.GetCards(Owner, PileType.Draw, PileType.Discard, PileType.Hand)
            .Where(c => c != this)
            .ToList();
        if (deck.Count == 0) return;

        var rng = Owner.RunState.Rng.CombatCardSelection;
        int count = Math.Min(IsUpgraded ? 3 : 2, deck.Count);
        for (int i = 0; i < count; i++)
        {
            int idx = rng.NextInt(deck.Count);
            var original = deck[idx];
            deck.RemoveAt(idx);

            // Auto-play the projection card — triggers its effect at no cost,
            // ignoring conditions. Playing a projection (e.g. 伪幻想崩坏) still fires
            // AfterCardPlayed, so 幻想崩坏's cost-reduction triggers normally.
            var projection = ProjectionUtil.CreateProjectionCard(original, CombatState, Owner);
            await CardCmd.AutoPlay(choiceContext, projection, null);
        }
    }

    protected override void OnUpgrade() { }
}
