using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Model
{
    /// <summary>
    /// 牌库
    /// </summary>
    public class LibraryModel
    {
        /// <summary>
        /// 牌队列
        /// </summary>
        public Queue<CardDto> CardQueue { get; set; }
        public LibraryModel()
        {
            Init();
        }
        public void Init()
        {
            Create();
            Shuffle();
        }
        /// <summary>
        /// 创建卡牌
        /// </summary>
        private void Create()
        {
            CardQueue = new Queue<CardDto>();
            //创建普通的牌
            for (int color = (int)CardColor.Club; color <= (int)CardColor.Square; color++)
            {
                for (int weight = (int)CardWeight.Three; weight <= (int)CardWeight.Two; weight++)
                {
                    CardDto card = new CardDto
                    {
                        Name = GetName(color) + GetWeight(weight),
                        Color = color,
                        Weight = (CardWeight)weight
                    };
                    CardQueue.Enqueue(card);
                }
            }
            //创建大王小王
            CardQueue.Enqueue(new CardDto
            {
                Name = "SJoker",
                Color = (int)CardColor.None,
                Weight = CardWeight.SJOKER
            });
            CardQueue.Enqueue(new CardDto
            {
                Name = "LJoker",
                Color =(int)CardColor.None,
                Weight =CardWeight.LJODER
            });
        }
        /// <summary>
        /// 洗牌
        /// </summary>
        private void Shuffle()
        {
            List<CardDto> newList = new List<CardDto>();
            //随机打乱牌顺序
            Random random = new Random();
            foreach (CardDto card in CardQueue)
            {
                int index = random.Next(0, newList.Count + 1);
                newList.Insert(index, card);
            }
            CardQueue.Clear();
            //添加到队列
            foreach (CardDto card in newList)
            {
                CardQueue.Enqueue(card);
            }
        }
        /// <summary>
        /// 发牌
        /// </summary>
        /// <returns></returns>
        public CardDto Deal()
        {
            return CardQueue.Dequeue();
        }
        /// <summary>
        /// 获取花色名称
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private string GetName(int color)
        {
            var c = (CardColor)color;
            switch (c)
            {
                case CardColor.Club:
                    return "Club";
                case CardColor.Heart:
                    return "Heart";
                case CardColor.Spade:
                    return "Spade";
                case CardColor.Square:
                    return "Square";
                default:
                    return string.Empty;
            }
        }
        /// <summary>
        /// 获取花色对应卡牌
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        private string GetWeight(int weight)
        {
            var c = (CardWeight)weight;
            switch (c)
            {
                case CardWeight.Three:
                    return "Three";
                case CardWeight.Four:
                    return "Four";
                case CardWeight.Five:
                    return "Five";
                case CardWeight.Six:
                    return "Six";
                case CardWeight.Seven:
                    return "Seven";
                case CardWeight.Eight:
                    return "Eight";
                case CardWeight.Nine:
                    return "Nine";
                case CardWeight.Ten:
                    return "Ten";
                case CardWeight.Jack:
                    return "Jack";
                case CardWeight.Queen:
                    return "Queen";
                case CardWeight.King:
                    return "King";
                case CardWeight.One:
                    return "One";
                case CardWeight.Two:
                    return "Two";
                case CardWeight.SJOKER:
                    return "SJoker";
                case CardWeight.LJODER:
                    return "LJoker";
                default:
                    return string.Empty;
            }
        }
    }
}
