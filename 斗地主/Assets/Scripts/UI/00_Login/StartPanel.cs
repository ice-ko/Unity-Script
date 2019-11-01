using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : UIBase
{
    private Button btnLogin;
    private Button btnClose;
    private InputField inputAccount;
    private InputField inputPassword;

    PromptMsg promptMsg;
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Start_Code:
                SetPanelActive(true); break;
        }
    }
    void Start()
    {
        promptMsg = new PromptMsg();

        btnLogin = transform.Find("btn_Login").GetComponent<Button>();
        btnClose = transform.Find("btn_Close").GetComponent<Button>();
        inputAccount = transform.Find("txt_Account/Input_Account").GetComponent<InputField>();
        inputPassword = transform.Find("txt_Password/Input_Password").GetComponent<InputField>();
        //注册事件
        btnLogin.onClick.AddListener(OnLoginClick);
        btnClose.onClick.AddListener(OnCloseClick);

        SetPanelActive(false);

        Bind(UIEvent.Start_Code);

    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        btnLogin.onClick.RemoveAllListeners();
        btnClose.onClick.RemoveAllListeners();
    }
    void OnLoginClick()
    {
        if (string.IsNullOrEmpty(inputAccount.text))
        {
            promptMsg.Text = "用户名不能为空！";
            Dispatch(AreaCode.UI,UIEvent.Prompt_Msg,promptMsg);
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
        UserInfoDto userInfo = new UserInfoDto
        {
            Account = inputAccount.text,
            Password = inputPassword.text
        };
        //发送数据
        Dispatch(AreaCode.NET, 0, new SocketMsg
        {
            OpCode = MsgType.Account,
            SubCode = AccountCode.Login,
            value = userInfo
        });
    }
    void OnCloseClick()
    {
        SetPanelActive(false);
    }
}
