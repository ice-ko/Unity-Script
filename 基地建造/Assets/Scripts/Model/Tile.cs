using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    TileType tileType = TileType.Empty;

    public TileType TileType
    {
        get
        {
            return tileType;
        }
        set
        {
            tileType = value;
            cbTileTypeChanged(this);
        }
    }

    Action<Tile> cbTileTypeChanged;

    LooseObject looseObject;
    public Furniture furniture;

    public World world;
    public int x;
    public int y;


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
    public bool IsNeighbour(Tile tile)
    {
        if (this.x == tile.x && (this.y == tile.y + 1 || this.y == tile.y - 1))
        {
            return true;
        }
        if (this.y == tile.y && (this.x == tile.x + 1 || this.x == tile.x - 1))
        {
            return true;
        }
        return false;
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
