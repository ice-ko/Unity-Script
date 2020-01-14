using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 战斗操作码
/// </summary>
public enum FightCode
{
    /// <summary>
    /// 抢地主请求
    /// </summary>
    Grab_Landlord_CREQ,
    /// <summary>
    /// 服务端广播抢地主的结果
    /// </summary>
    Grab_Landlord_Bro,
    /// <summary>
    /// 服务器广播下一个玩家抢地主的结果
    /// </summary>
    Turn_Grab_Bro,
    /// <summary>
    /// 客户端发起出牌请求
    /// </summary>
    Deal_Creq,
    /// <summary>
    /// 服务器响应客户端出牌
    /// </summary>
    Deal_Result,
    /// <summary>
    /// 服务器广播出牌结果
    /// </summary>
    Deal_Bro,
    /// <summary>
    /// 客户端发起不出请求
    /// </summary>
    Pass_Creq,
    /// <summary>
    /// 服务端响应客户端的不出请求
    /// </summary>
    Pass_Result,
    /// <summary>
    /// 服务器广播转换出牌的结果
    /// </summary>
    Turn_Deal_Bro,
    /// <summary>
    /// 服务器广播有玩家退出游戏
    /// </summary>
    Leave_Bro,
    /// <summary>
    /// 服务器广播游戏结束
    /// </summary>
    Over_Bro,
    /// <summary>
    /// 服务器给客户端卡牌响应
    /// </summary>
    Get_Card_Result,
    /// <summary>
    ///  成功 消息状态 当操作成功时返回给客户端的状态
    /// </summary>
    Success,
    必须大于上次一次出牌,
    出牌
}