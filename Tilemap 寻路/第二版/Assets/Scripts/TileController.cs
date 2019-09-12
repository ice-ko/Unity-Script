using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    #region 单例
    private static TileController instance;
    public static TileController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TileController>();
            }
            return instance;
        }
    }
    #endregion    
    public SerializableDictionary tileDic;
    public Tilemap tilemap;
    public Canvas canvas;


    private TileType tileType;

    public GameObject tileUI;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && tileUI != null && tileUI.activeInHierarchy)
        {
            var pos = tilemap.WorldToCell(UtilityClass.GetMouseWorldPos());
            ChangeTile(pos);
        }
        if (Input.GetMouseButtonDown(1) && tileUI != null)
        {
            tileUI.SetActive(false);
        }
    }
    public void ChangeTile(Vector3Int clickPos)
    {
        tilemap.SetTile(clickPos, tileDic[tileType]);
        switch (tileType)
        {
            case TileType.start:
                if (Astar.Instance.start)
                {
                    tilemap.SetTile(Astar.Instance.startPos, tileDic[TileType.grass]);
                }
                Astar.Instance.start = true;
                Astar.Instance.startPos = clickPos; break;

            case TileType.end:
                if (Astar.Instance.goal)
                {
                    tilemap.SetTile(Astar.Instance.goalPos, tileDic[TileType.grass]);
                }
                Astar.Instance.goal = true;
                Astar.Instance.goalPos = clickPos; break;
            case TileType.water:
                AStarTilemap.Instance.waterTiles.Add(clickPos); break;
        }

        //Astart.Instance.changedTiles.Add(clickPos);
    }
    public void ChangeTileType(int type)
    {
        tileType = (TileType)type;
        if (tileUI == null)
        {
            tileUI = new GameObject(tileDic[tileType].name);
            tileUI.AddComponent<Image>();
            tileUI.GetComponent<Image>().sprite = tileDic[tileType].sprite;
            tileUI.transform.SetParent(canvas.transform, false);
            tileUI.AddComponent<MoveHandler>();
            tileUI.GetComponent<RectTransform>().sizeDelta = new Vector2(30f, 30f);
        }
        else
        {
            tileUI.GetComponent<Image>().sprite = tileDic[tileType].sprite;
        }
        tileUI.transform.position = UtilityClass.GetWorldToScreenPos();
        tileUI.SetActive(true);
    }
    public bool IsBuild()
    {
        return tileUI == null || !tileUI.activeInHierarchy ? false : true;
    }
}
public enum TileType
{
    start,
    end,
    water,
    /// <summary>
    /// 草地
    /// </summary>
    grass,
    /// <summary>
    /// 沙
    /// </summary>
    sand_tile
}
