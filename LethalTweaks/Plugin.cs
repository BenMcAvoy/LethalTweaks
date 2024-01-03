using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalTweaks.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LethalTweaks
{
    [BepInPlugin(modGUID, modName, modVerison)]
    public class LethalTweaksBase : BaseUnityPlugin
    {
        public const string modGUID = "HydrxAndBen.LethalTweaks";
        public const string modName = "Lethal Tweaks";
        public const string modVerison = "0.0.1";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static LethalTweaksBase Instance;

        internal ManualLogSource mls;


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            mls.LogInfo("Lethal Tweaks has started");

            harmony.PatchAll(typeof(LethalTweaksBase));
            harmony.PatchAll(typeof(PlayerControllerBPatch));
        }

    }
}
