using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 移动到目标位置
/// </summary>
public class MoveTargetPosition : SingleObject<MoveTargetPosition>
{
    #region 寻路   
    public Tilemap tilemap;
    public AStarTilemap astar;
    //移动速度
    public float speed;
    //路径节点
    private Stack<Vector3> path;
    //目的地
    private Vector3 destination;

    #endregion

    private Action onArrivedAtPosition;

    void Start()
    {

    }


    void Update()
    {
        ClickToMove();
        //设置角色限制
        transform.position = astar.SetLimits(transform);
    }
    /// <summary>
    /// 获取路径
    /// </summary>
    /// <param name="goal">目标位置（鼠标点击的位置坐标）.</param>
    public void GetPath(Vector3 goal, Action onArrivedAtPosition)
    {
        path = astar.Algorithm(transform.position, goal);
        if (path != null && path.Count > 0)
        {
            destination = path.Pop();
            this.onArrivedAtPosition = onArrivedAtPosition;
        }
        else
        {
            SimplePool.Despawn(WorkerTaskAI.Instance.currentTask.PrefabGame);
            TaskHandler.Instance.taskSystem.EnqueueTask(() =>
            {
                return WorkerTaskAI.Instance.currentTask; 
            });
        }
    }
    /// <summary>
    /// 点击移动到指定位置
    /// </summary>
    public void ClickToMove()
    {
        if (path != null)
        {
            //移向目标
            transform.parent.position = Vector2.MoveTowards(transform.parent.position, destination, speed * Time.deltaTime);
            //计算距离
            float distance = Vector2.Distance(destination, transform.parent.position);
            if (distance <= 0f)
            {
                if (path.Count > 1)
                {
                    destination = path.Pop();
                }
                else
                {
                    path = null;
                    onArrivedAtPosition?.Invoke();
                }
            }
        }
    }
}
