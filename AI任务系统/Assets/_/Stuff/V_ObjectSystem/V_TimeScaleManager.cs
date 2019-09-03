using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace V_ObjectSystem {

    public static class V_TimeScaleManager {

        private static bool isInit = false;

        public enum TimeType {
            Combo_SuperSelfTimer,
            Combo_RefreshTimer,
            Hero_Intro
        }

        private static Dictionary<TimeType, float> deltaTimeModifierDic;

        public enum Effect {
            Hero_Intro,
            Combo_Slowdown,
            Window_GameMenu,
            Window_Map,
            Tutorial_WaitingForFirstAttack,
            Tutorial_WaitingForComboWindow,
            TutorialVillage_WaitingForSetupWaypoint,
            Player_Dead,
            SecondChance,
            SecondChance_AfterEffect,
            SecondChance_AfterEffect_2,
            SecondChance_AfterEffect_3,
            Combo_SuperSelf,
        }
        public static List<Effect> activeEffectsList;

        private static float timeScale;

        public static void Init() {
            if (isInit) return;
            isInit = true;
            timeScale = 1f;

            activeEffectsList = new List<Effect>();
            deltaTimeModifierDic = new Dictionary<TimeType, float>();
            foreach (TimeType en in System.Enum.GetValues(typeof(TimeType))) {
                deltaTimeModifierDic[en] = 1f;
            }
        }
        public static float GetTimeScale() {
            return timeScale;
        }
        public static float GetDeltaTime(TimeType timeType) {
            return Time.unscaledDeltaTime * deltaTimeModifierDic[timeType];
        }
        private static void UpdateTimeScale() {
            // Get highest priority effect
            if (activeEffectsList.Count <= 0) {
                // No effects
                //Debug.Log("TimeScale Effect: "+"None"+", "+"1");
                //GameHandler.FootstepsSoundResume();
                timeScale = 1f;
                V_Object.SetAllDeltaTimeModifier(timeScale);
            } else {
                // Sort by priority
                for (int i = 0; i < activeEffectsList.Count; i++) {
                    for (int j = i + 1; j < activeEffectsList.Count; j++) {
                        if (GetEffectPriority(activeEffectsList[j]) > GetEffectPriority(activeEffectsList[i])) {
                            Effect tmp = activeEffectsList[i];
                            activeEffectsList[i] = activeEffectsList[j];
                            activeEffectsList[j] = tmp;
                        }
                    }
                }
                ApplyEffect(activeEffectsList[0]);
            }
            // Update individual time
            foreach (TimeType en in System.Enum.GetValues(typeof(TimeType))) {
                deltaTimeModifierDic[en] = 1f;
            }
            if (HasEffect(Effect.Hero_Intro) || HasEffect(Effect.Window_GameMenu) || HasEffect(Effect.Window_Map)) {
                deltaTimeModifierDic[TimeType.Combo_SuperSelfTimer] = 0f;
                deltaTimeModifierDic[TimeType.Combo_RefreshTimer] = 0f;
            }
            if (HasEffect(Effect.Window_GameMenu) || HasEffect(Effect.Window_Map)) {
                deltaTimeModifierDic[TimeType.Hero_Intro] = 0f;
            }
        }
        public static void ResetEffects() {
            activeEffectsList = new List<Effect>();
            UpdateTimeScale();
        }
        private static bool HasEffect(Effect effect) {
            return activeEffectsList.IndexOf(effect) != -1;
        }
        public static void AddEffect(Effect effect, bool stackEffects = true) {
            if (!stackEffects) {
                // Don't stack
                activeEffectsList.Remove(effect);
            }
            activeEffectsList.Add(effect);
            UpdateTimeScale();
        }
        public static void RemoveEffect(Effect effect) {
            activeEffectsList.Remove(effect);
            UpdateTimeScale();
        }
        private static int GetEffectPriority(Effect effect) {
            switch (effect) {
            case Effect.Combo_SuperSelf:
                return 160;
            case Effect.SecondChance:
                return 160;
            case Effect.SecondChance_AfterEffect:
                return 120;
            case Effect.SecondChance_AfterEffect_2:
                return 115;
            case Effect.SecondChance_AfterEffect_3:
                return 110;
            case Effect.Combo_Slowdown:
                return 130;
            case Effect.Hero_Intro:
                return 140;
            case Effect.Player_Dead:
                return 100;
            case Effect.Tutorial_WaitingForFirstAttack:
            case Effect.Tutorial_WaitingForComboWindow:
            case Effect.TutorialVillage_WaitingForSetupWaypoint:
                return 100;
            case Effect.Window_GameMenu:
                return 200;
            case Effect.Window_Map:
                return 150;
            }
            return 0;
        }
        private static void ApplyEffect(Effect effect) {
            float timeScale = 1f;
            switch (effect) {
            case Effect.Tutorial_WaitingForFirstAttack:
            case Effect.Tutorial_WaitingForComboWindow:
            case Effect.TutorialVillage_WaitingForSetupWaypoint:
            case Effect.Hero_Intro:
            case Effect.Window_GameMenu:
            case Effect.Window_Map:
            case Effect.SecondChance:
                timeScale = 0f;
                break;
            case Effect.Combo_SuperSelf:
                timeScale = .3f;
                break;
            case Effect.SecondChance_AfterEffect:
                timeScale = .1f;
                break;
            case Effect.SecondChance_AfterEffect_2:
                timeScale = .3f;
                break;
            case Effect.SecondChance_AfterEffect_3:
                timeScale = .6f;
                break;
            case Effect.Combo_Slowdown:
                timeScale = .1f;
                break;
            case Effect.Player_Dead:
                timeScale = 5f;
                break;
            }
            if (timeScale == 0f) {
                //GameHandler.FootstepsSoundPause();
            } else {
                //GameHandler.FootstepsSoundResume();
            }
            V_TimeScaleManager.timeScale = timeScale;

            V_Object.SetAllDeltaTimeModifier(timeScale);
        }
    }

}