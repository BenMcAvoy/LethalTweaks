using BepInEx;
using HarmonyLib;

using LethalTweaks.Patches;

namespace LethalTweaks {
    [BepInPlugin(modGUID, "Lethal Tweaks", "0.0.1")]
    public class Plugin : BaseUnityPlugin {
        public const string modGUID = "FatalSyndicate.LethalTweaks";
        public static Plugin Instance;

        private readonly Harmony harmony = new Harmony(modGUID);

        void Awake() {
            if (Instance == null)
                Instance = this;

            Logger.LogInfo("Lethal Tweaks is patching.");

            Patch(typeof(GameNetworkManagerPatch));
            Patch(typeof(PlayerControllerBPatch));
            Patch(typeof(ItemDropshipPatch));
            Patch(typeof(Plugin));
            Patch(typeof(HUDManagerPatch));

            Patch(typeof(devPatches)); // Temporary

            Logger.LogInfo("Lethal Tweaks has finished patching.");
        }

        void Patch(System.Type type) {
            Logger.LogInfo("Running patch: " + type.ToString());
            harmony.PatchAll(type);
        }

        public static void LogInfo(string message) {
            Instance.Logger.LogInfo(message);
        }

        public static void LogWarn(string message) {
            Instance.Logger.LogWarning(message);
        }

        public static void LogError(string message) {
            Instance.Logger.LogError(message);
        }
    }
}
