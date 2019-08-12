using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 拾起工具
/// </summary>
public class PickUpTool : GoapAction
{
    public PickUpTool()
    {
        //如果我们已经有一个工具，请不要使用它
        AddPrecondition("hasTool", false); 
        //我们现在有了一个工具
        AddEffect("hasTool", true);
        //必填效果
        AddEffect(Goals.FillOther, true);
    }
    /// <summary>
    /// 是否有工具
    /// </summary>
    private bool isTool;
    /// <summary>
    /// 目标仓库
    /// </summary>
    private Warehouse targetWarehouse;
    /// <summary>
    /// 检查程序前提条件
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="bb"></param>
    /// <returns></returns>
    public override bool CheckProceduralPrecondition(GameObject agent, BlackBoard bb)
    {
        Warehouse[] warehouse = (Warehouse[])bb.GetData("Warehouse");
        Warehouse closestWarehouse = null;
        //最近的距离
        float closestDist = 0;
        foreach (Warehouse item in warehouse)
        {
            if (item.toolNum > 0)
            {
                //
                if (closestWarehouse == null)
                {
                    closestWarehouse = item;
                    //计算到达当前仓库的距离
                    closestDist = (item.gameObject.transform.position - agent.transform.position).magnitude;
                }
                else
                {
                    //计算到达当前仓库的距离
                    float dist = (item.gameObject.transform.position - agent.transform.position).magnitude;
                    //如果当前仓库的距离小于上一个仓库的距离 就说明当前仓库更近
                    if (dist < closestDist)
                    {
                        closestWarehouse = item;
                        closestDist = dist;
                    }
                }
            }
        }
        if (closestWarehouse == null)
            return false;

        targetWarehouse = closestWarehouse;
        //设置目标
        target = closestWarehouse.gameObject;

        return closestWarehouse != null;
    }
   
    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="bb"></param>
    /// <returns></returns>
    public override bool Perform(GameObject agent, BlackBoard bb)
    {
        if (targetWarehouse.toolNum > 0)
        {
            //工具数量减1
            targetWarehouse.toolNum -= 1;
            //当前已装备工具
            isTool = true;
            //在当前角色背包中添加工具
            Backpack backpack = agent.GetComponent<Backpack>();
            //
            GameObject prefab = Resources.Load<GameObject>(backpack.toolType);
            GameObject tool = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
            backpack.tool = tool;
            //设置父级
            tool.transform.parent = transform;

            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 是否需要靠近目标
    /// </summary>
    /// <returns></returns>
    public override bool RequiresInRange()
    {
        return true;
    }
    /// <summary>
    /// 重置
    /// </summary>
    public override void Reset()
    {
        isTool = false;
        targetWarehouse = null;
    }
    /// <summary>
    /// 已经完成了
    /// </summary>
    /// <returns></returns>
    public override bool IsDone()
    {
        return isTool;
    }
}
