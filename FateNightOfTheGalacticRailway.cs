using System.Reflection;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using FateNightOfTheGalacticRailway.Core.Cards;
using FateNightOfTheGalacticRailway.Core.Characters;
using FateNightOfTheGalacticRailway.Core.Relics;
using STS2RitsuLib;
using STS2RitsuLib.Interop;
using STS2RitsuLib.Scaffolding.Content;

namespace FateNightOfTheGalacticRailway;

[ModInitializer(nameof(Initialize))]
public static class FateNightOfTheGalacticRailwayMod
{
    public const string MOD_ID = "FateNightOfTheGalacticRailway";

    public static Logger Logger { get; private set; }

    public static void Initialize()
    {
        Logger = RitsuLibFramework.CreateLogger(MOD_ID);

        Assembly asm = Assembly.GetExecutingAssembly();
        RitsuLibFramework.EnsureGodotScriptsRegistered(asm, Logger);
        ModTypeDiscoveryHub.RegisterModAssembly(MOD_ID, asm);

        var pack = RitsuLibFramework.CreateContentPack(MOD_ID)
            // Attack cards
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, Cooperation>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, FullAttack>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, SuppressionTactic>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, MagicGemFire>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, MagicGemVoid>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, GaeBolg>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, RuleBreaker>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, AhaStrike>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, AhaSweep>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, AhaSword>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, Boring>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, CraneWingThree>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, ProjectionBegin>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, FantasyCollapse>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, GoldenSlash1>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, GoldenSlash2>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, GoldenSlash3>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, ManaBurst>())
            // Skill cards
            // RinsPendant split into 4 independent cards
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, RinsPendantPioneer>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, RinsPendantTreasure>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, RinsPendantProjection>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, RinsPendantSword>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, MagicGemWind>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, MagicGemEarth>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, MagicGemWater>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, EpicClayTablet>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, SimpleTrial>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, PerfectProjector>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, LawAka>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, WantedPoster>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, PassTheParcel>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, KingWine>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, FakeBook>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, MoralApproval>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, KingGoblet>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, FriendshipProof>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, AhaMirror>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, DeathTitanCloak>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, AhaSupport>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, GoldenRule>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, OpenLock>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, TrueEye>())
            // Power cards
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, Avalon>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, WhyAreYouHere>())
            .Entry(new CharacterStarterCardRegistrationEntry<TosakaRin, RejuvenationSpecial>())
            // Starter relic
            .Entry(new CharacterStarterRelicRegistrationEntry<TosakaRin, TosakaStyle>());
        pack.Apply();

        var patcher = RitsuLibFramework.CreatePatcher(MOD_ID, "core");
        patcher.PatchAll();
    }
}
