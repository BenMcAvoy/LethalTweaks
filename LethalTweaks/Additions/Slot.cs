using GameNetcodeStuff;
using HarmonyLib;
using System.Reflection;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Object = UnityEngine.Object;
using System.Linq;

namespace LethalTweaks.Additions
{
    class numSlot
    {
        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        static void Update(PlayerControllerB __instance)
        {
            if (!ToolToggles.ShouldProcess(__instance)) return;

            if (!__instance.twoHanded)
            {
                var playerController = GameNetworkManager.Instance.localPlayerController;
                MethodInfo switchSlotMethod = playerController.GetType().GetMethod("SwitchToItemSlot", BindingFlags.NonPublic | BindingFlags.Instance);

                var slots = GameObject.FindObjectsOfType<GameObject>()
                    .Where(obj => obj.name.StartsWith("Slot") && int.TryParse(obj.name.Substring(4), out _))
                    .ToList();

                for (int i = 0; i < slots.Count; i++)
                {
                    if (IsDigitKeyPressed(i))
                    {
                        switchSlotMethod.Invoke(playerController, new object[] { i, null });
                        break;
                    }
                }
            }
        }

        static bool IsDigitKeyPressed(int digit)
        {
            if (digit == 0) return Keyboard.current.digit1Key.isPressed;
            if (digit == 1) return Keyboard.current.digit2Key.isPressed;
            if (digit == 2) return Keyboard.current.digit3Key.isPressed;
            if (digit == 3) return Keyboard.current.digit4Key.isPressed;
            if (digit == 4) return Keyboard.current.digit5Key.isPressed;
            if (digit == 5) return Keyboard.current.digit6Key.isPressed;
            if (digit == 6) return Keyboard.current.digit7Key.isPressed;
            if (digit == 7) return Keyboard.current.digit8Key.isPressed;

            return false;
        }
    }

    class extraSlots
    {
        static float xOffset;
        static GameObject inv;
        static PlayerControllerB[] playerControllerB;

        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        static void Update()
        {
            bool flag = SceneManager.GetActiveScene().name == "InitSceneLaunchOptions" || SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "InitScene";
            if (!flag)
            {
                playerControllerB = Object.FindObjectsOfType<PlayerControllerB>();
                if (playerControllerB.Length != 0)
                {
                    for (int i = 0; i < playerControllerB.Length; i++)
                    {
                        if (playerControllerB[i].ItemSlots.Length < 8)
                        {
                            Array.Resize(ref playerControllerB[i].ItemSlots, 8);
                            for (int j = 4; j < 8; j++)
                                playerControllerB[i].ItemSlots[j] = playerControllerB[i].ItemSlots[j - 4];
                        }
                    }
                }

                if (HUDManager.Instance.itemSlotIconFrames.Length < 8)
                {
                    Image[] itemSlotIconFrames = HUDManager.Instance.itemSlotIconFrames;
                    xOffset = itemSlotIconFrames[1].transform.position.x - itemSlotIconFrames[0].transform.position.x;

                    for (int k = 0; k < 4; k++)
                        itemSlotIconFrames[k].transform.position -= new Vector3(xOffset * 2, 0, 0);

                    for (int k = 4; k < 8; k++)
                    {
                        if (itemSlotIconFrames.Length <= k)
                        {
                            Image image = Object.Instantiate(HUDManager.Instance.itemSlotIconFrames[0], HUDManager.Instance.itemSlotIconFrames[0].transform.parent);
                            Array.Resize(ref itemSlotIconFrames, itemSlotIconFrames.Length + 1);
                            itemSlotIconFrames[itemSlotIconFrames.Length - 1] = image;
                            HUDManager.Instance.itemSlotIconFrames = itemSlotIconFrames;
                        }

                        itemSlotIconFrames[k].transform.position = itemSlotIconFrames[k - 4].transform.position + new Vector3(4 * xOffset, 0, 0);
                        itemSlotIconFrames[k].name = "Slot" + k;
                    }
                }

                if (HUDManager.Instance.itemSlotIcons.Length < 8)
                {
                    for (int i = 4; i < 8; i++)
                    {
                        Image newItemSlotIcon = Object.Instantiate(HUDManager.Instance.itemSlotIcons[0], HUDManager.Instance.itemSlotIcons[0].transform.parent);
                        Array.Resize(ref HUDManager.Instance.itemSlotIcons, HUDManager.Instance.itemSlotIcons.Length + 1);
                        HUDManager.Instance.itemSlotIcons[HUDManager.Instance.itemSlotIcons.Length - 1] = newItemSlotIcon;

                        newItemSlotIcon.transform.position = HUDManager.Instance.itemSlotIcons[i - 4].transform.position + new Vector3(4 * xOffset, 0, 0);
                        newItemSlotIcon.name = "ItemSlotIcon" + i;
                    }

                    for (int m = 0; m < 8; m++)
                        HUDManager.Instance.itemSlotIcons[m].transform.position = HUDManager.Instance.itemSlotIconFrames[m].transform.position;
                }

                if (inv == null) inv = GameObject.Find("Inventory");
                else
                {
                    if (inv.transform.childCount < 8)
                    {
                        xOffset = inv.transform.Find("Slot0").gameObject.transform.position.x - inv.transform.Find("Slot1").gameObject.transform.position.x;

                        for (int i = 0; i < 4; i++)
                            inv.transform.Find("Slot" + i).gameObject.transform.position -= new Vector3(xOffset * 2, 0, 0);

                        for (int i = 4; i < 8; i++)
                        {
                            GameObject originalSlot = inv.transform.Find("Slot" + (i - 4)).gameObject;
                            GameObject newSlot = Object.Instantiate(originalSlot, inv.transform);
                            newSlot.name = "Slot" + i;
                            newSlot.transform.position = originalSlot.transform.position + new Vector3(4 * xOffset, 0, 0);
                        }
                    }
                }
            }
        }
    }
}
