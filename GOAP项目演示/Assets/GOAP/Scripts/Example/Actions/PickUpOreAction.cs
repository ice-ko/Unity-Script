
using System;
using UnityEngine;

public class PickUpOreAction : GoapAction
{
	private bool hasOre = false;
	private SupplyPileComponent targetSupplyPile; // where we get the ore from
	
	public PickUpOreAction () {
		AddPrecondition ("hasOre", false); // don't get a ore if we already have one
		AddEffect ("hasOre", true); // we now have a ore
	}
	
	
	public override void Reset ()
	{
		hasOre = false;
		targetSupplyPile = null;
	}
	
	public override bool IsDone ()
	{
		return hasOre;
	}
	
	public override bool RequiresInRange ()
	{
		return true; // yes we need to be near a supply pile so we can pick up the ore
	}
	
	public override bool CheckProceduralPrecondition (GameObject agent,BlackBoard bb)
	{
		// find the nearest supply pile that has spare ores
        SupplyPileComponent[] supplyPiles = (SupplyPileComponent[])bb.GetData("supplyPiles");
		SupplyPileComponent closest = null;
		float closestDist = 0;
		
		foreach (SupplyPileComponent supply in supplyPiles) {
			if (supply.numOre >= 3) { // we need to take 3 ore
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
		if (targetSupplyPile.numOre >= 3) {
			targetSupplyPile.numOre -= 3;
			hasOre = true;
			//TODO play effect, change actor icon
			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
			backpack.numOre += 3;
			
			return true;
		} else {
			// we got there but there was no ore available! Someone got there first. Cannot perform action
			return false;
		}
	}
}
