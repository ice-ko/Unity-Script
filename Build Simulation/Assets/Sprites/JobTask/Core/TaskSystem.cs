using CodeMonkey;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 任务队列
/// </summary>
/// <typeparam name="T"></typeparam>
public class QueuedTask<T> where T : TaskBase
{
    private Func<T> getTaskFunc;
    public QueuedTask(Func<T> getTaskFunc)
    {
        this.getTaskFunc = getTaskFunc;
    }
    public T DequeueTask()
    {
        return getTaskFunc();
    }
}
/// <summary>
/// 任务基类
/// </summary>
public abstract class TaskBase
{
    /// <summary>
    /// 任务名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 目标位置
    /// </summary>
    public Vector3 TargetPosition { get; set; }
    /// <summary>
    /// 预制游戏对象
    /// </summary>
    public GameObject PrefabGame { get; set; }
}
/// <summary>
/// 任务系统
/// </summary>
/// <typeparam name="T"></typeparam>
public class TaskSystem<T> where T : TaskBase
{
    /// <summary>
    /// 任务列表
    /// </summary>
    private List<T> taskList;
    /// <summary>
    /// 任务队列列表
    /// </summary>
    private List<QueuedTask<T>> queuedTaskList;
    public TaskSystem()
    {
        taskList = new List<T>();
        queuedTaskList = new List<QueuedTask<T>>();
        FunctionPeriodic.Create(DequeueTask, 0.2f);
    }
    /// <summary>
    /// 请求下一个任务。
    /// </summary>
    /// <returns></returns>
    public T RequestNextTask()
    {
        if (taskList.Count > 0)
        {
            T task = taskList[0];
            taskList.RemoveAt(0);
            return task;
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 添加任务
    /// </summary>
    /// <param name="task">The task.</param>
    public void AddTask(T task)
    {
        taskList.Add(task);
    }
    /// <summary>
    /// 入列任务
    /// </summary>
    /// <param name="queuedTask"></param>
    public void EnqueueTask(QueuedTask<T> queuedTask)
    {
        queuedTaskList.Add(queuedTask);
    }
    /// <summary>
    /// 入列任务(检测任务是否已存在，存在返回true)
    /// </summary>
    /// <param name="getTaskFunc"></param>
    public bool EnqueueTask(Func<T> getTaskFunc)
    {
        QueuedTask<T> queuedTask = new QueuedTask<T>(getTaskFunc);
        if (!queuedTaskList.Contains(queuedTask))
        {
            queuedTaskList.Add(queuedTask);
            return false;
        }
        else
        {
            return true;
        }
    }
    /// <summary>
    /// 出列任务
    /// </summary>
    public void DequeueTask()
    {
        for (int i = 0; i < queuedTaskList.Count; i++)
        {
            QueuedTask<T> queuedTask = queuedTaskList[i];
            T task = queuedTask.DequeueTask();
            if (task != null)
            {
                AddTask(task);
                queuedTaskList.RemoveAt(i);
                i--;
            }
        }
    }
}
