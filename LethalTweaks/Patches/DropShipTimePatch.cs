using HarmonyLib;

namespace LethalTweaks.Patches {
    [HarmonyPatch(typeof(ItemDropship))]
    internal class DropShipTimePatch {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void Update(ref float ___shipTimer, ref bool ___deliveringOrder, ItemDropship __instance) {
            if (__instance.IsServer && !___deliveringOrder && ___shipTimer > 2f)
                ___shipTimer = 30f;
        }
    }
}
