using System;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;
using FateNightOfTheGalacticRailway.Core.Cards;
using FateNightOfTheGalacticRailway.Core.Potions;

namespace FateNightOfTheGalacticRailway.Core.Relics;

/// <summary>
/// 远坂流 — +3 potion slots (applied once on obtain). Each combat, generate 2 random gem potions.
/// Also manages GoldenSlash counter resets.
/// </summary>
[Pool(typeof(SharedRelicPool))]
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

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        await base.AfterPlayerTurnStart(choiceContext, player);
        GoldenSlashTracker.ResetForTurn();

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
