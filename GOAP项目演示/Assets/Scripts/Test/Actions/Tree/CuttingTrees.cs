using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 砍伐树木
/// </summary>
public class CuttingTrees : GoapAction
{

    /// <summary>
    /// 是否完成砍伐树木
    /// </summary>
    public bool isCuttingTrees;
    /// <summary>
    /// 树木
    /// </summary>
    public Trees targetTrees;
    /// <summary>
    /// 砍伐树木工作开始时间默认为0
    /// </summary>
    public float startTime = 0;
    /// <summary>
    /// 砍伐树木工作时间单位：秒
    /// </summary>
    public float workDuration = 2f;

    public CuttingTrees()
    {
        AddPrecondition("hasTool", true); // 砍伐树木需要工具 前置条件必须有工具
        AddPrecondition("hasTree", false); // 如果已经有了 就不需要那么多
        AddEffect("hasTree", true);
        //必填效果
        AddEffect(Goals.FillOther, true);
    }
    /// <summary>
    /// 检测工作前提
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="bb"></param>
    /// <returns></returns>
    public override bool CheckProceduralPrecondition(GameObject agent, BlackBoard bb)
    {
        //获取所有树木
        Trees[] blocks = (Trees[])bb.GetData("Tree");
        //最近的树木
        Trees closest = null;
        //距离
        float closestDist = 0;
        foreach (Trees block in blocks)
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
                //当前距离小于上一个树木
                if (dist < closestDist)
                {
                    closest = block;
                    closestDist = dist;
                }
            }
        }
        if (closest == null)
            return false;

        targetTrees = closest;
        target = targetTrees.gameObject;

        return closest != null;
    }
    /// <summary>
    /// 执行工作
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="bb"></param>
    /// <returns></returns>
    public override bool Perform(GameObject agent, BlackBoard bb)
    {
        if (startTime == 0)
            startTime = Time.time;

        if (Time.time - startTime > workDuration)
        {
            // 完成砍树
            Backpack backpack = (Backpack)bb.GetData("backpack");
            //增加背包中的木材数量
            backpack.woodNum += 5;
            //完成劈柴
            isCuttingTrees = true;
            ToolComponent tool = backpack.tool.GetComponent<ToolComponent>();
            //减少工具耐久度
            tool.use(0.34f);
            //耐久度小于等于0销毁当前工具
            if (tool.destroyed())
            {
                Destroy(backpack.tool);
                backpack.tool = null;
            }
        }
        return true;
    }
    /// <summary>
    /// 是否到指定范围
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
        isCuttingTrees = false;
        targetTrees = null;
        startTime = 0;
    }
    /// <summary>
    /// 是否完成
    /// </summary>
    /// <returns></returns>
    public override bool IsDone()
    {
        return isCuttingTrees;
    }
}
