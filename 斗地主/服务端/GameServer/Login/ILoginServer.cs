using Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Login
{
    public interface ILoginServer
    {
        void OnReceive(ClientPeer client, SocketMsg msg);
        void OnDisconnect(ClientPeer client);
    }
}
