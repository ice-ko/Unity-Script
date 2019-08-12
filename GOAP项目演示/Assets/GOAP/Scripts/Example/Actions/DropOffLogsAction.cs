
using System;
using UnityEngine;

public class DropOffLogsAction: GoapAction
{
	private bool droppedOffLogs = false;
	private SupplyPileComponent targetSupplyPile; // where we drop off the logs
	
	public DropOffLogsAction () {
		AddPrecondition ("hasLogs", true); // can't drop off logs if we don't already have some
		AddEffect ("hasLogs", false); // we now have no logs
        AddEffect("collectLogs", true); // we collected logs
        AddEffect(Goals.FillOther, true); // we collected ore
	}
	
	
	public override void Reset ()
	{
		droppedOffLogs = false;
		targetSupplyPile = null;
	}
	
	public override bool IsDone ()
	{
		return droppedOffLogs;
	}
	
	public override bool RequiresInRange ()
	{
		return true; // yes we need to be near a supply pile so we can drop off the logs
	}
	
	public override bool CheckProceduralPrecondition (GameObject agent,BlackBoard bb)
	{
		// find the nearest supply pile
        SupplyPileComponent[] supplyPiles = (SupplyPileComponent[])bb.GetData("supplyPiles");
		SupplyPileComponent closest = null;
		float closestDist = 0;
		
		foreach (SupplyPileComponent supply in supplyPiles) {
			if (closest == null) {
				// first one, so choose it for now
				closest = supply;
				closestDist = (supply.gameObject.transform.position - agent.transform.position).magnitude;
			} else {
				// is this one closer than the last?
				float dist = (supply.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist) {
					// we found a closer one, use it
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
	
	public override bool Perform(GameObject agent, BlackBoard bb)
	{
		BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
		targetSupplyPile.numLogs += backpack.numLogs;
		droppedOffLogs = true;
		backpack.numLogs = 0;
		
		return true;
	}
}
