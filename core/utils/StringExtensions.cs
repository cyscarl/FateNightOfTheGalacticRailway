using MegaCrit.Sts2.Core.Assets;

namespace FateNightOfTheGalacticRailway.Core.Utils;

public static class StringExtensions
{
    private const string ModId = "FateNightOfTheGalacticRailway";

    public static string CardPortraitPath(this string filename) =>
        Path.Join(ModId, "images", "card_portraits", filename);

    public static string BigCardPortraitPath(this string filename) =>
        Path.Join(ModId, "images", "card_portraits", "big", filename);
}
