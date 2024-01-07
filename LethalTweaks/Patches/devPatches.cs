using HarmonyLib;
using System;
using UnityEngine;
using GameNetcodeStuff;
using LethalCompanyInputUtils.Api;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DevKeybinds : LcInputActions {
    [InputAction("<Keyboard>/p", Name = "Sprint cheat")]
    public InputAction SprintCheat { get; set; }

    [InputAction("<Keyboard>/l", Name = "God cheat")]
    public InputAction GodCheat { get; set; }

    [InputAction("<Keyboard>/c", Name = "Add cash")]
    public InputAction AddCash { get; set; }

    [InputAction("<Keyboard>/k", Name = "Remove cash")]
    public InputAction RemoveCash { get; set; }
}

namespace LethalTweaks.Patches {
    internal class devPatches {
        static bool sprintEnabled = false;
        static bool godEnabled = false;

        private static DateTime lastTriggerTime = DateTime.MinValue;

        internal static DevKeybinds DevBinds = new DevKeybinds();

        private static Terminal terminal;

        [HarmonyPatch(typeof(PlayerControllerB), "Awake")]
        [HarmonyPrefix]
        public static void Awake() {
            terminal = UnityEngine.Object.FindObjectsOfType<Terminal>()[0];
        }

        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPrefix]
        public static void UpdateBinds() {
            TimeSpan elapsedSinceLastTrigger = DateTime.Now - lastTriggerTime;

            if (elapsedSinceLastTrigger.TotalMilliseconds >= 100) {
                if (DevBinds.SprintCheat.triggered) {
                    Plugin.LogInfo("Toggling sprint cheat");
                    sprintEnabled = !sprintEnabled;

                    lastTriggerTime = DateTime.Now;
                }
            }

            if (DevBinds.AddCash.triggered || DevBinds.RemoveCash.triggered) {
                int multiplier = DevBinds.AddCash.triggered ? 1 : DevBinds.RemoveCash.triggered ? -1 : 0;

                if (terminal != null) {
                    terminal.groupCredits += 10 * multiplier;

                    if (!GameNetworkManager.Instance.localPlayerController.IsServer)
                        terminal.SyncGroupCreditsClientRpc(terminal.groupCredits, multiplier);
                }
            }
        }

        [HarmonyPatch(typeof(PlayerControllerB))]
        [HarmonyPatch("AllowPlayerDeath")]
        public static bool OverrideDeath() {
            return !godEnabled;
        }

        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        public static void Update(ref float ___sprintMeter, ref float ___sprintMultiplier) {
            if (sprintEnabled) {
                ___sprintMultiplier = 3f;
                ___sprintMeter = 100f;
            }
            else
                ___sprintMultiplier = 1f;
        }

        [HarmonyPatch(typeof(Application), "isEditor", MethodType.Getter)]
        static bool IsEditorGetter(ref bool __result) {
            Plugin.LogInfo("Being called");
            __result = true;
            return false;
        }

        [HarmonyPatch(typeof(QuickMenuManager), "OpenQuickMenu")]
        [HarmonyPostfix]
        private static void openQuickMenu(QuickMenuManager __instance) {
            Plugin.LogInfo("Opening quick menu");
            Plugin.LogInfo(__instance.debugMenuUI.activeSelf);
            __instance.debugMenuUI.SetActive(true);
        }
    }
}
