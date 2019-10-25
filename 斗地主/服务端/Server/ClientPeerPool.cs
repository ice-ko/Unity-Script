using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    /// <summary>
    /// 连接池
    /// </summary>
    public class ClientPeerPool
    {
        private Queue<ClientPeer> clientPeersQueue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity">池子容量</param>
        public ClientPeerPool(int capacity)
        {
            clientPeersQueue = new Queue<ClientPeer>(capacity);
        }
        /// <summary>
        /// 添加队列
        /// </summary>
        public void Enqueue(ClientPeer client)
        {
            clientPeersQueue.Enqueue(client);
        }
        /// <summary>
        /// 获取队列
        /// </summary>
        /// <returns></returns>
        public ClientPeer Dequeue()
        {
            return clientPeersQueue.Dequeue();
        }
    }
}
