using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V_AnimationSystem;

namespace V_ObjectSystem {

    public class V_ObjectLogic_SkeletonUpdater : V_IObjectActiveLogic, V_IDestroySelf {

        private bool active;
        private V_UnitSkeleton skeleton;
        private bool useUnscaledDeltaTime;

        public V_ObjectLogic_SkeletonUpdater(V_UnitSkeleton skeleton) {
            this.skeleton = skeleton;
            active = true;
            useUnscaledDeltaTime = false;
        }

        public void Update(float deltaTime) {
            if (!active) return;
            if (useUnscaledDeltaTime) deltaTime /= V_TimeScaleManager.GetTimeScale();
            skeleton.Update(deltaTime);
        }
        public void UpdateAsSuperLogicActive(float deltaTime) {
            Update(deltaTime);
        }
        public void UpdateAsSuperLogicInactive(float deltaTime) {
            Update(deltaTime);
        }

        public void DestroySelf() {
            skeleton.DestroySelf();
        }


        public void SetUseUnscaledDeltaTime(bool set) {
            useUnscaledDeltaTime = set;
        }
    }

}