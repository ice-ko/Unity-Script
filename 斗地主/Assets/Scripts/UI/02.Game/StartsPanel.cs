using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartsPanel : UIBase
{
    /// <summary>
    /// 用户角色数据
    /// </summary>
    protected UserCharacterDto userCharacterDto;

    //角色形象
    protected Image imgIdentity;
    //准备状态显示
    protected Text txtReady;
    //聊天文本框
    protected Image imgChat;
    //聊天文本框
    protected Text txtChat;
    public virtual void Start()
    {
        imgIdentity = transform.Find("img_Identety").GetComponent<Image>();
        txtReady = transform.Find("txt_Ready").GetComponent<Text>();
        imgChat = transform.Find("img_Chat").GetComponent<Image>();
        txtChat = transform.Find("img_Chat/Text").GetComponent<Text>();
        //默认隐藏
        txtReady.gameObject.SetActive(false);
        imgChat.gameObject.SetActive(false);
        SetIdentity(0);
        //绑定
        Bind(
            UIEvent.Player_Hide_State,
            UIEvent.Player_Ready,
            UIEvent.Player_Leave,
            UIEvent.Player_Enter,
            UIEvent.Player_Chat
            );
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Player_Hide_State:
                Invoke("HideUI", 0.5f);
                break;
            case UIEvent.Player_Ready:
                //显示准备文本框
                if (userCharacterDto == null)
                {
                    break;
                }
                int userId = (int)message;
                if (userCharacterDto.Id == userId)
                {
                    ReadyState(true);
                }
                break;
            case UIEvent.Player_Leave:
                if (userCharacterDto == null)
                {
                    break;
                }
                //玩家离开隐藏面板
                int leaveUserId = (int)message;
                if (userCharacterDto.Id == leaveUserId)
                {
                    ReadyState(false);
                    SetPanelActive(false);
                    userCharacterDto = null;
                }
                break;
            case UIEvent.Player_Enter:
                //玩家进入房间
                if (userCharacterDto == null)
                {
                    break;
                }
                int enterUserId = (int)message;
                if (userCharacterDto.Id == enterUserId)
                {
                    SetPanelActive(true);
                }
                break;
            case UIEvent.Player_Chat:
                {
                    if (userCharacterDto == null)
                    {
                        break;
                    }
                    ChatDto chatDto = message as ChatDto;
                    if (userCharacterDto.Id == chatDto.UserId)
                    {
                        var text = ConstantHelper.GetChatText(chatDto.Type);
                        ShowChat(text);
                    }
                    break;
                }
            case UIEvent.Player_Change_Identity:
                {
                    if (userCharacterDto==null)
                    {
                        break;
                    }
                    GrabDto dto = message as GrabDto;
                    if (userCharacterDto.Id==dto.UserId)
                    {
                        SetIdentity(1);
                    }
                    break;
                }
        }
    }
    /// <summary>
    /// 隐藏准备面板
    /// </summary>
    void HideUI()
    {
        txtReady.gameObject.SetActive(false);
    }
    /// <summary>
    /// 准备状态
    /// </summary>
    protected virtual void ReadyState(bool isReady)
    {
        //显示准备文本框
        this.txtReady.gameObject.SetActive(isReady);
    }
    /// <summary>
    /// 设置身份 0 农民 1 地主
    /// </summary>
    /// <param name="index"></param>
    protected void SetIdentity(int index)
    {
        var identityStr = index == 0 ? "Farmer" : "Landlord";
        imgIdentity.sprite = Resources.Load<Sprite>("Identity/" + identityStr);
    }
    /// <summary>
    /// 显示时间 
    /// </summary>
    protected int showTime = 2;
    /// <summary>
    /// 计时器
    /// </summary>
    protected float timer = 0f;
    /// <summary>
    /// 是否显示
    /// </summary>
    protected bool isShow = false;
    protected virtual void Update()
    {
        if (isShow)
        {
            timer += Time.deltaTime;
            if (timer >= showTime)
            {
                SetChatActive(false);
                timer = 0f;
                isShow = false;
            }
        }
    }
    /// <summary>
    /// 设置聊天显示动画
    /// </summary>
    /// <param name="active"></param>
    protected void SetChatActive(bool active)
    {
        imgChat.gameObject.SetActive(active);
    }
    /// <summary>
    /// 聊天文本显示
    /// </summary>
    /// <param name="text"></param>
    protected void ShowChat(string text)
    {
        txtChat.text = text;
        //显示动画
        SetChatActive(true);
        isShow = true;
    }
}
