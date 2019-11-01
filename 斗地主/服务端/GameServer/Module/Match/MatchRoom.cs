using Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Module.Match
{
    /// <summary>
    /// 匹配房间
    /// </summary>
    public class MatchRoom
    {
        public int Id;
        /// <summary>
        /// 房间内的用户列表
        /// </summary>
        public Dictionary<int, ClientPeer> UIdClientDict;
        /// <summary>
        /// 已经准备的玩家ID列表
        /// </summary>
        public List<int> ReadyUIdList;
        public MatchRoom(int id)
        {
            this.Id = id;
            this.UIdClientDict = new Dictionary<int, ClientPeer>();
            this.ReadyUIdList = new List<int>(0);
        }
        /// <summary>
        /// 房间是否满了
        /// </summary>
        public bool IsFull()
        {
            return UIdClientDict.Count == 3;
        }
        /// <summary>
        /// 房间是否空了
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return UIdClientDict.Count == 0;
        }
        /// <summary>
        /// 是否所有人都已准备
        /// </summary>
        /// <returns></returns>
        public bool IsAllReady()
        {
            return ReadyUIdList.Count == 3;
        }
        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="userId"></param>
        public void Enter(int userId, ClientPeer client)
        {
            UIdClientDict.Add(userId, client);
        }
        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="userId"></param>
        public void Leave(int userId)
        {
            UIdClientDict.Remove(userId);
        }
        /// <summary>
        /// 玩家准备
        /// </summary>
        public void Ready(int userId)
        {
            ReadyUIdList.Add(userId);
        }
        /// <summary>
        /// 广播发送信息给房间内所有玩家
        /// </summary>
        /// <param name="socket"></param>
        public void Brocast(SocketMsg socket,ClientPeer exClient=null)
        {
            byte[] data = EncodeHelper.EncodeMsg(socket);
            byte[] packet = EncodeHelper.EncodePacket(data);
            foreach (var client in UIdClientDict.Values)
            {
                if (client == exClient)
                {
                    continue; 
                }
                client.Send(packet);
            }
        }
    }
}
