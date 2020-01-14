using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Cache;
using GameServer.Module.Match;
using Server;

namespace GameServer.Module.Chat
{

    public class ChatHandler : IHandler
    {
        UserCache userCache = Caches.User;
        MatchCache matchCache = Caches.MatchCache;

        SocketMsg socketMsg = new SocketMsg();
        public void OnDisconnect(ClientPeer client)
        {
            
        }

        public void OnReceive(ClientPeer client, SocketMsg msg)
        {
            ChatCode code = (ChatCode)msg.SubCode;
            switch (code)
            {
                case ChatCode.Default:
                    ChatRequest(client, msg);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="client"></param>
        /// <param name="socketMsg"></param>
        private void ChatRequest(ClientPeer client, SocketMsg socketMsg)
        {
            if (!userCache.IsOnline(client))
            {
                return;
            }
            int userId = userCache.GetClientUserId(client);
            ChatDto chatDto = new ChatDto
            {
                UserId = userId,
                Type = (int)socketMsg.value
            };
            //向当前匹配房间所有玩家广播消息
            if (matchCache.IsMatching(userId))
            {
                MatchRoom room = matchCache.GetRoom(userId);
                socketMsg.OpCode = MsgType.Chat;
                socketMsg.SubCode = ChatCode.Result;
                socketMsg.value = chatDto;
                room.Brocast(socketMsg);
            }
            else if (true)
            {

            }
        }
    }
}
