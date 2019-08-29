using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 任务目标
/// </summary>
public class Testers : Character
{
    /**
     * Our only goal will ever be to make tools.
     * 我们唯一的目标是制作工具。
     * The ForgeTooldAction will be able to fulfill this goal.
     * 锻造工具操作将能够实现这一目标。
     */
    public override Dictionary<string, bool> CreateGoalState()
    {
        return State.NextGoal();
    }

    public override void Tick()
    {
        base.Tick();

        if (NeedAbort)
        {
            Agent.AbortFsm();
            NeedAbort = false;
        }
    }

    public bool NeedAbort;
}
