using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Canvas canvas;
    public List<Tile> tileList;

    private Vector3Int startPos, goalPos;
    private string tileName;
    private GameObject tileGameObject;
    private Node current;
    private HashSet<Node> openList;
    private HashSet<Node> closedList;
    private Stack<Vector3Int> path;

    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();
    private Dictionary<string, Tile> tileDic = new Dictionary<string, Tile>();

    private List<Vector3Int> waterTiles = new List<Vector3Int>();
    void Start()
    {
        foreach (var item in tileList)
        {
            tileDic.Add(item.name, item);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AIgorithm();
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (tileGameObject != null && tileGameObject.activeInHierarchy)
            {
                var pos = tilemap.WorldToCell(UtilityClass.GetMouseWorldPos());
                tilemap.SetTile(pos, tileDic[tileName]);
                if (tileName == "start")
                {
                    startPos = pos;
                }
                else if (tileName == "end")
                {
                    goalPos = pos;
                }
                else if (tileName == "water")
                {
                    waterTiles.Add(pos);
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            tileGameObject.SetActive(false);
        }
    }
    public void Init()
    {
        current = GetNode(startPos);
        openList = new HashSet<Node>();
        closedList = new HashSet<Node>();
        openList.Add(current);
    }
    public void AIgorithm()
    {
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
        }
        Astar.Instance.CreateTiles(openList, closedList, allNodes, startPos, goalPos, path);
    }

    public bool ConntedDiagonally(Node currentNode, Node neighbor)
    {
        Vector3Int direct = currentNode.Position - neighbor.Position;
        Vector3Int first = new Vector3Int(current.Position.x + (direct.x * -1), current.Position.y, current.Position.z);
        Vector3Int second = new Vector3Int(current.Position.x, current.Position.y + (direct.y * -1), current.Position.z);
        if (waterTiles.Contains(first) || waterTiles.Contains(second))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
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

    /// <summary>
    /// 获取邻居
    /// </summary>
    /// <param name="parentPosition">The parent position.</param>
    /// <returns></returns>
    public List<Node> FindNeighbors(Vector3Int parentPosition)
    {
        List<Node> neighbors = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighborPos = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);
                if (y != 0 || x != 0)
                {
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
    public void ExamineNeighbors(List<Node> neighbors, Node curent)
    {
        for (int i = 0; i < neighbors.Count; i++)
        {
            Node neighbor = neighbors[i];
            if (!ConntedDiagonally(curent, neighbor))
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
            else if (!closedList.Contains(neighbor))
            {
                CalcValues(current, neighbor, gScore);
                openList.Add(neighbor);
            }
        }
    }
    public void CalcValues(Node parent, Node neighbor, int cost)
    {
        neighbor.Parent = parent;
        neighbor.G = parent.G + cost;
        neighbor.H = (Mathf.Abs(neighbor.Position.x - goalPos.x) + Mathf.Abs(neighbor.Position.x - goalPos.y * 10));
        neighbor.F = neighbor.G + neighbor.H;
    }
    public int DetermineGScore(Vector3Int neighbor, Vector3Int current)
    {
        int gScore = 0;
        int x = current.x - neighbor.x;
        int y = current.y - neighbor.y;
        if (Mathf.Abs(x - y) % 2 == 1)
        {
            gScore = 10;
        }
        else
        {
            gScore = 14;
        }
        return gScore;
    }
    public void UpdateCurrentTile(ref Node current)
    {
        openList.Remove(current);
        closedList.Add(current);
        if (openList.Count > 0)
        {
            current = openList.OrderBy(x => x.F).First();
        }
    }
    /// <summary>
    /// 获取节点
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public Node GetNode(Vector3Int position)
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
    public void ChangeTile(string tileName)
    {
        this.tileName = tileName;
        if (tileGameObject == null)
        {
            tileGameObject = new GameObject(tileDic[tileName].name);
            tileGameObject.AddComponent<Image>();
            tileGameObject.GetComponent<Image>().sprite = tileDic[tileName].sprite;
            tileGameObject.transform.SetParent(canvas.transform, false);
            tileGameObject.AddComponent<MoveHandler>();
            tileGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(30f, 30f);
        }
        else
        {
            tileGameObject.GetComponent<Image>().sprite = tileDic[tileName].sprite;
        }
        tileGameObject.transform.position = UtilityClass.GetWorldToScreenPos();
        tileGameObject.SetActive(true);
    }
}
