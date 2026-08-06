using System.Collections.Generic;
using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

namespace FateNightOfTheGalacticRailway.Core.Patches;

/// <summary>
/// Registers the two Spine animation patches that remap vanilla animation
/// names (used by the merchant / rest-site scenes) to TosakaRin's model
/// animations.
/// </summary>
public class SpineAnimationPatches : IModPatches
{
    public static void AddTo(ModPatcher patcher)
    {
        patcher.RegisterPatch<SpineAnimStateCachePatch>();
        patcher.RegisterPatch<SpineAnimationPatch>();
    }
}

/// <summary>
/// Cache the MegaAnimationState instance ids that belong to TosakaRin's
/// Spine skeletons (detected by the presence of the "Relax" animation).
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
        bool hasAnim = __instance.HasAnimation(DetectAnim);
        GD.Print($"[RinAnim] GetAnimationState cached={hasAnim} id={__result?.BoundObject?.GetInstanceId()}");
        if (__result?.BoundObject != null && hasAnim)
            RinStateIds.Add(__result.BoundObject.GetInstanceId());
    }
}

/// <summary>
/// Remap vanilla animation names to the animations actually present in the
/// TosakaRin Spine model. Covers merchant (relaxed_loop) and rest-site
/// (overgrowth_loop / hive_loop / glory_loop) scenes.
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

    public static ModPatchTarget[] GetTargets() =>
        [new(typeof(MegaAnimationState), nameof(MegaAnimationState.SetAnimation))];

    public static void Prefix(MegaAnimationState __instance, ref string animationName)
    {
        var id = __instance?.BoundObject?.GetInstanceId();
        bool cached = id.HasValue && SpineAnimStateCachePatch.RinStateIds.Contains(id.Value);
        GD.Print($"[RinAnim] SetAnimation('{animationName}') id={id} cached={cached}");
        if (id == null || !cached) return;
        if (Remap.TryGetValue(animationName, out string mapped))
            animationName = mapped;
    }
}
