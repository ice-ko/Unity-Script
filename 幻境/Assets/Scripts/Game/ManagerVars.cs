using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreatManagerVars")]
public class ManagerVars : ScriptableObject
{
    public List<Sprite> bgTheme;
    /// <summary>
    /// 普通平台
    /// </summary>
    public GameObject normalPlatform;
    /// <summary>
    /// 下一个位置的x,y
    /// </summary>
    public float nextXPos = 0.554f, nextYPos = 0.645f;

    public static ManagerVars GetManagerVars()
    {
        return Resources.Load<ManagerVars>("BgTheme");
    }
}
