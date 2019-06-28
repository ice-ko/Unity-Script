using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public Sprite floorSprite;
    World world;
    void Start()
    {
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
            }
        }
        world.RandomizeTiles();
    }
    float randomizeTileTimer = 2f;
    void Update()
    {
        randomizeTileTimer -= Time.deltaTime;
        if (randomizeTileTimer < 0)
        {
            world.RandomizeTiles();
            randomizeTileTimer = 2f;
        }
    }
    /// <summary>
    /// 刷新tile类型
    /// </summary>
    void OnTileTypeChanged(Tile tile_data, GameObject tile_go)
    {
        if (tile_data.tileType == TileType.Floor)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = floorSprite;
        }
        else
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}
