using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// a*寻路
/// </summary>
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

    public Grid grid;
    public Tilemap tilemap;
    public Canvas canvas;
    public Tile tile;

    public GameObject debugTextPrefab;

    public Color openColor, closedColor, pathColor, currentColor, startColor, goalColor;


    private List<GameObject> debugObject = new List<GameObject>();
    public void CreateTiles(HashSet<Node> nodes, HashSet<Node> closedList, Dictionary<Vector3Int, Node> allNodes, Vector3Int start, Vector3Int goal, Stack<Vector3Int> path = null)
    {
        foreach (var node in nodes)
        {
            ColorTile(node.Position, openColor);
        }
        foreach (var node in closedList)
        {
            ColorTile(node.Position, closedColor);
        }
      
        if (path != null)
        {
            foreach (var pos in path)
            {
                if (pos != start && pos != goal)
                {
                    ColorTile(pos, pathColor);
                }
            }
        }
        ColorTile(start, startColor);
        ColorTile(goal, goalColor);
        foreach (var node in allNodes)
        {
            if (node.Value.Parent != null)
            {
                GameObject go = Instantiate(debugTextPrefab, canvas.transform);
                go.transform.position = grid.CellToWorld(node.Key);
                debugObject.Add(go);
                GenerateDebugText(node.Value, go.GetComponent<DebugText>());
            }
        }
    }
    public void GenerateDebugText(Node node, DebugText debugText)
    {
        debugText.p.text = $"P:{node.Position.x},{node.Position.y}";
        debugText.f.text = $"F:{node.F}";
        debugText.h.text = $"H:{node.H}";
        debugText.g.text = $"G:{node.G}";
        if (node.Parent.Position.x < node.Position.x && node.Parent.Position.y == node.Position.y)
        {
            debugText.arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        else if (node.Parent.Position.x < node.Position.x && node.Parent.Position.y > node.Position.y)
        {
            debugText.arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 135));
        }
        else if (node.Parent.Position.x < node.Position.x && node.Parent.Position.y < node.Position.y)
        {
            debugText.arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 255));
        }
        else if (node.Parent.Position.x > node.Position.x && node.Parent.Position.y == node.Position.y)
        {
            debugText.arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (node.Parent.Position.x > node.Position.x && node.Parent.Position.y > node.Position.y)
        {
            debugText.arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 45));
        }
        else if (node.Parent.Position.x > node.Position.x && node.Parent.Position.y < node.Position.y)
        {
            debugText.arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, -45));
        }
        else if (node.Parent.Position.x == node.Position.x && node.Parent.Position.y > node.Position.y)
        {
            debugText.arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        else if (node.Parent.Position.x == node.Position.x && node.Parent.Position.y < node.Position.y)
        {
            debugText.arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 270));
        }
    }
    public void ColorTile(Vector3Int position, Color color)
    {
        tilemap.SetTile(position, tile);
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, color);
    }
}
