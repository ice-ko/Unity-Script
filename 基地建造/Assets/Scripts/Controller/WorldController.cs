using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public static WorldController instance;

    public World world;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        world = new World();
       
        //设置摄像机位置
        Camera.main.transform.position = new Vector3(world.width / 2, world.height / 2, Camera.main.transform.position.z);
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
