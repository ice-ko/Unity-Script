using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 用户操作码 请求操作码 发送给服务端 返回操作码只能给客户端
/// </summary>
public enum UserCode
{
    /// <summary>
    /// 成功 消息状态 当操作成功时返回给客户端的状态
    /// </summary>
    Success,
    /// <summary>
    /// 获取数据
    /// </summary>
    GetInfoRequest,
    /// <summary>
    /// 获取数据返回
    /// </summary>
    GetInfoResult,
    /// <summary>
    /// 用户不存在
    /// </summary>
    UserExist,
    /// <summary>
    /// 创建角色请求
    /// </summary>
    CreateCharacterRequest,
    /// <summary>
    /// 创建角色返回
    /// </summary>
    CreateCharacterResult,
    /// <summary>
    /// 上线请求
    /// </summary>
    OnlineRequest,
    /// <summary>
    /// 上线返回
    /// </summary>
    OnlineResult,
    /// <summary>
    /// 账号不在线
    /// </summary>
    AccountNotOnline,
    /// <summary>
    /// 角色存在
    /// </summary>
    CharacterExExist,
}

