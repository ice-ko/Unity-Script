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
            return Mathf.Lerp(currTile.x, nextTile.x, movementPercentage);
        }
    }
    public float Y
    {
        get
        {
            return Mathf.Lerp(currTile.y, nextTile.y, movementPercentage);
        }
    }
    //当前
    public Tile currTile;
    //如果我们没有移动，那么destTile = currTile
    Tile destTile;
    //下一个寻路点
    Tile nextTile;
    //A*寻路
    Path_AStar path_AStar;
    //当我们从currTile转移到destTile时，从0到1
    float movementPercentage;

    float speed = 5f;

    Action<Character> cbCharacterChanged;

    Job myJob;
    public void Update(float deltaTime)
    {
        Update_DoJob(deltaTime);
        Update_DoMovement(deltaTime);



    }
    /// <summary>
    /// 更新任务
    /// </summary>
    /// <param name="deltaTime"></param>
    /// <returns></returns>
    void Update_DoJob(float deltaTime)
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
        //if (path_AStar != null && path_AStar.Length() == 1)
        {
            if (myJob != null)
            {
                myJob.DoWork(deltaTime);
            }
        }
    }
    /// <summary>
    /// 更新移动
    /// </summary>
    /// <param name="deltaTime"></param>
    void Update_DoMovement(float deltaTime)
    {
        if (currTile == destTile)
        {
            path_AStar = null;
            return;
        }
        if (nextTile == null || nextTile == currTile)
        {
            //获取下一个寻路点
            if (path_AStar == null || path_AStar.Length() == 0)
            {
                //生存到目的地寻路
                path_AStar = new Path_AStar(currTile.world, currTile, destTile);
                if (path_AStar.Length() == 0)
                {
                    AbandonJob();
                    path_AStar = null;
                    return;
                }
            }
            //获取下一个路径tile
            nextTile = path_AStar.Dequeue();

        }
        //距离目标还是一个路径节点时返回
        //if (path_AStar.Length() == 1)
        //{
        //    return;
        //}
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

        // 
        movementPercentage += percThisFrame;

        if (movementPercentage >= 1)
        {
            // TODO：从寻路系统中获取下一个图块。
            //如果没有更多的瓷砖，那么我们就有了TRULY
            //到达目的地

            // 我们到达了目的地
            currTile = nextTile;
            movementPercentage = 0;
            // 我们真的想保留任何超车运动吗？
        }
        if (cbCharacterChanged != null)
        {
            cbCharacterChanged(this);
        }
    }
    public void AbandonJob()
    {
        nextTile = destTile = currTile;
        path_AStar = null;
        currTile.world.jobsQueue.Enqueue(myJob);
        myJob = null;
    }
    public Character(Tile tile)
    {
        currTile = destTile = nextTile = tile;
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
