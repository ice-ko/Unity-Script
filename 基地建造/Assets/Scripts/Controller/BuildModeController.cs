using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildModeController : MonoBehaviour
{
    TileType buildModeTile = TileType.Floor;
    //是否构建模式
    bool buildModeIsObjects = false;
    string buindModeObjectType;
    void Start()
    {

    }
    /// <summary>
    /// 建造地板
    /// </summary>
    public void SetMode_BuildFloor()
    {
        buildModeIsObjects = false;
        buildModeTile = TileType.Floor;
    }
    /// <summary>
    /// 拆除地板
    /// </summary>
    public void SetMode_Bulldoze()
    {
        buildModeIsObjects = false;
        buildModeTile = TileType.Empty;
    }
    /// <summary>
    /// 建造墙
    /// </summary>
    public void SetMode_BuilWall(string objecttype)
    {
        // 墙不是瓷砖！ Wall是存在于tile的TOP上的“Furniture”。
        buildModeIsObjects = true;
        buindModeObjectType = objecttype;
    }
    /// <summary>
    /// 建造示例围墙
    /// </summary>
    public void SetMode_BuilWallExample()
    {
        // 墙不是瓷砖！ Wall是存在于tile的TOP上的“Furniture”。
        WorldController.instance.world.SetupPathfingingExample();
        //测试路径点
        Path_TileGraph tileGraph = new Path_TileGraph(WorldController.instance.world);
    }
    /// <summary>
    /// 建造任务
    /// </summary>
    /// <param name="t"></param>
    public void DoBuild(Tile t)
    {
        if (buildModeIsObjects == true)
        {
            var check = WorldController.instance.world.IsFurniturePlacementValid(buindModeObjectType, t);
            if (check && t.pendingFurnitureJob == null)
            {
                //此瓷砖位置对此家具有效
                //创建一个要构建的作业
                //创建任务
                Job job = new Job(t, buindModeObjectType, (theJob) =>
                {
                    WorldController.instance.world.PlaceFurniture(buindModeObjectType, theJob.tile);
                    t.pendingFurnitureJob = null;
                });
                // FIXME：我不喜欢手动和明确设置
                //预防冲突的标志 忘记设置/清除它们太容易了！
                t.pendingFurnitureJob = job;
                job.RegisterJobCancelCallback((theJob) =>
                {
                    theJob.tile.pendingFurnitureJob = null;
                });
                //把任务添加到任务队列中
                WorldController.instance.world.jobsQueue.Enqueue(job);
            }
        }
        else
        {
            // 我们处于换砖模式。
            t.TileType = buildModeTile;
        }
    }
}
