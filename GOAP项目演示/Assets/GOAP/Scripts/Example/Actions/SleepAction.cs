using UnityEngine;
using System.Collections;
using System.Linq;
/// <summary>
/// 睡眠动作
/// </summary>
public class SleepAction : GoapAction
{
    private bool sleeped = false;
    private CampComponent camp;

    private float startTime = 0;
    public float workDuration = 3; // seconds
    public SleepAction()
    {
		AddEffect (Goals.FillMind, true);
	}
    public override void Reset()
    {
        sleeped = false;
        startTime = 0;
    }

    public override bool IsDone()
    {
        return sleeped;
    }

    public override bool CheckProceduralPrecondition(GameObject agent, BlackBoard bb)
    {
        CampComponent[] c = bb.GetData("camp") as CampComponent[];
        if (c == null || c.Length <= 0)
            return false;
        camp = c[0];
        target = camp.gameObject;
        return camp != null;
    }

    public override bool Perform(GameObject agent, BlackBoard bb)
    {
        if (startTime == 0)
            startTime = Time.time;

        if (Time.time - startTime > workDuration)
        {
            // finished chopping
            Brain brain = bb.GetData("brain") as Brain;
            brain.Mind+=50;
            sleeped = true;
        }
        return true;
    }

    public override bool RequiresInRange()
    {
        return true; // yes we need to be near a tree
    }
}
