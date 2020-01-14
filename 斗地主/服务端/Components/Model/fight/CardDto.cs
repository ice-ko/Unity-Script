using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 卡片底牌
/// </summary>
[Serializable]
public class CardDto
{
    public int Id { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 颜色 例如：红桃
    /// </summary>
    public int Color { get; set; }
    /// <summary>
    /// 例如：9  红桃9
    /// </summary>
    public CardWeight Weight { get; set; }
}
