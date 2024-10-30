using BepInEx;

using HarmonyLib;
using System;

using LethalTweaks.Additions;
using LethalTweaks.Patches;

namespace LethalTweaks {
    [BepInPlugin(modGUID, "Lethal Tweaks", "0.0.1")]
    public class Plugin : BaseUnityPlugin {
        public const string modGUID = "FatalSyndicate.LethalTweaks";

        private readonly Harmony harmony = new Harmony(modGUID);
        private static Plugin Instance;

        void Awake() {
            if (Instance == null)
                Instance = this;

            LogInfo("Lethal Tweaks is patching.");

            // Patches
            Patch(typeof(DropShipTimePatch));
            Patch(typeof(JumpDelayPatch));
            Patch(typeof(Plugin));

            // Additions
            Patch(typeof(HUDTextInformation));
            Patch(typeof(ToolToggles));
            Patch(typeof(numSlot));
            Patch(typeof(extraSlots));

            LogInfo("Lethal Tweaks has finished patching.");
        }

        void Patch(Type type) {
            LogInfo("Running patch: " + type.ToString());
            harmony.PatchAll(type);
        }

        public static void LogInfo<T>(T message) => Instance.Logger.LogInfo(message); 
        public static void LogWarn<T>(T message) => Instance.Logger.LogWarning(message);
        public static void LogError<T>(T message) => Instance.Logger.LogError(message); 
    }
}
