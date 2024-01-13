using LethalCompanyInputUtils.Api;
using UnityEngine.InputSystem;
using GameNetcodeStuff;
using UnityEngine;
using HarmonyLib;

using System.Collections.Generic;
using System.Reflection.Emit;
using System.Linq;
using System;


namespace LethalTweaks.Patches {
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class JumpDelayPatch {
        [HarmonyPatch("PlayerJump", (MethodType)5)]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            List<CodeInstruction> list = instructions.ToList();

            for (int i = 0; i < list.Count; i++)
                if (list[i].opcode == OpCodes.Ldc_R4)
                    list[i] = new CodeInstruction(OpCodes.Ldc_R4, 0f);

            return list;
        }
    }
}
