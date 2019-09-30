using CodeMonkey;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerTransporterTaskAI : MonoBehaviour
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
    private IWorker worker;
    private State state;
    /// <summary>
    /// 等待时间
    /// </summary>
    private float waitingTimer;
    private TaskSystem<TransporterTask> taskSystem;
    public void Setup(IWorker worker, TaskSystem<TransporterTask> taskSystem)
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
        TransporterTask task = taskSystem.RequestNextTask();
        if (task == null)
        {
            state = State.WaitingForNextTask;
        }
        else
        {
            state = State.ExecutingTask;
            if (task is TransporterTask.TakeWeaponFromSlotToPosition)
            {
                ExecuteTask_TaskWeaponToWeaponSlot(task as TransporterTask.TakeWeaponFromSlotToPosition);
                return;
            }
            
        }
    }

    private void ExecuteTask_TaskWeaponToWeaponSlot(TransporterTask.TakeWeaponFromSlotToPosition task)
    {
        worker.MoveTo(task.weaponPosition, () =>
        {
            task.grabWeapon(this);
            worker.MoveTo(task.targetPosition, () =>
            {
                task.dropWeapon();
                state = State.WaitingForNextTask;
            });
        });
    }
}
