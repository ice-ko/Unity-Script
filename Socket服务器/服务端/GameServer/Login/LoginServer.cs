using System;
using System.Collections.Generic;
using System.Text;
using Components.Code;
using Components.Model;
using GameServer.Cache;
using Server;
using Server.Pool;
using SqlSugar;

namespace GameServer.Login
{
    /// <summary>
    /// 登录服务
    /// </summary>
    public class LoginServer : ILoginServer
    {
        AccountCache accountCache = Caches.Account;

        public void OnDisconnect(ClientPeer client)
        {
            if (accountCache.IsOnline(client))
                accountCache.Offline(client);
        }

        public void OnReceive(ClientPeer client, SocketMsg msg)
        {
            var info = msg.value as UserInfo;
            switch (msg.SubCode)
            {
                case (int)MsgType.Login:
                    Login(client, info); break;
                case (int)MsgType.Registe:
                    Regist(client, info); break;
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="client"></param>
        /// <param name="info"></param>
        private void Regist(ClientPeer client, UserInfo info)
        {
            SingleExecute.Instance.Execute(() =>
            {
                var msg = new SocketMsg
                {
                    OpCode = (int)MsgType.Account,
                    SubCode = (int)MsgType.Regist_Check,
                    value = MsgType.Success
                };
                if (string.IsNullOrEmpty(info.Account))
                {
                    msg.value = MsgType.AccountEntryIsIllegal;
                    client.Send(msg);
                    return;
                }

                if (string.IsNullOrEmpty(info.Password) || info.Password.Length < 4 || info.Password.Length > 16)
                {
                    msg.value = MsgType.ThePasswordIsIllegal;
                    client.Send(msg);
                    return;
                }
                //查询数据库是否存在当前账号
                var db = DatabaseHelper.GetInstance();
                var userInfo = db.Queryable<UserInfo>().Where(w => w.Account == info.Account).ToList();
                if (userInfo.Count > 0)
                {
                    msg.value = MsgType.AccountAlreadyExists;
                    client.Send(msg);
                    return;
                }

                //可以注册了 放入缓存
                //accountCache.Create(info.Account, info.Password);
                try
                {
                    var user = new UserInfo
                    {
                        Account = info.Account,
                        Password = info.Password
                    };
                    db.Insertable(user).ExecuteCommand();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }

                client.Send(msg);
            });
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="client"></param>
        /// <param name="info"></param>
        private void Login(ClientPeer client, UserInfo info)
        {
            SingleExecute.Instance.Execute(() =>
            {
                var msg = new SocketMsg
                {
                    OpCode = (int)MsgType.Account,
                    SubCode = (int)MsgType.Login_Check,
                    value = MsgType.Success
                };
                //if (!accountCache.IsExist(info.Account))
                //{
                //    msg.value = MsgType.AccountDoesNotExist;
                //    client.Send(msg);
                //    return;
                //}

                //if (accountCache.IsOnline(info.Account))
                //{
                //    msg.value = MsgType.AccountOnline;
                //    client.Send(msg);
                //    return;
                //}

                //if (!accountCache.IsMatch(info.Account, info.Password))
                //{
                //    msg.value = MsgType.AccountPasswordDoesNotMatch;
                //    client.Send(msg);
                //    return;
                //}
                //查询数据库是否存在当前账号
                var db = DatabaseHelper.GetInstance();
                var userInfo = db.Queryable<UserInfo>().Where(w => w.Account == info.Account && w.Password == info.Password).First();
                if (userInfo == null)
                {
                    msg.value = MsgType.AccountPasswordDoesNotMatch;
                    client.Send(msg);
                    return;
                }
                //登陆成功 放入缓存
                //accountCache.Online(client, info.Account);
                client.Send(msg);
            });
        }
    }
}
