using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Model;
using Server;
using Server.Concurrent;

namespace GameServer.Cache
{
    /// <summary>
    /// 用户角色缓存
    /// </summary>
    public class UserCache
    {
        /// <summary>
        /// 用户角色信息
        /// </summary>
        private Dictionary<int, UserCharacterInfo> userInfo = new Dictionary<int, UserCharacterInfo>();

        ConcurrentInt id = new ConcurrentInt(-1);
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="name">角色名</param>
        /// <param name="id">账号id</param>
        public void Create(string name, int id)
        {
            UserCharacterInfo user = new UserCharacterInfo
            {
                Id = this.id.Add_Get(),
                Name = name,
            };
            userInfo.Add(user.Id, user);
        }
        /// <summary>
        /// 判断是否存在角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsExist(int id)
        {
            return userInfo.ContainsKey(id);
        }
        /// <summary>
        /// 根据id获取角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserCharacterInfo GetUserInfo(int id)
        {
            return userInfo[id];
        }
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="info"></param>
        public void Update(UserCharacterInfo info)
        {
            if (userInfo.ContainsKey(info.Id))
            {
                userInfo[info.Id] = info;
            }
        }
        /// <summary>
        /// 客户端是否在线
        /// </summary>
        private Dictionary<int, ClientPeer> idclientInfo = new Dictionary<int, ClientPeer>();
        private Dictionary<ClientPeer, int> clientInfo = new Dictionary<ClientPeer, int>();

        /// <summary>
        /// 客户端是否在线
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool IsOnline(ClientPeer client)
        {
            return clientInfo.ContainsKey(client);
        }
        /// <summary>
        /// 用户是否在线
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsOnline(int id)
        {
            return idclientInfo.ContainsKey(id);
        }
        /// <summary>
        /// 角色上线
        /// </summary>
        /// <param name="client"></param>
        /// <param name="id"></param>
        public void Online(ClientPeer client, int id)
        {
            idclientInfo.Add(id, client);
            clientInfo.Add(client, id);
        }
        /// <summary>
        /// 客户端下线
        /// </summary>
        /// <param name="client"></param>
        public void Offline(ClientPeer client)
        {
            int id = clientInfo[client];
            clientInfo.Remove(client);
            idclientInfo.Remove(id);
        }
        /// <summary>
        /// 根据连接对象获取角色信息
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public UserCharacterInfo GetClientPeer(ClientPeer client)
        {
            int id = clientInfo[client];
            return userInfo[id];
        }
        /// <summary>
        /// 根据用户id获取连接对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClientPeer GetClient(int id)
        {
            if (idclientInfo.ContainsKey(id))
            {
                return idclientInfo[id];
            }
            return null;
        }
        /// <summary>
        /// 根据连接对象获取用户id
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetClientUserId(ClientPeer  client)
        {
            return clientInfo[client];
        }
        
    }
}
