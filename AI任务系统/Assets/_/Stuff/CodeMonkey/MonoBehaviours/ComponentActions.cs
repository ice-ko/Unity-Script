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

namespace CodeMonkey.MonoBehaviours {

    /*
     * Trigger Actions on MonoBehaviour Component events
     * */
    public class ComponentActions : MonoBehaviour {

        public Action OnDestroyFunc;
        public Action OnEnableFunc;
        public Action OnDisableFunc;
        public Action OnUpdate;

        void OnDestroy() {
            if (OnDestroyFunc != null) OnDestroyFunc();
        }
        void OnEnable() {
            if (OnEnableFunc != null) OnEnableFunc();
        }
        void OnDisable() {
            if (OnDisableFunc != null) OnDisableFunc();
        }
        void Update() {
            if (OnUpdate != null) OnUpdate();
        }


        public static void CreateComponent(Action OnDestroyFunc = null, Action OnEnableFunc = null, Action OnDisableFunc = null, Action OnUpdate = null) {
            GameObject gameObject = new GameObject("ComponentActions");
            AddComponent(gameObject, OnDestroyFunc, OnEnableFunc, OnDisableFunc, OnUpdate);
        }
        public static void AddComponent(GameObject gameObject, Action OnDestroyFunc = null, Action OnEnableFunc = null, Action OnDisableFunc = null, Action OnUpdate = null) {
            ComponentActions componentFuncs = gameObject.AddComponent<ComponentActions>();
            componentFuncs.OnDestroyFunc = OnDestroyFunc;
            componentFuncs.OnEnableFunc = OnEnableFunc;
            componentFuncs.OnDisableFunc = OnDisableFunc;
            componentFuncs.OnUpdate = OnUpdate;
        }
    }

}