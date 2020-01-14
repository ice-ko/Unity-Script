using System;
using System.Collections.Generic;
using System.Text;
using Server;
using GameServer.Cache;
using Server.Pool;
using GameServer.Model;

namespace GameServer.Module.Fight
{
    public class FightHandler : IHandler
    {
        private FightCache FightCache = Caches.FightCache;
        private UserCache UserCache = Caches.User;

        SocketMsg socketMsg = new SocketMsg
        {
            OpCode = MsgType.Fight,
            State = FightCode.Success
        };
        public void OnDisconnect(ClientPeer client)
        {
            Leave(client);
        }

        public void OnReceive(ClientPeer client, SocketMsg msg)
        {
            var code = (FightCode)msg.SubCode;
            switch (code)
            {
                case FightCode.Grab_Landlord_CREQ:
                    //如果是true就是抢地主 如果是false就是不抢
                    bool result = (bool)msg.value;
                    GrabLandlord(client, result);
                    break;
                case FightCode.Deal_Creq:
                    Deal(client, msg.value as DealDto);
                    break;
                case FightCode.Pass_Creq:
                    Pass(client);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 用户离开
        /// </summary>
        /// <param name="client"></param>
        private void Leave(ClientPeer client)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (UserCache.IsOnline(client) == false)
                {
                    socketMsg.State = null;
                    Console.WriteLine("当前用户不在线，不能从房间中剔除！");
                    return;
                }
                int userId = UserCache.GetClientUserId(client);
                if (FightCache.IsFighting(userId) == false)
                {
                    return;
                }
                FightRoom room = FightCache.GetRoomByUId(userId);
                //中途退出房间的玩家
                room.LeaveUIdList.Add(userId);
                //
                socketMsg.SubCode = FightCode.Leave_Bro;
                socketMsg.value = userId;
                BroCast(room, socketMsg);
                //当前房间玩家是否都退出
                if (room.LeaveUIdList.Count == 3)
                {
                    for (int i = 0; i < room.LeaveUIdList.Count; i++)
                    {
                        UserCharacterInfo user = UserCache.GetUserInfo(room.LeaveUIdList[i]);
                        user.RunCount++;
                        user.Been -= (room.Multiple * 1000) * 3;
                        user.Exp += 0;
                        UserCache.Update(user);
                    }
                    FightCache.Destroy(room);
                }
            });
        }
        /// <summary>
        /// 发牌
        /// </summary>
        private void Deal(ClientPeer client, DealDto dto)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (UserCache.IsOnline(client) == false)
                {
                    socketMsg.State = null;
                    return;
                }
                int userId = UserCache.GetClientUserId(client);
                FightRoom room = FightCache.GetRoomByUId(userId);
                //玩家出牌、玩家掉线
                if (room.LeaveUIdList.Contains(userId))
                {
                    Turn(room);
                }
                bool canDeal = room.DeadCard(dto.Type, dto.Weight, dto.Length, userId, dto.SelectCardList);
                if (canDeal == false)
                {
                    socketMsg.State = FightCode.必须大于上次一次出牌;
                    socketMsg.SubCode = FightCode.Deal_Result;
                    client.Send(socketMsg);
                    return;
                }
                else
                {
                    //返回客户端出牌成功
                    socketMsg.State = FightCode.Success;
                    socketMsg.SubCode = FightCode.Deal_Result;
                    client.Send(socketMsg);
                    //广播出牌结果
                    socketMsg.value = dto;
                    BroCast(room, socketMsg, client);
                    //检查剩余手牌
                    List<CardDto> remainCardList = room.GetPlayerModel(userId).CardList;
                    if (remainCardList.Count == 0)
                    {
                        //游戏结束
                        GameOver(userId, room);
                    }
                    else
                    {
                        Turn(room);
                    }
                }
            });
        }
        /// <summary>
        /// 不出
        /// </summary>
        /// <param name="client"></param>
        private void Pass(ClientPeer client)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (UserCache.IsOnline(client) == false)
                {
                    socketMsg.State = null;
                    return;
                }
                int userId = UserCache.GetClientUserId(client);
                FightRoom room = FightCache.GetRoomByUId(userId);
                socketMsg.SubCode = FightCode.Pass_Result;
                //当前玩家是最大的出牌者 不能不出牌
                if (room.RoundModel.BiggestUId == userId)
                {
                    socketMsg.State = FightCode.出牌;
                    client.Send(socketMsg);
                    return;
                }
                else
                {
                    socketMsg.State = FightCode.Success;
                    client.Send(socketMsg);
                    Turn(room);
                }

            });
        }
        /// <summary>
        /// 游戏结束
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="room"></param>
        private void GameOver(int userId, FightRoom room)
        {
            //获取获胜玩家身份
            Identity winIdentity = room.GetPlayerIdentity(userId);
            List<int> winUIds = room.GetSameIdentityUId(winIdentity);
            //豆子
            int winBeen = room.Multiple * 1000;
            //给胜利玩家添加胜场
            for (int i = 0; i < winUIds.Count; i++)
            {
                UserCharacterInfo user = UserCache.GetUserInfo(winUIds[i]);
                user.WinCount++;
                user.Been += winBeen;
                user.Exp += 100;
                UserCache.Update(user);
            }
            List<int> loseUIds = room.GetDifferentIdentityUId(winIdentity);
            //给失败玩家添加负场
            for (int i = 0; i < loseUIds.Count; i++)
            {
                UserCharacterInfo user = UserCache.GetUserInfo(loseUIds[i]);
                user.LoseCount++;
                user.Been -= winBeen;
                user.Exp += 10;
                UserCache.Update(user);
            }
            //逃跑玩家添加逃跑场次
            for (int i = 0; i < room.LeaveUIdList.Count; i++)
            {
                UserCharacterInfo user = UserCache.GetUserInfo(loseUIds[i]);
                user.RunCount++;
                user.Been -= (winBeen) * 3;
                user.Exp += 0;
                UserCache.Update(user);
            }
            //发送广播
            OverDto dto = new OverDto
            {
                WinIdentity = winIdentity,
                WinUIdList = winUIds,
                BeenCount = winBeen
            };
            socketMsg.SubCode = FightCode.Over_Bro;
            socketMsg.value = dto;
            BroCast(room, socketMsg);
            //销毁房间
            FightCache.Destroy(room);
        }
        /// <summary>
        /// 转换出牌者
        /// </summary>
        /// <param name="room"></param>
        private void Turn(FightRoom room)
        {
            //下一个出牌的id
            int nextUId = room.Turn();
            //如果下一个玩家掉线
            if (room.LeaveUIdList.Contains(nextUId))
            {

            }
            else
            {
                ClientPeer nextClient = UserCache.GetClient(nextUId);
                socketMsg.State = FightCode.Success;
                socketMsg.SubCode = FightCode.Turn_Deal_Bro;
                socketMsg.value = nextUId;
                nextClient.Send(socketMsg);
            }
        }
        /// <summary>
        /// 抢地主
        /// </summary>
        /// <param name="client"></param>
        /// <param name="resulit"></param>
        private void GrabLandlord(ClientPeer client, bool resulit)
        {
            SingleExecute.Instance.Execute(() =>
            {

                if (UserCache.IsOnline(client) == false)
                {
                    socketMsg.State = null;
                    return;
                }
                //获取用户id
                int userId = UserCache.GetClientUserId(client);
                FightRoom room = FightCache.GetRoomByUId(userId);
                if (resulit == true)
                {
                    //抢地主
                    room.SetLandlord(userId);
                    //发放底牌
                    GrabDto dto = new GrabDto
                    {
                        UserId = userId,
                        TableCardList = room.TabkeCardList,
                        PlayerCardList = room.GetUserCard(userId)
                    };
                    socketMsg.SubCode = FightCode.Grab_Landlord_Bro;
                    socketMsg.value = dto;
                    BroCast(room, socketMsg);
                }
                else
                {
                    //不抢
                    int nextUID = room.GetNextUId(userId);
                    socketMsg.SubCode = FightCode.Turn_Grab_Bro;
                    socketMsg.value = nextUID;
                    BroCast(room, socketMsg);
                }
            });
        }
        /// <summary>
        /// 开始发牌、抢地主
        /// </summary>
        /// <param name="uidList"></param>
        public void StartFight(List<int> uidList)
        {
            SingleExecute.Instance.Execute(() =>
            {
                FightRoom room = FightCache.Create(uidList);
                room.InitPlayerCards();
                room.Sort();
                //返回给客户端
                foreach (var uid in uidList)
                {
                    ClientPeer client = UserCache.GetClient(uid);
                    List<CardDto> cardDtos = room.GetUserCard(uid);
                    client.Send(new SocketMsg
                    {
                        OpCode = MsgType.Fight,
                        SubCode = FightCode.Get_Card_Result,
                        value = cardDtos
                    });
                }
                //开始抢地主
                int firstUserId = room.GetFirstUId();
                var socketMsg = new SocketMsg
                {
                    OpCode = MsgType.Fight,
                    SubCode = FightCode.Turn_Grab_Bro,
                    value = firstUserId
                };
                BroCast(room, socketMsg);
            });
        }
        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="room"></param>
        /// <param name="socketMsg"></param>
        /// <param name="client"></param>
        private void BroCast(FightRoom room, SocketMsg socketMsg, ClientPeer client = null)
        {
            byte[] data = EncodeHelper.EncodeMsg(socketMsg);
            byte[] packet = EncodeHelper.EncodePacket(data);
            foreach (var user in room.playerList)
            {
                ClientPeer clientPeer = UserCache.GetClient(user.UserId);
                if (clientPeer == null || clientPeer == client)
                {
                    continue;
                }
                clientPeer.Send(packet);
            }
        }
    }
}
