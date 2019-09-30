using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 材料信息
/// </summary>
public class MaterialInfo
{
    /// <summary>
    /// 材料名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 材料类型
    /// </summary>
    public MaterialType Type { get; set; }
    /// <summary>
    /// 工作量
    /// </summary>
    public float Workload { get; set; }
}
public enum MaterialType
{
    /// <summary>
    /// 矿石
    /// </summary>
    Ore,
    /// <summary>
    /// 木材
    /// </summary>
    Timber,
}