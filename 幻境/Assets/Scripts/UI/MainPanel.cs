using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    private Button btn_Start;
    private Button btn_Shop;
    private Button btn_Rank;
    private Button btn_Sound;
    void Start()
    {
        Init();
    }

    void Update()
    {

    }

    void Init()
    {
        //获取按钮
        btn_Start = transform.Find("btn_Start").GetComponent<Button>();
        btn_Shop = transform.Find("Btns/btn_Shop").GetComponent<Button>();
        btn_Rank = transform.Find("Btns/btn_Rank").GetComponent<Button>();
        btn_Sound = transform.Find("Btns/btn_Sound").GetComponent<Button>();
        //添加添加事件监听
        btn_Start.onClick.AddListener(OnStartClick);
        btn_Shop.onClick.AddListener(OnShopClick);
        btn_Rank.onClick.AddListener(OnRankClick);
        btn_Sound.onClick.AddListener(OnSoundClick);
    }
    /// <summary>
    /// 开始按钮点击
    /// </summary>
    void OnStartClick()
    {
        EventCenter.Broadcast(EventType.ShowGamePanel);
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 商店按钮点击
    /// </summary>
    void OnShopClick()
    {

    }
    /// <summary>
    /// 排行按钮点击
    /// </summary>
    void OnRankClick()
    {

    }
    /// <summary>
    /// 音效按钮点击
    /// </summary>
    void OnSoundClick()
    {

    }
}
