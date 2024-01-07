using GameNetcodeStuff;
using UnityEngine;
using HarmonyLib;
using Unity.Netcode;

namespace LethalTweaks.Patches {
    [HarmonyPatch(typeof(GameNetworkManager))]
    internal class GameNetworkManagerPatch {
        [HarmonyPatch("ConnectionApproval")]
        static bool Prefix(NetworkManager.ConnectionApprovalResponse response) {
            response.Approved = true;
            return true;
        }
    }
}
