using MegaCrit.Sts2.Core.Entities.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace FateNightOfTheGalacticRailway.Core.Powers;

/// <summary>
/// Base class for all FateNight mod powers. Auto-registers subclasses and
/// resolves icons to FateNightOfTheGalacticRailway/images/powers/tosaka_rin/.
/// Class name → filename: PascalToSnakeCase, then strip "_power" suffix.
/// </summary>
[RegisterPower(Inherit = true)]
public abstract class RinPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.None;
    public override PowerStackType StackType => PowerStackType.None;

    private static string PascalToSnakeCase(string name)
    {
        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < name.Length; i++)
        {
            if (i > 0 && char.IsUpper(name[i]))
            {
                bool prevLower = !char.IsUpper(name[i - 1]);
                bool nextLower = i + 1 < name.Length && char.IsLower(name[i + 1]);
                if (prevLower || nextLower)
                    sb.Append('_');
            }
            sb.Append(char.ToLower(name[i]));
        }
        return sb.ToString();
    }

    private string SnakedName => PascalToSnakeCase(GetType().Name);

    public override string CustomIconPath =>
        $"FateNightOfTheGalacticRailway/images/powers/tosaka_rin/{SnakedName}.png";

    public override string CustomBigIconPath =>
        $"FateNightOfTheGalacticRailway/images/powers/tosaka_rin/{SnakedName}.png";
}
