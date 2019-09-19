using CodeMonkey;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 工作任务AI
/// </summary>
public class WorkerTaskAI : SingleObject<WorkerTaskAI>
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
    /// <summary>
    /// 当前状态
    /// </summary>
    public State state;
    /// <summary>
    /// 当前任务
    /// </summary>
    public Task currentTask;
    /// <summary>
    /// 等待时间
    /// </summary>
    private float waitingTimer;
    /// <summary>
    /// 任务系统
    /// </summary>
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
        //CMDebug.TextPopup("请求下一个任务", transform.position);
        TaskBase task = taskSystem.RequestNextTask();
        if (task == null)
        {
            state = State.WaitingForNextTask;
        }
        else
        {
            state = State.ExecutingTask;
            if (task is MiningTask.Excavate)
            {
                ExecuteTask_ShellFloorCleanUp(task as MiningTask.Excavate);
                return;
            }

        }
    }
    /// <summary>
    /// 执行清理任务
    /// </summary>
    /// <param name="task"></param>
    private void ExecuteTask_ShellFloorCleanUp(MiningTask.Excavate task)
    {
        currentTask = task;
        worker.MoveTo(task.TargetPosition, () =>
        {
            task.ExcavateAction(task);
        });
    }
    /// <summary>
    /// 完成工作更新任务状态
    /// </summary>
    public void FinishTheWork()
    {
        state = State.WaitingForNextTask;
    }
}
