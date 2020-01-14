using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtomPanel : UIBase
{
    //豆子
    private Text txtBeen;
    //当前倍数
    private Text txtMutiple;
    private Button butChat;
    private Button[] btns;
    private Image chooseListPanel;

    SocketMsg socketMsg;
    void Start()
    {
        Bind(UIEvent.Change_Mutiple, UIEvent.Change_Been);
        socketMsg = new SocketMsg();
        btns = new Button[7];

        txtBeen = transform.Find("txt_Been").GetComponent<Text>();
        txtMutiple = transform.Find("txt_Mutiple").GetComponent<Text>();
        butChat = transform.Find("but_Chat").GetComponent<Button>();
        chooseListPanel = transform.Find("ChooseListPanel").GetComponent<Image>();
        //btns = transform.Find("ChooseListPanel").GetComponentsInChildren<Button>();

        for (int i = 0; i < chooseListPanel.transform.GetChild(0).transform.childCount; i++)
        {
            int index = i + 1;
            btns[i] = chooseListPanel.transform.GetChild(0).transform.GetChild(i).GetComponent<Button>();
            btns[i].onClick.AddListener(() =>
              {
                  OnCharClick(index);
              });
        }
        var userInfo = Data.GameData.UserCharacterDto;
        if (userInfo != null)
        {
            RefreshView(userInfo.Been);
        }
        //添加监听
        butChat.onClick.AddListener(SetChooseListPanel);
        //默认设置
        chooseListPanel.gameObject.SetActive(false);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Change_Mutiple:
                ChangeMutiple((int)message);
                break;
            case UIEvent.Change_Been:
                RefreshView((int)message);
                break;
        }
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        butChat.onClick.RemoveListener(SetChooseListPanel);
    }
    void Update()
    {

    }
    /// <summary>
    /// 刷新豆子
    /// </summary>
    /// <param name="beenCount"></param>
    private void RefreshView(int beenCount)
    {
        txtBeen.text = "x " + beenCount;
    }
    /// <summary>
    /// 更改当前局倍数
    /// </summary>
    /// <param name="beenCount"></param>
    private void ChangeMutiple(int mutiple)
    {
        txtMutiple.text = "倍数 x " + mutiple;
    }
    /// <summary>
    /// 设置快捷喊话面板
    /// </summary>
    private void SetChooseListPanel()
    {
        chooseListPanel.gameObject.SetActive(!chooseListPanel.gameObject.activeInHierarchy);
    }
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="index"></param>
    private void OnCharClick(int index)
    {
        socketMsg.OpCode = MsgType.Chat;
        socketMsg.SubCode = ChatCode.Default;
        socketMsg.value = index;
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
}
