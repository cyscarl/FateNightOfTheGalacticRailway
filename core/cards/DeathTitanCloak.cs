using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 「死亡」泰坦的隐身衣 — Innate. Exhaust 1 card from draw pile, then draw 1. Exhaust.
/// </summary>
[Pool(typeof(RinCardPool))]
public class DeathTitanCloak : CustomCardModel
{
    public DeathTitanCloak() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => new[]
    {
        CardKeyword.Innate,
        CardKeyword.Exhaust,
    };

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { };

    public override string PortraitPath => "DeathTitanCloak.png".CardPortraitPath();
    public override string CustomPortraitPath => "DeathTitanCloak.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "DeathTitanCloak.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner == null) return;

        var drawPile = PileType.Draw.GetPile(Owner);
        if (drawPile == null || drawPile.Cards.Count == 0) return;

        var candidates = await CardSelectCmd.FromCombatPile(
            choiceContext, drawPile, Owner,
            new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 1));

        foreach (var card in candidates)
            await CardCmd.Exhaust(choiceContext, card);

        await CardPileCmd.Draw(choiceContext, 1m, Owner);
    }

    protected override void OnUpgrade() { }
}
