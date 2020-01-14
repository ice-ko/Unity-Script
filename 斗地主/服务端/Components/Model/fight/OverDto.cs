using System;
using System.Collections.Generic;
using System.Text;
/// <summary>
/// 游戏结束传输协议
/// </summary>
[Serializable]
public class OverDto
{
    /// <summary>
    /// 胜利身份 地主还是农民
    /// </summary>
    public Identity WinIdentity { get; set; }
    /// <summary>
    /// 胜利玩家列表
    /// </summary>
    public List<int> WinUIdList { get; set; }
    /// <summary>
    /// 豆子总数
    /// </summary>
    public int BeenCount { get; set; }
}