using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 卡牌
/// </summary>
public class CardHelper
{
    /// <summary>
    /// 获取卡牌的权值
    /// </summary>
    /// <param name="cardList">选中的卡片</param>
    /// <param name="type">出牌类型</param>
    /// <returns></returns>
    public static int GetWeight(List<CardDto> cardList, CardType type)
    {
        int totalWeight = 0;
        if (type == CardType.Three_One || type == CardType.Three_Two)
        {
            for (int i = 0; i < cardList.Count; i++)
            {
                if (cardList[i].Weight == cardList[i + 1].Weight && cardList[i].Weight == cardList[i + 2].Weight)
                {
                    totalWeight += ((int)cardList[i].Weight * 3);
                }
            }
        }
        else
        {
            for (int i = 0; i < cardList.Count; i++)
            {
                totalWeight += (int)cardList[i].Weight;
            }
        }
        return totalWeight;
    }
    /// <summary>
    /// 是否单牌
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsSingle(List<CardDto> cards)
    {
        return cards.Count == 1;
    }
    /// <summary>
    /// 判断是否是对子
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsDouble(List<CardDto> cards)
    {
        if (cards.Count == 2 && cards[0].Weight == cards[1].Weight)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 是否是顺子
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsStraight(List<CardDto> cards)
    {
        if (cards.Count < 5 || cards.Count > 12)
        {
            return false;
        }
        for (int i = 0; i < cards.Count - 1; i++)
        {
            CardWeight tempWeight = cards[i].Weight;
            if (cards[i + 1].Weight - tempWeight != 1)
            {
                return false;
            }
            //顺子不能超过a
            if (tempWeight > CardWeight.One || cards[i + 1].Weight > CardWeight.One)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 是否双顺
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsDoubleStraight(List<CardDto> cards)
    {
        if (cards.Count < 6 || cards.Count % 2 != 0)
        {
            return false;
        }
        for (int i = 0; i < cards.Count - 2; i += 2)
        {
            if (cards[i].Weight != cards[i + 1].Weight)
            {
                return false;
            }
            if (cards[i + 2].Weight - cards[i].Weight != 1)
            {
                return false;
            }
            if (cards[1].Weight > CardWeight.One || cards[i + 2].Weight > CardWeight.One)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 是否是飞机
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsTripleStraight(List<CardDto> cards)
    {
        //333 444 555
        // 33344456  333444 66 77
        if (cards.Count < 6 || cards.Count % 3 != 0)
            return false;

        for (int i = 0; i < cards.Count - 3; i += 3)
        {
            if (cards[i].Weight != cards[i + 1].Weight)
                return false;
            if (cards[i + 2].Weight != cards[i + 1].Weight)
                return false;
            if (cards[i].Weight != cards[i + 2].Weight)
                return false;

            if (cards[i + 3].Weight - cards[i].Weight != 1)
                return false;
            //不能超过A
            if (cards[i].Weight > CardWeight.One || cards[i + 3].Weight > CardWeight.One)
                return false;
        }

        return true;
    }

    /// <summary>
    /// 是否是三不带
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsThree(List<CardDto> cards)
    {
        //333
        if (cards.Count != 3)
            return false;
        if (cards[0].Weight != cards[1].Weight)
            return false;
        if (cards[2].Weight != cards[1].Weight)
            return false;
        if (cards[0].Weight != cards[2].Weight)
            return false;

        return true;
    }

    /// <summary>
    /// 是否是三带一
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsThreeAndOne(List<CardDto> cards)
    {
        if (cards.Count != 4)
            return false;

        //5333 3335
        if (cards[0].Weight == cards[1].Weight && cards[1].Weight == cards[2].Weight)
            return true;
        else if (cards[1].Weight == cards[2].Weight && cards[2].Weight == cards[3].Weight)
            return true;

        return false;
    }

    /// <summary>
    /// 是否是三带二
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsThreeAndTwo(List<CardDto> cards)
    {
        if (cards.Count != 5)
            return false;
        //33355 55333
        if (cards[0].Weight == cards[1].Weight && cards[1].Weight == cards[2].Weight)
        {
            if (cards[3].Weight == cards[4].Weight)
                return true;
        }
        else if (cards[2].Weight == cards[3].Weight && cards[3].Weight == cards[4].Weight)
        {
            if (cards[0].Weight == cards[1].Weight)
                return true;
        }

        return false;
    }

    /// <summary>
    /// 判断是否是炸弹
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsBoom(List<CardDto> cards)
    {
        if (cards.Count != 4)
            return false;
        // 0000
        if (cards[0].Weight != cards[1].Weight)
            return false;
        if (cards[1].Weight != cards[2].Weight)
            return false;
        if (cards[2].Weight != cards[3].Weight)
            return false;

        return true;
    }

    /// <summary>
    /// 判断是不是王炸
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsJokerBoom(List<CardDto> cards)
    {
        if (cards.Count != 2)
            return false;

        if (cards[0].Weight == CardWeight.SJOKER && cards[1].Weight == CardWeight.LJODER)
            return true;
        else if (cards[0].Weight == CardWeight.LJODER && cards[1].Weight == CardWeight.SJOKER)
            return true;

        return false;
    }


    /// <summary>
    /// 获取卡牌类型
    /// </summary>
    /// <param name="cardList">要出的牌</param>
    public static CardType GetCardType(List<CardDto> cardList)
    {
        CardType cardType = CardType.None;

        switch (cardList.Count)
        {
            case 1:
                if (IsSingle(cardList))
                {
                    cardType = CardType.Singe;
                }
                break;
            case 2:
                if (IsDouble(cardList))
                {
                    cardType = CardType.Double;
                }
                else if (IsJokerBoom(cardList))
                {
                    cardType = CardType.Joker_Boom;
                }
                break;
            case 3:
                if (IsThree(cardList))
                {
                    cardType = CardType.Three;
                }
                break;
            case 4:
                if (IsBoom(cardList))
                {
                    cardType = CardType.Boom;
                }
                else if (IsThreeAndOne(cardList))
                {
                    cardType = CardType.Three_One;
                }
                break;
            case 5:
                if (IsStraight(cardList))
                {
                    cardType = CardType.Straight;
                }
                else if (IsThreeAndTwo(cardList))
                {
                    cardType = CardType.Three_Two;
                }
                break;
            case 6:
                if (IsStraight(cardList))
                {
                    cardType = CardType.Straight;
                }
                else if (IsDoubleStraight(cardList))
                {
                    cardType = CardType.Double_Straight;
                }
                else if (IsTripleStraight(cardList))
                {
                    cardType = CardType.Triple_Straight;
                }
                break;
            case 7:
                if (IsStraight(cardList))
                {
                    cardType = CardType.Straight;
                }
                break;
            case 8:
                if (IsStraight(cardList))
                {
                    cardType = CardType.Straight;
                }
                else if (IsDoubleStraight(cardList))
                {
                    cardType = CardType.Double_Straight;
                }
                break;
            case 9:
                if (IsStraight(cardList))
                {
                    cardType = CardType.Straight;
                }
                //777 888 999 
                else if (IsTripleStraight(cardList))
                {
                    cardType = CardType.Triple_Straight;
                }
                break;
            case 10:
                if (IsStraight(cardList))
                {
                    cardType = CardType.Straight;
                }
                else if (IsDoubleStraight(cardList))
                {
                    cardType = CardType.Double_Straight;
                }
                break;
            case 11:
                if (IsStraight(cardList))
                {
                    cardType = CardType.Straight;
                }
                break;
            case 12:
                if (IsStraight(cardList))
                {
                    cardType = CardType.Straight;
                }
                else if (IsDoubleStraight(cardList))
                {
                    cardType = CardType.Double_Straight;
                }
                // 444 555 666 777
                else if (IsTripleStraight(cardList))
                {
                    cardType = CardType.Triple_Straight;
                }
                break;
            case 13:
                //345678910JQKA
                break;
            case 14:
                if (IsDoubleStraight(cardList))
                {
                    cardType = CardType.Double_Straight;
                }
                break;
            case 15:
                if (IsTripleStraight(cardList))
                {
                    cardType = CardType.Triple_Straight;
                }
                break;
            case 16:
                if (IsDoubleStraight(cardList))
                {
                    cardType = CardType.Double_Straight;
                }
                break;
            case 17:
                break;
            case 18:
                if (IsDoubleStraight(cardList))
                {
                    cardType = CardType.Double_Straight;
                }
                // 444 555 666 777 888 999 
                else if (IsTripleStraight(cardList))
                {
                    cardType = CardType.Triple_Straight;
                }
                break;
            case 19:
                break;
            case 20:
                //33 44 55 66 77 88 99 1010 JJ QQ KK AA
                if (IsDoubleStraight(cardList))
                {
                    cardType = CardType.Double_Straight;
                }
                break;
            default:
                break;
        }

        return cardType;
    }
}