using System.Collections.Generic;
using BaseLib.Abstracts;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Keywords;
using FateNightOfTheGalacticRailway.Core;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards.Projection;

/// <summary>
/// Base class for "（伪）" projection cards. All projections cost 0, have Exhaust, are
/// hidden from the card library, and are only ever spawned on demand — never offered
/// as rewards.
///
/// Projection cards live in the hidden <see cref="WeakenedCardPool"/>, which is not part
/// of <c>ModelDb.AllCardPools</c> (character + shared pools only), so they are never a
/// reward source. But card rendering resolves <see cref="CardModel.Pool"/> against
/// <c>AllCardPools</c> and would fall back to <c>MockCardPool</c> (which crashes), so we
/// report the character pool here purely for rendering.
/// </summary>
public abstract class ProjectionCardBase : CustomCardModel
{
    protected ProjectionCardBase(CardType type, CardRarity rarity, TargetType targetType)
        : base(0, type, rarity, targetType)
    {
        // Hide from the card library — ShouldShowInCardLibrary is get-only and
        // non-virtual, so flip its backing field directly.
        typeof(CardModel)
            .GetField("<ShouldShowInCardLibrary>k__BackingField",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            ?.SetValue(this, false);
    }

    // All projection cards Exhaust and carry the 投影 keyword. Declared via
    // CanonicalKeywords (not AddKeyword) because AddKeyword asserts mutability and
    // throws during ModelDb canonical init.
    public override IEnumerable<CardKeyword> CanonicalKeywords => new[]
    {
        CardKeyword.Exhaust,
        ProjectionKeywords.Projection.GetModCardKeyword()
    };

    // Rendering only: resolve a real pool so card visuals don't hit MockCardPool.
    public override CardPoolModel Pool => ModelDb.CardPool<RinCardPool>();

    // Slightly warmer, muted frame so projection cards read as "fake/downgraded"
    // (the character pool frame is golden, hue 0.121).
    public override ShaderMaterial? CreateCustomFrameMaterial => ShaderUtils.GenerateHsv(0.0f, 1.0f, 1.0f);
}
