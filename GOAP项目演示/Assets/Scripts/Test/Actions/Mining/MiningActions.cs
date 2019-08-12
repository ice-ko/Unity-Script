using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挖矿动作
/// </summary>
public class MiningActions : GoapAction
{
    /// <summary>
    /// 是否完成当前动作
    /// </summary>
    public bool isComplete;
    /// <summary>
    /// 当前任务目标
    /// </summary>
    public Mining currentTaskTarget;
    /// <summary>
    /// 工作开始时间默认为0
    /// </summary>
    public float startTime = 0;
    /// <summary>
    /// 工作时间单位：秒
    /// </summary>
    public float workDuration = 2f;
    public MiningActions()
    {
        AddPrecondition("hasTool", true); //  前置条件必须有工具
        AddPrecondition("hasMining", false); // 工作目标
        AddEffect("hasMining", true);

        //必填效果
        AddEffect(Goals.FillOther, true);
    }
    /// <summary>
    /// 程序检查此操作是否可以运行。 并非所有行动
    /// 需要这个，但有些可能。
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="bb"></param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <inheritdoc />
    public override bool CheckProceduralPrecondition(GameObject agent, BlackBoard bb)
    {
        //获取任务目标点
        Mining[] taskTargetPoint = (Mining[])bb.GetData("Mining");
        //最近的目标
        Mining closest = null;
        //距离
        float closestDist = 0;
        foreach (Mining target in taskTargetPoint)
        {
            if (closest == null)
            {
                closest = target;
                //计算到达目标的距离
                closestDist = (target.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                //计算到达目标的距离
                float dist = (target.gameObject.transform.position - agent.transform.position).magnitude;
                
                if (dist < closestDist)
                {
                    closest = target;
                    closestDist = dist;
                }
            }
        }
        if (closest == null)
            return false;

        currentTaskTarget = closest;
        target = currentTaskTarget.gameObject;

        return closest != null;
    }
    /// <summary>
    /// 行动完成了吗？
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <inheritdoc cref="GoapAction" />
    public override bool IsDone()
    {
        return isComplete;
    }

    /// <summary>
    /// 执行
    /// 如果操作成功执行则返回True，否则返回false，
    /// 如果发生了什么事，它就再也无法执行了。 在这种情况下
    /// 行动队列应清除，无法达到目标。</summary>
    /// <param name="agent"></param>
    /// <param name="bb"></param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public override bool Perform(GameObject agent, BlackBoard bb)
    {
        if (startTime == 0)
            startTime = Time.time;

        if (Time.time - startTime > workDuration)
        {
            //
            Backpack backpack = (Backpack)bb.GetData("backpack");
            //增加背包中的数量
            backpack.stoneNum += 5;
            //完成
            isComplete = true;
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
    /// 此操作是否需要在目标游戏对象的范围内？
    /// 如果不是，则moveTo状态不需要为此操作运行。
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public override bool RequiresInRange()
    {
        return true;
    }
    /// <summary>
    /// 在计划再次发生之前，重置需要重置的所有变量。
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void Reset()
    {
        isComplete = false;
        currentTaskTarget = null;
        startTime = 0;
    }
}
