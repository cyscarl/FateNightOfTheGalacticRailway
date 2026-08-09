using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 魔力放出 — Innate. Retain. Deal 50 AOE. Cost -1 per card played this combat.
/// </summary>
[Pool(typeof(RinCardPool))]
public class ManaBurst : CustomCardModel
{
    public ManaBurst() : base(40, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { CardKeyword.Innate, CardKeyword.Retain };

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(50m, ValueProp.Move)
    };

    public override string PortraitPath => "ManaBurst.png".CardPortraitPath();
    public override string CustomPortraitPath => "ManaBurst.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "ManaBurst.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(base.CombatState!)
            .Execute(choiceContext);
    }

    // ── Cost -1 per card played this combat ─────────────────────────────────

    public override async Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this || IsClone) return;
        // Catch up on cards already played
        int played = CombatManager.Instance.History.CardPlaysFinished
            .Count(e => e.CardPlay.Card.Owner == Owner);
        for (int i = 0; i < played; i++)
            EnergyCost.AddThisCombat(-1);
        await Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner) return;
        EnergyCost.AddThisCombat(-1);
        await Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(15m);
    }
}
