using UnityEngine;
using HarmonyLib;
using System;
using TMPro;

namespace LethalTweaks.Patches {
    [HarmonyPatch(typeof(HUDManager))]
    internal class HUDManagerPatch {
        // Health text storage
        private static TextMeshProUGUI healthText;

        // External values (HUDManager)
        public static int _oldValuehealthValueForUpdater = 0;
        public static int _healthValueForUpdater = 100;

        // Colors
        private static readonly Color _healthyColor = new Color32(0, byte.MaxValue, 0, byte.MaxValue);
        private static readonly Color _criticalHealthColor = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void Start(ref HUDManager __instance) {
            GameObject healthHUD = new GameObject("HealthHUDDisplay");
            healthHUD.AddComponent<RectTransform>();

            TextMeshProUGUI healthUGUI = healthHUD.AddComponent<TextMeshProUGUI>();

            RectTransform rectTransform = healthUGUI.rectTransform;

            rectTransform.SetParent(__instance.PTTIcon.transform, false);
            rectTransform.anchoredPosition = new Vector2(8f, -60f);

            healthUGUI.font = __instance.controlTipLines[0].font;
            healthUGUI.fontSize = 20f;
            healthUGUI.text = "100";
            healthUGUI.color = _healthyColor;
            healthUGUI.overflowMode = TextOverflowModes.Overflow;
            healthUGUI.enabled = true;

            healthText = healthUGUI;
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void Update() {
            if (healthText == null) return;
            if (_healthValueForUpdater == _oldValuehealthValueForUpdater) return;

            _oldValuehealthValueForUpdater = _healthValueForUpdater;

            if (_healthValueForUpdater <= 0) {
                healthText.text = "X";
            } else {
                healthText.text = _healthValueForUpdater.ToString().PadLeft(_healthValueForUpdater < 10 ? 2 : 3);
            }

            double percentage = (double)_healthValueForUpdater / 100.0;
            healthText.color = ColorGradient(_criticalHealthColor, _healthyColor, percentage);
        }

        public static int LinearInterpolation(int start, int end, double percentage) {
            return start + (int)Math.Round(percentage * (double)(end - start));
        }

        public static Color ColorGradient(Color start, Color end, double percentage) {
            float interpolatedR = (float)(start.r + (percentage * (end.r - start.r)));
            float interpolatedG = (float)(start.g + (percentage * (end.g - start.g)));
            float interpolatedB = (float)(start.b + (percentage * (end.b - start.b)));

            return new Color(interpolatedR, interpolatedG, interpolatedB, 1f);
        }
    }
}
