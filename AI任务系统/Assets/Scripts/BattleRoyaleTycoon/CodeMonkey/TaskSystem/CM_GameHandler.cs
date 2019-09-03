/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this Code Monkey project
    I hope you find it useful in your own projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

namespace CM_TaskSystem {

    public class CM_GameHandler : MonoBehaviour {
        
        [SerializeField] private Sprite floorShellsSprite;
        [SerializeField] private Sprite pistolSprite;
        [SerializeField] private Sprite whitePixelSprite;
        
        private CM_TaskSystem<Task> taskSystem;
        public static CM_TaskSystem<TransporterTask> transporterTaskSystem;

        private List<WeaponSlot> weaponSlotList;

        private void Start() {
            taskSystem = new CM_TaskSystem<Task>();
            transporterTaskSystem = new CM_TaskSystem<TransporterTask>();

            //CM_Worker worker = CM_Worker.Create(new Vector3(450, 500));
            //CM_WorkerTaskAI workerTaskAI = worker.gameObject.AddComponent<CM_WorkerTaskAI>();
            //workerTaskAI.Setup(worker, taskSystem);

            //worker = CM_Worker.Create(new Vector3(550, 500));
            //CM_WorkerTransporterTaskAI workerTransporterTaskAI = worker.gameObject.AddComponent<CM_WorkerTransporterTaskAI>();
            //workerTransporterTaskAI.Setup(worker, transporterTaskSystem);

            //weaponSlotList = new List<WeaponSlot>();
            //GameObject weaponSlotGameObject = SpawnWeaponSlot(new Vector3(500, 500));
            //weaponSlotList.Add(new WeaponSlot(weaponSlotGameObject.transform));
            
            //weaponSlotGameObject = SpawnWeaponSlot(new Vector3(500, 490));
            //weaponSlotList.Add(new WeaponSlot(weaponSlotGameObject.transform));
            
            //weaponSlotGameObject = SpawnWeaponSlot(new Vector3(500, 510));
            //weaponSlotList.Add(new WeaponSlot(weaponSlotGameObject.transform));
            /*
            GameObject pistolGameObject = SpawnPistolSprite(new Vector3(550, 500));
            weaponSlot.SetWeaponTransform(pistolGameObject.transform);
            */
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                // Spawn a pistol and queue the task to take it to a slot when possible
                GameObject pistolGameObject = SpawnPistolSprite(UtilsClass.GetMouseWorldPosition());
                taskSystem.EnqueueTask(() => {
                    foreach (WeaponSlot weaponSlot in weaponSlotList) {
                        if (weaponSlot.IsEmpty()) {
                            // If the weapon slot is empty lets create the task to take it there
                            weaponSlot.SetHasWeaponIncoming(true);
                            Task task = new Task.TakeWeaponToWeaponSlot {
                                weaponPosition = pistolGameObject.transform.position,
                                weaponSlotPosition = weaponSlot.GetPosition(),
                                grabWeapon = (CM_WorkerTaskAI weaponWorkerTaskAI) => {
                                    // Grab weapon, parent the weapon to the worker
                                    pistolGameObject.transform.SetParent(weaponWorkerTaskAI.transform);
                                },
                                dropWeapon = () => { 
                                    // Drop weapon, set parent back to null
                                    pistolGameObject.transform.SetParent(null);
                                    // Notify the weapon slot that the weapon has arrived
                                    weaponSlot.SetWeaponTransform(pistolGameObject.transform);
                                },
                            };
                            return task;
                        }
                        // Weapon slot not empty, keep looking
                    }
                    // No weapon slot is empty, try again later
                    return null;
                });
                //CMDebug.TextPopupMouse("Add Task: ShellFloorCleanUp, 5s delay");
                //SpawnFloorShellsWithTask(UtilsClass.GetMouseWorldPosition());
            }
            if (Input.GetMouseButtonDown(1)) {
                CMDebug.TextPopupMouse("Add Task: MoveToPosition");
                //CM_TaskSystem.Task task = new CM_TaskSystem.Task.Victory { };
                //taskSystem.AddTask(task);
                Task task = new Task.MoveToPosition { targetPosition = UtilsClass.GetMouseWorldPosition() };
                taskSystem.AddTask(task);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                CMDebug.TextPopupMouse("Add Task: Victory");
                Task task = new Task.Victory { };
                taskSystem.AddTask(task);
            }
        }


        private GameObject SpawnFloorShells(Vector3 position) {
            GameObject gameObject = new GameObject("FloorShells", typeof(SpriteRenderer));
            gameObject.GetComponent<SpriteRenderer>().sprite = floorShellsSprite;
            gameObject.transform.position = position;
            return gameObject;
        }

