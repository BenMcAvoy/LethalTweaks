﻿using BepInEx.Logging;
using BepInEx;

using LethalTweaks.Patches;
using HarmonyLib;
using System;

namespace LethalTweaks {
    [BepInPlugin(modGUID, "Lethal Tweaks", "0.0.1")]
    public class LethalTweaksBase : BaseUnityPlugin {
        public const string modGUID = "FatalSyndicate.LethalTweaks";

        private readonly Harmony harmony = new Harmony(modGUID);
        private static LethalTweaksBase Instance;
        internal ManualLogSource logger;

        void Awake() {
            if (Instance == null)
                Instance = this;

            logger = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            logger.LogInfo("Lethal Tweaks is patching.");

            Patch(typeof(GameNetworkManagerPatch));
            Patch(typeof(PlayerControllerBPatch));
            Patch(typeof(ItemDropshipPatch));
            Patch(typeof(LethalTweaksBase));
            Patch(typeof(HUDManagerPatch));
            Patch(typeof(devPatches));

            logger.LogInfo("Lethal Tweaks has finished patching.");
        }

        void Patch(Type type) {
            logger.LogInfo("Running patch: " + type.ToString());
            harmony.PatchAll(type);
        }
    }
}
