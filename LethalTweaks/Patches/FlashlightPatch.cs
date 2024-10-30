using GameNetcodeStuff;
using HarmonyLib;
using System.Reflection;

namespace LethalTweaks.Patches {
    class FlashlightPatch {
        static FieldInfo _previousPlayerField;

        [HarmonyPatch(typeof(FlashlightItem), "DiscardItem")]
        [HarmonyPostfix]
        static void DiscardItem(FlashlightItem __instance) {
            if (_previousPlayerField == null)
                _previousPlayerField = typeof(FlashlightItem).GetField("previousPlayerHeldBy", BindingFlags.Instance | BindingFlags.NonPublic);

            var field = _previousPlayerField.GetValue(__instance);
            PlayerControllerB controller = (PlayerControllerB)((field is PlayerControllerB) ? field : null);
        }
    }
}
