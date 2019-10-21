using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    /// <summary>
    /// 游戏是否开始
    /// </summary>
    public bool IsGameStart { get; set; }
    /// <summary>
    /// 游戏是否结束
    /// </summary>
    public bool IsGameOver { get; set; }
    /// <summary>
    /// 是否暂停游戏
    /// </summary>
    public bool IsPause { get; set; }

    public bool PlayerIsMove { get; set; }
    /// <summary>
    /// 游戏分数
    /// </summary>
    public int GameScore { get; set; }
    /// <summary>
    /// 钻石数量
    /// </summary>
    public int DiamondCount { get; set; }
    private void Awake()
    {
        Instance = this;
        EventCenter.AddListener(EventType.AddScore, AddGaneScore);
        EventCenter.AddListener(EventType.PlayerMove, PlayerMove);
        EventCenter.AddListener(EventType.AddDiamond, AddDiamond);

        if (GameData.IsAgainGame)
        {
            IsGameStart = true;
        }
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.AddScore, AddGaneScore);
        EventCenter.RemoveListener(EventType.PlayerMove, PlayerMove);
        EventCenter.RemoveListener(EventType.AddDiamond, AddDiamond);
    }
    void Start()
    {

    }

    void Update()
    {

    }
    /// <summary>
    /// 添加分数
    /// </summary>
    void AddGaneScore()
    {
        if (IsGameStart == false || IsGameOver || IsPause)
        {
            return;
        }
        GameScore++;
        EventCenter.Broadcast(EventType.UpdateScoreText, GameScore);
    }
    void PlayerMove()
    {
        PlayerIsMove = true;
    }
    /// <summary>
    /// 添加钻石
    /// </summary>
    void AddDiamond()
    {
        DiamondCount++;
        EventCenter.Broadcast(EventType.UpdateDiamondText, DiamondCount);
    }
}
