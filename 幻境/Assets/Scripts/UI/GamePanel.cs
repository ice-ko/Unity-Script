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
    }

    void Update()
    {

    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.ShowGamePanel, Show);
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
    }
    /// <summary>
    /// 开始按钮点击
    /// </summary>
    void OnPlayButClick()
    {
        btn_Pause.gameObject.SetActive(true);
        btn_Play.gameObject.SetActive(false);
    }
}
