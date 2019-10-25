using Components.Code;
using Components.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistPanel : UIBase
{
    private Button btnRegist;
    private Button btnClose;
    private InputField inputAccount;
    private InputField inputPassword;
    private InputField inputPasswordRepeat;

    PromptMsg promptMsg;
    void Start()
    {
        promptMsg = new PromptMsg();

        btnRegist = transform.Find("btn_Regist").GetComponent<Button>();
        btnClose = transform.Find("btn_Close").GetComponent<Button>();
        inputAccount = transform.Find("txt_Account/Input_Account").GetComponent<InputField>();
        inputPassword = transform.Find("txt_Password/Input_Password").GetComponent<InputField>();
        inputPasswordRepeat = transform.Find("txt_RepeatPwd/Input_Password").GetComponent<InputField>();

        //注册事件
        btnRegist.onClick.AddListener(OnRegistClick);
        btnClose.onClick.AddListener(OnCloseClick);

        SetPanelActive(false);
        Bind(UIEvent.regist_Code);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.regist_Code:
                SetPanelActive(true); break;
        }
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        btnRegist.onClick.RemoveAllListeners();
        btnClose.onClick.RemoveAllListeners();
    }
    private void OnCloseClick()
    {
        SetPanelActive(false);
    }

    private void OnRegistClick()
    {
        if (string.IsNullOrEmpty(inputAccount.text))
        {
            promptMsg.Text = "用户名不能为空！";
            Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
            return;
        }
        if (string.IsNullOrEmpty(inputPassword.text))
        {
            promptMsg.Text = "密码不能为空！";
            Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
            return;
        }
        if (inputPassword.text.Length < 4 || inputPassword.text.Length > 16)
        {
            promptMsg.Text = "密码长度错误！应在4~16个字符之间";
            Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
            return;
        }
        if (string.IsNullOrEmpty(inputPasswordRepeat.text) || inputPasswordRepeat.text != inputPassword.text)
        {
            promptMsg.Text = "请确保两次输入的密码一致！";
            Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
            return;
        }
        UserInfo userInfo = new UserInfo
        {
            Account = inputAccount.text,
            Password = inputPassword.text
        };
        Dispatch(AreaCode.NET, 0, new SocketMsg
        {
            OpCode = (int)MsgType.Account,
            SubCode = (int)MsgType.Registe,
            value = userInfo
        });
    }

    void Update()
    {

    }
}
