using UnityEngine;
using System.Collections;

public class EatMeatAction : GoapAction
{
	private bool finishEat = false;
    private SupplyPileComponent targetSupplyPile; // where we drop off the  tools
    private float startTime = 0;
    public float workDuration = 2; // seconds

    public EatMeatAction()
    {
        AddPrecondition("hasMeat", true);
        AddEffect("hasMeat", false); 
		AddEffect (Goals.FillHunger, true); 
	}
	
	
	public override void Reset ()
	{
		finishEat = false;
		targetSupplyPile = null;
	}
	
	public override bool IsDone ()
	{
		return finishEat;
	}
	
	public override bool RequiresInRange ()
	{
		return true; // yes we need to be near a supply pile so we can drop off the tools
	}
	
	public override bool CheckProceduralPrecondition (GameObject agent,BlackBoard bb)
	{
		// find the nearest supply pile that has spare tools
	    SupplyPileComponent[] supplyPiles = (SupplyPileComponent[]) bb.GetData("supplyPiles");
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
        if (startTime == 0)
            startTime = Time.time;

        if (Time.time - startTime > workDuration)
        {
            BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
            backpack.numMeat -= 1;
            // finished chopping
            Brain brain = (Brain)agent.GetComponent(typeof(Brain));
            brain.Hunger += 100;
            finishEat = true;
        }
        return true;
	}
}