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
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 心眼（真） — Create copies of 2 random cards from deck and trigger their effects.
/// Originals stay where they are.
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
        if (Owner == null) return;

        var allCards = CardPile.GetCards(Owner, PileType.Draw, PileType.Hand, PileType.Discard)
            .Where(c => c != this)
            .ToList();

        if (allCards.Count == 0) return;

        var rng = Owner.RunState.Rng.CombatCardSelection;
        int count = Math.Min(2, allCards.Count);
        for (int i = 0; i < count; i++)
        {
            var idx = rng.NextInt(allCards.Count);
            var card = allCards[idx];
            allCards.RemoveAt(idx);
            // Create a dupe and auto-play it — original stays in place
            var dupe = card.CreateDupe();
            await CardCmd.AutoPlay(choiceContext, dupe, null);
        }
    }

    protected override void OnUpgrade() { }
}
