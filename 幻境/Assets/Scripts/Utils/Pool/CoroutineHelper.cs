using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 协程助手
/// </summary>
public class CoroutineHelper
{
    /// <summary>
    /// 回收对象池协程
    /// </summary>
    /// <returns></returns>
    public static IEnumerator RecoverPoolCoroutine(GameObject go)
    {
        yield return new WaitForSeconds(0.5f);
        SimplePool.Despawn(go);
    }
    /// <summary>
    /// 延迟隐藏
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public static IEnumerator DealyHide(GameObject go)
    {
        yield return new WaitForSeconds(1f);
        go.SetActive(false);
    }
    /// <summary>
    /// 延迟广播事件
    /// </summary>
    /// <param name="eventType">广播类型</param>
    /// <returns></returns>
    public static IEnumerator DealyBroadcast(EventType eventType)
    {
        yield return new WaitForSeconds(0.8f);
        EventCenter.Broadcast(eventType);
    }
}