using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 用户角色
/// </summary>
[Serializable]
public class UserCharacterDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    /// <summary>
    /// 豆子数量
    /// </summary>
    public int Been { get; set; } = 10000;
    /// <summary>
    /// 胜利场次
    /// </summary>
    public int WinCount { get; set; }
    /// <summary>
    /// 失败场次
    /// </summary>
    public int LoseCount { get; set; }
    /// <summary>
    /// 逃跑场次
    /// </summary>
    public int RunCount { get; set; }
    /// <summary>
    /// 等级
    /// </summary>
    public int Lv { get; set; } = 1;
    /// <summary>
    /// 经验
    /// </summary>
    public int Exp { get; set; }
}

