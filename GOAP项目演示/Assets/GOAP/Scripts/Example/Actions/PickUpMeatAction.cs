
using System;
using UnityEngine;

public class PickUpMeatAction : GoapAction
{
	private bool hasMeat = false;
	private SupplyPileComponent targetSupplyPile; // where we get the logs from
	
	public PickUpMeatAction () {
        AddPrecondition("hasMeat", false); // don't get a logs if we already have one
        AddEffect("hasMeat", true); // we now have a logs
	}
	
	
	public override void Reset ()
	{
		hasMeat = false;
		targetSupplyPile = null;
	}
	
	public override bool IsDone ()
	{
		return hasMeat;
	}
	
	public override bool RequiresInRange ()
	{
		return true; // yes we need to be near a supply pile so we can pick up the logs
	}
	
	public override bool CheckProceduralPrecondition (GameObject agent,BlackBoard bb)
	{
		// find the nearest supply pile that has spare logs
        SupplyPileComponent[] supplyPiles = (SupplyPileComponent[])bb.GetData("supplyPiles");
		SupplyPileComponent closest = null;
		float closestDist = 0;
		
		foreach (SupplyPileComponent supply in supplyPiles) {
			if (supply.numMeat > 0) {
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
		}
		if (closest == null)
			return false;

		targetSupplyPile = closest;
		target = targetSupplyPile.gameObject;
		
		return closest != null;
	}
	
	public override bool Perform(GameObject agent, BlackBoard bb)
	{
        if (targetSupplyPile.numMeat > 0)
        {
            targetSupplyPile.numMeat -= 1;
			hasMeat = true;
			//TODO play effect, change actor icon
			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
            backpack.numMeat = 1;
			
			return true;
		} else {
			// we got there but there was no logs available! Someone got there first. Cannot perform action
			return false;
		}
	}
}

