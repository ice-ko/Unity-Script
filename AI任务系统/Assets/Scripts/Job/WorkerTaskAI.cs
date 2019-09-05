using CodeMonkey;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerTaskAI : MonoBehaviour
{
    public enum State
    {
        /// <summary>
        /// 等待下一个任务
        /// </summary>
        WaitingForNextTask,
        /// <summary>
        /// 执行任务
        /// </summary>
        ExecutingTask,
    }
    IWorker worker;
    public State state;
    /// <summary>
    /// 等待时间
    /// </summary>
    private float waitingTimer;
    private TaskSystem<Task> taskSystem;
    public void Setup(IWorker worker, TaskSystem<Task> taskSystem)
    {
        this.worker = worker;
        this.taskSystem = taskSystem;
        state = State.WaitingForNextTask;
    }
    private void Update()
    {
        switch (state)
        {
            case State.WaitingForNextTask:
                waitingTimer -= Time.deltaTime;
                if (waitingTimer <= 0)
                {
                    float waitingTimerMax = .2f;
                    waitingTimer = waitingTimerMax;
                    RequestNextTask();
                }
                break;
            case State.ExecutingTask:

                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 请求下一个任务
    /// </summary>
    private void RequestNextTask()
    {
        CMDebug.TextPopup("请求下一个任务", transform.position);
        TaskBase task = taskSystem.RequestNextTask();
        if (task == null)
        {
            state = State.WaitingForNextTask;
        }
        else
        {
            state = State.ExecutingTask;
            if (task is Task.MoveToPosition)
            {
                ExecuteTask_MoveToPosition(task as Task.MoveToPosition);
                return;
            }
            if (task is Task.Victory)
            {
                ExecuteTask_Victory(task as Task.Victory);
                return;
            }
            if (task is Task.ShellFloorCleanUp)
            {
                ExecuteTask_ShellFloorCleanUp(task as Task.ShellFloorCleanUp);
                return;
            }
            if (task is Task.TaskWeaponToWeaponSlot)
            {
                ExecuteTask_TaskWeaponToWeaponSlot(task as Task.TaskWeaponToWeaponSlot);
                return;
            }
        }
    }

    private void ExecuteTask_TaskWeaponToWeaponSlot(Task.TaskWeaponToWeaponSlot task)
    {
        worker.MoveTo(task.weaponPosition, () =>
        {
            task.grabWeapon(this);
            worker.MoveTo(task.weaponSlotPosition, () =>
            {
                 task.dropWeapon();
                 state = State.WaitingForNextTask;
             });
        });
    }

    /// <summary>
    /// 执行任务_移动到指定位置
    /// </summary>
    /// <param name="task">The task.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    private void ExecuteTask_MoveToPosition(Task.MoveToPosition task)
    {
        CMDebug.TextPopup("ExecuteTask_MoveToPosition", transform.position);
        worker.MoveTo(task.targetPosition, () =>
        {
            state = State.WaitingForNextTask;
            // Destroy(gameObject.GetComponent<MoveHandler>());
        });
    }
    private void ExecuteTask_Victory(Task.Victory task)
    {
        CMDebug.TextPopup("ExecuteTask_Victory", transform.position);
        worker.PlayVictoryAnimation(() =>
        {
            state = State.WaitingForNextTask;
        });
    }
    /// <summary>
    /// 执行清理任务
    /// </summary>
    /// <param name="task"></param>
    private void ExecuteTask_ShellFloorCleanUp(Task.ShellFloorCleanUp task)
    {
        worker.MoveTo(task.targetPosition, () =>
        {
            worker.PlayCleanUpAnimation(() =>
            {
                task.cleanUpAction();
                state = State.WaitingForNextTask;
            });
        });
    }

}
