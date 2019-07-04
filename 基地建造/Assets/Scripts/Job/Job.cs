using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 任务中心
/// 此类包含排队作业的信息，其中包括
/// 放置家具，移动存储的库存，
/// 在办公桌前工作，甚至可能与敌人作战
/// </summary>
public class Job
{
    public Tile tile { get; protected set; }
    float jobTime;
    //工作完成
    Action<Job> cbJobComplete;
    //取消工作
    Action<Job> cbJobCancel;
    //
    public string jobObjectType;

    public Job(Tile tile, string jobObjectType, Action<Job> cbJobComplete, float jobTime = 1f)
    {
        this.tile = tile;
        this.cbJobComplete += cbJobComplete;
        this.jobObjectType = jobObjectType;
        this.jobTime = jobTime;
    }
   
    /// <summary>
    /// 做工作
    /// </summary>
    /// <param name="workTime"></param>
    public void DoWork(float workTime)
    {
        jobTime -= workTime;

        if (jobTime <= 0)
        {
            if (cbJobComplete != null)
                cbJobComplete(this);
        }
    }
    /// <summary>
    /// 取消工作
    /// </summary>
    public void CancelJob()
    {
        if (cbJobCancel != null)
            cbJobCancel(this);
    }
    /// <summary>
    /// 注册任务完成回调
    /// </summary>
    /// <param name="cb"></param>
    public void RegisterJobCompleteCallback(Action<Job> cb)
    {
        cbJobComplete += cb;
    }

    /// <summary>
    /// 注册任务完取消回调
    /// </summary>
    /// <param name="cb"></param>
    public void RegisterJobCancelCallback(Action<Job> cb)
    {
        cbJobCancel += cb;
    }
    /// <summary>
    /// 注销任务完成回调
    /// </summary>
    /// <param name="cb"></param>
    public void UnregisterJobCompleteCallback(Action<Job> cb)
    {
        cbJobComplete += cb;
    }

    /// <summary>
    /// 注销任务完取消回调
    /// </summary>
    /// <param name="cb"></param>
    public void UnregisterJobCancelCallback(Action<Job> cb)
    {
        cbJobCancel += cb;
    }
}
