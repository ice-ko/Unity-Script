using GameServer.Login;
using GameServer.Module;
using GameServer.Module.Login;
using GameServer.Module.Match;
using Server;
using System;

namespace GameServer
{
    /// <summary>
    /// 网络消息中心
    /// </summary>
    public class NetMsgCenter : IApplication
    {
        IHandler loginServer = new LoginHandler();
        IHandler userServer = new UserHandler();
        IHandler matchServer = new MatchHandler();

        public void onConnect(ClientPeer client)
        {
            throw new NotImplementedException();
        }

        public void OnDisconnect(ClientPeer client)
        {
            matchServer.OnDisconnect(client);
            userServer.OnDisconnect(client);
            loginServer.OnDisconnect(client);
        }

        public void OnReceive(ClientPeer client, SocketMsg msg)
        {
            switch (msg.OpCode)
            {
                case MsgType.Account:
                    loginServer.OnReceive(client, msg); break;
                case MsgType.User:
                    userServer.OnReceive(client, msg); break;
                case MsgType.Match:
                    matchServer.OnReceive(client, msg); break;
            }
        }
    }
}
