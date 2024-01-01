using LethalCompanyInputUtils.Api;
using UnityEngine.InputSystem;
using BepInEx;

namespace LethalTweaks {
    public class LethalTweaksInput : LcInputActions {
        [InputAction("<Keyboard>/1", Name = "Slot 1")]
        public InputAction SlotOne { get; set; }
    }

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
        private void Awake() {
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            
        }
    }
}