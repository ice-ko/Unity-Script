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

    public GameObject debugTextPrefab;

    public Color openColor, closedColor, pathColor, currentColor, startColor, goalColor;

    public Tile tile;

    private List<GameObject> debugObject = new List<GameObject>();
    public void CreateTiles(HashSet<Node> nodes, Vector3Int start, Vector3Int goal)
    {
        foreach (var node in nodes)
        {
            ColorTile(node.Position, openColor);
        }
        ColorTile(start, startColor);
        ColorTile(goal, goalColor);
    }
    public void ColorTile(Vector3Int position, Color color)
    {
        tilemap.SetTile(position, tile);
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, color);
    }
}
