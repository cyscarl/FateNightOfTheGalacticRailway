using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using FateNightOfTheGalacticRailway.Core.Cards;
using FateNightOfTheGalacticRailway.Core.Characters;
using FateNightOfTheGalacticRailway.Core.Potions;

namespace FateNightOfTheGalacticRailway.Core.Relics;

/// <summary>
/// 远坂流 — +3 potion slots (applied once on obtain). Each combat, generate 2 random gem potions.
/// Also manages GoldenSlash counter resets.
/// </summary>
[Pool(typeof(RinRelicPool))]
public sealed class TosakaStyle : CustomRelicModel
{
    private bool _potionsGeneratedThisCombat;

    private static readonly Type[] GemPotionTypes =
    {
        typeof(EnergyGemPotion),
        typeof(PioneerGemPotion),
        typeof(TreasureGemPotion),
        typeof(ProjectionGemPotion),
        typeof(ExcaliburGemPotion),
    };

    /// <summary>The 4 special Boss relics, offered as a 3-choice after each act boss.</summary>
    private static readonly Type[] BossRelicTypes =
    {
        typeof(FlySafely),
        typeof(Excalibur),
        typeof(EnumaElish),
        typeof(UnlimitedBladeWorks),
    };

    public override RelicRarity Rarity => RelicRarity.Starter;
    public override bool IsAllowed(IRunState runState) => true;
    public override bool ShouldReceiveCombatHooks => true;

    public override string PackedIconPath =>
        "FateNightOfTheGalacticRailway/images/relics/TosakaStyle.png";
    protected override string PackedIconOutlinePath =>
        "FateNightOfTheGalacticRailway/images/relics/TosakaStyle_outline.png";
    protected override string BigIconPath =>
        "FateNightOfTheGalacticRailway/images/relics/big/TosakaStyle.png";

    public override Task AfterObtained()
    {
        // Only add slots if not already increased (safety against save/load re-fire)
        if (Owner != null && Owner.MaxPotionCount <= 3)
            PlayerCmd.GainMaxPotionCount(3, Owner);
        return base.AfterObtained();
    }

    public override Task BeforeCombatStartLate()
    {
        GoldenSlashTracker.ResetForCombat();
        _potionsGeneratedThisCombat = false;
        return base.BeforeCombatStartLate();
    }

    /// <summary>
    /// Reset the GoldenSlash cost counter at the end of each turn so that
    /// next turn's draws start from the initial cost (fixes 2-turn bleed).
    /// </summary>
    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        GoldenSlashTracker.ResetForTurn();
        return base.AfterSideTurnEnd(choiceContext, side, participants);
    }

    /// <summary>
    /// After each act boss, award 1 random special Boss relic not yet obtained
    /// (FlySafely / Excalibur / EnumaElish / UnlimitedBladeWorks). Once all are
    /// owned, no further relic reward appears.
    /// (LinkedRewardSet pick-one was unreliable — the reward screen let the
    /// player keep taking every option, so this uses a single RelicReward.)
    /// </summary>
    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (player != Owner) return false;
        if (room?.RoomType != RoomType.Boss) return false;

        var ownedTypes = player.Relics.Select(r => r.GetType()).ToHashSet();
        var candidates = BossRelicTypes.Where(t => !ownedTypes.Contains(t)).ToList();
        if (candidates.Count == 0) return false;

        var rng = player.PlayerRng.Rewards;
        var type = candidates[rng.NextInt(candidates.Count)];
        rewards.Add(new RelicReward(CreateBossRelic(type), player));
        return true;
    }

    private static RelicModel CreateBossRelic(Type type)
    {
        var method = typeof(ModelDb).GetMethod(nameof(ModelDb.Relic))!
            .MakeGenericMethod(type);
        // RelicReward requires a mutable instance (canonical throws), so clone it.
        return ((RelicModel)method.Invoke(null, null)!).ToMutable();
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        await base.AfterPlayerTurnStart(choiceContext, player);

        if (_potionsGeneratedThisCombat || Owner == null || player != Owner) return;
        _potionsGeneratedThisCombat = true;

        var rng = Owner.RunState.Rng.CombatPotionGeneration;
        for (int i = 0; i < 2; i++)
        {
            var typeIdx = rng.NextInt(GemPotionTypes.Length);
            var type = GemPotionTypes[typeIdx];
            PotionModel potion;
            if (type == typeof(EnergyGemPotion)) potion = ModelDb.Potion<EnergyGemPotion>().ToMutable();
            else if (type == typeof(PioneerGemPotion)) potion = ModelDb.Potion<PioneerGemPotion>().ToMutable();
            else if (type == typeof(TreasureGemPotion)) potion = ModelDb.Potion<TreasureGemPotion>().ToMutable();
            else if (type == typeof(ProjectionGemPotion)) potion = ModelDb.Potion<ProjectionGemPotion>().ToMutable();
            else potion = ModelDb.Potion<ExcaliburGemPotion>().ToMutable();

            await PotionCmd.TryToProcure(potion, Owner);
        }
    }
}
