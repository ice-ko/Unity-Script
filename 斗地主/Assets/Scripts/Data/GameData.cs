using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏数据存储
/// </summary>
public class GameData
{
    /// <summary>
    /// 登录的角色信息
    /// </summary>
    public UserCharacterDto UserCharacterDto { get; set; }
    /// <summary>
    /// 匹配房间信息
    /// </summary>
    public MatchRoomDto MatchRoomDto { get; set; }
}
