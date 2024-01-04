using BepInEx.Logging;
using BepInEx;

using LethalTweaks.Patches;
using HarmonyLib;
using System;

namespace LethalTweaks {
    [BepInPlugin(modGUID, modName, modVerison)]
    public class LethalTweaksBase : BaseUnityPlugin {
        public const string modGUID = "HydrxAndBen.LethalTweaks";
        public const string modName = "Lethal Tweaks";
        public const string modVerison = "0.0.1";

        private readonly Harmony harmony = new Harmony(modGUID);
        private static LethalTweaksBase Instance;
        internal ManualLogSource mls;

        void Awake() {
            if (Instance == null)
                Instance = this;

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            mls.LogInfo("Lethal Tweaks is patching.");

            Patch(typeof(PlayerControllerBPatch));
            Patch(typeof(ItemDropshipPatch));
            Patch(typeof(LethalTweaksBase));
            Patch(typeof(HUDManagerPatch));

            mls.LogInfo("Lethal Tweaks has finished patching.");
        }

        void Patch(Type type) {
            mls.LogInfo("Running patch: " + type.ToString());
            harmony.PatchAll(type);
        }
    }
}
