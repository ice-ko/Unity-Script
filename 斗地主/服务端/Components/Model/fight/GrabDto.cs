using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 地主
/// </summary>
[Serializable]
public class GrabDto
{
    /// <summary>
    /// 用户id
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// 底牌
    /// </summary>
    public List<CardDto> TableCardList { get; set; }
    /// <summary>
    /// 玩家手牌
    /// </summary>
    public List<CardDto> PlayerCardList { get; set; }
}
