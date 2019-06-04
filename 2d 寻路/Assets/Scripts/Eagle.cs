using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Eagle : MonoBehaviour
{
    public AIPath aiPath;
    void Start()
    {

    }
    void Update()
    {
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
