using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTap : MonoBehaviour
{
    public bool isMainMenu;
    public GameObject water;
    public float timeToSpawnWater;

    private float timeToSpawn = 0.03f;
    private float spawnTime;

    void Start()
    {

    }

    void Update()
    {
        if (timeToSpawnWater <= 0)
        {
            return;
        }
        timeToSpawnWater -= Time.deltaTime;
        if (spawnTime + timeToSpawn < Time.time)
        {
            float r = Random.Range(transform.position.x - 0.1f, transform.position.x + 0.1f);
            GameObject go = Instantiate(water, new Vector2(r, transform.position.y), Quaternion.identity);
            spawnTime = Time.time;
        }
    }
}
