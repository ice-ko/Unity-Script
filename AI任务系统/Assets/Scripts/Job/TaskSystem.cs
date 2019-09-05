using CodeMonkey;
using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
public abstract class TaskBase
{
  
}
public class TaskSystem<T> where T : TaskBase
{



    private List<T> taskList;
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
    public void EnqueueTask(QueuedTask<T> queuedTask)
    {
        queuedTaskList.Add(queuedTask);
    }
    public void EnqueueTask(Func<T> getTaskFunc)
    {
        QueuedTask<T> queuedTask = new QueuedTask<T>(getTaskFunc);
        queuedTaskList.Add(queuedTask);
    }
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
                CMDebug.TextPopupMouse("Task Dequeue");
            }
        }
    }
}
