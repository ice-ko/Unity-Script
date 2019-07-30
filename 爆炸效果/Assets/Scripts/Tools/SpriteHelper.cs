using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 精灵助手
/// </summary>
public class SpriteHelper
{
    public static SpriteHelper _instance;
    public static SpriteHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SpriteHelper();
            }
            return _instance;
        }
    }
    /// <summary>
    /// 加载精灵
    /// </summary>
    public Sprite LoadSprite(string name)
    {
        Sprite[] spPokers = Resources.LoadAll<Sprite>("farm/Crop_Spritesheet");
        Sprite sprite = null;
        foreach (var item in spPokers)
        {
            if (item.name == name)
            {
                sprite = item;
                break;
            }
        }
        return sprite;
    }
}
