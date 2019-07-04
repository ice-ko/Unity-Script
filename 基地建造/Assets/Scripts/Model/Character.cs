using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 人物信息
/// </summary>
public class Character
{
    public float X
    {
        get
        {
            return Mathf.Lerp(currTile.x, destTile.x, movementPercentage);
        }
    }
    public float Y
    {
        get
        {
            return Mathf.Lerp(currTile.y, destTile.y, movementPercentage);
        }
    }
    //当前
    public Tile currTile;
    //如果我们没有移动，那么destTile = currTile
    Tile destTile;
    //当我们从currTile转移到destTile时，从0到1
    float movementPercentage;

    float speed = 5f;

    Action<Character> cbCharacterChanged;

    Job myJob;
    public void Update(float deltaTime)
    {
        if (myJob == null)
        {
            //获取新的工作任务
            myJob = currTile.world.jobsQueue.Dequeue();
            if (myJob != null)
            {
                destTile = myJob.tile;
                myJob.RegisterJobCompleteCallback(OnJobEnded);
                myJob.RegisterJobCancelCallback(OnJobEnded);
            }
        }
        // 我们到了吗？
        if (currTile == destTile)
        {
            if (myJob != null)
            {
                myJob.DoWork(deltaTime);
            }
            return;
        }
        //从A点到B点的总距离是多少？
        //我们将使用欧几里德距离现在......
        //但是当我们进行寻路系统时，我们很可能会这样做
        //切换到曼哈顿或切比雪夫的距离
        float distToTravel = Mathf.Sqrt(
            Mathf.Pow(currTile.x - destTile.x, 2)
            + Mathf.Pow(currTile.y - destTile.y, 2)
            );

        // 这个更新可以运动多长时间？
        float distThisFrame = speed * deltaTime;

        // 到目的地的需要多久？
        float percThisFrame = distThisFrame / distToTravel;

        // 将其添加到旅行的总体百分比。
        movementPercentage += percThisFrame;

        if (movementPercentage >= 1)
        {
            // TODO：从寻路系统中获取下一个图块。
            //如果没有更多的瓷砖，那么我们就有了TRULY
            //到达目的地

            // 我们到达了目的地
            currTile = destTile;
            movementPercentage = 0;
            // 我们真的想保留任何超车运动吗？
        }
        if (cbCharacterChanged != null)
        {
            cbCharacterChanged(this);
        }
    }
    public Character(Tile tile)
    {
        currTile = destTile = tile;
    }
    /// <summary>
    /// 设置目的地
    /// </summary>
    /// <param name="tile"></param>
    public void SetDestination(Tile tile)
    {
        if (!currTile.IsNeighbour(tile, true))
        {
            return;
        }
    }
    /// <summary>
    /// 注册已创建的 Character
    /// </summary>
    /// <param name="cakkbackFunc"></param>
    public void RegisterCharacterCreated(Action<Character> cakkbackFunc)
    {
        cbCharacterChanged += cakkbackFunc;
    }
    /// <summary>
    ///注销已创建的 Character
    /// </summary>
    /// <param name="cakkbackFunc"></param>
    public void UnregisterCharacterCreated(Action<Character> cakkbackFunc)
    {
        cbCharacterChanged -= cakkbackFunc;
    }
    /// <summary>
    /// 工作结束
    /// </summary>
    void OnJobEnded(Job job)
    {
        if (job != myJob)
        {
            return;
        }
        myJob = null;
    }
}
