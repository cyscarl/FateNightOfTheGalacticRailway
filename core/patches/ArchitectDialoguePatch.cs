using System;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Models.Events;
using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

namespace FateNightOfTheGalacticRailway.Core.Patches;

/// <summary>
/// The base game only defines TheArchitect dialogue for built-in characters.
/// A mod character (TosakaRin) has no dialogue → Dialogue is null → WinRun()
/// throws NullReferenceException on Dialogue.EndAttackers, freezing the game.
/// This injects an empty dialogue so the victory run completes normally.
/// (RitsuLib does the same, but only when its debug-compat master is on.)
/// </summary>
public class ArchitectDialoguePatch : IPatchMethod
{
    public static string PatchId => "fate_night_architect_dialogue_fallback";
    public static string Description => "Inject empty Architect dialogue for mod characters to prevent WinRun NRE";
    public static bool IsCritical => false;

    public static ModPatchTarget[] GetTargets() =>
        [new(typeof(TheArchitect), "LoadDialogue", Array.Empty<Type>())];

    public static void Postfix(TheArchitect __instance)
    {
        var field = AccessTools.Field(typeof(TheArchitect), "_dialogue");
        if (field == null || field.GetValue(__instance) != null) return;

        // Empty two-line dialogue (like Ironclad's). EndAttackers stays at its
        // default so the WinRun attack animations are skipped — no NRE.
        field.SetValue(__instance, new AncientDialogue("", ""));
    }
}
