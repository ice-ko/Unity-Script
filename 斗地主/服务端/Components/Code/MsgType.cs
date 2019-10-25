using System;
using System.Collections.Generic;
using System.Text;

namespace Components.Code
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MsgType
    {
        /// <summary>
        /// 账号信息
        /// </summary>
        Account,
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
        /// 成功
        /// </summary>
        Success,
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
        AccountPasswordDoesNotMatch
    }

}
