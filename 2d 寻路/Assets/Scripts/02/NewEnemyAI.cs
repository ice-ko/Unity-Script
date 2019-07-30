using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NewEnemyAI : MonoBehaviour
{
    //目标
    public Transform target;
    //速度
    public float speed = 200f;
    //下一个导航距离
    public float nextWaypointDistance = 3f;

    //路径
    Path path;
    //当前导航节点
    int currentWaypoint = 0;
    //是否到达导航节点
    bool reachedEndOfPath = false;

    //引导组件
    Seeker seeker;
    Rigidbody2D rb;

    private void Start()
    {
        // seeker = gameObject.AddComponent<Seeker>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        //开始路径  
        InvokeRepeating("UpdatePath", 0f, 0.5f);

    }
    /// <summary>
    /// 在路径完成的时候
    /// </summary>
    void OnPathComplete(Path path)
    {
        if (!path.error)
        {
            this.path = path;
            currentWaypoint = 0;
        }
    }
    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }
        //如果当前路径点大于路径数量 则到达目标点
        if (currentWaypoint >= path.vectorPath.Count)
        {
            //到达目标点
            reachedEndOfPath = true;
            return;
        }
        else
        {
            //还为到达目标点
            reachedEndOfPath = false;
        }
        //方向 当前路径节点减去目标位置
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);
        //距离
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        //如果当前距离小于下一个导航节点 继续下一个导航节点
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (force.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (force.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }
}
