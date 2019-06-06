using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassLine : MonoBehaviour
{
    int hitCount;
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            collision.tag = "WaterInGlass";
            collision.GetComponent<Rigidbody2D>().gravityScale = 0.3f;
            collision.GetComponent<Rigidbody2D>().velocity = collision.GetComponent<Rigidbody2D>().velocity / 4;
            hitCount++;
            if (hitCount > 60)
            {
                GetComponentInParent<Glass>().ChangeSprite(1);
            }
            else
            {
                GetComponentInParent<Glass>().ChangeSprite(0);
            }
        }
        
    }
}
