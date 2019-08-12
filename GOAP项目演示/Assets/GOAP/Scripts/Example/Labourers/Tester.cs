using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tester : Labourer
{
    /**
     * Our only goal will ever be to make tools.
     * 我们唯一的目标是制作工具。
     * The ForgeTooldAction will be able to fulfill this goal.
     */
    public override Dictionary<string, bool> CreateGoalState()
    {
        return Brain.NextGoal();
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
