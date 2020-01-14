using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MyStartsPanel : StartsPanel
{
    //出牌
    private Button btnDeal;
    //不出牌
    private Button btnNotDeal;
    //叫地主
    private Button btnGrab;
    //不叫地主
    private Button btnNotGrab;
    //准备
    private Button btnReady;

    SocketMsg socketMsg;
    public override void Start()
    {
        base.Start();
        socketMsg = new SocketMsg();

        Bind(
            UIEvent.Show_Grab_Button, 
            UIEvent.Show_Deal_Button, 
            //UIEvent.Set_MyPlayer_Data,
            UIEvent.Hide_Ready_Button
            );

        btnDeal = transform.Find("btn_Deal").GetComponent<Button>();
        btnNotDeal = transform.Find("btn_NotDeal").GetComponent<Button>();
        btnGrab = transform.Find("btn_Grab").GetComponent<Button>();
        btnNotGrab = transform.Find("btn_NotGrab").GetComponent<Button>();
        btnReady = transform.Find("btn_Ready").GetComponent<Button>();
        //添加监听
        btnDeal.onClick.AddListener(OnDealClick);
        btnNotDeal.onClick.AddListener(OnNotDealClick);
        btnGrab.onClick.AddListener(delegate ()
        {
            OnGrabClick(true);
        });
        btnNotGrab.onClick.AddListener(delegate ()
        {
            OnGrabClick(false);
        });
        btnReady.onClick.AddListener(OnReadyClick);
        //
        btnGrab.gameObject.SetActive(false);
        btnNotGrab.gameObject.SetActive(false);
        btnDeal.gameObject.SetActive(false);
        btnNotDeal.gameObject.SetActive(false);
        //

        UserCharacterDto myUserDto = Data.GameData.MatchRoomDto.UIdClientDict[Data.GameData.UserCharacterDto.Id];
        this.userCharacterDto = myUserDto;
    }
    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            //case UIEvent.Set_MyPlayer_Data:
            //    {
            //        this.userCharacterDto = message as UserCharacterDto;
            //        break;
            //    }
            case UIEvent.Show_Grab_Button:
                {
                    bool active = (bool)message;
                    btnGrab.gameObject.SetActive(active);
                    btnNotGrab.gameObject.SetActive(active);
                    break;
                }
            case UIEvent.Show_Deal_Button:
                {
                    bool active = (bool)message;
                    btnDeal.gameObject.SetActive(active);
                    btnNotDeal.gameObject.SetActive(active);
                    break;
                }
            case UIEvent.Hide_Ready_Button:
                {
                    btnReady.gameObject.SetActive(false);
                    break;
                }
              
        }
    }
    public override void OnDestroy()
    {
        btnDeal.onClick.RemoveListener(OnDealClick);
        btnNotDeal.onClick.RemoveListener(OnNotDealClick);
        btnGrab.onClick.RemoveAllListeners();
        btnNotGrab.onClick.RemoveAllListeners();
        btnReady.onClick.RemoveListener(OnReadyClick);
    }
    /// <summary>
    /// 准备
    /// </summary>
    /// <returns></returns>
    private void OnReadyClick()
    {
        socketMsg.OpCode = MsgType.Match;
        socketMsg.SubCode = MatchCode.Ready_Request;
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    /// <summary>
    /// 抢地主
    /// </summary>
    /// <returns></returns>
    private void OnGrabClick(bool result)
    {
        socketMsg.OpCode = MsgType.Fight;
        socketMsg.SubCode = FightCode.Grab_Landlord_CREQ;
        socketMsg.value = result;
        Dispatch(AreaCode.NET,0,socketMsg);
        //隐藏按钮
        btnGrab.gameObject.SetActive(false);
        btnNotGrab.gameObject.SetActive(false);
    }
    /// <summary>
    /// 不出牌
    /// </summary>
    /// <returns></returns>
    private void OnNotDealClick()
    {
    }
    /// <summary>
    /// 出牌
    /// </summary>
    /// <returns></returns>
    private void OnDealClick()
    {

    }
}
