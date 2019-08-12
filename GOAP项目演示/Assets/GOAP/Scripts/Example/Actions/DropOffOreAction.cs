
using System;
using UnityEngine;

/// <summary>
/// 放下矿石行动
/// </summary>
public class DropOffOreAction : GoapAction
{
	private bool droppedOffOre = false;
	private SupplyPileComponent targetSupplyPile; //我们放下矿石的地方
	
	public DropOffOreAction () {
		AddPrecondition ("hasOre", true); // can't drop off ore if we don't already have some
		AddEffect ("hasOre", false); // we now have no ore
        AddEffect("collectOre", true); // we collected ore
        AddEffect(Goals.FillOther, true); // we collected ore
	}
	
	
	public override void Reset ()
	{
		droppedOffOre = false;
		targetSupplyPile = null;
	}
	
	public override bool IsDone ()
	{
		return droppedOffOre;
	}
	
	public override bool RequiresInRange ()
	{
		return true; // yes we need to be near a supply pile so we can drop off the ore
	}
	
	public override bool CheckProceduralPrecondition (GameObject agent,BlackBoard bb)
	{
		// find the nearest supply pile that has spare ore
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
		targetSupplyPile.numOre += backpack.numOre;
		droppedOffOre = true;
		backpack.numOre = 0;
		//TODO play effect, change actor icon
		
		return true;
	}
}
