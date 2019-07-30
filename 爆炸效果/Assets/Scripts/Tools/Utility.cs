using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 通用类
/// </summary>
public class Utility
{
    /// <summary>
    /// 屏幕坐标转换为世界坐标
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetMouseWorldPos()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }
    /// <summary>
    /// UGUI屏幕坐标转UI坐标
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetWorldToScreenPos()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GameObject.Find("BackpackCanvas").transform as RectTransform, Input.mousePosition, null, out position);
        return position;
    }
}
