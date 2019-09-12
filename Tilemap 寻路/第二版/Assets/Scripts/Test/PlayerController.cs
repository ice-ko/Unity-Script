using CodeMonkey;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    #region 单例
    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerController>();
            }
            return instance;
        }
    }
    #endregion
    #region 寻路   
    public Tilemap tilemap;
    public AStarTilemap astar;
    public float speed;
    private Stack<Vector3> path;
    //目的地
    private Vector3 destination;
    #endregion



    void Start()
    {

    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0)&& !TileController.Instance.IsBuild())
        {
            var pos = tilemap.WorldToCell(UtilityClass.GetMouseWorldPos());
            Debug.Log("点击" + pos);
            if (tilemap.GetTile(pos) != null)
            {
                GetPath(UtilityClass.GetMouseWorldPos());
            }
        }
        ClickToMove();
        //设置角色限制
        transform.position = astar.SetLimits(transform);
    }
    /// <summary>
    /// 获取路径
    /// </summary>
    /// <param name="goal">目标位置（鼠标点击的位置坐标）.</param>
    public void GetPath(Vector3 goal)
    {
        path = astar.Algorithm(transform.position, goal);
        destination = path.Pop();
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
                if (path.Count > 0)
                {
                    destination = path.Pop();
                }
                else
                {
                    path = null;
                }
            }
        }

    }
}
