using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_ObjectSystem {

    public class V_ObjectTransform : V_IObjectTransform, V_IDestroySelf {

        private GameObject gameObject;
        private Transform transform;

        public V_ObjectTransform(Transform transform) {
            this.transform = transform;
            gameObject = transform.gameObject;
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
            Object.Destroy(gameObject);
        }
    }


}