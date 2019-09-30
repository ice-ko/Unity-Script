/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading the Code Monkey Utilities
    I hope you find them useful in your projects
    If you have any questions use the contact form
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using UnityEngine;
using System.Collections.Generic;

namespace CodeMonkey.Utils {

    /*
     * Triggers a Action after a certain time 
     * */
    public class FunctionTimer {

        /*
         * Class to hook Actions into MonoBehaviour
         * */
        private class MonoBehaviourHook : MonoBehaviour {

            public Action OnUpdate;

            private void Update() {
                if (OnUpdate != null) OnUpdate();
            }

        }


        private static List<FunctionTimer> timerList; // Holds a reference to all active timers
        private static GameObject initGameObject; // Global game object used for initializing class, is destroyed on scene change

        private static void InitIfNeeded() {
            if (initGameObject == null) {
                initGameObject = new GameObject("FunctionTimer_Global");
                timerList = new List<FunctionTimer>();
            }
        }




        public static FunctionTimer Create(Action action, float timer) {
            return Create(action, timer, "", false, false);
        }
        public static FunctionTimer Create(Action action, float timer, string functionName) {
            return Create(action, timer, functionName, false, false);
        }
        public static FunctionTimer Create(Action action, float timer, string functionName, bool useUnscaledDeltaTime) {
            return Create(action, timer, functionName, useUnscaledDeltaTime, false);
        }
        public static FunctionTimer Create(Action action, float timer, string functionName, bool useUnscaledDeltaTime, bool stopAllWithSameName) {
            InitIfNeeded();

            if (stopAllWithSameName) {
                StopAllTimersWithName(functionName);
            }

            GameObject obj = new GameObject("FunctionTimer Object "+functionName, typeof(MonoBehaviourHook));
            FunctionTimer funcTimer = new FunctionTimer(obj, action, timer, functionName, useUnscaledDeltaTime);
            obj.GetComponent<MonoBehaviourHook>().OnUpdate = funcTimer.Update;

            timerList.Add(funcTimer);

            return funcTimer;
        }
        public static void RemoveTimer(FunctionTimer funcTimer) {
            InitIfNeeded();
            timerList.Remove(funcTimer);
        }
        public static void StopAllTimersWithName(string functionName) {
            InitIfNeeded();
            for (int i = 0; i < timerList.Count; i++) {
                if (timerList[i].functionName == functionName) {
                    timerList[i].DestroySelf();
                    i--;
                }
            }
        }
        public static void StopFirstTimerWithName(string functionName) {
            InitIfNeeded();
            for (int i = 0; i < timerList.Count; i++) {
                if (timerList[i].functionName == functionName) {
                    timerList[i].DestroySelf();
                    return;
                }
            }
        }





        private GameObject gameObject;
        private float timer;
        private string functionName;
        private bool active;
        private bool useUnscaledDeltaTime;
        private Action action;



        public FunctionTimer(GameObject gameObject, Action action, float timer, string functionName, bool useUnscaledDeltaTime) {
            this.gameObject = gameObject;
            this.action = action;
            this.timer = timer;
            this.functionName = functionName;
            this.useUnscaledDeltaTime = useUnscaledDeltaTime;
        }

        private void Update() {
            if (useUnscaledDeltaTime) {
                timer -= Time.unscaledDeltaTime;
            } else {
                timer -= Time.deltaTime;
            }
            if (timer <= 0) {
                // Timer complete, trigger Action
                action();
                DestroySelf();
            }
        }
        private void DestroySelf() {
            RemoveTimer(this);
            if (gameObject != null) {
                UnityEngine.Object.Destroy(gameObject);
            }
        }




        /*
         * Class to trigger Actions manually without creating a GameObject
         * */
        public class FunctionTimerObject {

            private float timer;
            private Action callback;

            public FunctionTimerObject(Action callback, float timer) {
                this.callback = callback;
                this.timer = timer;
            }

            public void Update() {
                Update(Time.deltaTime);
            }
            public void Update(float deltaTime) {
                timer -= deltaTime;
                if (timer <= 0) {
                    callback();
                }
            }
        }

        // Create a Object that must be manually updated through Update();
        public static FunctionTimerObject CreateObject(Action callback, float timer) {
            return new FunctionTimerObject(callback, timer);
        }
    }
}