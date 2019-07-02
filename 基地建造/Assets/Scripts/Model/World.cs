using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    Tile[,] tiles;

    Dictionary<string, Furniture> furniturePrototype = new Dictionary<string, Furniture>();
    //地图宽高
    public int width;
    public int height;
    //
    Action<Furniture> cbFurnitureCreated;


    public World(int width = 100, int height = 100)
    {
        this.width = width;
        this.height = height;

        tiles = new Tile[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y] = new Tile(this, x, y);
            }
        }

        CreateFurniturePrototype();
    }

    void CreateFurniturePrototype()
    {
        // 类型、是否可以链接邻居、是否可以移动、宽、高（墙，不能移动，1,1）
        Furniture wallPrototype = Furniture.CreatePrototype("Wall", true, 0, 1, 1);
        furniturePrototype.Add("Wall", wallPrototype);
    }
    /// <summary>
    /// 获取tile
    /// </summary>
    /// <returns></returns>
    public Tile GetTileAt(int x, int y)
    {
        if (x > width || x < 0 || y > height || y < 0)
        {
            Debug.LogError("Tile (" + x + "," + y + ") is out of range.");
            return null;
        }
        return tiles[x, y];
    }
    /// <summary>
    /// 随机生成tile类型
    /// </summary>
    public void RandomizeTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    tiles[x, y].TileType = TileType.Empty;
                }
                else
                {
                    tiles[x, y].TileType = TileType.Floor;
                }
            }
        }
    }
    public void PlaceFurniture(string objecttype, Tile tile)
    {
        if (!furniturePrototype.ContainsKey(objecttype))
        {
            return;
        }
        Furniture obj = Furniture.PlaceObject(furniturePrototype[objecttype], tile);

        if (obj == null)
        {
            return;
        }
        if (cbFurnitureCreated != null)
        {
            cbFurnitureCreated(obj);
        }
    }
    /// <summary>
    /// 注册已创建的 Furniture
    /// </summary>
    /// <param name="cakkbackFunc"></param>
    public void RegisterFurnitureCreated(Action<Furniture> cakkbackFunc)
    {
        cbFurnitureCreated += cakkbackFunc;
    }
    /// <summary>
    ///注销已创建的 Furniture
    /// </summary>
    /// <param name="cakkbackFunc"></param>
    public void UnregisterFurnitureCreated(Action<Furniture> cakkbackFunc)
    {
        cbFurnitureCreated -= cakkbackFunc;
    }
}
