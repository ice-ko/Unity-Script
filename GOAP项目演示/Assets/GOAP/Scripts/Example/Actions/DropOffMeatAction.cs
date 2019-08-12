
using System;
using UnityEngine;

/// <summary>
/// 放下肉类动作
/// </summary>
public class DropOffMeatAction : GoapAction
{
    /// <summary>
    /// 是否放下肉
    /// </summary>
	private bool droppedOffMeat = false;
    /// <summary>
    /// 目标供应桩 我们放下工具的地方
    /// </summary>
	private SupplyPileComponent targetSupplyPile; // where we drop off the  tools

    public DropOffMeatAction()
    {
        ////如果我们还没有工具，就不能放下工具
        AddPrecondition("hasMeat", true); // can't drop off tools if we don't already have some
        //我们现在没有工具
        AddEffect("hasMeat", false); // we now have no tools
        //我们收集了工具
        AddEffect(Goals.FillOther, true); // we collected tools
    }

    /// <summary>
    /// 重置
    /// </summary>
    public override void Reset()
    {
        droppedOffMeat = false;
        targetSupplyPile = null;
    }
    /// <summary>
    /// 是否已经完成了
    /// </summary>
    /// <returns></returns>
    public override bool IsDone()
    {
        return droppedOffMeat;
    }
    /// <summary>
    /// 要求范围
    /// </summary>
    /// <returns></returns>
    public override bool RequiresInRange()
    {
        //是的，我们需要靠近供应堆，这样我们才能放下工具
        return true; // yes we need to be near a supply pile so we can drop off the tools
    }
    /// <summary>
    /// 检查程序前提条件
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="bb"></param>
    /// <returns></returns>
    public override bool CheckProceduralPrecondition(GameObject agent, BlackBoard bb)
    {
        // find the nearest supply pile that has spare tools
        //找到最近的有备用工具的供应堆
        SupplyPileComponent[] supplyPiles = (SupplyPileComponent[])bb.GetData("supplyPiles");
        SupplyPileComponent closest = null;
        float closestDist = 0;

        foreach (SupplyPileComponent supply in supplyPiles)
        {
            if (closest == null)
            {
                // first one, so choose it for now
                //第一个，所以现在选择它
                closest = supply;
                closestDist = (supply.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                // is this one closer than the last?
                //这个比最后一个更近吗？
                float dist = (supply.gameObject.transform.position - agent.transform.position).magnitude;
                if (dist < closestDist)
                {
                    // we found a closer one, use it
                    //我们找到了一个更近的，使用它
                    closest = supply;
                    closestDist = dist;
                }
            }
        }
        if (closest == null)
            return false;

        targetSupplyPile = closest;
        target = targetSupplyPile.gameObject;

        return closest != null;
    }
    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="bb"></param>
    /// <returns></returns>
    public override bool Perform(GameObject agent, BlackBoard bb)
    {
        BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
        targetSupplyPile.numMeat += backpack.numMeat;
        backpack.numMeat = 0;
        droppedOffMeat = true;
        // TODO播放效果，更改演员图标

        return true;
    }
}