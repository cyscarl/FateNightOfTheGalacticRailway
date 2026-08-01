namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// Per-combat counter for GoldenSlash chain mechanic.
/// Tracks how many GoldenSlash cards have been played this turn,
/// so all copies (hand + draw + discard) share the same cost increment.
/// Reset at combat start and at player turn end.
/// </summary>
public static class GoldenSlashTracker
{
    /// <summary>Extra cost applied to all GoldenSlash cards this turn.</summary>
    public static int ExtraCost { get; private set; }

    /// <summary>Whether a GoldenSlash was played this turn (for chain logic).</summary>
    public static bool AnyPlayedThisTurn { get; private set; }

    public static void Increment()
    {
        ExtraCost++;
        AnyPlayedThisTurn = true;
    }

    public static void ResetForTurn()
    {
        ExtraCost = 0;
        AnyPlayedThisTurn = false;
    }

    public static void ResetForCombat()
    {
        ExtraCost = 0;
        AnyPlayedThisTurn = false;
    }
}
