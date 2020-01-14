using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 发牌传输信息
/// </summary>
[Serializable]
public class DealDto
{
    /// <summary>
    /// 选中要出的牌
    /// </summary>
    public List<CardDto> SelectCardList { get; set; }
    /// <summary>
    /// 长度
    /// </summary>
    public int Length { get; set; }
    /// <summary>
    /// 权值
    /// </summary>
    public int Weight { get; set; }
    /// <summary>
    /// 类型
    /// </summary>
    public CardType Type { get; set; }
    /// <summary>
    /// 谁出的牌
    /// </summary>
    public int UsetId { get; set; }
    /// <summary>
    /// 牌是否合法
    /// </summary>
    public bool IsRegular { get; set; }
    public DealDto() { }
    public DealDto(List<CardDto> cards, int uid)
    {
        this.SelectCardList = cards;
        this.Length = cards.Count;
        this.Type = CardHelper.GetCardType(cards);
        this.Weight = CardHelper.GetWeight(cards, this.Type);
        this.UsetId = uid;
        this.IsRegular = (this.Type != CardType.None);
    }
}
