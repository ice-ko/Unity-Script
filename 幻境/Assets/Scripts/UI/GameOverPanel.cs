using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    /// <summary>
    /// 分数  最高分 钻石数量
    /// </summary>
    public Text txt_Score, txt_BestScore, txt_DiamondCount;
    /// <summary>
    /// 排行 主页 重新开始
    /// </summary>
    public Button btn_Rank, btn_Home, btn_Restart;
    private void Awake()
    {
        gameObject.SetActive(false);
        EventCenter.AddListener(EventType.ShowGameOverPanel, Show);

        btn_Restart.onClick.AddListener(OnRestartButtonOnClick);
        btn_Home.onClick.AddListener(OnHomeButtonOnClick);
        btn_Rank.onClick.AddListener(OnRankButtonOnClick);
    }



    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.ShowGameOverPanel, Show);
    }
    void Start()
    {

    }

    void Update()
    {

    }
    void Show()
    {
        txt_Score.text = GameManager.Instance.GameScore.ToString();
        txt_DiamondCount.text = "+" + GameManager.Instance.DiamondCount;
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 重新开始
    /// </summary>
    void OnRestartButtonOnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.IsAgainGame = true;

    }
    /// <summary>
    /// 返回首页
    /// </summary>
    void OnHomeButtonOnClick()
    {
        GameData.IsAgainGame = false;
    }
    /// <summary>
    /// 打开排行
    /// </summary>
    void OnRankButtonOnClick()
    {
    }
}
