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
/// 投影，开始 — Deal 4 dmg. Copy 1 random card from draw/discard with -1 cost, Ethereal, Exhaust.
/// </summary>
[Pool(typeof(RinCardPool))]
public class ProjectionBegin : CustomCardModel
{
    public ProjectionBegin() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

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

        // Pick random card from draw + discard
        var candidates = CardPile.GetCards(Owner, PileType.Draw, PileType.Discard).ToList();
        if (candidates.Count == 0) return;

        var idx = Owner.RunState.Rng.CombatCardSelection.NextInt(candidates.Count);
        var original = candidates[idx];

        // Create a dupe and modify it
        var dupe = original.CreateDupe();
        dupe.EnergyCost.AddThisTurn(-1);  // -1 cost this turn
        dupe.AddKeyword(CardKeyword.Ethereal);
        dupe.AddKeyword(CardKeyword.Exhaust);
        await CardPileCmd.AddGeneratedCardToCombat(dupe, PileType.Hand, Owner);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
