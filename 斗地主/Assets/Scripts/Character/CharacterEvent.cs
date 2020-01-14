using System;
using System.Collections.Generic;

public class CharacterEvent
{
    /// <summary>
    /// 初始化卡牌
    /// </summary>
    public const int Init_MyCard = 1;
    /// <summary>
    /// 初始化左边玩家卡牌
    /// </summary>
    public const int Init_LeftCard = 2;
    /// <summary>
    /// 初始化右边玩家卡牌
    /// </summary>
    public const int Init_RightCard = 3;
    /// <summary>
    /// 玩家自身添加底牌
    /// </summary>
    public const int Add_MyCard = 4;
    /// <summary>
    /// 左边玩家添加底牌
    /// </summary>
    public const int Add_LeftCard = 5;
    /// <summary>
    /// 右边玩家添加底牌
    /// </summary>
    public const int Add_RightCard = 6;
}
