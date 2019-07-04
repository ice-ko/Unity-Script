using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileSpriteController : MonoBehaviour
{
    public Sprite floorSprite;
    public Sprite emptySprite;

    public World world { get { return WorldController.instance.world; } }
    public Dictionary<Tile, GameObject> tileGameObjectMap;

    void Start()
    {
        //
        tileGameObjectMap = new Dictionary<Tile, GameObject>();

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
                //添加空的默认sprite
                SpriteRenderer sprite = tile_go.AddComponent<SpriteRenderer>();
                sprite.sprite = emptySprite;
                sprite.sortingLayerName = "Tiles";
                tile_go.transform.parent = transform;
                //
            }
        }

        //注册委托
        world.RegisterTileChanged(OnTileTypeChanged);
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
        else if (tile_data.TileType == TileType.Empty)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = emptySprite;
        }
    }
}
