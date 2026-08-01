using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Scaffolding.Content;

namespace FateNightOfTheGalacticRailway.Core.Potions;

/// <summary>
/// Base class for FateNight gem potions. Subclasses must override CustomImagePath and CustomOutlinePath.
/// Re-exports commonly needed types for convenience.
/// </summary>
public abstract class GemPotion : ModPotionTemplate
{
}
