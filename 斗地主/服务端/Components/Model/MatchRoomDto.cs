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
    public List<int> ReadyUIdList { get; set; } = new List<int>();
    /// <summary>
    /// 存储玩家进入的顺序
    /// </summary>
    public List<int> UIdList { get; set; } = new List<int>();

    /// <summary>
    /// 左边玩家
    /// </summary>
    public int LeftId { get; set; }
    /// <summary>
    /// 右边玩家
    /// </summary>
    public int RightId { get; set; }

    /// <summary>
    /// 重置玩家位置
    /// </summary>
    /// <param name="myUserId">自己</param>
    public void ResetPosition(int myUserId)
    {
        LeftId = -1;
        RightId = -1;
        if (UIdList.Count == 0)
        {
            return;
        }
        //房间只存在两个玩家 
        else if (UIdList.Count == 2)
        {
            if (UIdList[0] == myUserId)
            {
                RightId = UIdList[1];
            }
            if (UIdList[1] == myUserId)
            {
                LeftId = UIdList[0];
            }
        }
        else if (UIdList.Count == 3)
        {
            if (UIdList[0] == myUserId)
            {
                LeftId = UIdList[2];
                RightId = UIdList[1];
            }
            if (UIdList[1] == myUserId)
            {
                LeftId = UIdList[0];
                RightId = UIdList[2];
            }
            if (UIdList[2] == myUserId)
            {
                LeftId = UIdList[1];
                RightId = UIdList[0];
            }
        }
    }
}