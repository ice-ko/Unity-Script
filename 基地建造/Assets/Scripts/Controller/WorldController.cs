using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public static WorldController instance;
    public Sprite floorSprite;

    public World world;
    public Dictionary<Tile, GameObject> tileGameObjectMap;
    public Dictionary<Furniture, GameObject> FurnitureGameObjectMap;
    public Dictionary<string, Sprite> FurnitureSprite;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        world = new World();
        //加载Sprite
        FurnitureSprite = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Furniture/Wall");
        foreach (var item in sprites)
        {
            FurnitureSprite.Add(item.name, item);
        }
        world.RegisterFurnitureCreated(OnFurnitureCreated);

        tileGameObjectMap = new Dictionary<Tile, GameObject>();
        FurnitureGameObjectMap = new Dictionary<Furniture, GameObject>();
        //循环生成地图
        for (int x = 0; x < world.width; x++)
        {
            for (int y = 0; y < world.height; y++)
            {
                Tile tile_data = world.GetTileAt(x, y);

                GameObject tile_go = new GameObject();

                tileGameObjectMap.Add(tile_data, tile_go);

                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector3(tile_data.x, tile_data.y, 0);
                tile_go.AddComponent<SpriteRenderer>();
                tile_go.transform.parent = transform;
                //
                tile_data.RegisterTileTypeChangedCallback(OnTileTypeChanged);
            }
        }
        world.RandomizeTiles();
    }
    void Update()
    {

    }
    /// <summary>
    /// 删除所有存储的tile信息
    /// </summary>
    void DestroyAllTileGameObgjects()
    {
        while (tileGameObjectMap.Count > 0)
        {
            Tile tile = tileGameObjectMap.Keys.First();
            GameObject tile_go = tileGameObjectMap[tile];
            //移除
            tileGameObjectMap.Remove(tile);
            //注销action事件
            tile.UnregisterTileTypeChangedCallback(OnTileTypeChanged);
            //删除gameobgject
            Destroy(tile_go);
        }
    }
    /// <summary>
    /// 刷新tile类型
    /// </summary>
    void OnTileTypeChanged(Tile tile_data)
    {
        if (!tileGameObjectMap.ContainsKey(tile_data))
        {
            Debug.LogError("tile :" + tile_data + " 不存在");
            return;
        }
        GameObject tile_go = tileGameObjectMap[tile_data];

        if (tile_data.TileType == TileType.Floor)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = floorSprite;
        }
        else
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = null;
        }
    }
    /// <summary>
	/// 获取空间坐标处的tile
	/// </summary>
	/// <returns>世界坐标处的图块。</returns>
	/// <param name="coord">Unity World-Space坐标。</param>
	public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        return world.GetTileAt(x, y);
    }
    /// <summary>
    /// 创建Furniture
    /// </summary>
    /// <param name="obj"></param>
    public void OnFurnitureCreated(Furniture obj)
    {
        // 创建链接到此数据的可视GameObject。

        // FIXME：不考虑多瓦片对象也不考虑旋转对象

        //这会创建一个新的GameObject并将其添加到我们的场景中。
        GameObject obj_go = new GameObject();
        //将我们的tile / GO对添加到字典中。
        FurnitureGameObjectMap.Add(obj, obj_go);

        obj_go.name = obj.objectType + "_" + obj.tile.x + "_" + obj.tile.y;
        obj_go.transform.position = new Vector3(obj.tile.x, obj.tile.y, 0);
        obj_go.transform.parent = transform;
        // FIXME：我们假设对象必须是墙，所以请 
        //添加SpriteRenderer
        var sr = obj_go.AddComponent<SpriteRenderer>();
        sr.sprite = GetSpriteForFurniture(obj);
        sr.sortingLayerName = "Wall";
        //注册我们的回调，以便我们的GameObject随时更新
        obj.RegisterOnChangedCallback(OnFurnitureChanged);
    }
    /// <summary>
    /// 获取已安装对象的精灵
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    Sprite GetSpriteForFurniture(Furniture obj)
    {
        if (!obj.linksToNeighbour)
        {
            return FurnitureSprite[obj.objectType];
        }

        var spriteName = obj.objectType + "_";
        //判断方向
        int x = obj.tile.x; int y = obj.tile.y;
        Tile tile;
        tile = world.GetTileAt(x, y + 1);
        if (tile != null && tile.furniture != null && tile.furniture.objectType == obj.objectType)
        {
            spriteName += "N";
        }
        tile = world.GetTileAt(x + 1, y);
        if (tile != null && tile.furniture != null && tile.furniture.objectType == obj.objectType)
        {
            spriteName += "E";
        }
        tile = world.GetTileAt(x, y - 1);
        if (tile != null && tile.furniture != null && tile.furniture.objectType == obj.objectType)
        {
            spriteName += "S";
        }
        tile = world.GetTileAt(x - 1, y);
        if (tile != null && tile.furniture != null && tile.furniture.objectType == obj.objectType)
        {
            spriteName += "W";
        }
        if (!FurnitureSprite.ContainsKey(spriteName))
        {
            return null;
        }
        return FurnitureSprite[spriteName];


    }
    void OnFurnitureChanged(Furniture  furniture)
    {
        if (!FurnitureGameObjectMap.ContainsKey(furniture))
        {
            return;
        }
        GameObject obj_go = FurnitureGameObjectMap[furniture];
        obj_go.GetComponent<SpriteRenderer>().sprite = GetSpriteForFurniture(furniture);
    }
}
