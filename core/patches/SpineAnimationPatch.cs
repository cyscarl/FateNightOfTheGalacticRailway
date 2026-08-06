using System.Collections.Generic;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

namespace FateNightOfTheGalacticRailway.Core.Patches;

/// <summary>
/// Registers the Spine animation patches that remap vanilla animation names
/// (merchant / rest-site scenes) to TosakaRin's model animations, and speed
/// up the attack/cast/hurt animations 3x.
/// </summary>
public class SpineAnimationPatches : IModPatches
{
    public static void AddTo(ModPatcher patcher)
    {
        patcher.RegisterPatch<SpineAnimStateCachePatch>();
        patcher.RegisterPatch<SpineAnimationPatch>();
        patcher.RegisterPatch<ArchitectDialoguePatch>();
    }
}

/// <summary>
/// Cache the MegaAnimationState instance ids that belong to TosakaRin's
/// Spine skeletons (detected by the presence of the "Relax" animation).
/// Waits until the skeleton data is ready before caching.
/// </summary>
public class SpineAnimStateCachePatch : IPatchMethod
{
    public static string PatchId => "fate_night_spine_anim_state_cache";
    public static string Description => "Cache MegaAnimationState IDs that belong to TosakaRin spines";
    public static bool IsCritical => false;

    private const string DetectAnim = "Relax";
    internal static readonly HashSet<ulong> RinStateIds = new();

    public static ModPatchTarget[] GetTargets() =>
        [new(typeof(MegaSprite), nameof(MegaSprite.GetAnimationState))];

    public static void Postfix(MegaSprite __instance, MegaAnimationState __result)
    {
        if (__result?.BoundObject == null) return;
        if (__instance.HasAnimation(DetectAnim))
            RinStateIds.Add(__result.BoundObject.GetInstanceId());
    }
}

/// <summary>
/// Remap vanilla animation names to the animations actually present in the
/// TosakaRin Spine model. Covers merchant (relaxed_loop) and rest-site
/// (overgrowth_loop / hive_loop / glory_loop) scenes. Also plays the
/// attack/cast/hurt animations (Interact / Move) at 3x speed.
/// </summary>
public class SpineAnimationPatch : IPatchMethod
{
    public static string PatchId => "fate_night_spine_animation_remap";
    public static string Description => "Remap vanilla animation names to TosakaRin Spine animations";
    public static bool IsCritical => false;

    private static readonly Dictionary<string, string> Remap = new()
    {
        ["idle_loop"] = "Relax",
        ["relaxed_loop"] = "Relax",
        ["attack"] = "Interact",
        ["cast"] = "Interact",
        ["hurt"] = "Move",
        ["die"] = "Sleep",
        ["overgrowth_loop"] = "Sit",
        ["hive_loop"] = "Sit",
        ["glory_loop"] = "Sit",
    };

    /// <summary>Animations to play at 3x speed (attack/cast/hurt).</summary>
    private static readonly HashSet<string> FastAnims = new() { "Interact", "Move" };
    private const float FastSpeed = 3f;

    public static ModPatchTarget[] GetTargets() =>
        [new(typeof(MegaAnimationState), nameof(MegaAnimationState.SetAnimation))];

    public static void Prefix(MegaAnimationState __instance, ref string animationName)
    {
        if (__instance?.BoundObject == null) return;
        if (!SpineAnimStateCachePatch.RinStateIds.Contains(__instance.BoundObject.GetInstanceId())) return;
        if (Remap.TryGetValue(animationName, out string mapped))
            animationName = mapped;
    }

    public static void Postfix(MegaAnimationState __instance, ref string animationName, MegaTrackEntry __result)
    {
        if (__result == null || __instance?.BoundObject == null) return;
        if (!SpineAnimStateCachePatch.RinStateIds.Contains(__instance.BoundObject.GetInstanceId())) return;
        __result.SetTimeScale(FastAnims.Contains(animationName) ? FastSpeed : 1f);
    }
}
