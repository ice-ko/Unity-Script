using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public class World : IXmlSerializable
{
    Tile[,] tiles;
    //人物
    List<Character> charactersList;
    //用于导航我们的世界地图的寻路图。
    public Path_TileGraph tileGraph;

    Dictionary<string, Furniture> furniturePrototype = new Dictionary<string, Furniture>();
    //地图宽高
    public int width;
    public int height;
    //
    Action<Furniture> cbFurnitureCreated;
    Action<Tile> cbTileChanged;
    Action<Character> cbCharacterCreated;
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

        charactersList = new List<Character>();
        var characterInfo = new Character(this.tiles[width / 2, height / 2]);
    }

    public void Update(float deltaTime)
    {
        foreach (Character item in charactersList)
        {
            item.Update(deltaTime);
        }
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
        if (x >= width || x < 0 || y >= height || y < 0)
        {
            //Debug.LogError("Tile (" + x + "," + y + ") is out of range.");
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
    /// <summary>
    /// 放置家具
    /// </summary>
    /// <param name="objecttype"></param>
    /// <param name="tile"></param>
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
            //重置寻路点
            InvalidateTileGraph();
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
    ///注销已创建的 Furniture
    /// </summary>
    /// <param name="cakkbackFunc"></param>
    public void UnregisterCharacterCreated(Action<Character> cakkbackFunc)
    {
        cbCharacterCreated -= cakkbackFunc;
    }
    /// <summary>
    /// 注册已创建的 Character
    /// </summary>
    /// <param name="cakkbackFunc"></param>
    public void RegisterCharacterCreated(Action<Character> cakkbackFunc)
    {
        cbCharacterCreated += cakkbackFunc;
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
    //每当改变世界时都应该调用它
    //表示我们的旧寻路信息无效。
    public void InvalidateTileGraph()
    {
        tileGraph = null;
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
    /// <summary>
    /// Character改变时调用委托
    /// </summary>
    /// <param name="tile"></param>
    public Character CreateCharacter(Tile tile)
    {
        Character character = new Character(tile);
        charactersList.Add(character);
        if (character != null)
        {
            cbCharacterCreated(character);
        }
        return character;
    }
    /// <summary>
    /// 设置路径示例
    /// </summary>
    public void SetupPathfingingExample()
    {
        int l = width / 2 - 5;
        int b = height / 2 - 5;

        for (int x = l - 5; x < l + 15; x++)
        {
            for (int y = b - 5; y < b + 15; y++)
            {
                tiles[x, y].TileType = TileType.Floor;


                if (x == l || x == (l + 9) || y == b || y == (b + 9))
                {
                    if (x != (l + 9) && y != (b + 4))
                    {
                        PlaceFurniture("Wall", tiles[x, y]);
                    }
                }
            }
        }
    }

    public XmlSchema GetSchema()
    {
        throw new NotImplementedException();
    }

    public void ReadXml(XmlReader reader)
    {
        throw new NotImplementedException();
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("Width", width.ToString());
        writer.WriteAttributeString("Height", height.ToString());

        //writer.WriteStartAttribute("Width");
        //writer.WriteValue(width);
        //writer.WriteEndElement();
    }

    public World()
    {

    }
}
