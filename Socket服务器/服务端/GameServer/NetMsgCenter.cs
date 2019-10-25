using Components.Code;
using GameServer.Login;
using Server;
using System;

namespace GameServer
{
    /// <summary>
    /// 网络消息中心
    /// </summary>
    public class NetMsgCenter : IApplication
    {
        ILoginServer loginServer = new LoginServer();
        public void onConnect(ClientPeer client)
        {
            throw new NotImplementedException();
        }

        public void OnDisconnect(ClientPeer client)
        {
            loginServer.OnDisconnect(client);
        }

        public void OnReceive(ClientPeer client, SocketMsg msg)
        {
            switch (msg.OpCode)
            {
                case (int)MsgType.Account:
                    loginServer.OnReceive(client, msg); break;
            }
        }
    }
}
