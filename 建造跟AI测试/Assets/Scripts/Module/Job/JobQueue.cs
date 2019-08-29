using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 任务队列
/// </summary>
public class JobQueue
{
    //任务队列
    Queue<Job> jobsQueue;
    //任务创建委托
    Action<Job> cbJobCreated;
    public JobQueue()
    {
        jobsQueue = new Queue<Job>();
    }
    /// <summary>
    /// 加入队列
    /// </summary>
    /// <param name="job"></param>
    public void Enqueue(Job job)
    {
        jobsQueue.Enqueue(job);
        if (cbJobCreated != null)
        {
            cbJobCreated(job);
        }
    }
    /// <summary>
    /// 出列
    /// </summary>
    /// <returns></returns>
    public Job Dequeue()
    {
        if (jobsQueue.Count == 0)
        {
            return null;
        }
        return jobsQueue.Dequeue();
    }
    /// <summary>
    /// 注册任务创建的回调
    /// </summary>
    /// <param name="action"></param>
    public void RegisterJobCreatedCallback(Action<Job> action)
    {
        cbJobCreated += action;
    }
    /// <summary>
    /// 注销任务创建的回调
    /// </summary>
    /// <param name="action"></param>
    public void UnregisterJobCreatedCallback(Action<Job> action)
    {
        cbJobCreated -= action;
    }
}
