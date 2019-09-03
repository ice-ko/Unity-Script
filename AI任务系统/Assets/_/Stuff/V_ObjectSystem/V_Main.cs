using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_ObjectSystem {

    public static class V_Main {
        
        /*
         * Class to hook Actions into MonoBehaviour
         * */
        private class MonoBehaviourHook : MonoBehaviour {
            
            public Action OnUpdate;
            
            private void Update() {
                OnUpdate();
            }

        }

        public delegate void DelUpdate(float deltaTime);

        private static Dictionary<UpdateType, float> updateTimeModDic; // Update Time Modifier
        private static Dictionary<UpdateType, List<DelUpdate>> onUpdateListTypeDic; // Update Action List
        private static Dictionary<UpdateType, List<DelUpdate>> onLateUpdateListTypeDic; // Late Update Action List

        private static MonoBehaviourHook monoBehaviourHook;

        public enum UpdateType {
            Main,
            Unit,
            Camera,
            Unpaused,
        }
        private static UpdateType[] updateTypeArr;

        public static void Init() {
            if (monoBehaviourHook != null) {
                // Already initialized
                return;
            }
            V_TimeScaleManager.Init();

            updateTypeArr = (UpdateType[])Enum.GetValues(typeof(UpdateType));

            updateTimeModDic = new Dictionary<UpdateType, float>();

            onUpdateListTypeDic = new Dictionary<UpdateType, List<DelUpdate>>();
            onLateUpdateListTypeDic = new Dictionary<UpdateType, List<DelUpdate>>();

            foreach (UpdateType updateType in updateTypeArr) {
                updateTimeModDic[updateType] = 1f;

                onUpdateListTypeDic[updateType] = new List<DelUpdate>();
                onLateUpdateListTypeDic[updateType] = new List<DelUpdate>();
            }

            GameObject gameObject = new GameObject("V_Main_GameObject");
            monoBehaviourHook = gameObject.AddComponent<MonoBehaviourHook>();
            monoBehaviourHook.OnUpdate = Update;
        }
        public static void SetUpdateTimeMod(UpdateType updateType, float mod) {
            if (updateTimeModDic == null) return;
            updateTimeModDic[updateType] = mod;
        }

        // Manually trigger Update
        public static void TriggerFakeUpdate(float deltaTime) {
            Update(deltaTime);
        }

        private static void Update() {
            Update(Time.deltaTime);
        }
        private static void Update(float deltaTime) {
            foreach (UpdateType updateType in updateTypeArr) {
                List<DelUpdate> tmpOnUpdateList = new List<DelUpdate>(onUpdateListTypeDic[updateType]);
                for (int i = 0; i < tmpOnUpdateList.Count; i++) tmpOnUpdateList[i](deltaTime * updateTimeModDic[updateType]);
            }

            // Update V_Object's
            V_Object.Static_Update(deltaTime, updateTimeModDic);

            foreach (UpdateType updateType in updateTypeArr) {
                List<DelUpdate> tmpOnLateUpdateList = new List<DelUpdate>(onLateUpdateListTypeDic[updateType]);
                for (int i = 0; i < tmpOnLateUpdateList.Count; i++) tmpOnLateUpdateList[i](deltaTime * updateTimeModDic[updateType]);
            }
        }




        public delegate void DelRegisterOnUpdate(DelUpdate onUpdate, UpdateType updateType);

        public static void RegisterOnUpdate(DelUpdate onUpdate, UpdateType updateType = UpdateType.Main) {
            Init();
            onUpdateListTypeDic[updateType].Add(onUpdate);
        }
        public static void DeregisterOnUpdate(DelUpdate onUpdate, UpdateType updateType = UpdateType.Main) {
            onUpdateListTypeDic[updateType].Remove(onUpdate);
        }



        public delegate void DelRegisterOnLateUpdate(DelUpdate onLateUpdate, UpdateType updateType);


        public static void RegisterOnLateUpdate(DelUpdate onLateUpdate, UpdateType updateType = UpdateType.Main) {
            Init();
            onLateUpdateListTypeDic[updateType].Add(onLateUpdate);
        }
        public static void DeregisterOnLateUpdate(DelUpdate onLateUpdate, UpdateType updateType = UpdateType.Main) {
            onLateUpdateListTypeDic[updateType].Remove(onLateUpdate);
        }

    }

}