using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 房间信息
/// </summary>
[Serializable]
public class MatchRoomDto
{
    /// <summary>
    /// 房间内的用户列表
    /// </summary>
    public Dictionary<int, UserCharacterDto> UIdClientDict { get; set; } = new Dictionary<int, UserCharacterDto>();
    /// <summary>
    /// 已经准备的玩家ID列表
    /// </summary>
    public List<int> ReadyUIdList { get; set; }
}