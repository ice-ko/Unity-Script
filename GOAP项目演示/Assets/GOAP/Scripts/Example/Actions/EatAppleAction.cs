using UnityEngine;
using System.Collections;

/// <summary>
/// 吃苹果动作
/// </summary>
public class EatAppleAction : GoapAction
{
    /// <summary>
    /// 是否吃完
    /// </summary>
    private bool eated = false;
    /// <summary>
    /// //我们从哪里获取日志
    /// </summary>
    private AppleTreeComponet targetAppleTree; // where we get the logs from
    /// <summary>
    /// 开始时间
    /// </summary>
    private float startTime = 0;
    /// <summary>
    /// 工作时间
    /// </summary>
    public float workDuration = 2; // seconds 单位：秒
    /// <summary>
    /// 吃苹果动作
    /// </summary>
    public EatAppleAction()
    {
		AddEffect (Goals.FillHunger, true);
	}
    /// <summary>
    /// 重置
    /// </summary>
    public override void Reset()
    {
        eated = false;
        targetAppleTree = null;
        startTime = 0;
    }
    /// <summary>
    ///  是否已经完成了
    /// </summary>
    /// <returns></returns>
    public override bool IsDone()
    {
        return eated;
    }
    /// <summary>
    /// 检查程序前提条件
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="bb"></param>
    /// <returns></returns>
    public override bool CheckProceduralPrecondition(GameObject agent, BlackBoard bb)
    {
        // find the nearest tree that we can chop
        //找到我们能砍掉的最近的树
        AppleTreeComponet[] trees = (AppleTreeComponet[])bb.GetData("appleTree");
        AppleTreeComponet closest = null;
        float closestDist = 0;

        foreach (AppleTreeComponet tree in trees)
        {
            if (closest == null)
            {
                // first one, so choose it for now
                //第一个，现在就选择它
                closest = tree;
                closestDist = (tree.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                // is this one closer than the last?
                //这个比最后一个更近吗？
                float dist = (tree.gameObject.transform.position - agent.transform.position).magnitude;
                if (dist < closestDist)
                {
                    // we found a closer one, use it
                    //我们找到了一个更近的，用它
                    closest = tree;
                    closestDist = dist;
                }
            }
        }
        if (closest == null)
            return false;

        targetAppleTree = closest;
        target = targetAppleTree.gameObject;

        return closest != null && closest.AppleNum > 0;
    }
    /// <summary>
    /// 执行
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
            // finished chopping
            //切碎完成
            Brain brain = (Brain)agent.GetComponent(typeof(Brain));
            targetAppleTree.AppleNum--;
            brain.Hunger+=50;
            eated = true;
        }
        return true;
    }
    /// <summary>
    /// 要求范围
    /// </summary>
    /// <returns></returns>
    public override bool RequiresInRange()
    {
       // 是的，我们需要靠近一棵树
        return true; // yes we need to be near a tree
    }
}
