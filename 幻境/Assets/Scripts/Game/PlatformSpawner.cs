using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    /// <summary>
    /// 开始生成位置
    /// </summary>
    public Vector3 startSpawner;
    /// <summary>
    /// 里程碑计数
    /// </summary>
    public int milestoneCount = 10;
    public float fallTime;
    public float minFallTime;
    public float multiole;

    /// <summary>
    /// 生成平台数量
    /// </summary>
    private int spawnPlatformCount;
    /// <summary>
    /// 平台位置
    /// </summary>
    private Vector3 platformSpawnPosition;
    /// <summary>
    /// 是否向左生成，否则向右生成
    /// </summary>
    private bool isLeftSpawn = false;
    /// <summary>
    /// 选择的平台图
    /// </summary>
    private Sprite selectSprite;
    /// <summary>
    /// 钉子是否生成在左边，否则在右边
    /// </summary>
    private bool spikeSpawnLeft;
    /// <summary>
    /// 钉子方向位置
    /// </summary>
    private Vector3 spikeDirPlatformPos;
    /// <summary>
    /// 生成钉子之后需要在钉子方向生成的平台数
    /// </summary>
    private int afterSpawnSpikeSpawnCount;
    private bool isSpawnSpike = false;

    private ManagerVars vars;
    private PlatformGroupType groupType;
    private void Awake()
    {
        EventCenter.AddListener(EventType.DecidePath, DecidePath);
        vars = ManagerVars.GetManagerVars();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.DecidePath, DecidePath);
    }
    void Start()
    {
        RandomPlatform();
        platformSpawnPosition = startSpawner;
        for (int i = 0; i < 5; i++)
        {
            spawnPlatformCount = 5;
            DecidePath();
        }
        //生成人物
        GameObject go = Instantiate(vars.player);
        go.transform.position = new Vector3(0, -1.9f, 0);
    }
    void Update()
    {
        if (GameManager.Instance.IsGameStart && GameManager.Instance.IsGameOver == false)
        {
            UpdateFallTime();
        }
    }
    /// <summary>
    /// 更新平台掉落时间
    /// </summary>
    private void UpdateFallTime()
    {
        if (GameManager.Instance.GameScore > milestoneCount)
        {
            milestoneCount *= 2;
            fallTime *= multiole;
            if (fallTime < minFallTime)
            {
                fallTime = minFallTime;
            }
        }
    }
    /// <summary>
    /// 随机平台主题
    /// </summary>
    private void RandomPlatform()
    {
        int index = Random.Range(0, vars.platformThemeSpriteList.Count);
        selectSprite = vars.platformThemeSpriteList[index];

        if (index == 2)
        {
            groupType = PlatformGroupType.Winter;
        }
        else
        {
            groupType = PlatformGroupType.Grass;
        }
    }
    /// <summary>
    /// 确定路径
    /// </summary>
    private void DecidePath()
    {
        if (isSpawnSpike)
        {
            AfterSpikePlatform();
            return;
        }
        if (spawnPlatformCount > 0)
        {
            spawnPlatformCount--;
            SpawnPlatform();
        }
        else
        {
            //反正方向生成
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
        int ranObstacleDir = Random.Range(0, 2);
        //生成单个平台
        if (spawnPlatformCount >= 1)
        {
            SpawnNormalPlatform(ranObstacleDir);
        }
        //生成组合平台
        else if (spawnPlatformCount == 0)
        {
            int index = Random.Range(0, 3);
            //生成通用组合平台
            if (index == 0)
            {
                SpawnCommonPlatform(ranObstacleDir);
            }
            //生成主题组合平台
            else if (index == 1)
            {
                switch (groupType)
                {
                    case PlatformGroupType.Grass:
                        SpawnGrassPlatform(ranObstacleDir);
                        break;
                    case PlatformGroupType.Winter:
                        SpawnWinterPlatform(ranObstacleDir);
                        break;
                    default:
                        break;
                }
            }
            //生成钉子组合平台
            else
            {
                int value = -1;
                if (isLeftSpawn)
                {
                    value = 0;//生成右边方向得钉子
                }
                else
                {
                    value = 1;//生成左边方向得钉子
                }
                SpawnSpikePlatform(value);

                isSpawnSpike = true;
                afterSpawnSpikeSpawnCount = 4;

                //钉子在左边
                if (spikeSpawnLeft)
                {
                    spikeDirPlatformPos = new Vector3(platformSpawnPosition.x - 1.65f, platformSpawnPosition.y + vars.nextYPos, 0);
                }
                else
                {
                    spikeDirPlatformPos = new Vector3(platformSpawnPosition.x + 1.65f, platformSpawnPosition.y + vars.nextYPos, 0);
                }
            }
        }
        //生成钻石
        int ranSpawnDiamond = Random.Range(0, 10);
        if (ranSpawnDiamond == 6 && GameManager.Instance.PlayerIsMove)
        {
            GameObject go = SimplePool.Spawn(vars.diamond, new Vector3(platformSpawnPosition.x, platformSpawnPosition.y + 0.5f, 0));
        }
        //向左生成
        if (isLeftSpawn)
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos, platformSpawnPosition.y + vars.nextYPos, 0);
        }
        else
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos, platformSpawnPosition.y + vars.nextYPos, 0);
        }
    }
    /// <summary>
    /// 生成普通平台（单个）
    /// </summary>
    private void SpawnNormalPlatform(int ranObstacleDir)
    {
        //GameObject go = Instantiate(vars.normalPlatform);
        GameObject go = SimplePool.Spawn(vars.normalPlatform);
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectSprite, fallTime, ranObstacleDir);
    }
    /// <summary>
    /// 生成通用平台
    /// </summary>
    private void SpawnCommonPlatform(int ranObstacleDir)
    {
        int index = Random.Range(0, vars.commonPlarformGroup.Count);
        //GameObject go = Instantiate(vars.commonPlarformGroup[index]);
        GameObject go = SimplePool.Spawn(vars.commonPlarformGroup[index]);
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectSprite, fallTime, ranObstacleDir);
    }
    /// <summary>
    /// 生成草地平台
    /// </summary>
    private void SpawnGrassPlatform(int ranObstacleDir)
    {
        int index = Random.Range(0, vars.grassPlarformGroup.Count);
        //GameObject go = Instantiate(vars.grassPlarformGroup[index]);
        GameObject go = SimplePool.Spawn(vars.grassPlarformGroup[index]);
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectSprite, fallTime, ranObstacleDir);
    }
    /// <summary>
    /// 生成冬季平台
    /// </summary>
    private void SpawnWinterPlatform(int ranObstacleDir)
    {
        int index = Random.Range(0, vars.winterPlarformGroup.Count);
        //GameObject go = Instantiate(vars.winterPlarformGroup[index]);
        GameObject go = SimplePool.Spawn(vars.winterPlarformGroup[index]);
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectSprite, fallTime, ranObstacleDir);
    }
    /// <summary>
    /// 生成钉子平台
    /// </summary>
    private void SpawnSpikePlatform(int dir)
    {

        GameObject temp = null;
        if (dir == 0)
        {
            spikeSpawnLeft = false;
            //temp = Instantiate(vars.spikePlarform[1]);
            temp = SimplePool.Spawn(vars.spikePlarform[1]);
        }
        else
        {
            spikeSpawnLeft = true;
            //temp = Instantiate(vars.spikePlarform[0]);
            temp = SimplePool.Spawn(vars.spikePlarform[0]);
        }
        temp.transform.position = platformSpawnPosition;
        temp.GetComponent<PlatformScript>().Init(selectSprite, fallTime, dir);
    }
    /// <summary>
    /// 生成钉子平台之后需要生成的平台
    /// </summary>
    private void AfterSpikePlatform()
    {
        if (afterSpawnSpikeSpawnCount > 0)
        {
            afterSpawnSpikeSpawnCount--;
            for (int i = 0; i < 2; i++)
            {
                //GameObject temp = Instantiate(vars.normalPlatform);
                GameObject temp = SimplePool.Spawn(vars.normalPlatform);
                //原来方向的平台
                if (i == 0)
                {
                    temp.transform.position = platformSpawnPosition;
                    //如果钉子在左边，原先路径就是在右边
                    if (spikeSpawnLeft)
                    {
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos, platformSpawnPosition.y + vars.nextYPos, 0);
                    }
                    else
                    {
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos, platformSpawnPosition.y + vars.nextYPos, 0);
                    }
                }
                else//生成钉子方向
                {
                    temp.transform.position = spikeDirPlatformPos;
                    if (spikeSpawnLeft)
                    {

                        spikeDirPlatformPos = new Vector3(spikeDirPlatformPos.x - vars.nextXPos, spikeDirPlatformPos.y + vars.nextYPos, 0);
                    }
                    else
                    {
                        spikeDirPlatformPos = new Vector3(spikeDirPlatformPos.x + vars.nextXPos, spikeDirPlatformPos.y + vars.nextYPos, 0);
                    }
                }
                temp.GetComponent<PlatformScript>().Init(selectSprite, fallTime, 0);
            }
        }
        else
        {
            isSpawnSpike = false;
            DecidePath();
        }
    }
}
/// <summary>
/// 组合平台类型
/// </summary>
public enum PlatformGroupType
{
    /// <summary>
    /// 草地
    /// </summary>
    Grass,
    /// <summary>
    /// 冬季
    /// </summary>
    Winter
}