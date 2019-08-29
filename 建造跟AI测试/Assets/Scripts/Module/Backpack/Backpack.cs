using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    ///<summary>
    ///工具
    ///</summary>
    public GameObject tool;
    ///<summary>
    ///木材数量
    ///</summary>
    public int woodNum;
    ///<summary>
    ///木材数量
    ///</summary>
    public int firewoodNum;
    ///<summary>
    ///石材数量
    ///</summary>
    public int stoneNum;
    ///<summary>
    ///工具类型
    ///</summary>
    public string toolType = "ToolAxe";//默认斧头    
    /// <summary>
    /// The tool number工具数量
    /// </summary>
    public int toolNum;
}