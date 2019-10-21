using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{

    private Button btn_Play;
    private Button btn_Pause;
    private Text txt_Score;
    private Text txt_DiamondCount;
    void Start()
    {
        Init();
        EventCenter.AddListener(EventType.ShowGamePanel, Show);
        EventCenter.AddListener<int>(EventType.UpdateScoreText, UpdateScore);
        EventCenter.AddListener<int>(EventType.UpdateDiamondText, UpdateAddDiamond);
    }

    void Update()
    {

    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.ShowGamePanel, Show);
        EventCenter.RemoveListener<int>(EventType.UpdateScoreText, UpdateScore);
        EventCenter.RemoveListener<int>(EventType.UpdateDiamondText, UpdateAddDiamond);
    }
    void Init()
    {
        btn_Play = transform.Find("btn_Play").GetComponent<Button>();
        btn_Pause = transform.Find("btn_Pause").GetComponent<Button>();
        txt_Score = transform.Find("txt_Score").GetComponent<Text>();
        txt_DiamondCount = transform.Find("Diamond/txt_DiamondCount").GetComponent<Text>();
        //注册点击事件
        btn_Play.onClick.AddListener(OnPlayButClick);
        btn_Pause.onClick.AddListener(OnPauseButClick);

        gameObject.SetActive(false);
        btn_Play.gameObject.SetActive(false);
    }
    /// <summary>
    /// 显示面板
    /// </summary>
    void Show()
    {
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 暂停按钮点击
    /// </summary>
    void OnPauseButClick()
    {
        btn_Pause.gameObject.SetActive(false);
        btn_Play.gameObject.SetActive(true);
        //游戏暂停
        Time.timeScale = 0;
        GameManager.Instance.IsPause = true;
    }
    /// <summary>
    /// 开始按钮点击
    /// </summary>
    void OnPlayButClick()
    {
        btn_Pause.gameObject.SetActive(true);
        btn_Play.gameObject.SetActive(false);
        //继续游戏
        Time.timeScale = 1;
        GameManager.Instance.IsPause = false;
    }
    /// <summary>
    /// 更新分数
    /// </summary>
    /// <param name="score"></param>
    void UpdateScore(int score)
    {
        txt_Score.text = score.ToString();
    }
    /// <summary>
    /// 更新钻石
    /// </summary>
    /// <param name="amount"></param>
    void UpdateAddDiamond(int amount)
    {
        txt_DiamondCount.text = amount.ToString();
    }
}
