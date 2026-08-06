using HarmonyLib;
using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace FateNightOfTheGalacticRailway.Core.Patches;

[HarmonyPatch(typeof(NCreatureVisuals), "_Ready")]
public static class SpineFixPatch
{
    private const string AtlasPath =
        "res://FateNightOfTheGalacticRailway/spines/tosaka_rin/tosaka_rin.atlas";
    private const string SkelPath =
        "res://FateNightOfTheGalacticRailway/spines/tosaka_rin/tosaka_rin.skel";

    [HarmonyPostfix]
    private static void Postfix(NCreatureVisuals __instance)
    {
        if (__instance.SpineBody != null) return;

        var body = __instance.GetCurrentBody();
        if (body == null) { GD.PushWarning("[RinSpine] body is null"); return; }
        if (body.GetClass() != "SpineSprite") { GD.PushWarning($"[RinSpine] body class is {body.GetClass()}, not SpineSprite"); return; }

        var atlasRes = GD.Load<Resource>(AtlasPath);
        var skelRes = GD.Load<Resource>(SkelPath);
        if (atlasRes == null) { GD.PushWarning("[RinSpine] atlasRes is null"); return; }
        if (skelRes == null) { GD.PushWarning("[RinSpine] skelRes is null"); return; }

        var skeletonRes = ClassDB.Instantiate("SpineSkeletonDataResource").AsGodotObject();
        if (skeletonRes == null) { GD.PushWarning("[RinSpine] skeletonRes is null"); return; }

        skeletonRes.Set("atlas_res", atlasRes);
        skeletonRes.Set("skeleton_file_res", skelRes);

        body.Call("set_skeleton_data_res", skeletonRes);

        var megaSprite = new MegaSprite(body);
        if (megaSprite.GetSkeleton()?.GetData() == null)
        {
            GD.PushWarning("[RinSpine] skeleton data still null after manual setup");
            return;
        }

        typeof(NCreatureVisuals)
            .GetProperty("SpineBody")
            ?.SetValue(__instance, megaSprite);
        GD.Print("[RinSpine] Successfully loaded skeleton data");
    }
}
