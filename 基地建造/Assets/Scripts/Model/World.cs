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
    Action<Tile> cbTileChanged;
    // TODO：很可能会被专用的替换掉
    //用于管理作业队列的类（复数！）也可能
    //半静态或自我初始化或某些该死的东西。
    //现在，这只是世界公共成员
    public JobQueue jobsQueue;
    /// <summary>
    /// 初始化<see cref ="World"/>类的新实例。
    /// <param name ="width"> tile中的宽度。</ param>
    /// <param name ="height">瓷砖高度。</ param>
    ///</ summary>
    public World(int width = 100, int height = 100)
    {
        jobsQueue = new JobQueue();

        this.width = width;
        this.height = height;

        tiles = new Tile[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y] = new Tile(this, x, y);
                tiles[x, y].RegisterTileTypeChangedCallback(OnTileChanged);
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
    /// <summary>
    /// 注册已创建的 Tile
    /// </summary>
    /// <param name="cakkbackFunc"></param>
    public void RegisterTileChanged(Action<Tile> cakkbackFunc)
    {
        cbTileChanged += cakkbackFunc;
    }
    /// <summary>
    ///注销已创建的 Tile
    /// </summary>
    /// <param name="cakkbackFunc"></param>
    public void UnregisterTileChanged(Action<Tile> cakkbackFunc)
    {
        cbTileChanged -= cakkbackFunc;
    }
    /// <summary>
    /// tile改变时调用委托
    /// </summary>
    /// <param name="tile"></param>
    public void OnTileChanged(Tile tile)
    {
        if (cbTileChanged == null)
        {
            return;
        }
        cbTileChanged(tile);
    }
    /// <summary>
    /// 家具布置是否有效
    /// </summary>
    /// <param name="furnitureType">类型</param>
    /// <param name="tile">tile</param>
    /// <returns></returns>
    public bool IsFurniturePlacementValid(string furnitureType, Tile tile)
    {
        return furniturePrototype[furnitureType].funcPositionValidation(tile);
    }
    public Furniture GetFurniture(string objectType)
    {
        if (!furniturePrototype.ContainsKey(objectType))
        {
            return null;
        }
        return furniturePrototype[objectType];
    }
}
