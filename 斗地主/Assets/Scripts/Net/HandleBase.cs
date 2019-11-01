using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HandleBase
{
    /// <summary>
    /// 收到数据时的处理
    /// </summary>
    /// <param name="msg"></param>
    public abstract void OnReceive(SocketMsg msg);
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="areaCode"></param>
    /// <param name="eventCode"></param>
    /// <param name="message"></param>
    protected void Dispatch(int areaCode, int eventCode, object message)
    {
        MsgCenter.Instance.Dispatch(areaCode, eventCode, message);
    }
}
