using LethalCompanyInputUtils.Api;
using UnityEngine.InputSystem;
using GameNetcodeStuff;
using UnityEngine;
using HarmonyLib;

using System.Collections.Generic;
using System.Reflection.Emit;
using System.Linq;
using System;

public class Keybinds : LcInputActions {
    [InputAction("<Keyboard>/f", Name = "Flashlight")]
    public InputAction Flashlight { get; set; }
}

namespace LethalTweaks.Patches {
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatch {
        internal static Keybinds InputActionInstance = new Keybinds();

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
                }
                catch { }
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

        [HarmonyPatch("PlayerJump", (MethodType)5)]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            List<CodeInstruction> list = instructions.ToList<CodeInstruction>();

            for (int i = 0; i < list.Count; i++)
                if (list[i].opcode == OpCodes.Ldc_R4)
                    list[i] = new CodeInstruction(OpCodes.Ldc_R4, 0f);

            return list;
        }
    }
}
