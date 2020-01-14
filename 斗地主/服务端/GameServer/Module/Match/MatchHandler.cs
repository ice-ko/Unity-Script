using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Cache;
using GameServer.Model;
using Server;
using Server.Pool;
using System.Linq;

namespace GameServer.Module.Match
{
    /// <summary>
    /// 所有玩家准备完成后的委托
    /// </summary>
    /// <param name="uIdList"></param>
    public delegate void StartFight(List<int> uIdList);

    public class MatchHandler : IHandler
    {
        public StartFight startFight;

        private MatchCache matchCache = Caches.MatchCache;
        private UserCache userCache = Caches.User;


        SocketMsg socketMsg = new SocketMsg();
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="client"></param>
        public void OnDisconnect(ClientPeer client)
        {
            if (!userCache.IsOnline(client))
            {
                return;
            }
            int userId = userCache.GetClientUserId(client);
            if (matchCache.IsMatching(userId))
            {
                Leave(client);
            }
        }
        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        public void OnReceive(ClientPeer client, SocketMsg msg)
        {
            var code = (MatchCode)msg.SubCode;
            switch (code)
            {
                case MatchCode.EnterMatch_Request:
                    Enter(client);
                    break;
                case MatchCode.LeaveMatch_Request:
                    Leave(client);
                    break;
                case MatchCode.Ready_Request:
                    Ready(client);
                    break;
            }
        }
        /// <summary>
        /// 进入匹配
        /// </summary>
        /// <param name="client"></param>
        private void Enter(ClientPeer client)
        {
            SingleExecute.Instance.Execute(() =>
            {
                socketMsg.State = MatchCode.Success;

                int userId = userCache.GetClientUserId(client);
                //判断是否已在匹配房间
                if (matchCache.IsMatching(userId))
                {
                    socketMsg.OpCode = MsgType.Match;
                    socketMsg.SubCode = MatchCode.EnterMatch_Result;
                    socketMsg.State = MatchCode.Repeat_Match;
                    client.Send(socketMsg);
                    return;
                }
                //进入房间
                MatchRoom room = matchCache.Enter(userId, client);
                //广播信息
                socketMsg.OpCode = MsgType.Match;
                socketMsg.SubCode = MatchCode.EnterMatch_Broadcast_Result;
                //新进入房间的玩家信息
                var userInfo = userCache.GetUserInfo(userId);
                socketMsg.value = EntityHelper.Mapping<UserCharacterDto, UserCharacterInfo>(userInfo);
                room.Brocast(socketMsg, client);
                //返回给客户端数据
                MatchRoomDto dto = MakeRoomDto(room);
                socketMsg.SubCode = MatchCode.EnterMatch_Result;
                socketMsg.value = dto;

                client.Send(socketMsg);
            });
        }
        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="client"></param>
        private void Leave(ClientPeer client)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (!userCache.IsOnline(client))
                {
                    return;
                }
                int userId = userCache.GetClientUserId(client);
                //没有匹配不能退出
                if (!matchCache.IsMatching(userId))
                {
                    return;
                }
                //正常离开 返回离开的用户标识
                MatchRoom room = matchCache.Leave(userId);
                socketMsg.OpCode = MsgType.Match;
                socketMsg.SubCode = MatchCode.LeaveMatch_Broadcast_Result;
                socketMsg.State = MatchCode.Success;
                socketMsg.value = userId;
                room.Brocast(socketMsg);
            });
        }
        /// <summary>
        /// 准备
        /// </summary>
        /// <param name="client"></param>
        private void Ready(ClientPeer client)
        {
            socketMsg.State = MatchCode.Success;
            socketMsg.OpCode = MsgType.Match;
            socketMsg.SubCode = MatchCode.Ready_Broadcast_Result;
            SingleExecute.Instance.Execute(() =>
            {
                if (!userCache.IsOnline(client))
                {
                    return;
                }
                int userId = userCache.GetClientUserId(client);
                if (!matchCache.IsMatching(userId))
                {
                    return;
                }
                //准备
                MatchRoom room = matchCache.GetRoom(userId);
                if (room.ReadyUIdList.Where(w => w == userId).ToList().Count > 0)
                {
                    return;
                }
                room.Ready(userId);
                socketMsg.value = userId;
                room.Brocast(socketMsg);

                //检测是否所有玩家都准备了
                if (room.IsAllReady())
                {
                    //开始游戏
                    startFight(room.ReadyUIdList);
                    //广播通知所有玩家游戏开始
                    socketMsg.SubCode = MatchCode.Start_Broadcast_Requst;
                    room.Brocast(socketMsg);
                    //销毁房间
                    matchCache.Destroy(room);
                }
            });
        }
        private MatchRoomDto MakeRoomDto(MatchRoom room)
        {
            MatchRoomDto dto = new MatchRoomDto();
            //给 UIdClientDict 赋值
            foreach (var uId in room.UIdClientDict.Keys)
            {
                var info = userCache.GetUserInfo(uId);
                UserCharacterDto userCharacter = EntityHelper.Mapping<UserCharacterDto, UserCharacterInfo>(info);
                dto.UIdClientDict.Add(uId, userCharacter);
                dto.UIdList.Add(uId);
            }
            dto.ReadyUIdList = room.ReadyUIdList;
            return dto;
        }
    }
}
