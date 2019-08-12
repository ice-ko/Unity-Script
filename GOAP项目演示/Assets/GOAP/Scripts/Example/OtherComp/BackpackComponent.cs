using UnityEngine;
using System.Collections;

/**
 * Holds resources for the Agent.
 * 保留代理的资源。
 */


///<summary>
/// 临时背包
///</summary>
public class BackpackComponent : MonoBehaviour
{
    [Header("工具")]
    ///<summary>
    ///工具
    ///</summary>
    public GameObject tool;
    [Header("材料数量")]
    ///<summary>
    ///材料数量
    ///</summary>
    public int numLogs;
    [Header("木材数量")]
    ///<summary>
    ///木材数量
    ///</summary>
    public int numFirewood;
    ///<summary>
    ///
    ///</summary>
    public int numOre;
    [Header("肉数量")]
    ///<summary>
    ///肉数量
    ///</summary>
    public int numMeat;
    [Header("工具类型")]
    ///<summary>
    ///工具类型
    ///</summary>
    public string toolType = "ToolAxe";//默认斧头

}

