using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 家具行为
/// </summary>
public static class FurnitureActions
{
    public static void Door_UpdateAction(Furniture furn, float deltaTime)
    {
       Debug.Log("Door_UpdateAction: " + furn.furnParameters["openess"]);
        if (furn.furnParameters["is_opening"] >= 1)
        {
            furn.furnParameters["openess"] += deltaTime;
            if (furn.furnParameters["openess"] >= 1)
            {
                furn.furnParameters["is_opening"] = 0;
            }
        }
        else
        {
            furn.furnParameters["openess"] -= deltaTime;
        }

        furn.furnParameters["openess"] = Mathf.Clamp01(furn.furnParameters["openess"]);
    }

    public static Enterability Door_IsEnterable(Furniture furn)
    {
        furn.furnParameters["is_opening"] = 1;

        if (furn.furnParameters["openess"] >= 1)
        {
            return Enterability.Yes;
        }

        return Enterability.Soon;
    }
}
