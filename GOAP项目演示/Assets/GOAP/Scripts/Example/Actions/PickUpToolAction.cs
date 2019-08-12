using System;
using UnityEngine;

/// <summary>
/// 拿起工具动作
/// </summary>
public class PickUpToolAction : GoapAction
{
    private bool hasTool = false;
    /// <summary>
    /// 我们从哪里获得工具
    /// </summary>
	private SupplyPileComponent targetSupplyPile; // where we get the tool from

    public PickUpToolAction()
    {
        //如果我们已经有一个工具，请不要使用它
		AddPrecondition("hasTool", false); // don't get a tool if we already have one
        //我们现在有了一个工具
        AddEffect("hasTool", true); // we now have a tool
    }


    public override void Reset()
    {
        hasTool = false;
        targetSupplyPile = null;
    }

    public override bool IsDone()
    {
        return hasTool;
    }

    public override bool RequiresInRange()
    {
        //是的，我们需要靠近供应堆，这样我们才能拿起工具
        return true; // yes we need to be near a supply pile so we can pick up the tool
    }

    public override bool CheckProceduralPrecondition(GameObject agent, BlackBoard bb)
    {
        // find the nearest supply pile that has spare tools
        SupplyPileComponent[] supplyPiles = (SupplyPileComponent[])bb.GetData("supplyPiles");
        SupplyPileComponent closest = null;
        float closestDist = 0;

        foreach (SupplyPileComponent supply in supplyPiles)
        {
            if (supply.numTools > 0)
            {
                if (closest == null)
                {
                    // first one, so choose it for now
                    closest = supply;
                    closestDist = (supply.gameObject.transform.position - agent.transform.position).magnitude;
                }
                else
                {
                    // is this one closer than the last?
                    float dist = (supply.gameObject.transform.position - agent.transform.position).magnitude;
                    if (dist < closestDist)
                    {
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
        if (targetSupplyPile.numTools > 0)
        {
            targetSupplyPile.numTools -= 1;
            hasTool = true;

            // create the tool and add it to the agent

            BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
            GameObject prefab = Resources.Load<GameObject>(backpack.toolType);
            GameObject tool = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
            backpack.tool = tool;
            tool.transform.parent = transform; // attach the tool

            return true;
        }
        else
        {
            // we got there but there was no tool available! Someone got there first. Cannot perform action
            return false;
        }
    }

}


