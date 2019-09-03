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

    public class CM_WorkerTaskAI : MonoBehaviour {

        private enum State {
            WaitingForNextTask,
            ExecutingTask,
        }

        private CM_IWorker worker;
        private CM_TaskSystem<CM_GameHandler.Task> taskSystem;
        private State state;
        private float waitingTimer;

        public void Setup(CM_IWorker worker, CM_TaskSystem<CM_GameHandler.Task> taskSystem) {
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
            CM_GameHandler.Task task = taskSystem.RequestNextTask();
            if (task == null) {
                // No tasks available, wait before asking again
                state = State.WaitingForNextTask;
            } else {
                // There is a task available, execute it depending on type
                state = State.ExecutingTask;
                if (task is CM_GameHandler.Task.MoveToPosition) {
                    ExecuteTask_MoveToPosition(task as CM_GameHandler.Task.MoveToPosition);
                    return;
                }
                if (task is CM_GameHandler.Task.Victory) {
                    ExecuteTask_Victory(task as CM_GameHandler.Task.Victory);
                    return;
                }
                if (task is CM_GameHandler.Task.ShellFloorCleanUp) {
                    ExecuteTask_ShellFloorCleanUp(task as CM_GameHandler.Task.ShellFloorCleanUp);
                    return;
                }
                if (task is CM_GameHandler.Task.TakeWeaponToWeaponSlot) {
                    ExecuteTask_TakeWeaponToWeaponSlot(task as CM_GameHandler.Task.TakeWeaponToWeaponSlot);
                    return;
                }
                // Task type unknown, error!
                Debug.LogError("Task type unknown!");
            }
        }

        private void ExecuteTask_MoveToPosition(CM_GameHandler.Task.MoveToPosition moveToPositionTask) {
            // Move Worker to target position
            CMDebug.TextPopup("ExecuteTask_MoveToPosition", worker.GetPosition());
            worker.MoveTo(moveToPositionTask.targetPosition, () => {
                state = State.WaitingForNextTask;
            });
        }

        private void ExecuteTask_Victory(CM_GameHandler.Task.Victory victoryTask) {
            // Play Victory animation
            CMDebug.TextPopup("ExecuteTask_Victory", worker.GetPosition());
            worker.PlayVictoryAnimation(() => { 
                state = State.WaitingForNextTask;
            });
        }

        private void ExecuteTask_ShellFloorCleanUp(CM_GameHandler.Task.ShellFloorCleanUp shellFloorCleanUpTask) {
            // Clean up shells on floor
            worker.MoveTo(shellFloorCleanUpTask.targetPosition, () => {
                worker.PlayCleanUpAnimation(() => {
                    shellFloorCleanUpTask.cleanUpAction();
                    state = State.WaitingForNextTask;
                });
            });
        }

        private void ExecuteTask_TakeWeaponToWeaponSlot(CM_GameHandler.Task.TakeWeaponToWeaponSlot takeWeaponToWeaponSlotTask) {
            worker.MoveTo(takeWeaponToWeaponSlotTask.weaponPosition, () => {
                takeWeaponToWeaponSlotTask.grabWeapon(this);
                worker.MoveTo(takeWeaponToWeaponSlotTask.weaponSlotPosition, () => {
                    takeWeaponToWeaponSlotTask.dropWeapon();
                    state = State.WaitingForNextTask;
                });
            });
        }
    }

}