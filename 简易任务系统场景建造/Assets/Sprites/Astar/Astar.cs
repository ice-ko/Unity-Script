using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Astar : MonoBehaviour
{
    #region 单例
    private static Astar instance;
    public static Astar Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Astar>();
            }
            return instance;
        }
    }
    #endregion    
    public Tilemap tilemap;
    /// <summary>
    /// 开始位置、目标位置
    /// </summary>
    [HideInInspector]
    public Vector3Int startPos, goalPos;
    [HideInInspector]
    public bool start, goal;

    //水Tile
    [HideInInspector]
    public List<Vector3Int> waterTiles = new List<Vector3Int>();
    [HideInInspector]
    public HashSet<Vector3Int> changedTiles = new HashSet<Vector3Int>();


    private HashSet<Node> openList = new HashSet<Node>();
    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();
    private Node current;
    private HashSet<Node> closeList = new HashSet<Node>();
    private Stack<Vector3Int> path;


    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Algotithm();
        //}
    }
    private void Init()
    {
        current = GetNode(startPos);

        openList.Add(current);
    }
    /// <summary>
    /// 测试
    /// </summary>
    public void Algotithm(Vector3Int startPos, Vector3Int goalPos, bool step)
    {
        this.startPos = startPos;
        this.goalPos = goalPos;
        if (current == null)
        {
            Init();
        }

        while (openList.Count > 0 && path == null)
        {
            List<Node> neighbors = FindNeighbors(current.Position);

            ExamineNeighbors(neighbors, current);

            UpdateCurrentTile(ref current);

            path = GeneratePath(current);
            if (step)
            {
                break;
            }
        }
        //if (path != null)
        //{
        //    foreach (var postion in path)
        //    {
        //        tilemap.SetTile(postion, TileController.Instance.tileDic[TileType.sand_tile]);
        //    }
        //}

        AstarDebug.Instance.CreateTiles(openList, closeList, allNodes, startPos, goalPos, path);
    }
    public Stack<Vector3Int> Algotithm(Vector3 start,Vector3 goal)
    {
        startPos = tilemap.WorldToCell(start);
        goalPos = tilemap.WorldToCell(goal);

      

        openList = new HashSet<Node>();
        closeList = new HashSet<Node>();

       

        path = null;
        Reset();

        current = GetNode(startPos);
        openList.Add(current);
        while (openList.Count > 0 && path == null)
        {
            List<Node> neighbors = FindNeighbors(current.Position);

            ExamineNeighbors(neighbors, current);

            UpdateCurrentTile(ref current);

            path = GeneratePath(current);
        }
        AstarDebug.Instance.CreateTiles(openList, closeList, allNodes, startPos, goalPos, path);
        if (path != null)
        {
            return path;
        }
        return null;
    }
    /// <summary>
    /// 查找邻居
    /// </summary>
    /// <param name="parentPosition"></param>
    /// <returns></returns>
    private List<Node> FindNeighbors(Vector3Int parentPosition)
    {
        List<Node> neighbors = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighborPos = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);
                if (y != 0 || x != 0)
                {
                    //排除开始位置、障碍位置
                    if (neighborPos != startPos && !waterTiles.Contains(neighborPos) && tilemap.GetTile(neighborPos))
                    {
                        Node neighbor = GetNode(neighborPos);
                        neighbors.Add(neighbor);
                    }
                }
            }
        }
        return neighbors;
    }
    /// <summary>
    /// 检查邻居
    /// </summary>
    /// <param name="neighbors"></param>
    /// <param name="current"></param>
    private void ExamineNeighbors(List<Node> neighbors, Node current)
    {
        for (int i = 0; i < neighbors.Count; i++)
        {
            Node neighbor = neighbors[i];

            if (!ConntedDiagonally(current, neighbor))
            {
                continue;
            }
            int gScore = DetermineGScore(neighbors[i].Position, current.Position);

            if (openList.Contains(neighbor))
            {
                if (current.G + gScore < neighbor.G)
                {
                    CalcValues(current, neighbor, gScore);
                }
            }
            else if (!closeList.Contains(neighbor))
            {
                CalcValues(current, neighbor, gScore);
                openList.Add(neighbor);
            }
        }
    }
    /// <summary>
    /// 计算选择路径消耗值
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="neighbor"></param>
    /// <param name="cost"></param>
    private void CalcValues(Node parent, Node neighbor, int cost)
    {
        neighbor.Parent = parent;
        neighbor.G = parent.G + cost;
        neighbor.H = ((Math.Abs(neighbor.Position.x - goalPos.x) + Math.Abs((neighbor.Position.y - goalPos.y))) * 10);
        neighbor.F = neighbor.G + neighbor.H;
    }
    /// <summary>
    /// 确定分数
    /// </summary>
    /// <param name="neighbor"></param>
    /// <param name="current"></param>
    /// <returns></returns>
    private int DetermineGScore(Vector3Int neighbor, Vector3Int current)
    {
        int gScore = 0;
        int x = current.x - neighbor.x;
        int y = current.y - neighbor.y;
        if (Math.Abs(x - y) % 2 == 1)
        {
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
        openList.Remove(current);
        closeList.Add(current);
        //获取最小的消耗
        if (openList.Count > 0)
        {
            current = openList.OrderBy(x => x.F).First();
        }
    }
    /// <summary>
    /// 获取节点
    /// </summary>
    /// <param name="position">坐标.</param>
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
    /// 对角连接
    /// </summary>
    /// <param name="currentNode"></param>
    /// <param name="neighbor"></param>
    /// <returns></returns>
    private bool ConntedDiagonally(Node currentNode, Node neighbor)
    {
        Vector3Int direct = currentNode.Position - neighbor.Position;
        //第一
        Vector3Int first = new Vector3Int(current.Position.x + (direct.x * -1), current.Position.y, current.Position.z);
        //第二
        Vector3Int second = new Vector3Int(current.Position.x, current.Position.y + (direct.y * -1), current.Position.z);
        if (waterTiles.Contains(first) || waterTiles.Contains(second))
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// 生成路径。
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    private Stack<Vector3Int> GeneratePath(Node current)
    {
        if (current.Position == goalPos)
        {
            Stack<Vector3Int> finalPath = new Stack<Vector3Int>();
            while (current.Position != startPos)
            {
                finalPath.Push(current.Position);
                current = current.Parent;
            }
            return finalPath;
        }
        return null;
    }
    public void Reset()
    {
        AstarDebug.Instance.Reset(allNodes);


        //foreach (var position in changedTiles)
        //{
        //    tilemap.SetTile(position, TileController.Instance.tileDic[TileType.grass]);
        //}
        //if (path!=null)
        //{
        //    foreach (var position in path)
        //    {
        //        tilemap.SetTile(position, TileController.Instance.tileDic[TileType.grass]);
        //    }
        //}
        
        //tilemap.SetTile(startPos, TileController.Instance.tileDic[TileType.grass]);
        //tilemap.SetTile(goalPos, TileController.Instance.tileDic[TileType.grass]);

        waterTiles.Clear();
        openList.Clear();
        closeList.Clear();
        allNodes.Clear();

        start = false;
        goal = false;
        current = null;
        path = null;


    }
}