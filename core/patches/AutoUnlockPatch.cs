using HarmonyLib;
using MegaCrit.Sts2.Core.Unlocks;

namespace FateNightOfTheGalacticRailway.Core.Patches;

/// <summary>
/// Automatically unlocks all base game content — all characters, encounters, epochs.
/// </summary>
[HarmonyPatch(typeof(UnlockState), MethodType.Constructor,
    new[] { typeof(System.Collections.Generic.IEnumerable<string>),
            typeof(System.Collections.Generic.IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>),
            typeof(int) })]
public static class AutoUnlockPatch
{
    /// <summary>
    /// Replace any new UnlockState with UnlockState.all.
    /// </summary>
    [HarmonyPrefix]
    private static bool Prefix(ref UnlockState __result)
    {
        __result = UnlockState.all;
        return false; // Skip original constructor
    }
}
