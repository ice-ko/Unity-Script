using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

/// <summary>
/// Tilemap A 星寻路
/// </summary>
public class AStarTilemap : MonoBehaviour
{
    #region 单例
    private static AStarTilemap instance;
    public static AStarTilemap Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AStarTilemap>();
            }
            return instance;
        }
    }
    #endregion        
    /// <summary>
    /// tilemap 地面图层
    /// </summary>
    public Tilemap tilemap;

    private Node current;

    private Stack<Vector3> path;

    private HashSet<Node> openList;

    private HashSet<Node> closedList;

    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

    private static HashSet<Vector3Int> noDiagonalTiles = new HashSet<Vector3Int>();
    /// <summary>
    /// 障碍列表（水、栅栏等）
    /// </summary>
    [HideInInspector]
    public List<Vector3Int> waterTiles = new List<Vector3Int>();


    private Vector3Int startPos, goalPos;
    /// <summary>
    /// tilemap大小
    /// </summary>
    private Vector3 min, max;
    public Tilemap MyTilemap
    {
        get
        {
            return tilemap;
        }
    }

    public static HashSet<Vector3Int> NoDiagonalTiles
    {
        get
        {
            return noDiagonalTiles;
        }
    }

    private void Start()
    {
        min = tilemap.CellToWorld(tilemap.cellBounds.min);
        max = tilemap.CellToWorld(tilemap.cellBounds.max);
    }
    /// <summary>
    /// 算法指定的开始。
    /// </summary>
    /// <param name="start">开始坐标.</param>
    /// <param name="goal">目标坐标.</param>
    /// <returns></returns>
    public Stack<Vector3> Algorithm(Vector3 start, Vector3 goal)
    {
        startPos = MyTilemap.WorldToCell(start);
        goalPos = MyTilemap.WorldToCell(goal);

        current = GetNode(startPos);

        //为我们稍后可能要查看的节点创建一个打开的列表
        openList = new HashSet<Node>();

        //为我们检查过的节点创建一个封闭列表
        closedList = new HashSet<Node>();

        foreach (KeyValuePair<Vector3Int, Node> node in allNodes)
        {
            node.Value.Parent = null;
        }

        allNodes.Clear();

        //将当前节点添加到打开列表（我们已经检查过）
        openList.Add(current);

        path = null;

        while (openList.Count > 0 && path == null)
        {
            List<Node> neighbours = FindNeighbours(current.Position);

            ExamineNeighbours(neighbours, current);

            UpdateCurrentTile(ref current);

            path = GeneratePath(current);
        }

        if (path != null)
        {
            return path;
        }


        return null;

    }
    /// <summary>
    /// 查找邻居
    /// </summary>
    /// <param name="parentPosition">The parent position.</param>
    /// <returns></returns>
    private List<Node> FindNeighbours(Vector3Int parentPosition)
    {
        List<Node> neighbours = new List<Node>();
        //这两个for循环确保我们当前节点周围的所有节点
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (y != 0 || x != 0)
                {
                    Vector3Int neighbourPosition = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);
                    //排除开始位置、障碍位置、未铺设tile的位置
                    if (neighbourPosition != startPos && !waterTiles.Contains(neighbourPosition)&& tilemap.GetTile(neighbourPosition))
                    {
                        Node neighbour = GetNode(neighbourPosition);
                        neighbours.Add(neighbour);
                    }

                }
            }
        }

        return neighbours;
    }
    /// <summary>
    /// 检查邻居
    /// </summary>
    /// <param name="neighbours">The neighbours.</param>
    /// <param name="current">The current.</param>
    private void ExamineNeighbours(List<Node> neighbours, Node current)
    {
        for (int i = 0; i < neighbours.Count; i++)
        {
            Node neighbour = neighbours[i];

            if (!ConnectedDiagonally(current, neighbour))
            {
                continue;
            }

            int gScore = DetermineGScore(neighbour.Position, current.Position);

            if (gScore == 14 && NoDiagonalTiles.Contains(neighbour.Position) && NoDiagonalTiles.Contains(current.Position))
            {
                continue;
            }

            if (openList.Contains(neighbour))
            {
                if (current.G + gScore < neighbour.G)
                {
                    CalcValues(current, neighbour, goalPos, gScore);
                }
            }
            else if (!closedList.Contains(neighbour))
            {
                //额外检查包含邻居的开放集
                CalcValues(current, neighbour, goalPos, gScore);

                if (!openList.Contains(neighbour))
                {
                    //然后我们需要将节点添加到开放列表中
                    openList.Add(neighbour);
                }
            }
        }
    }
    /// <summary>
    /// 判断是对角连接
    /// </summary>
    /// <param name="currentNode">The current node.</param>
    /// <param name="neighbour">The neighbour.</param>
    /// <returns></returns>
    private bool ConnectedDiagonally(Node currentNode, Node neighbour)
    {
        //获取方向
        Vector3Int direction = currentNode.Position - neighbour.Position;

        //获取节点的位置
        Vector3Int first = new Vector3Int(currentNode.Position.x + (direction.x * -1), currentNode.Position.y, currentNode.Position.z);
        Vector3Int second = new Vector3Int(currentNode.Position.x, currentNode.Position.y + (direction.y * -1), currentNode.Position.z);

        //检查两个节点是否都为空
        if (waterTiles.Contains(first) || waterTiles.Contains(second))
        {
            return false;
        }

        // ndoes是空的
        return true;
    }
    /// <summary>
    /// 确定G分数
    /// </summary>
    /// <param name="neighbour"></param>
    /// <param name="current"></param>
    /// <returns></returns>
    private int DetermineGScore(Vector3Int neighbour, Vector3Int current)
    {
        int gScore = 0;

        int x = current.x - neighbour.x;
        int y = current.y - neighbour.y;

        if (Math.Abs(x - y) % 2 == 1)
        {
            //垂直或水平节点的gscore为10
            gScore = 10;
        }
        else
        {
            gScore = 14;
        }

        return gScore;
    }
    /// <summary>
    /// 更新当前Tile
    /// </summary>
    /// <param name="current"></param>
    private void UpdateCurrentTile(ref Node current)
    {
        //从打开的列表中删除当前节点
        openList.Remove(current);

        //当前节点已添加到关闭列表中
        closedList.Add(current);


        if (openList.Count > 0)
        {
            //按f值对列表进行排序，以便更容易选择具有最低F val的节点
            current = openList.OrderBy(x => x.F).First();
        }
    }
    /// <summary>
    /// 生成路径
    /// </summary>
    /// <param name="current">.</param>
    /// <returns></returns>
    private Stack<Vector3> GeneratePath(Node current)
    {
        //如果我们当前的节点是目标，那么我们找到了一条路径
        if (current.Position == goalPos)
        {
            //创建一个包含最终路径的堆栈
            Stack<Vector3> finalPath = new Stack<Vector3>();

            //将节点添加到最终路径
            while (current != null)
            {
                //将当前节点添加到最终路径
                finalPath.Push(MyTilemap.CellToWorld(current.Position));
                //找到节点的父节点，这实际上是回溯开始的整个路径
                //通过这样做，我们将以完整的路径结束。
                current = current.Parent;
            }

            //返回完整路径
            return finalPath;
        }

        return null;

    }
    /// <summary>
    /// 计算选择路径消耗值
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="neighbour"></param>
    /// <param name="goalPos"></param>
    /// <param name="cost"></param>
    private void CalcValues(Node parent, Node neighbour, Vector3Int goalPos, int cost)
    {
        //设置父节点
        neighbour.Parent = parent;

        //计算此节点g cost，父节点g cost +移动节点的成本
        neighbour.G = parent.G + cost;

        //计算H，它是从该节点到目标* 10的距离
        neighbour.H = ((Math.Abs((neighbour.Position.x - goalPos.x)) + Math.Abs((neighbour.Position.y - goalPos.y))) * 10);

        //计算F.
        neighbour.F = neighbour.G + neighbour.H;
    }


    /// <summary>
    /// 获取节点
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private Node GetNode(Vector3Int position)
    {
        if (allNodes.ContainsKey(position))
        {
            return allNodes[position];
        }
        else
        {
            Node node = new Node(position);
            allNodes.Add(position, node);
            return node;
        }
    }
   
    /// <summary>
    ///设置玩家的限制，这样他就无法离开游戏地图
    /// </summary>
    /// <param name="min">玩家的最低位置</param>
    /// <param name="max">玩家的最大位置</param>
    public Vector3 SetLimits(Transform transform)
    {
        return new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x),
              Mathf.Clamp(transform.position.y, min.y, max.y),
              transform.position.z);
    }
}



