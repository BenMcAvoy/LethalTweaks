using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx;

using LethalTweaks.Patches;
using HarmonyLib;
using System;

namespace LethalTweaks {
    [BepInPlugin(modGUID, "Lethal Tweaks", "0.0.1")]
    public class Plugin : BaseUnityPlugin {
        public const string modGUID = "FatalSyndicate.LethalTweaks";

        private readonly Harmony harmony = new Harmony(modGUID);
        private static Plugin Instance;

        public bool isDev = true;

        void Awake() {
            if (Instance == null)
                Instance = this;

            LogInfo("Lethal Tweaks is patching.");

            Patch(typeof(GameNetworkManagerPatch));
            Patch(typeof(PlayerControllerBPatch));
            Patch(typeof(ItemDropshipPatch));
            Patch(typeof(Plugin));
            Patch(typeof(HUDManagerPatch));

            if (isDev)
                Patch(typeof(devPatches));

            LogInfo("Lethal Tweaks has finished patching.");
        }

        void Patch(Type type) {
            LogInfo("Running patch: " + type.ToString());
            harmony.PatchAll(type);
        }

        public static void LogInfo<T>(T message) { Plugin.LogInfo(message); }
        public static void LogWarn<T>(T message) { Plugin.LogWarn(message); }
        public static void LogError<T>(T message) { Plugin.LogError(message); }
    }
}
