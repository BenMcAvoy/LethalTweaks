using HarmonyLib;
using System;
using UnityEngine;

namespace LethalTweaks.Patches {
    internal class devPatches {
        const bool enabled = false;

        [HarmonyPatch(typeof(Application), "isEditor", MethodType.Getter)]
        static bool IsEditorGetter(ref bool __result) {
            Console.WriteLine("Being called");
            __result = enabled;
            return false;
        }

        [HarmonyPatch(typeof(QuickMenuManager), "OpenQuickMenu")]
        [HarmonyPostfix]
        private static void openQuickMenu(QuickMenuManager __instance) {
            Console.WriteLine("Opening quick menu");
            Console.WriteLine(__instance.debugMenuUI.activeSelf);
            __instance.debugMenuUI.SetActive(true);
        }
    }
}
