using Components.Code;
using Components.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    public PromptPanel promptPanel;

    private Button btnLogin;
    private Button btnClose;
    private InputField inputAccount;
    private InputField inputPassword;

    PromptMsg promptMsg;

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



    }
    public void OnDestroy()
    {
        btnLogin.onClick.RemoveAllListeners();
        btnClose.onClick.RemoveAllListeners();
    }
    void OnLoginClick()
    {
        if (string.IsNullOrEmpty(inputAccount.text))
        {
            promptMsg.Text = "用户名不能为空！";
            promptPanel.PromptMessage(promptMsg);
            return;
        }
        if (string.IsNullOrEmpty(inputPassword.text))
        {
            promptMsg.Text = "密码不能为空！";
            promptPanel.PromptMessage(promptMsg);
            return;
        }
        if (inputPassword.text.Length < 4 || inputPassword.text.Length > 16)
        {
            promptMsg.Text = "密码长度错误！应在4~16个字符之间";
            promptPanel.PromptMessage(promptMsg);
            return;
        }
        UserInfo userInfo = new UserInfo
        {
            Account = inputAccount.text,
            Password = inputPassword.text
        };
        //给服务端发送数据
        NetManager.Instance.Send(new SocketMsg
        {
            OpCode = (int)MsgType.Account,
            SubCode = (int)MsgType.Login,
            value = userInfo
        });
    }
    void OnCloseClick()
    {
        gameObject.SetActive(false);
    }
}
