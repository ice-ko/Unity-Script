using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 为什么写着脚本？
///     我们想扩展 MonoBehaviour
/// </summary>
public class MonoBase : MonoBehaviour 
{

    /// <summary>
    /// 定义一个虚方法
    /// </summary>
    public virtual void Execute(int eventCode, object message)
    {
    }

}
