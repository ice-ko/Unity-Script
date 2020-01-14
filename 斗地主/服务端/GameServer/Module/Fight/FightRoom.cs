using GameServer.Model;
using System;
using System.Collections.Generic;
using System.Text;
namespace GameServer.Module.Fight
{
    /// <summary>
    /// 战斗房间
    /// </summary>
    public class FightRoom
    {
        /// <summary>
        /// 房间标识
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 玩家列表
        /// </summary>
        public List<PlayerDto> playerList { get; set; }
        /// <summary>
        /// 中途退出的玩家列表
        /// </summary>
        public List<int> LeaveUIdList { get; set; }
        /// <summary>
        /// 牌库
        /// </summary>
        public LibraryModel LibraryModels { get; set; }
        /// <summary>
        /// 底牌
        /// </summary>
        public List<CardDto> TabkeCardList { get; set; }
        /// <summary>
        /// 倍数
        /// </summary>
        public int Multiple { get; set; }
        /// <summary>
        /// 回合管理信息
        /// </summary>
        public RoundModel RoundModel { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="id">房间id</param>
        /// <param name="uidList">用户列表</param>
        public FightRoom(int id, List<int> uidList)
        {
            this.Id = id;
            this.Multiple = 1;
            this.playerList = new List<PlayerDto>();
            foreach (var uid in uidList)
            {
                PlayerDto player = new PlayerDto(uid);
                this.playerList.Add(player);
            }
            this.LeaveUIdList = new List<int>();
            this.LibraryModels = new LibraryModel();
            this.TabkeCardList = new List<CardDto>();
            this.RoundModel = new RoundModel();
        }
        /// <summary>
        /// 转换出牌者
        /// </summary>
        public int Turn()
        {
            int currUId = RoundModel.CurrentUId;
            int nextUId = GetNextUId(currUId);
            //更改回合信息
            RoundModel.CurrentUId = nextUId;
            return nextUId;
        }
        /// <summary>
        /// 计算下一个出牌者
        /// </summary>
        /// <param name="currUId">当前出牌者id</param>
        /// <returns></returns>
        public int GetNextUId(int currUId)
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerList[i].UserId == currUId)
                {
                    if (i == 2)
                    {
                        return playerList[0].UserId;
                    }
                    else
                    {
                        return playerList[i + 1].UserId;
                    }
                }
            }
            throw new Exception("出牌者不存在");
        }
        /// <summary>
        /// 能否压上一次的出牌
        /// </summary>
        /// <param name="type"></param>
        /// <param name="length"></param>
        /// <param name="userId"></param>
        /// <param name="cardList"></param>
        /// <returns></returns>
        public bool DeadCard(CardType type, int weight, int length, int userId, List<CardDto> cardList)
        {
            bool canDeal = false;
            //用什么牌管什么牌 大管小
            if (type == RoundModel.LastCardType && weight > RoundModel.LastWeight)
            {
                //特殊牌型：顺子 进行长度限制
                if (type == CardType.Straight || type == CardType.Double_Straight || type == CardType.Triple_Straight)
                {
                    if (length == RoundModel.LastLength)
                    {
                        canDeal = true;
                    }
                }
                else
                {
                    canDeal = true;
                }
            }
            //普通的炸弹 可以管不是炸弹的牌
            else if (type == CardType.Boom && RoundModel.LastCardType != CardType.Boom)
            {
                canDeal = true;
            }
            //王炸管所有牌
            else if (type == CardType.Joker_Boom)
            {
                canDeal = true;
            }
            //出牌
            if (canDeal)
            {
                //移除手牌
                RemoveCard(userId, cardList);
                //炸弹倍数翻倍
                if (type == CardType.Boom)
                {
                    Multiple *= 4;
                }
                else if (type == CardType.Joker_Boom)
                {
                    Multiple *= 8;
                }
                //保存回合信息
                RoundModel.Change(length, type, weight, userId);
            }
            return canDeal;
        }
        /// <summary>
        /// 移除玩家手牌
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cardList"></param>
        private void RemoveCard(int userId, List<CardDto> cardList)
        {
            List<CardDto> currList = GetUserCard(userId);
            for (int i = currList.Count - 1; i >= 0; i--)
            {
                foreach (var item in cardList)
                {
                    if (currList[i].Name == item.Name)
                    {
                        currList.RemoveAt(i);
                    }
                }
            }
        }
        /// <summary>
        /// 获取玩家手牌
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<CardDto> GetUserCard(int userId)
        {
            foreach (var item in playerList)
            {
                if (item.UserId == userId)
                {
                    return item.CardList;
                }
            }
            throw new Exception("当前玩家不存在");
        }
        /// <summary>
        /// 发牌（初始化角色手牌）
        /// </summary>
        public void InitPlayerCards()
        {
            //每个人17张
            for (int i = 0; i < 17; i++)
            {
                CardDto card = LibraryModels.Deal();
                playerList[0].Add(card);
            }
            for (int i = 0; i < 17; i++)
            {
                CardDto card = LibraryModels.Deal();
                playerList[1].Add(card);
            }
            for (int i = 0; i < 17; i++)
            {
                CardDto card = LibraryModels.Deal();
                playerList[2].Add(card);
            }
            //发底牌
            for (int i = 0; i < 3; i++)
            {
                CardDto card = LibraryModels.Deal();
                TabkeCardList.Add(card);
            }
        }
        /// <summary>
        /// 设置地主身份
        /// </summary>
        public void SetLandlord(int userId)
        {
            foreach (var item in playerList)
            {
                if (item.UserId == userId)
                {
                    item.Identity = Identity.Landlord;
                    //发放底牌
                    for (int i = 0; i < TabkeCardList.Count; i++)
                    {
                        item.Add(TabkeCardList[i]);
                    }
                    //开始出牌
                    RoundModel.Start(userId);
                }
            }
        }
        /// <summary>
        /// 获取玩家数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public PlayerDto GetPlayerModel(int userId)
        {
            foreach (var item in playerList)
            {
                if (item.UserId == userId)
                {
                    return item;
                }
            }
            throw new Exception("当前玩家不存在，无法获取信息！");
        }
        /// <summary>
        /// 获取玩家角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Identity GetPlayerIdentity(int userId)
        {
            return GetPlayerModel(userId).Identity;
            throw new Exception("当前玩家不存在，无法获取角色信息！");
        }
        /// <summary>
        /// 获取相同身份的玩家id
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public List<int> GetSameIdentityUId(Identity identity)
        {
            List<int> id = new List<int>();
            foreach (var item in playerList)
            {
                if (item.Identity == identity)
                {
                    id.Add(item.UserId);
                }
            }
            return id;
        }
        /// <summary>
        /// 获取不同身份的玩家id
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public List<int> GetDifferentIdentityUId(Identity identity)
        {
            List<int> id = new List<int>();
            foreach (var item in playerList)
            {
                if (item.Identity != identity)
                {
                    id.Add(item.UserId);
                }
            }
            return id;
        }
        /// <summary>
        /// 获取房间内第一个玩家的id 
        /// </summary>
        /// <returns></returns>
        public int GetFirstUId()
        {
            return playerList[0].UserId;
        }
        /// <summary>
        /// 手牌排序
        /// </summary>
        /// <param name="cardsList"></param>
        /// <param name="asc"></param>
        private void SortCard(List<CardDto> cardsList, bool asc = true)
        {
            cardsList.Sort(delegate (CardDto a, CardDto b)
            {
                if (asc)
                {
                    return a.Weight.CompareTo(b.Weight);
                }
                else
                {
                    return a.Weight.CompareTo(b.Weight) * -1;
                }
            });
        }
        /// <summary>
        /// 排序 
        /// </summary>
        public void Sort(bool asc = true)
        {
            SortCard(playerList[0].CardList, asc);
            SortCard(playerList[1].CardList, asc);
            SortCard(playerList[2].CardList, asc);
        }
    }
}