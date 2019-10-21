using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreatManagerVars")]
public class ManagerVars : ScriptableObject
{
    public List<Sprite> bgTheme;
    public List<Sprite> platformThemeSpriteList;
    /// <summary>
    /// 普通平台
    /// </summary>
    public GameObject normalPlatform;
    public GameObject player;
    /// <summary>
    /// 死亡特效
    /// </summary>
    public GameObject deathEffect;
    /// <summary>
    /// 钻石
    /// </summary>
    public GameObject diamond;
    /// <summary>
    /// 通用平台
    /// </summary>
    public List<GameObject> commonPlarformGroup;
    /// <summary>
    /// 草主题
    /// </summary>
    public List<GameObject> grassPlarformGroup;
    /// <summary>
    /// 冬季主题
    /// </summary>
    public List<GameObject> winterPlarformGroup;
    /// <summary>
    /// 钉子平台
    /// </summary>
    public List<GameObject> spikePlarform;
    /// <summary>
    /// 下一个位置的x,y
    /// </summary>
    public float nextXPos = 0.554f, nextYPos = 0.645f;

    public static ManagerVars GetManagerVars()
    {
        return Resources.Load<ManagerVars>("ManagerVars");
    }
}
