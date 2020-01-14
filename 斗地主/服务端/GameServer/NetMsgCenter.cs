using GameServer.Login;
using GameServer.Module;
using GameServer.Module.Chat;
using GameServer.Module.Login;
using GameServer.Module.Match;
using GameServer.Module.Fight;
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
        MatchHandler matchServer = new MatchHandler();
        IHandler chatServer = new ChatHandler();
        FightHandler fightServer = new FightHandler();

        public NetMsgCenter()
        {
            matchServer.startFight += fightServer.StartFight;
        }
        public void onConnect(ClientPeer client)
        {
            throw new NotImplementedException();
        }

        public void OnDisconnect(ClientPeer client)
        {
            fightServer.OnDisconnect(client);
            chatServer.OnDisconnect(client);
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
                case MsgType.Chat:
                    chatServer.OnReceive(client, msg); break;
                case MsgType.Fight:
                    fightServer.OnReceive(client, msg); break;
            }
        }
    }
}
