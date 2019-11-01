using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 账号登录操作码 
/// </summary>
public enum AccountCode
{
    /// <summary>
    /// 登录
    /// </summary>
    Login,
    /// <summary>
    /// 注册
    /// </summary>
    Registe,
    /// <summary>
    /// 注册用校验
    /// </summary>
    Regist_Check,
    /// <summary>
    /// 登录校验
    /// </summary>
    Login_Check,
    /// <summary>
    /// 账号已存在
    /// </summary>
    AccountAlreadyExists,
    /// <summary>
    /// 账号输入不合法
    /// </summary>
    AccountEntryIsIllegal,
    /// <summary>
    /// 密码不合法
    /// </summary>
    ThePasswordIsIllegal,
    /// <summary>
    /// 账号不存在
    /// </summary>
    AccountDoesNotExist,
    /// <summary>
    /// 账号在线
    /// </summary>
    AccountOnline,
    /// <summary>
    /// 账号密码不匹配
    /// </summary>
    AccountPasswordDoesNotMatch,
    /// <summary>
    /// 成功 消息状态 当操作成功时返回给客户端的状态
    /// </summary>
    Success,
}