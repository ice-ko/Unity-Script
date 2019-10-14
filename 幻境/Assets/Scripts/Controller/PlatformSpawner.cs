using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public Vector3 startSpawner;

    private int spawnPlatformCount;
    private Vector3 platformSpawnPosition;
    private bool isLeftSpawn=false;

    private ManagerVars vars;
    void Start()
    {
        platformSpawnPosition = startSpawner;
        vars = ManagerVars.GetManagerVars();
        for (int i = 0; i < 5; i++)
        {
            spawnPlatformCount = 5;
            DecidePath();
        }
    }
    void Update()
    {

    }
    /// <summary>
    /// 确定路径
    /// </summary>
    private void DecidePath()
    {
        if (spawnPlatformCount > 0)
        {
            spawnPlatformCount--;
        }
        else
        {
            isLeftSpawn = !isLeftSpawn;
            spawnPlatformCount = UnityEngine.Random.Range(1, 4);
            SpawnPlatform();
        }
    }
    /// <summary>
    /// 生成平台
    /// </summary>
    private void SpawnPlatform()
    {
        GameObject go = Instantiate(vars.normalPlatform);
        go.transform.position = platformSpawnPosition;
        //向左生成
        if (isLeftSpawn)
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos, platformSpawnPosition.y + vars.nextYPos,0);
        }
        else
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos, platformSpawnPosition.y + vars.nextYPos, 0);
        }
    }
}
