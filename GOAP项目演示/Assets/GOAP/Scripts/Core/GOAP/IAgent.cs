using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 代理管理
/// </summary>
public interface IAgent
{
    /// <summary>
    /// 添加动作
    /// </summary>
    /// <param name="a"></param>
    void AddAction(GoapAction a);
    /// <summary>
    /// 获取动作
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    GoapAction GetAction(Type action);
    /// <summary>
    /// 移除动作
    /// </summary>
    /// <param name="action"></param>
    void RemoveAction(GoapAction action);
    /// <summary>
    /// 中止状态机
    /// </summary>
    void AbortFsm();
}
