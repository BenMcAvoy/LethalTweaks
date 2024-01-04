using GameNetcodeStuff;
using HarmonyLib;

using System.Collections.Generic;
using System.Reflection.Emit;
using System.Linq;
using System;

namespace LethalTweaks.Patches {
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatch {
        /*[HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void infiniteSprintPatch(ref float ___sprintMeter) {
            ___sprintMeter = 1f;
        }*/

        [HarmonyPatch("LateUpdate")]
        [HarmonyPrefix]
        private static void Prefix(PlayerControllerB __instance) {
            bool isOwner = __instance.IsOwner;
            bool isServer = __instance.IsServer;
            bool isHostPlayerObject = __instance.isHostPlayerObject;

            if (isOwner && (!isServer || isHostPlayerObject)) {
                HUDManagerPatch._healthValueForUpdater = Math.Max(__instance.health, 0);
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
