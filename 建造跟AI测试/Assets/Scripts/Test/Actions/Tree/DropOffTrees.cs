using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffTrees : GoapAction
{
    private bool isDropOffTrees = false;
    /// <summary>
    /// 目标仓库
    /// </summary>
    private Warehouse targetWarehouse;

    public DropOffTrees()
    {
        AddPrecondition("hasTree", true); // 如果没有木材 不能放下木材
        AddEffect("hasTree", false); // 我们现在没有木材
                                     // AddEffect("collectTree", true); // 我们收集了木材

        AddPrecondition("hasMining", true); // 如果没有木材 不能放下木材
        AddEffect("hasMining", false); // 我们现在没有木材
        //AddEffect(CharacterGoals.FillOther, true); // 我们收集了矿石
    }
    public override bool CheckProceduralPrecondition(GameObject agent, BlackBoard bb)
    {
        //获取所有仓库
        Warehouse[] blocks = (Warehouse[])bb.GetData("Warehouse");
        //最近的仓库
        Warehouse closest = null;
        //距离
        float closestDist = 0;
        foreach (Warehouse block in blocks)
        {
            if (closest == null)
            {
                closest = block;
                //计算距离
                closestDist = (block.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                //计算距离
                float dist = (block.gameObject.transform.position - agent.transform.position).magnitude;
                //当前距离小于上一个仓库
                if (dist < closestDist)
                {
                    closest = block;
                    closestDist = dist;
                }
            }
        }
        if (closest == null)
            return false;

        targetWarehouse = closest;
        target = targetWarehouse.gameObject;

        return closest != null;
    }

    public override bool Perform(GameObject agent, BlackBoard bb)
    {
        Backpack backpack = agent.GetComponent<Backpack>();
        if (backpack.woodNum > 0)
        {
            targetWarehouse.woodNum += backpack.woodNum;
            backpack.woodNum = 0;
        }
        if (backpack.stoneNum > 0)
        {
            targetWarehouse.stoneNum += backpack.stoneNum;
            backpack.stoneNum = 0;
        }
        isDropOffTrees = true;

        return true;
    }
    public override bool IsDone()
    {
        return isDropOffTrees;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override void Reset()
    {
        isDropOffTrees = false;
        targetWarehouse = null;
    }
}
