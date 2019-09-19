using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 矿石类型
/// </summary>
public enum OreType
{
    /// <summary>
    /// 普通
    /// </summary>
    general,
    /// <summary>
    /// 金矿
    /// </summary>
    goldmine
}
/// <summary>
/// 采矿工作量
/// </summary>
public enum MiningWorkload
{
    /// <summary>
    /// 普通
    /// </summary>
    general = 10,
    /// <summary>
    /// 金矿
    /// </summary>
    goldmine = 25
}