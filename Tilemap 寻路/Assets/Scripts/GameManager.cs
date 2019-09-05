using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Canvas canvas;

    public List<Tile> tileList = new List<Tile>();

    private Vector3Int startPos, goalPos;
    private int type;
    private GameObject tileGameObject;
    private Node current;
    private HashSet<Node> openList;
    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();
    void Start()
    {

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (current == null)
            {
                Init();
            }
            List<Node> neighbors = FindNeighbors(current.Position);

            ExamineNeighbors(neighbors, current);

            Astar.Instance.CreateTiles(openList, startPos, goalPos);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (tileGameObject != null && tileGameObject.activeInHierarchy)
            {
                var pos = tilemap.WorldToCell(UtilityClass.GetMouseWorldPos());
                tilemap.SetTile(pos, tileList[type]);
                if (type == 0)
                {
                    startPos = pos;
                }
                else
                {
                    goalPos = pos;
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
        openList.Add(current);
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
                    if (neighborPos != startPos && tilemap.GetTile(neighborPos))
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
            openList.Add(neighbors[i]);
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
    public void ChangeTile(int type)
    {
        this.type = type;
        if (tileGameObject == null)
        {
            tileGameObject = new GameObject(tileList[type].name);
            tileGameObject.AddComponent<Image>();
            tileGameObject.GetComponent<Image>().sprite = tileList[type].sprite;
            tileGameObject.transform.SetParent(canvas.transform, false);
            tileGameObject.AddComponent<MoveHandler>();
            tileGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(30f, 30f);

        }
        else if (type == 1)
        {
            tileGameObject.GetComponent<Image>().sprite = tileList[1].sprite;
        }
        else
        {
            tileGameObject.GetComponent<Image>().sprite = tileList[0].sprite;
        }
        tileGameObject.transform.position = UtilityClass.GetWorldToScreenPos();
        tileGameObject.SetActive(true);
    }
}
