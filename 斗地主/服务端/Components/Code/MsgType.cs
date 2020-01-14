using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 消息类型
/// </summary>
public enum MsgType
{

    /// <summary>
    /// 账号登录信息模块
    /// </summary>
    Account,
    /// <summary>
    /// 用户角色信息模块
    /// </summary>
    /// 
    User,
    /// <summary>
    /// 匹配模块
    /// </summary>
    Match,
    /// <summary>
    /// 聊天
    /// </summary>
    Chat,
    /// <summary>
    /// 战斗
    /// </summary>
    Fight
}