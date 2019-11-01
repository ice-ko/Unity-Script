using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MatchPanel : UIBase
{
    private Button btnMatch;
    private Button btnCancel;
    private Button btnEnter;
    private Image imgBg;
    private Text txtDes;

    /// <summary>
    /// 默认文本
    /// </summary>
    string defaultText = "正在寻找房间";
    /// <summary>
    /// 点的数量
    /// </summary>
    int dotCount = 0;
    bool isMatch = false;
    /// <summary>
    /// 计时器
    /// </summary>
    float timer = 0f;
    /// <summary>
    /// 间隔时间
    /// </summary>
    float intervalTime = 1f;

    SocketMsg socketMsg;
    void Start()
    {
        btnMatch = transform.Find("btn_Match").GetComponent<Button>();
        btnCancel = transform.Find("btn_Cancel").GetComponent<Button>();
        btnEnter = transform.Find("btn_Enter").GetComponent<Button>();
        imgBg = transform.Find("img_bg").GetComponent<Image>();
        txtDes = transform.Find("txt_Des").GetComponent<Text>();

        btnMatch.onClick.AddListener(OnMatcheClick);
        btnCancel.onClick.AddListener(OnCancelClick);
        btnEnter.onClick.AddListener(OnEnterClick);

        Bind(UIEvent.Show_EnterRoom_Button);
        //默认隐藏
        ObjectActive(false);
        btnEnter.gameObject.SetActive(false);

        socketMsg = new SocketMsg();
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Show_EnterRoom_Button:
                btnEnter.gameObject.SetActive(true);
                break;
        }
    }
    public override void OnDestroy()
    {
        btnMatch.onClick.RemoveListener(OnMatcheClick);
        btnCancel.onClick.RemoveListener(OnCancelClick);
        btnEnter.onClick.RemoveListener(OnEnterClick);
    }

    void Update()
    {
        if (isMatch == false)
        {
            return;
        }
        timer += Time.deltaTime;
        if (timer >= intervalTime)
        {
            DoAnimation();
            timer = 0f;
        }
    }
    /// <summary>
    /// 向服务器发起匹配请求
    /// </summary>
    private void OnMatcheClick()
    {
        ObjectActive(true);
        isMatch = true;
        //向服务器发送匹配请求
        socketMsg.OpCode = MsgType.Match;
        socketMsg.SubCode = MatchCode.EnterMatch_Request;
        socketMsg.value = Data.GameData.UserCharacterDto.Id;
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    /// <summary>
    /// 向服务器发起取消匹配请求
    /// </summary>
    private void OnCancelClick()
    {
        socketMsg.OpCode = MsgType.Match;
        socketMsg.SubCode = MatchCode.LeaveMatch_Request;
        socketMsg.value = Data.GameData.UserCharacterDto.Id;
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    /// <summary>
    /// 进入房间
    /// </summary>
    private void OnEnterClick()
    {

    }
    /// <summary>
    /// 隐藏显示
    /// </summary>
    /// <param name="active"></param>
    void ObjectActive(bool active)
    {
        imgBg.gameObject.SetActive(active);
        btnCancel.gameObject.SetActive(active);
        txtDes.gameObject.SetActive(active);
    }
    /// <summary>
    /// 文本动画
    /// </summary>
    void DoAnimation()
    {
        txtDes.text = defaultText;
        dotCount++;

        if (dotCount > 6)
        {
            dotCount = 1;
        }
        for (int i = 0; i < dotCount; i++)
        {
            txtDes.text += ".";
        }
    }
}
