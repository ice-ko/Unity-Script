using UnityEngine;
using System.Collections;

public class CollectOreAction : GoapAction
{
	private bool mined = false;
	private IronRockComponent targetRock; // where we get the ore from

	private float startTime = 0;
	public float miningDuration = 5; // seconds

    public CollectOreAction()
    {
		AddPrecondition ("hasOre", false); // if we have ore we don't want more
		AddEffect ("hasOre", true);
	}
	
	
	public override void Reset ()
	{
		mined = false;
		targetRock = null;
		startTime = 0;
	}
	
	public override bool IsDone ()
	{
		return mined;
	}
	
	public override bool RequiresInRange ()
	{
		return true; // yes we need to be near a rock
	}
	
	public override bool CheckProceduralPrecondition (GameObject agent,BlackBoard bb)
	{
		// find the nearest rock that we can mine
        IronRockComponent[] rocks = bb.GetData("ironRock") as IronRockComponent[];
		IronRockComponent closest = null;
		float closestDist = 0;
		
		foreach (IronRockComponent rock in rocks) {
			if (closest == null) {
				// first one, so choose it for now
				closest = rock;
				closestDist = (rock.gameObject.transform.position - agent.transform.position).magnitude;
			} else {
				// is this one closer than the last?
				float dist = (rock.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist) {
					// we found a closer one, use it
					closest = rock;
					closestDist = dist;
				}
			}
		}
		targetRock = closest;
		target = targetRock.gameObject;
		
		return closest != null;
	}

    public override bool Perform(GameObject agent, BlackBoard bb)
	{
		if (startTime == 0)
			startTime = Time.time;

		if (Time.time - startTime > miningDuration) {
			// finished mining
			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
			backpack.numOre += 2;
			mined = true;
		}
		return true;
	}
	
}