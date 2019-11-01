using Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Module
{
    public interface IHandler
    {
        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        void OnReceive(ClientPeer client, SocketMsg msg);
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="client"></param>
        void OnDisconnect(ClientPeer client);
    }
}