        private void SpawnFloorShellsWithTask(Vector3 position) {
            GameObject floorShellsGameObject = SpawnFloorShells(position);
            SpriteRenderer floorShellsSpriteRenderer = floorShellsGameObject.GetComponent<SpriteRenderer>();

            float cleanUpTime = Time.time + 5f;
            taskSystem.EnqueueTask(() => {
                if (Time.time >= cleanUpTime) {
                    Task task = new Task.ShellFloorCleanUp {
                        targetPosition = floorShellsGameObject.transform.position,
                        cleanUpAction = () => {
                            // Clean Up Action, reduce alpha every frame until zero
                            float alpha = 1f;
                            FunctionUpdater.Create(() => {
                                alpha -= Time.deltaTime;
                                floorShellsSpriteRenderer.color = new Color(1, 1, 1, alpha);
                                if (alpha <= 0f) {
                                    return true;
                                } else {
                                    return false;
                                }
                            });
                        }
                    };
                    return task;
                } else {
                    return null;
                }
            });
        }

        private GameObject SpawnPistolSprite(Vector3 position) {
            GameObject gameObject = new GameObject("PistolSprite", typeof(SpriteRenderer));
            gameObject.GetComponent<SpriteRenderer>().sprite = pistolSprite;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 10000;
            gameObject.transform.position = position;
            gameObject.transform.localScale = new Vector3(7, 7);
            return gameObject;
        }

        private GameObject SpawnWeaponSlot(Vector3 position) {
            GameObject gameObject = new GameObject("WeaponSlot", typeof(SpriteRenderer));
            gameObject.GetComponent<SpriteRenderer>().sprite = whitePixelSprite;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(.5f , .5f, .5f);
            gameObject.transform.position = position;
            gameObject.transform.localScale = new Vector3(4, 4);
            return gameObject;
        }


        private class WeaponSlot {

            private Transform weaponSlotTransform;
            private Transform weaponTransform;
            private bool hasWeaponIncoming;

            public WeaponSlot(Transform weaponSlotTransform) {
                this.weaponSlotTransform = weaponSlotTransform;
                SetWeaponTransform(null);
            }

            public bool IsEmpty() {
                return weaponTransform == null && !hasWeaponIncoming;
            }

            public void SetHasWeaponIncoming(bool hasWeaponIncoming) {
                this.hasWeaponIncoming = hasWeaponIncoming;
                UpdateSprite();
            }

            public void SetWeaponTransform(Transform weaponTransform) {
                this.weaponTransform = weaponTransform;
                hasWeaponIncoming = false;
                UpdateSprite();

                if (weaponTransform != null) {
                    TransporterTask.TakeWeaponFromSlotToPosition task = new TransporterTask.TakeWeaponFromSlotToPosition { 
                        weaponSlotPosition = GetPosition(),
                        targetPosition = new Vector3(600, 500),
                        grabWeapon = (CM_WorkerTransporterTaskAI weaponWorkerTaskAI) => {
                            // Grab weapon, parent the weapon to the worker
                            weaponTransform.SetParent(weaponWorkerTaskAI.transform);
                            SetWeaponTransform(null);
                        },
                        dropWeapon = () => { 
                            // Drop weapon, set parent back to null
                            weaponTransform.SetParent(null);
                        },
                    };
                    transporterTaskSystem.AddTask(task);
                }
                /*FunctionTimer.Create(() => {
                    if (weaponTransform != null) {
                        Destroy(weaponTransform.gameObject);
                        SetWeaponTransform(null);
                    }
                }, 4f);*/
            }

            public Vector3 GetPosition() {
                return weaponSlotTransform.position;
            }

            public void UpdateSprite() {
                weaponSlotTransform.GetComponent<SpriteRenderer>().color = IsEmpty() ? Color.grey : Color.red;
            }

        }

        public class Task : TaskBase {

            // Worker moves to Target Position
            public class MoveToPosition : Task {
                public Vector3 targetPosition;
            }

            // Workers plays Victory animation
            public class Victory : Task {
            }

            // Worker moves to target position, plays clean up animation, and executes clean up action
            public class ShellFloorCleanUp : Task {
                public Vector3 targetPosition;
                public Action cleanUpAction;
            }

            // Worker moves to weapon position, grabs the weapon, takes it to weapon slot, drops weapon
            public class TakeWeaponToWeaponSlot : Task {
                public Vector3 weaponPosition;
                public Action<CM_WorkerTaskAI> grabWeapon;
                public Vector3 weaponSlotPosition;
                public Action dropWeapon;
            }

        }

        public class TransporterTask : TaskBase {

            public class TakeWeaponFromSlotToPosition : TransporterTask {
                public Vector3 weaponSlotPosition;
                public Vector3 targetPosition;
                public Action<CM_WorkerTransporterTaskAI> grabWeapon;
                public Action dropWeapon;
            }

        }
    }

}