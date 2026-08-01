using Godot;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace FateNightOfTheGalacticRailway.Core.Characters;

public class RinCardPool : TypeListCardPoolModel
{
    public override string Title => "Rin";
    public override bool IsColorless => false;
    public override Color DeckEntryCardColor => TosakaRin.Color;

    public override string EnergyColorName => "tosaka_rin";
    public override string BigEnergyIconPath =>
        "FateNightOfTheGalacticRailway/images/charui/tosaka_rin/big_energy_TosakaRin.png";
    public override string TextEnergyIconPath =>
        "FateNightOfTheGalacticRailway/images/charui/tosaka_rin/text_energy_TosakaRin.png";

    // Card frame color: same golden yellow
    public override Material PoolFrameMaterial =>
        MaterialUtils.CreateHsvShaderMaterial(0.121f, 1.0f, 0.9725f);
}
