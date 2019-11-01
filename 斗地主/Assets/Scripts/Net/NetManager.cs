using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NetManager : ManagerBase
{
    public static NetManager Instance = null;

    private  ClientPeer client = new ClientPeer("127.0.0.1", 9999);

    private void Start()
    {
        client.Connect();
    }

    private void Update()
    {
        if (client == null)
            return;

        while (client.socketMsgQueue.Count > 0)
        {
            SocketMsg msg = client.socketMsgQueue.Dequeue();
            //处理消息
            ProcessSocketMsg(msg);
        }
    }

    #region 处理接收到的服务器发来的消息

    HandleBase accountHandler = new AccoutHandle();
    HandleBase userHandler = new UserHandler();
    HandleBase matchHandler = new MatchHandler();
    /// <summary>
    /// 接受网络的消息
    /// </summary>
    private void ProcessSocketMsg(SocketMsg msg)
    {
        switch (msg.OpCode)
        {
            case MsgType.Account:
                accountHandler.OnReceive(msg);
                break;
            case MsgType.User:
                userHandler.OnReceive(msg);
                break;
            case MsgType.Match:
                matchHandler.OnReceive(msg);
                break;
        }
    }

    #endregion


    #region 处理客户端内部 给服务器发消息的 事件
    private void Awake()
    {
        Instance = this;

        Add(0, this);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case 0:
                client.Send(message as SocketMsg);
                break;
            default:
                break;
        }
    }

    #endregion

}