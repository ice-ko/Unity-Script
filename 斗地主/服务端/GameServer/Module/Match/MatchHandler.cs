using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Cache;
using GameServer.Model;
using Server;
using Server.Pool;

namespace GameServer.Module.Match
{
    public class MatchHandler : IHandler
    {
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
                socketMsg.State = MatchCode.Success;
                room.Brocast(socketMsg);
                //返回给客户端数据
                MatchRoomDto roomDto = new MatchRoomDto
                {
                    ReadyUIdList = room.ReadyUIdList
                };

                foreach (var uId in room.UIdClientDict.Keys)
                {
                    var info = userCache.GetUserInfo(uId);
                    UserCharacterDto userCharacter = EntityHelper.Mapping<UserCharacterDto, UserCharacterInfo>(info);
                    roomDto.UIdClientDict.Add(uId, userCharacter);
                }
                socketMsg.value = roomDto;

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
                room.Ready(userId);
                socketMsg.OpCode = MsgType.Match;
                socketMsg.SubCode = MatchCode.Ready_Broadcast_Result;
                socketMsg.value = userId;
                room.Brocast(socketMsg,client);

                //检测是否所有玩家都准备了
                if (room.IsAllReady())
                {
                    //开始游戏
                    //TODO
                    //广播通知所有玩家游戏开始
                    socketMsg.OpCode = MsgType.Match;
                    socketMsg.SubCode = MatchCode.Ready_Broadcast_Result;
                    room.Brocast(socketMsg);
                    //销毁房间
                    matchCache.Destroy(room);
                }
            });
        }
    }
}
