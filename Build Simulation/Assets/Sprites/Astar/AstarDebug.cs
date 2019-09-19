using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AstarDebug : MonoBehaviour
{
    #region 单例
    private static AstarDebug instance;
    public static AstarDebug Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AstarDebug>();
            }
            return instance;
        }
    }
    #endregion
    public Grid grid;
    public Canvas canvas;
    public Tilemap tilemap;

    public Tile tile;
    public Color openColor, closedColor, pathColor, cuurentColor, startColor, goalColor;
    public GameObject debugTextPrefab;

    private List<GameObject> debugObjects = new List<GameObject>();
    /// <summary>
    /// 创建tile
    /// </summary>
    /// <param name="start">开始坐标</param>
    /// <param name="goal">目标坐标</param>
    public void CreateTiles(HashSet<Node> openList, HashSet<Node> closedList, Dictionary<Vector3Int, Node> allNodes, Vector3Int start, Vector3Int goal, Stack<Vector3Int> path = null)
    {

        foreach (var go in debugObjects)
        {
            Destroy(go);
        }
        foreach (var node in openList)
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
                GameObject go = Instantiate(debugTextPrefab, canvas.transform, false);
                //将单元格位置转换为世界位置空间。
                go.transform.position = grid.CellToWorld(node.Key);
                debugObjects.Add(go);

                GenerateDebugText(node.Value, go.GetComponent<DebugText>());
            }
        }
    }
    /// <summary>
    /// 生成调试文本
    /// </summary>
    /// <param name="node"></param>
    /// <param name="debugText"></param>
    private void GenerateDebugText(Node node, DebugText debugText)
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
    /// <summary>
    /// tile颜色
    /// </summary>
    /// <param name="position"></param>
    /// <param name="color"></param>
    public void ColorTile(Vector3Int position, Color color)
    {
        tilemap.SetTile(position, tile);
        //将TileFlags设置为给定位置的Tile。
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, color);
    }
    /// <summary>
    /// 显示隐藏
    /// </summary>
    public void ShowHide()
    {
        canvas.gameObject.SetActive(!canvas.isActiveAndEnabled);
        tilemap.gameObject.SetActive(!tilemap.isActiveAndEnabled);
    }
    /// <summary>
    /// 刷新
    /// </summary>
    public void Reset(Dictionary<Vector3Int, Node> allNodes)
    {
        tilemap.ClearAllTiles();

        foreach (var go in debugObjects)
        {
            Destroy(go);
        }
        debugObjects.Clear();

        foreach (var position in allNodes.Keys)
        {
            tilemap.SetTile(position, null);
        }
    }
}
