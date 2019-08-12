using UnityEngine;
using System.Collections;

public class HuntAction : GoapAction
{
	private bool finishHunt = false;
	private WolfDen targetWolf; 

	private float startTime = 0;
	public float miningDuration = 2; // seconds

    public HuntAction()
    {
		AddPrecondition ("hasTool", true); // we need a tool to do this
		AddEffect ("hasMeat", true);
	}

    public override float GetWeight()
    {
        return (1 - (targetWolf==null?Risk:targetWolf.HuntRisk)) * Return;
    }
	
	public override void Reset ()
	{
        finishHunt = false;
        targetWolf = null;
		startTime = 0;
	}
	
	public override bool IsDone ()
	{
        return finishHunt;
	}
	
	public override bool RequiresInRange ()
	{
		return true; // yes we need to be near a rock
	}
	
	public override bool CheckProceduralPrecondition (GameObject agent,BlackBoard bb)
	{
        WolfDen[] dens = bb.GetData("wolfDen") as WolfDen[];
        WolfDen closest = null;
		float closestDist = 0;

        foreach (WolfDen den in dens)
        {
            if (den.WolfNum <= 0)
                continue;
			if (closest == null) {
				// first one, so choose it for now
				closest = den;
				closestDist = (den.gameObject.transform.position - agent.transform.position).magnitude;
			} else {
				// is this one closer than the last?
				float dist = (den.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist) {
					// we found a closer one, use it
					closest = den;
					closestDist = dist;
				}
			}
		}
	    if (closest == null)
	        return false;

		targetWolf = closest;
        target = targetWolf.gameObject;
		
		return closest != null;
	}
	
	public override bool Perform(GameObject agent, BlackBoard bb)
	{
		if (startTime == 0)
			startTime = Time.time;

		if (Time.time - startTime > miningDuration)
		{
		    targetWolf.WolfNum--;
			// finished hunt
			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
            backpack.numMeat += 3;
			finishHunt = true;

		    if (backpack.tool == null)
            {
                ToolComponent tool = backpack.tool.GetComponent(typeof(ToolComponent)) as ToolComponent;
                tool.use(0.5f);
                if (tool.destroyed())
                {
                    Destroy(backpack.tool);
                    backpack.tool = null;
                } 
		    }
		}
		return true;
	}
	
}
