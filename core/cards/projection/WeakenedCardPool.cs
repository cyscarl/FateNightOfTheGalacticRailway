using Godot;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards.Projection;

/// <summary>
/// Hidden pool that holds every "（伪）" projection card.
/// This pool is never assigned to a character, never shared/colorless, and is never a
/// reward source — so projection cards only ever appear when explicitly created.
/// The frame hue is slightly shifted from the character pool to mark them as fake/downgraded.
/// </summary>
public class WeakenedCardPool : TypeListCardPoolModel
{
    public override string Title => "Projection (hidden)";
    public override bool IsColorless => false;
    public override Color DeckEntryCardColor => TosakaRin.Color;

    public override string EnergyColorName => "tosaka_rin";
    public override string BigEnergyIconPath =>
        "FateNightOfTheGalacticRailway/images/charui/tosaka_rin/big_energy_TosakaRin.png";
    public override string TextEnergyIconPath =>
        "FateNightOfTheGalacticRailway/images/charui/tosaka_rin/text_energy_TosakaRin.png";

    // Golden character hue is 0.121; projections get a slightly warmer, muted frame.
    public override Material PoolFrameMaterial =>
        MaterialUtils.CreateHsvShaderMaterial(0.15f, 0.8f, 0.92f);
}
