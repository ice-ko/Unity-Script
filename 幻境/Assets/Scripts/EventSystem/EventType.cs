using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    Null,
    /// <summary>
    /// 显示游戏面板
    /// </summary>
    ShowGamePanel,
    /// <summary>
    /// 决定路径
    /// </summary>
    DecidePath,
    /// <summary>
    /// 添加分数
    /// </summary>
    AddScore,
    UpdateScoreText,
    PlayerMove,
    /// <summary>
    /// 添加钻石
    /// </summary>
    AddDiamond,
    UpdateDiamondText,
    //显示结束面板
    ShowGameOverPanel,
    /// <summary>
    /// 重新开始
    /// </summary>
    Restart
}
