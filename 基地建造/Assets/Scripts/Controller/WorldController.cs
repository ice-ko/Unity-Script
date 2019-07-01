using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public static WorldController instance;
    public Sprite floorSprite;
    public World world;
    void Start()
    {
        if (instance==null)
        {
            instance = this;
        }
        world = new World();

        for (int x = 0; x < world.width; x++)
        {
            for (int y = 0; y < world.height; y++)
            {
                Tile tile_data = world.GetTileAt(x, y);

                GameObject tile_go = new GameObject();
                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector3(tile_data.x, tile_data.y, 0);
                tile_go.AddComponent<SpriteRenderer>();
                tile_go.transform.parent = transform;
                //
                tile_data.RegisterTileTypeChangedCallback((tile)=> { OnTileTypeChanged(tile, tile_go); });
            }
        }
        world.RandomizeTiles();
    }
    void Update()
    {
       
    }
    void Foo(Tile tile_data) {

    }
    /// <summary>
    /// 刷新tile类型
    /// </summary>
    void OnTileTypeChanged(Tile tile_data, GameObject tile_go)
    {
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
}
