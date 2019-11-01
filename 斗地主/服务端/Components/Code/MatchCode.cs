using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 匹配操作 请求操作码 发送给服务端 返回操作码只能给客户端
/// </summary>
public enum MatchCode
{
    /// <summary>
    /// 成功 消息状态 当操作成功时返回给客户端的状态
    /// </summary>
    Success,
    /// <summary>
    /// 进入匹配队列请求
    /// </summary>
    EnterMatch_Request,
    /// <summary>
    /// 进入匹配队列返回
    /// </summary>
    EnterMatch_Result,
    /// <summary>
    /// 进入匹配广播
    /// </summary>
    EnterMatch_Broadcast_Result,
    /// <summary>
    /// 离开匹配队列请求
    /// </summary>
    LeaveMatch_Request,
    /// <summary>
    /// 离开匹配队列广播返回
    /// </summary>
    LeaveMatch_Broadcast_Result,
    /// <summary>
    /// 准备请求
    /// </summary>
    Ready_Request,
    /// <summary>
    /// 准备广播返回
    /// </summary>
    Ready_Broadcast_Result,
    /// <summary>
    /// 开始游戏广播请求
    /// </summary>
    Start_Broadcast_Requst,
    /// <summary>
    /// 重复匹配
    /// </summary>
    Repeat_Match
}