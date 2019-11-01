using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Cache;
using GameServer.Model;
using Server;
using Server.Pool;

namespace GameServer.Module.Login
{
    public class UserHandler : IHandler
    {
        private UserCache userCache = Caches.User;
        private AccountCache accountCache = Caches.Account;

        SocketMsg msg = new SocketMsg();
        public void OnDisconnect(ClientPeer client)
        {
            if (userCache.IsOnline(client))
            {
                userCache.Offline(client);
            }
        }

        public void OnReceive(ClientPeer client, SocketMsg msg)
        {
            var code = (UserCode)msg.SubCode;
            switch (msg.SubCode)
            {
                case UserCode.CreateCharacterRequest:
                    Create(client, msg.value.ToString());
                    break;
                case UserCode.GetInfoRequest:
                    GetCharacterInfo(client);
                    break;
                case UserCode.OnlineRequest:
                    Online(client);
                    break;
            }
        }
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="client">客户端连接对象</param>
        /// <param name="name">用户名</param>
        private void Create(ClientPeer client, string name)
        {
            SingleExecute.Instance.Execute(() =>
            {
                //判断是否在线
                if (!accountCache.IsOnline(client))
                {
                    msg.OpCode =MsgType.User;
                    msg.SubCode = UserCode.CreateCharacterResult;
                    msg.State = UserCode.AccountNotOnline;
                    client.Send(msg);
                    return;
                }
                int id = accountCache.GetId(client);
                //判断当前账号是否有角色
                if (userCache.IsExist(id))
                {
                    msg.OpCode =MsgType.User;
                    msg.SubCode = UserCode.CreateCharacterResult;
                    msg.State = UserCode.UserExist;
                    client.Send(msg);
                    return;
                }
                //创建角色
                userCache.Create(name, id);

                msg.OpCode = MsgType.User;
                msg.SubCode = UserCode.CreateCharacterResult;
                msg.State = UserCode.Success;
                client.Send(msg);
            });
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="client"></param>
        private void GetCharacterInfo(ClientPeer client)
        {
            SingleExecute.Instance.Execute(() =>
            {
                //判断是否在线
                if (!accountCache.IsOnline(client))
                {
                    msg.OpCode =MsgType.User;
                    msg.SubCode = UserCode.GetInfoResult;
                    msg.State =UserCode.AccountNotOnline;
                    client.Send(msg);
                    return;
                }
                int id = accountCache.GetId(client);
                if (!userCache.IsExist(id))
                {
                    msg.OpCode = MsgType.User;
                    msg.SubCode = UserCode.GetInfoResult;
                    msg.State = UserCode.UserExist;
                    client.Send(msg);
                    return;
                }
                //角色上线
                Online(client);
                //
                UserCharacterInfo user = userCache.GetUserInfo(id);

                UserCharacterDto userCharacter =EntityHelper.Mapping<UserCharacterDto, UserCharacterInfo>(user);

                msg.OpCode = MsgType.User;
                msg.SubCode =UserCode.GetInfoResult;
                msg.State = UserCode.Success;
                msg.value = userCharacter;
                client.Send(msg);
            });
        }
        /// <summary>
        /// 角色上线
        /// </summary>
        /// <param name="client"></param>
        private void Online(ClientPeer client)
        {
            SingleExecute.Instance.Execute(() =>
            {
                //判断是否在线
                if (!accountCache.IsOnline(client))
                {
                    msg.OpCode = MsgType.User;
                    msg.SubCode = UserCode.OnlineResult;
                    msg.State = UserCode.AccountNotOnline;
                    client.Send(msg);
                    return;
                }
                int id = accountCache.GetId(client);
                if (!userCache.IsExist(id))
                {
                    msg.OpCode = MsgType.User;
                    msg.SubCode =UserCode.OnlineResult;
                    msg.State = UserCode.UserExist;
                    client.Send(msg);
                }
                UserCharacterInfo userId = userCache.GetUserInfo(id);
                userCache.Online(client, userId.Id);

                //msg.OpCode = (int)MsgType.User;
                //msg.SubCode = (int)UserCode.OnlineResult;
                //client.Send(msg);
            });

        }
    }
}
