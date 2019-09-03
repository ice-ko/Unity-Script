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
using System.Collections.Generic;
using UnityEngine;

namespace CodeMonkey.MonoBehaviours {

    /*
     * Script to handle Camera Movement and Zoom
     * Place on Camera GameObject
     * */
    public class CameraFollow : MonoBehaviour {

        private Camera myCamera;
        private Func<Vector3> GetCameraFollowPositionFunc;
        private Func<float> GetCameraZoomFunc;

        public void Setup(Func<Vector3> GetCameraFollowPositionFunc, Func<float> GetCameraZoomFunc) {
            this.GetCameraFollowPositionFunc = GetCameraFollowPositionFunc;
            this.GetCameraZoomFunc = GetCameraZoomFunc;
        }

        private void Start() {
            myCamera = transform.GetComponent<Camera>();
        }

        public void SetCameraFollowPosition(Vector3 cameraFollowPosition) {
            SetGetCameraFollowPositionFunc(() => cameraFollowPosition);
        }

        public void SetGetCameraFollowPositionFunc(Func<Vector3> GetCameraFollowPositionFunc) {
            this.GetCameraFollowPositionFunc = GetCameraFollowPositionFunc;
        }

        public void SetCameraZoom(float cameraZoom) {
            SetGetCameraZoomFunc(() => cameraZoom);
        }

        public void SetGetCameraZoomFunc(Func<float> GetCameraZoomFunc) {
            this.GetCameraZoomFunc = GetCameraZoomFunc;
        }


        // Update is called once per frame
        void Update() {
            HandleMovement();
            HandleZoom();
        }

        private void HandleMovement() {
            if (GetCameraFollowPositionFunc == null) return;
            Vector3 cameraFollowPosition = GetCameraFollowPositionFunc();
            cameraFollowPosition.z = transform.position.z;

            Vector3 cameraMoveDir = (cameraFollowPosition - transform.position).normalized;
            float distance = Vector3.Distance(cameraFollowPosition, transform.position);
            float cameraMoveSpeed = 3f;

            if (distance > 0) {
                Vector3 newCameraPosition = transform.position + cameraMoveDir * distance * cameraMoveSpeed * Time.deltaTime;

                float distanceAfterMoving = Vector3.Distance(newCameraPosition, cameraFollowPosition);

                if (distanceAfterMoving > distance) {
                    // Overshot the target
                    newCameraPosition = cameraFollowPosition;
                }

                transform.position = newCameraPosition;
            }
        }

        private void HandleZoom() {
            if (GetCameraZoomFunc == null) return;
            float cameraZoom = GetCameraZoomFunc();

            float cameraZoomDifference = cameraZoom - myCamera.orthographicSize;
            float cameraZoomSpeed = 1f;

            myCamera.orthographicSize += cameraZoomDifference * cameraZoomSpeed * Time.deltaTime;

            if (cameraZoomDifference > 0) {
                if (myCamera.orthographicSize > cameraZoom) {
                    myCamera.orthographicSize = cameraZoom;
                }
            } else {
                if (myCamera.orthographicSize < cameraZoom) {
                    myCamera.orthographicSize = cameraZoom;
                }
            }
        }
    }

}