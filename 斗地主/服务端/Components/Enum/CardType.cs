using System;
using System.Collections.Generic;
using System.Text;

public enum CardType
{
    None,
    /// <summary>
    /// 单牌
    /// </summary>
    Singe,
    /// <summary>
    /// 对子
    /// </summary>
    Double,
    /// <summary>
    /// 顺子
    /// </summary>
    Straight,
    /// <summary>
    /// 双顺 如：44 55 66
    /// </summary>
    Double_Straight,
    /// <summary>
    /// 三顺 如：444 555 666
    /// </summary>
    Triple_Straight,
    /// <summary>
    /// 三不带 如：444
    /// </summary>
    Three,
    /// <summary>
    /// 三带一 如：444 5
    /// </summary>
    Three_One,
    /// <summary>
    /// 三带二 如：444 55
    /// </summary>
    Three_Two,
    /// <summary>
    /// 炸弹
    /// </summary>
    Boom,
    /// <summary>
    /// 王炸
    /// </summary>
    Joker_Boom

}