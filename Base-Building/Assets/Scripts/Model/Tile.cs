using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public TileType tileType = TileType.Empty;
    LooseObject looseObject;
    InstalledObject installedObject;

    World world;
    public int x;
    public int y;

    public Tile(World world, int x, int y)
    {
        this.world = world;
        this.x = x;
        this.y = y;
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
