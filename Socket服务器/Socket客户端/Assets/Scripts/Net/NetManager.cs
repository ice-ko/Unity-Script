using Components.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class NetManager : MonoBehaviour
{
    public static NetManager Instance;

    private ClientSocket client = new ClientSocket("127.0.0.1", 9999);
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        client.Connected();
    }
    private void Update()
    {
        if (client == null)
        {
            return;
        }
        while (client.socketMsgQueue.Count > 0)
        {
            SocketMsg msg = client.socketMsgQueue.Dequeue();
            ProcessSocketMsg(msg);
        }
    }

    #region 处理接收到的服务器发来的消息
    /// <summary>
    /// 注册账号服务
    /// </summary>
    HandleBase account = new AccoutHandle();
    /// <summary>
    /// 接收到的网络消息
    /// </summary>
    /// <param name="msg"></param>
    private void ProcessSocketMsg(SocketMsg msg)
    {
        switch (msg.OpCode)
        {
            case (int)MsgType.Account:
                account.OnReceive(msg.SubCode, msg.value); break;
        }
    }
    #endregion
    #region 发送数据
    /// <summary>
    /// 发送数据
    /// </summary>
    /// <param name="msg">消息</param>
    public void Send(SocketMsg msg)
    {
        client.Send(msg);
    }
    #endregion
}