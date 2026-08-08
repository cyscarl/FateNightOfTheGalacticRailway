using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;
using FateNightOfTheGalacticRailway.Core.Cards.Projection;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Relics;

/// <summary>
/// 无限剑制 — Project the first 2 original cards played each turn (generates their
/// "（伪）" projection cards instead of copies). Projection cards don't count toward the
/// two and don't trigger the relic. Counter shows how many were triggered this turn
/// (like Velvet Choker); resets at the end of the player's turn.
/// </summary>
[Pool(typeof(RinRelicPool))]
public sealed class UnlimitedBladeWorks : CustomRelicModel
{
    private const int ProjectionsPerTurn = 2;
    private int _projectedThisTurn;

    public override RelicRarity Rarity => RelicRarity.Rare;
    public override bool IsAllowed(IRunState runState) => true;
    public override bool ShouldReceiveCombatHooks => true;

    // Counter display like Velvet Choker: how many projections triggered this turn.
    public override bool ShowCounter => CombatManager.Instance.IsInProgress;
    public override int DisplayAmount => IsCanonical ? 0 : _projectedThisTurn;

    public override string PackedIconPath => "FateNightOfTheGalacticRailway/images/relics/UnlimitedBladeWorks.png";
    protected override string PackedIconOutlinePath => "FateNightOfTheGalacticRailway/images/relics/UnlimitedBladeWorks_outline.png";
    protected override string BigIconPath => "FateNightOfTheGalacticRailway/images/relics/big/UnlimitedBladeWorks.png";

    public override Task BeforeCombatStartLate()
    {
        _projectedThisTurn = 0;
        return base.BeforeCombatStartLate();
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner == null) return;
        if (cardPlay.Card.Owner != Owner) return;

        // Projection cards don't count toward the two played cards and don't trigger.
        if (cardPlay.Card is ProjectionCardBase) return;

        if (_projectedThisTurn >= ProjectionsPerTurn) return;

        var state = Owner.Creature.CombatState;
        if (state == null) return;

        _projectedThisTurn++;
        InvokeDisplayAmountChanged();

        var projection = ProjectionUtil.CreateProjectionCard(cardPlay.Card, state, Owner);
        await CardPileCmd.AddGeneratedCardToCombat(projection, PileType.Hand, Owner);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (Owner != null && participants.Contains(Owner.Creature))
        {
            _projectedThisTurn = 0;
            InvokeDisplayAmountChanged();
        }
    }
}
