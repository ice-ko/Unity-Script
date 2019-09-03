/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this Code Monkey project
    I hope you find it useful in your own projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

namespace CM_TaskSystem {

    public class CM_WorkerTransporterTaskAI : MonoBehaviour {

        private enum State {
            WaitingForNextTask,
            ExecutingTask,
        }

        private CM_IWorker worker;
        private CM_TaskSystem<CM_GameHandler.TransporterTask> taskSystem;
        private State state;
        private float waitingTimer;

        public void Setup(CM_IWorker worker, CM_TaskSystem<CM_GameHandler.TransporterTask> taskSystem) {
            this.worker = worker;
            this.taskSystem = taskSystem;
            state = State.WaitingForNextTask;
        }

        private void Update() {
            switch (state) {
            case State.WaitingForNextTask:
                // Waiting to request a new task
                waitingTimer -= Time.deltaTime;
                if (waitingTimer <= 0) {
                    float waitingTimerMax = .2f; // 200ms
                    waitingTimer = waitingTimerMax;
                    RequestNextTask();
                }
                break;
            case State.ExecutingTask:
                // Currently executing a task
                break;
            }
        }

        private void RequestNextTask() {
            CMDebug.TextPopup("RequestNextTask", worker.GetPosition());
            CM_GameHandler.TransporterTask task = taskSystem.RequestNextTask();
            if (task == null) {
                // No tasks available, wait before asking again
                state = State.WaitingForNextTask;
            } else {
                // There is a task available, execute it depending on type
                state = State.ExecutingTask;
                if (task is CM_GameHandler.TransporterTask.TakeWeaponFromSlotToPosition) {
                    ExecuteTask_TakeWeaponFromSlotToPosition(task as CM_GameHandler.TransporterTask.TakeWeaponFromSlotToPosition);
                    return;
                }
                // Task type unknown, error!
                Debug.LogError("Task type unknown!");
            }
        }

        private void ExecuteTask_TakeWeaponFromSlotToPosition(CM_GameHandler.TransporterTask.TakeWeaponFromSlotToPosition takeWeaponFromSlotToPositionTask) {
            worker.MoveTo(takeWeaponFromSlotToPositionTask.weaponSlotPosition, () => {
                takeWeaponFromSlotToPositionTask.grabWeapon(this);
                worker.MoveTo(takeWeaponFromSlotToPositionTask.targetPosition, () => {
                    takeWeaponFromSlotToPositionTask.dropWeapon();
                    state = State.WaitingForNextTask;
                });
            });
        }
    }

}