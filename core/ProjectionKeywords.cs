using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;

namespace FateNightOfTheGalacticRailway.Core;

/// <summary>
/// 自定义关键词「投影」— 投影牌不需要能量但效果下降。
/// Registered via <see cref="RegisterOwnedCardKeywordAttribute"/>; projection cards carry
/// it through <c>GetModCardKeyword()</c>. Localization lives in card_keywords.json
/// under the qualified id (<see cref="ModContentRegistry.GetQualifiedKeywordId"/>).
/// </summary>
[RegisterOwnedCardKeyword(nameof(Projection))]
public sealed class ProjectionKeywords
{
    public static readonly string Projection =
        ModContentRegistry.GetQualifiedKeywordId(FateNightOfTheGalacticRailwayMod.MOD_ID, nameof(Projection));
}
