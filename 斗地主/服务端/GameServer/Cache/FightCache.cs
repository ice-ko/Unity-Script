using GameServer.Module.Fight;
using Server.Concurrent;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Cache
{
    /// <summary>
    /// 战斗缓存
    /// </summary>
    public class FightCache
    {
        /// <summary>
        /// 存储用户id、房间id
        /// </summary>
        private Dictionary<int, int> uidRoomDict = new Dictionary<int, int>();
        /// <summary>
        /// 存储房间id、房间信息
        /// </summary>
        private Dictionary<int, FightRoom> idRomDict = new Dictionary<int, FightRoom>();
        /// <summary>
        /// 房间队列
        /// </summary>
        private Queue<FightRoom> roomQueue = new Queue<FightRoom>();
        /// <summary>
        /// 房间id
        /// </summary>
        private ConcurrentInt id = new ConcurrentInt(-1);
        /// <summary>
        /// 创建房间
        /// </summary>
        /// <param name="uidList"></param>
        /// <returns></returns>
        public FightRoom Create(List<int> uidList)
        {
            FightRoom room = null;
            if (roomQueue.Count > 0)
            {
                room = roomQueue.Dequeue();
            }
            else
            {
                room = new FightRoom(id.Add_Get(), uidList);
            }
            foreach (var item in uidList)
            {
                uidRoomDict.Add(item, room.Id);
                //上一次销毁后的房间数据信息不完整这里补充完整重用销毁的房间
                if (room.playerList.Count < 3)
                {
                    PlayerDto player = new PlayerDto(item);
                    room.playerList.Add(player);
                }
            }
            idRomDict.Add(room.Id, room);
            return room;
        }
        /// <summary>
        /// 根据id获取房间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FightRoom GetRoom(int id)
        {
            if (idRomDict.ContainsKey(id) == false)
            {
                throw new Exception("房间不存在");
            }
            return idRomDict[id];
        }
        /// <summary>
        /// 当前用户是否在房间中
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsFighting(int userId)
        {
            return uidRoomDict.ContainsKey(userId);
        }
        /// <summary>
        /// 根据用户id获取房间信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public FightRoom GetRoomByUId(int userId)
        {
            if (uidRoomDict.ContainsKey(userId) == false)
            {
                throw new Exception("当前用户不在房间内");
            }
            int roomId = uidRoomDict[userId];
            return GetRoom(roomId);
        }
        /// <summary>
        /// 销毁房间
        /// </summary>
        /// <param name="room"></param>
        public void Destroy(FightRoom room)
        {
            idRomDict.Remove(room.Id);
            foreach (var item in room.playerList)
            {
                uidRoomDict.Remove(item.UserId);
            }
            //初始化
            room.playerList.Clear();
            room.LeaveUIdList.Clear();
            room.TabkeCardList.Clear();
            room.LibraryModels.Init();
            room.Multiple = 1;
            room.RoundModel.Init();
            //
            roomQueue.Enqueue(room);
        }
    }
}
