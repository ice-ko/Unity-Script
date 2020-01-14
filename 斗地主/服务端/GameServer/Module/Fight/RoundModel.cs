using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Module.Fight
{
    /// <summary>
    /// 回合管理类
    /// </summary>
    public class RoundModel
    {
        /// <summary>
        /// 当前出牌者
        /// </summary>
        public int CurrentUId { get; set; }
        /// <summary>
        /// 当前回合出牌最大的出牌者
        /// </summary>
        public int BiggestUId { get; set; }
        /// <summary>
        /// 上次出牌的长度
        /// </summary>
        public int LastLength { get; set; }
        /// <summary>
        /// 上一次出牌的权值
        /// </summary>
        public int LastWeight { get; set; }
        /// <summary>
        /// 上一次出牌的类型
        /// </summary>
        public CardType LastCardType { get; set; }
        public RoundModel()
        {
            Init();
        }

        public void Init()
        {
            this.CurrentUId = -1;
            this.BiggestUId = -1;
            this.LastCardType = CardType.None;
            this.LastLength = -1;
            this.LastWeight = -1;
        }
        /// <summary>
        /// 开始出牌
        /// </summary>
        /// <param name="userId">要出牌的玩家id</param>
        public void Start(int userId)
        {
            this.CurrentUId = userId;
            this.BiggestUId = userId;
        }
        /// <summary>
        /// 改变出牌者
        /// </summary>
        /// <param name="length">当前出牌的所剩牌</param>
        /// <param name="type">出牌类型</param>
        /// <param name="weight">牌型</param>
        /// <param name="userId">当前出牌者</param>
        public void Change(int length, CardType type, int weight, int userId)
        {
            this.BiggestUId = userId;
            this.LastCardType = type;
            this.LastLength = length;
            this.LastWeight = weight;
        }
        /// <summary>
        /// 转换出牌者
        /// </summary>
        /// <param name="userId"></param>
        public void Turn(int userId)
        {
            this.CurrentUId = userId;
        }
    }
}
