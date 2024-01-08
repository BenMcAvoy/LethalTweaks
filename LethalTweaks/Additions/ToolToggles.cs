using LethalCompanyInputUtils.Api;
using UnityEngine.InputSystem;
using GameNetcodeStuff;
using UnityEngine;
using HarmonyLib;
using System;

public class ToolKeybinds : LcInputActions {
    [InputAction("<Keyboard>/f", Name = "Flashlight")]
    public InputAction Flashlight { get; set; }
}

namespace LethalTweaks.Additions {
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class ToolToggles {
        internal static ToolKeybinds InputActionInstance = new ToolKeybinds();

        [HarmonyPatch("KillPlayer")]
        [HarmonyPostfix]
        public static void ClearFlashlight(PlayerControllerB __instance) {
            __instance.pocketedFlashlight = null;
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void Update(PlayerControllerB __instance) {
            if (!ShouldProcess(__instance)) return;

            if (__instance.currentlyHeldObjectServer is FlashlightItem flashlightItem && __instance.currentlyHeldObjectServer != __instance.pocketedFlashlight)
                __instance.pocketedFlashlight = __instance.currentlyHeldObjectServer;

            if (__instance.pocketedFlashlight != null && InputActionInstance.Flashlight.triggered && __instance.pocketedFlashlight is FlashlightItem pocketedFlashlight && pocketedFlashlight.isHeld) {
                try {
                    pocketedFlashlight.UseItemOnClient(true);

                    if (!(__instance.currentlyHeldObjectServer is FlashlightItem))
                        HandleFlashlightUsage(__instance, pocketedFlashlight);
                } catch { }
            }
        }

        private static bool ShouldProcess(PlayerControllerB __instance) {
            return !(!__instance.IsOwner || !__instance.isPlayerControlled || (__instance.IsServer && !__instance.isHostPlayerObject)) && !__instance.isTestingPlayer && !__instance.inTerminalMenu && !__instance.isTypingChat && Application.isFocused;
        }

        private static void HandleFlashlightUsage(PlayerControllerB __instance, FlashlightItem pocketedFlashlight) {
            pocketedFlashlight.flashlightBulbGlow.enabled = false;
            pocketedFlashlight.flashlightBulb.enabled = false;

            __instance.helmetLight.enabled = pocketedFlashlight.isBeingUsed;
            pocketedFlashlight.usingPlayerHelmetLight = pocketedFlashlight.isBeingUsed;
            pocketedFlashlight.PocketFlashlightServerRpc(pocketedFlashlight.isBeingUsed);
        }

        [HarmonyPatch("LateUpdate")]
        [HarmonyPrefix]
        private static void Prefix(PlayerControllerB __instance) {
            bool isOwner = __instance.IsOwner;
            bool isServer = __instance.IsServer;
            bool isHostPlayerObject = __instance.isHostPlayerObject;

            if (isOwner && (!isServer || isHostPlayerObject)) {
                Additions.HUDTextInformation._healthValueForUpdater = Math.Max(__instance.health, 0);
            }

        }

    }
}
