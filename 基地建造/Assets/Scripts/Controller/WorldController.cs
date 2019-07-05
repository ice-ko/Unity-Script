using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldController : MonoBehaviour
{
    public static WorldController instance;

    public World world;

    bool loadWorld = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (loadWorld)
        {
            loadWorld = false;
            CreateWorldFromSaveFile();
        }
        else
        {
            CreateEmptyWorld();
        }
    }
    private void Update()
    {
        world.Update(Time.deltaTime);
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
    /// <summary>
    /// 创建空的世界
    /// </summary>
    public void CreateEmptyWorld()
    {
        world = new World(100,100);

        //设置摄像机位置
        Camera.main.transform.position = new Vector3(world.width / 2, world.height / 2, Camera.main.transform.position.z);
    }
    /// <summary>
    /// 新建世界
    /// </summary>
    public void NewWorld()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// 保存
    /// </summary>
    public void SaveWorld()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(World));
        TextWriter writer = new StringWriter();
        serializer.Serialize(writer, world);
        writer.Close();

        Debug.Log(writer.ToString());
    }
    /// <summary>
    /// 加载
    /// </summary>
    public void LoadWorld()
    {
        loadWorld = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// 创建
    /// </summary>
    public void CreateWorldFromSaveFile()
    {
        world = new World(100,100);

        //设置摄像机位置
        Camera.main.transform.position = new Vector3(world.width / 2, world.height / 2, Camera.main.transform.position.z);
    }
}
