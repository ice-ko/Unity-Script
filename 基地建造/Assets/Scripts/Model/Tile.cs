using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public class Tile : IXmlSerializable
{
    TileType _type = TileType.Empty;

    public TileType TileType
    {
        get
        {
            return _type;
        }
        set
        {
            TileType oldType = _type;
            _type = value;
            if (cbTileTypeChanged != null && oldType != _type)
            {
                cbTileTypeChanged(this);
            }

        }
    }

    Action<Tile> cbTileTypeChanged;

    LooseObject looseObject;
    public Furniture furniture;

    public World world;
    public int x;
    public int y;


    public float movementCost
    {
        get
        {
            if (_type == TileType.Empty)
            {
                return 0;
            }
            if (furniture == null)
            {
                return 1;
            }
            return 1 * furniture.movementCost;
        }
    }

    // FIXME：如果作业正在等待，这似乎是一种可怕的标记方式
    //在瓷砖上 这在设置/清除中容易出错。
    public Job pendingFurnitureJob;

    public Tile(World world, int x, int y)
    {
        this.world = world;
        this.x = x;
        this.y = y;
    }
    /// <summary>
    /// 注册Tile类型更改回调
    /// </summary>
    /// <param name="action"></param>
    public void RegisterTileTypeChangedCallback(Action<Tile> action)
    {
        cbTileTypeChanged += action;
    }
    /// <summary>
    /// 注销Tile类型更改回调
    /// </summary>
    /// <param name="action"></param>
    public void UnregisterTileTypeChangedCallback(Action<Tile> action)
    {
        cbTileTypeChanged -= action;
    }
    public bool PlaceObject(Furniture objInstance)
    {
        if (objInstance == null)
        {
            furniture = null;
            return true;
        }
        if (furniture != null)
        {
            return false;
        }
        furniture = objInstance;
        return true;
    }
    /// <summary>
    /// 检测两个瓷砖是否相邻。
    /// </summary>
    /// <param name="tile"></param>
    /// <returns></returns>
    public bool IsNeighbour(Tile tile, bool diagOkay)
    {

        //检查两者之间是否只有一个差异
        //平铺坐标 是这样，那么我们是垂直或水平的邻居。
        return
            //检查hori / vert邻接
            Mathf.Abs(this.x - tile.x) + Mathf.Abs(this.y - tile.y) == 1 ||
            ////检查diag邻接
            (diagOkay && (Mathf.Abs(this.x - tile.x) == 1 && Mathf.Abs(this.y - tile.y) == 1));

        //if (this.x == tile.x && (this.y == tile.y + 1 || this.y == tile.y - 1))
        //{
        //    return true;
        //}
        //if (this.y == tile.y && (this.x == tile.x + 1 || this.x == tile.x - 1))
        //{
        //    return true;
        //}
        //if (diagOkay)
        //{
        //    if (this.x== tile.x+1&&this.y== tile.y+1||this.y==tile.y-1)
        //    {
        //        return true;
        //    }
        //    if (this.x == tile.x - 1 && this.y == tile.y + 1 || this.y == tile.y - 1)
        //    {
        //        return true;
        //    }
        //}
        //return false;
    }
    /// <summary>
	/// 获取邻居Tile。
	/// </summary>
	/// <returns>返回邻居tile。</returns>
	/// <param name="diagOkay">是否对角线运动？.</param>
    public Tile[] GetNeighbours(bool diagOkay)
    {
        Tile[] tiles;
        if (!diagOkay)
        {
            //Tile 四个方向: N E S W
            tiles = new Tile[4];
        }
        else
        {
            //Tile 八个方向: N E S W NE SE SW NW
            tiles = new Tile[8];
        }
        Tile n;
        //可以为null，但没关系。
        n = world.GetTileAt(x, y + 1);
        tiles[0] = n;
        n = world.GetTileAt(x + 1, y);
        tiles[1] = n;
        n = world.GetTileAt(x, y - 1);
        tiles[2] = n;
        n = world.GetTileAt(x - 1, y);
        tiles[3] = n;

        if (diagOkay == true)
        {
            //可以为null，但没关系。
            n = world.GetTileAt(x + 1, y + 1);
            tiles[4] = n;
            n = world.GetTileAt(x + 1, y - 1);
            tiles[5] = n;
            n = world.GetTileAt(x - 1, y - 1);
            tiles[6] = n;
            n = world.GetTileAt(x - 1, y + 1);
            tiles[7] = n;
        }

        return tiles;
    }

    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        TileType = (TileType)int.Parse(reader.GetAttribute("Type"));
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("X", x.ToString());
        writer.WriteAttributeString("Y", y.ToString());
        writer.WriteAttributeString("Type", ((int)TileType).ToString());
    }

    public Enterability IsEnterable()
    {
        if (movementCost == 0)
        {
            return Enterability.Never;
        }
        if (furniture!=null&&furniture.IsEnterable!=null)
        {
            return furniture.IsEnterable(furniture);
        }
        return Enterability.Yes;
    }

}
/// <summary>
/// 类型
/// </summary>
public enum TileType
{
    /// <summary>
    /// 空的
    /// </summary>
    Empty,
    /// <summary>
    /// 地板
    /// </summary>
    Floor

}
public enum Enterability
{
    Yes, Never, Soon
}
