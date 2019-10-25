using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBase
{
    /// <summary>
    /// 自身关心的消息集合
    /// </summary>
    private List<int> list = new List<int>();

    /// <summary>
    /// 绑定一个或多个消息
    /// </summary>
    /// <param name="eventCodes">Event codes.</param>
    protected void Bind(params int[] eventCodes)
    {
        list.AddRange(eventCodes);
        CharacterManager.Instance.Add(list.ToArray(), this);
    }

    /// <summary>
    /// 接触绑定的消息
    /// </summary>
    protected void UnBind()
    {
        CharacterManager.Instance.Remove(list.ToArray(), this);
        list.Clear();
    }

    /// <summary>
    /// 自动移除绑定的消息
    /// </summary>
    public  virtual void OnDestroy()
    {
        if (list != null)
            UnBind();
    }

    /// <summary>
    /// 发消息
    /// </summary>
    /// <param name="areaCode">Area code.</param>
    /// <param name="eventCode">Event code.</param>
    /// <param name="message">Message.</param>
    public void Dispatch(int areaCode, int eventCode, object message)
    {
        MsgCenter.Instance.Dispatch(areaCode, eventCode, message);
    }

}