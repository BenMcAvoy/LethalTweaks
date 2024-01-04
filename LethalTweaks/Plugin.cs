using BepInEx.Logging;
using BepInEx;

using LethalTweaks.Patches;
using HarmonyLib;

namespace LethalTweaks {
    [BepInPlugin(modGUID, modName, modVerison)]
    public class LethalTweaksBase : BaseUnityPlugin {
        public const string modGUID = "HydrxAndBen.LethalTweaks";
        public const string modName = "Lethal Tweaks";
        public const string modVerison = "0.0.1";

        private readonly Harmony harmony = new Harmony(modGUID);
        internal ManualLogSource mls;

        void Awake() {
            if (Instance == null)
                Instance = this;

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            mls.LogInfo("Lethal Tweaks has started");

            harmony.PatchAll(typeof(LethalTweaksBase));
            harmony.PatchAll(typeof(PlayerControllerBPatch));
            harmony.PatchAll(typeof(ItemDropshipPatch));
        }
    }
}
