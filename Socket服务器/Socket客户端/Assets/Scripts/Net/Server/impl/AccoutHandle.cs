using Components.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AccoutHandle : HandleBase
{


    PromptMsg promptMsg = new PromptMsg();
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case (int)MsgType.Login_Check:
                LoginResponse((MsgType)value);
                break;
            case (int)MsgType.Regist_Check:
                RegistResponse((MsgType)value);
                break;
        }
    }
    /// <summary>
    /// 登录请求处理
    /// </summary>
    /// <param name="msg"></param>
    private void LoginResponse(MsgType msg)
    {
        if (msg == MsgType.Success)
        {
            promptMsg.Text = "登录成功";
            PromptPanel.Instance.PromptMessage(promptMsg);
            return;
        }
        switch (msg)
        {
            case MsgType.AccountDoesNotExist:
                promptMsg.Text = "账号不存在";
                break;
            case MsgType.AccountOnline:
                promptMsg.Text = "账号在线";
                break;
            case MsgType.AccountPasswordDoesNotMatch:
                promptMsg.Text = "账号密码不匹配";
                break;
        }
        PromptPanel.Instance.PromptMessage(promptMsg);
    }
    /// <summary>
    /// 注册请求处理
    /// </summary>
    /// <param name="msg"></param>
    private void RegistResponse(MsgType msg)
    {
        if (msg == MsgType.Success)
        {
            promptMsg.Text = "注册成功";
            PromptPanel.Instance.PromptMessage(promptMsg);
            return;
        }
        switch (msg)
        {
            case MsgType.AccountAlreadyExists:
                promptMsg.Text = "账号已存在";
                break;
            case MsgType.AccountEntryIsIllegal:
                promptMsg.Text = "账号输入不合法";
                break;
            case MsgType.ThePasswordIsIllegal:
                promptMsg.Text = "密码不合法,应在4~16个字符之间";
                break;
        }
        PromptPanel.Instance.PromptMessage(promptMsg);
    }
}