using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalUtilities.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LethalUtilities
{
    [BepInPlugin(modGUID, modName, modVerison)]
    public class LethalUtilitiesBase : BaseUnityPlugin
    {
        public const string modGUID = "Hydrx.LethalUtilities";
        public const string modName = "Lethal Utilities";
        public const string modVerison = "0.0.1";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static LethalUtilitiesBase Instance;

        internal ManualLogSource mls;


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            mls.LogInfo("Lethal Utilities has started");

            harmony.PatchAll(typeof(LethalUtilitiesBase));
            harmony.PatchAll(typeof(PlayerControllerBPatch));
        }

    }
}
