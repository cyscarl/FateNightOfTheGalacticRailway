using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using FateNightOfTheGalacticRailway.Core.Characters;
using FateNightOfTheGalacticRailway.Core.Potions;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 王之陈酿 — Obtain 1 random special potion (from the 5 gem types).
/// </summary>
[Pool(typeof(RinCardPool))]
public class KingWine : CustomCardModel
{
    private static readonly Type[] GemPotions =
    {
        typeof(EnergyGemPotion),
        typeof(PioneerGemPotion),
        typeof(TreasureGemPotion),
        typeof(ProjectionGemPotion),
        typeof(ExcaliburGemPotion),
    };

    public KingWine() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { };

    public override string PortraitPath => "KingWine.png".CardPortraitPath();
    public override string CustomPortraitPath => "KingWine.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "KingWine.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner == null) return;
        var rng = Owner.RunState.Rng.CombatPotionGeneration;
        var type = GemPotions[rng.NextInt(GemPotions.Length)];
        PotionModel potion;
        if (type == typeof(EnergyGemPotion)) potion = ModelDb.Potion<EnergyGemPotion>().ToMutable();
        else if (type == typeof(PioneerGemPotion)) potion = ModelDb.Potion<PioneerGemPotion>().ToMutable();
        else if (type == typeof(TreasureGemPotion)) potion = ModelDb.Potion<TreasureGemPotion>().ToMutable();
        else if (type == typeof(ProjectionGemPotion)) potion = ModelDb.Potion<ProjectionGemPotion>().ToMutable();
        else potion = ModelDb.Potion<ExcaliburGemPotion>().ToMutable();

        await PotionCmd.TryToProcure(potion, Owner);
    }

    protected override void OnUpgrade() { }
}
