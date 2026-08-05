using Godot;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace FateNightOfTheGalacticRailway.Nodes.Combat;

/// <summary>
/// Attached to the character_visuals.tscn root so the scene can use
/// Node2D as its root type while still inheriting NCreatureVisuals
/// behavior. This avoids type-resolution issues when loading from .pck.
/// </summary>
public partial class NRinCharacterVisuals : NCreatureVisuals
{
    public override void _Ready()
    {
        base._Ready();
    }
}
