using GameServer.Module.Match;
using Server;
using Server.Concurrent;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Cache
{
    /// <summary>
    /// 匹配缓存
    /// </summary>
    public class MatchCache
    {
        /// <summary>
        /// 储存正在等待的用户id和房间id
        /// </summary>
        private Dictionary<int, int> uIdRoomIdDict = new Dictionary<int, int>();
        /// <summary>
        /// 匹配中的房间信息
        /// </summary>
        private Dictionary<int, MatchRoom> matchRoom = new Dictionary<int, MatchRoom>();
        /// <summary>
        /// 房间队列
        /// </summary>
        private Queue<MatchRoom> roomQueue = new Queue<MatchRoom>();
        /// <summary>
        /// 房间id
        /// </summary>
        private ConcurrentInt id = new ConcurrentInt(-1);

        /// <summary>
        /// 进入匹配房间
        /// </summary>
        /// <returns></returns>
        public MatchRoom Enter(int userId, ClientPeer client)
        {
            foreach (var mr in matchRoom.Values)
            {
                if (mr.IsFull())
                {
                    continue;
                }
                mr.Enter(userId, client);
                uIdRoomIdDict.Add(userId, mr.Id);
                return mr;
            }
            //没有等待中的房间 开启新的房间
            MatchRoom room = null;
            if (roomQueue.Count > 0)
            {
                room = roomQueue.Dequeue();
            }
            else
            {
                room = new MatchRoom(id.Add_Get());
            }
            room.Enter(userId, client);
            //正在匹配的用户
            uIdRoomIdDict.Add(userId, room.Id);
            //房间信息
            matchRoom.Add(room.Id, room);
            return room;
        }
        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public MatchRoom Leave(int userId)
        {
            int roomId = uIdRoomIdDict[userId];
            MatchRoom room = matchRoom[roomId];
            room.Leave(userId);
            uIdRoomIdDict.Remove(userId);
            if (room.IsEmpty())
            {
                matchRoom.Remove(roomId);
                //放入房间队列
                roomQueue.Enqueue(room);
            }
            return room;
        }
        /// <summary>
        /// 检测用户是否已匹配
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsMatching(int id)
        {
            return uIdRoomIdDict.ContainsKey(id);
        }
        /// <summary>
        /// 获取用户所在的等待房间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public MatchRoom GetRoom(int userId)
        {
            int roomId = uIdRoomIdDict[userId];
            return matchRoom[roomId];
        }
        /// <summary>
        /// 摧毁房间
        /// </summary>
        /// <param name="room"></param>
        public void Destroy(MatchRoom room)
        {
            matchRoom.Remove(room.Id);
            foreach (var item in room.UIdClientDict.Keys)
            {
                uIdRoomIdDict.Remove(item);
            }
            room.UIdClientDict.Clear();
            room.ReadyUIdList.Clear();
            //放入房间队列
            roomQueue.Enqueue(room);
        }
    }
}
