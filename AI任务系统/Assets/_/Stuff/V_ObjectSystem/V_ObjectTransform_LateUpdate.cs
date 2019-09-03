using System;
using UnityEngine;

namespace V_ObjectSystem {

    // Reference to Unity Transform, auto updates position on LateUpdate
    public class V_ObjectTransform_LateUpdate : V_IObjectTransform, V_IDestroySelf {

        private GameObject gameObject;
        private Transform transform;
        private Func<Vector3> GetPositionFunc;
        private Action OnDestroySelf;

        public V_ObjectTransform_LateUpdate(Transform transform, Func<Vector3> GetPositionFunc, V_Main.DelRegisterOnLateUpdate RegisterOnLateUpdate, V_Main.DelRegisterOnLateUpdate DeregisterOnLateUpdate) {
            this.transform = transform;
            this.GetPositionFunc = GetPositionFunc;
            gameObject = transform.gameObject;

            RegisterOnLateUpdate(LateUpdate, V_Main.UpdateType.Main);

            OnDestroySelf = () => DeregisterOnLateUpdate(LateUpdate, V_Main.UpdateType.Main);
        }
        private void LateUpdate(float deltaTime) {
            SetPosition(GetPositionFunc());
        }

        public void SetPosition(Vector3 position) {
            transform.position = position;
        }
        public Vector3 GetPosition() {
            return transform.position;
        }
        public void SetScale(Vector3 scale) {
            transform.localScale = scale;
        }
        public Vector3 GetScale() {
            return transform.localScale;
        }
        public void SetEuler(Vector3 euler) {
            transform.localEulerAngles = euler;
        }
        public Vector3 GetEuler() {
            return transform.localEulerAngles;
        }
        public void SetEulerZ(float eulerZ) {
            Vector3 euler = transform.localEulerAngles;
            euler.z = eulerZ;
            transform.localEulerAngles = euler;
        }
        public float GetEulerZ() {
            return transform.localEulerAngles.z;
        }


        public Transform GetTransform() {
            return transform;
        }



        public void DestroySelf() {
            if (OnDestroySelf != null) OnDestroySelf();
            UnityEngine.Object.Destroy(gameObject);
        }
    }


}