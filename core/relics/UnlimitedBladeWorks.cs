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
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Relics;

/// <summary>
/// 无限剑制 — Copy the first 2 cards played each turn. Copies cost -1 and
/// gain Ethereal + Exhaust. Counter resets at the end of the player's turn.
/// </summary>
[Pool(typeof(RinRelicPool))]
public sealed class UnlimitedBladeWorks : CustomRelicModel
{
    private const int CopiesPerTurn = 2;
    private int _copiedThisTurn;

    public override RelicRarity Rarity => RelicRarity.Rare;
    public override bool IsAllowed(IRunState runState) => true;
    public override bool ShouldReceiveCombatHooks => true;

    public override string PackedIconPath => "FateNightOfTheGalacticRailway/images/relics/UnlimitedBladeWorks.png";
    protected override string PackedIconOutlinePath => "FateNightOfTheGalacticRailway/images/relics/UnlimitedBladeWorks_outline.png";
    protected override string BigIconPath => "FateNightOfTheGalacticRailway/images/relics/big/UnlimitedBladeWorks.png";

    public override Task BeforeCombatStartLate()
    {
        _copiedThisTurn = 0;
        return base.BeforeCombatStartLate();
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner == null || _copiedThisTurn >= CopiesPerTurn) return;
        if (cardPlay.Card.Owner != Owner) return;

        var state = Owner.Creature.CombatState;
        if (state == null) return;

        _copiedThisTurn++;
        var copy = state.CloneCard(cardPlay.Card);
        copy.EnergyCost.AddThisCombat(-1);
        copy.AddKeyword(CardKeyword.Ethereal);
        copy.AddKeyword(CardKeyword.Exhaust);

        await CardPileCmd.AddGeneratedCardToCombat(copy, PileType.Hand, Owner);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (Owner != null && participants.Contains(Owner.Creature))
            _copiedThisTurn = 0;
    }
}
