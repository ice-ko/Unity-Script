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
    public List<Character> charactersList;
    public List<Furniture> furnituresList;
    //用于导航我们的世界地图的寻路图。
    public Path_TileGraph tileGraph;

    Dictionary<string, Furniture> furniturePrototypes = new Dictionary<string, Furniture>();
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
        SetupWorld(width, height);
        Character character = CreateCharacter(GetTileAt(width / 2, height / 2));
    }
    void SetupWorld(int width, int height)
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
        furnituresList = new List<Furniture>();

        var characterInfo = new Character(this.tiles[width / 2, height / 2]);
    }
    public void Update(float deltaTime)
    {
        foreach (Character item in charactersList)
        {
            item.Update(deltaTime);
        }
        foreach (Furniture item in furnituresList)
        {
            item.Update(deltaTime);
        }
    }
    /// <summary>
    /// 创建房屋家具字典
    /// </summary>
    void CreateFurniturePrototype()
    {
        // 类型、是否可以链接邻居、移动速度、宽、高（墙，移动速度，宽，高）
        furniturePrototypes.Add("Wall", new Furniture("Wall", true, 0, 1, 1));
        furniturePrototypes.Add("Door", new Furniture("Door", false, 1, 1, 1));

        furniturePrototypes["Door"].furnParameters["openess"] = 0;
        furniturePrototypes["Door"].furnParameters["is_opening"] = 0;
        Debug.Log("ddd");
        furniturePrototypes["Door"].updateActions += FurnitureActions.Door_UpdateAction;

        furniturePrototypes["Door"].IsEnterable = FurnitureActions.Door_IsEnterable;
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
    public Furniture PlaceFurniture(string objecttype, Tile tile)
    {
        if (!furniturePrototypes.ContainsKey(objecttype))
        {
            return null;
        }
        Furniture furniture = Furniture.PlaceObject(furniturePrototypes[objecttype], tile);

        if (furniture == null)
        {
            return null;
        }

        furnituresList.Add(furniture);

        if (cbFurnitureCreated != null)
        {
            cbFurnitureCreated(furniture);
            //重置寻路点
            InvalidateTileGraph();
        }
        return furniture;
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
        return furniturePrototypes[furnitureType].IsValidPosition(tile);
    }
    public Furniture GetFurniture(string objectType)
    {
        if (!furniturePrototypes.ContainsKey(objectType))
        {
            return null;
        }
        return furniturePrototypes[objectType];
    }
    /// <summary>
    /// Character改变时调用委托
    /// </summary>
    /// <param name="tile"></param>
    public Character CreateCharacter(Tile tile)
    {
        Character character = new Character(tile);
        charactersList.Add(character);
        if (character != null && cbCharacterCreated != null)
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
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public XmlSchema GetSchema()
    {
        return null;
    }
    /// <summary>
    /// 读取xml
    /// </summary>
    /// <param name="reader"></param>
    public void ReadXml(XmlReader reader)
    {
        width = int.Parse(reader.GetAttribute("Width"));
        height = int.Parse(reader.GetAttribute("Height"));

        SetupWorld(width, height);

        while (reader.Read())
        {
            switch (reader.Name)
            {
                case "Tiles":
                    ReadXml_Tiles(reader);
                    break;
                case "Furnitures":
                    ReadXml_Furnitures(reader);
                    break;
                case "Characters":
                    ReadXml_Characters(reader);
                    break;
            }
        }
    }
    /// <summary>
    /// 写入xml
    /// </summary>
    /// <param name="writer"></param>
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("Width", width.ToString());
        writer.WriteAttributeString("Height", height.ToString());

        writer.WriteStartElement("Tiles");
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tiles[x, y].TileType!=TileType.Empty)
                {
                    writer.WriteStartElement("Tile");
                    tiles[x, y].WriteXml(writer);
                    writer.WriteEndElement();
                }
             
            }
        }
        writer.WriteEndElement();

        writer.WriteStartElement("Furnitures");
        foreach (Furniture furn in furnituresList)
        {
            writer.WriteStartElement("Furniture");
            furn.WriteXml(writer);
            writer.WriteEndElement();

        }
        writer.WriteEndElement();

        writer.WriteStartElement("Characters");
        foreach (Character c in charactersList)
        {
            writer.WriteStartElement("Character");
            c.WriteXml(writer);
            writer.WriteEndElement();

        }
        writer.WriteEndElement();
    }
    /// <summary>
    /// 读取tile xml
    /// </summary>
    /// <param name="reader"></param>
    void ReadXml_Tiles(XmlReader reader)
    {
        if (reader.ReadToDescendant("Tile"))
        {
            do
            {
                int x = int.Parse(reader.GetAttribute("X"));
                int y = int.Parse(reader.GetAttribute("Y"));
                tiles[x, y].ReadXml(reader);
            } while (reader.ReadToNextSibling("Tile"));
        }


    }

    void ReadXml_Furnitures(XmlReader reader)
    {
        if (reader.ReadToDescendant("Furniture"))
        {
            do
            {
                int x = int.Parse(reader.GetAttribute("X"));
                int y = int.Parse(reader.GetAttribute("Y"));
                Furniture furn = PlaceFurniture(reader.GetAttribute("objectType"), tiles[x, y]);
                furn.ReadXml(reader);
            }
            while (reader.ReadToNextSibling("Furniture"));
        }


    }
    void ReadXml_Characters(XmlReader reader)
    {
        if (reader.ReadToDescendant("Character"))
        {
            do
            {
                int x = int.Parse(reader.GetAttribute("X"));
                int y = int.Parse(reader.GetAttribute("Y"));

                Character c = CreateCharacter(tiles[x, y]);
                c.ReadXml(reader);
            } while (reader.ReadToNextSibling("Character"));
        }
    }
    public World()
    {

    }
}
