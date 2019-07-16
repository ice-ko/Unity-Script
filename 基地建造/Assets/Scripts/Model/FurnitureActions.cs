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

        if (furn.furnParameters["is_opening"] >= 1)
        {
            furn.furnParameters["openness"] += deltaTime * 4;   // FIXME: Maybe a door open speed parameter?
            if (furn.furnParameters["openness"] >= 1)
            {
                furn.furnParameters["is_opening"] = 0;
            }
        }
        else
        {
            furn.furnParameters["openness"] -= deltaTime * 4;
        }

        furn.furnParameters["openness"] = Mathf.Clamp01(furn.furnParameters["openness"]);

        if (furn.cbOnChanged != null)
        {
            furn.cbOnChanged(furn);
        }
    }

    public static Enterability Door_IsEnterable(Furniture furn)
    {
        furn.furnParameters["is_opening"] = 1;

        if (furn.furnParameters["openness"] >= 1)
        {
            return Enterability.Yes;
        }

        return Enterability.Soon;
    }
}
