using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 玩家数据传输
/// </summary>
public class PlayerDto
{
    /// <summary>
    /// 用户id
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// 身份 地主/农民
    /// </summary>
    public Identity Identity { get; set; }
    /// <summary>
    /// 卡牌列表
    /// </summary>
    public List<CardDto> CardList { get; set; }
    /// <summary>
    /// 是否有手牌
    /// </summary>
    public bool HasCard
    {
        get { return CardList.Count != 0; }
    }
    /// <summary>
    /// 当前卡牌数量
    /// </summary>
    public int CardCount
    {
        get
        {
            return CardList.Count;
        }
    }
    /// <summary>
    /// 构造函数初始化
    /// </summary>
    /// <param name="userId">用户id</param>
    public PlayerDto(int userId)
    {
        this.Identity = Identity.Farmer;
        this.UserId = userId;
        this.CardList = new List<CardDto>();
    }
    /// <summary>
    /// 添加卡牌
    /// </summary>
    /// <param name="card"></param>
    public void Add(CardDto card)
    {
        CardList.Add(card);
    }
    /// <summary>
    /// 移除卡牌
    /// </summary>
    /// <param name="card"></param>
    public void Remove(CardDto card)
    {
        CardList.Remove(card);
    }
}
