
using System;
using UnityEngine;

public class ForgeToolAction : GoapAction
{
	private bool forged = false;
	private ForgeComponent targetForge; // where we forge tools
	
	private float startTime = 0;
	public float forgeDuration = 2; // seconds
	
	public ForgeToolAction () {
		AddPrecondition ("hasOre", true);
		AddEffect ("hasNewTools", true);
	}
	
	
	public override void Reset ()
	{
		forged = false;
		targetForge = null;
		startTime = 0;
	}
	
	public override bool IsDone ()
	{
		return forged;
	}
	
	public override bool RequiresInRange ()
	{
		return true; // yes we need to be near a forge
	}
	
	public override bool CheckProceduralPrecondition (GameObject agent,BlackBoard bb)
	{
		// find the nearest forge
		ForgeComponent[] forges = (ForgeComponent[]) bb.GetData("forge");
		ForgeComponent closest = null;
		float closestDist = 0;
		
		foreach (ForgeComponent forge in forges) {
			if (closest == null) {
				// first one, so choose it for now
				closest = forge;
				closestDist = (forge.gameObject.transform.position - agent.transform.position).magnitude;
			} else {
				// is this one closer than the last?
				float dist = (forge.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist) {
					// we found a closer one, use it
					closest = forge;
					closestDist = dist;
				}
			}
		}
		if (closest == null)
			return false;

		targetForge = closest;
		target = targetForge.gameObject;
		
		return closest != null;
	}
	
	public override bool Perform(GameObject agent, BlackBoard bb)
	{
		if (startTime == 0)
			startTime = Time.time;
		
		if (Time.time - startTime > forgeDuration) {
			// finished forging a tool
			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
			backpack.numOre = 0;
			forged = true;
		}
		return true;
	}
	
}
