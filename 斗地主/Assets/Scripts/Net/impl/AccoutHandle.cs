using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AccoutHandle : HandleBase
{


    PromptMsg promptMsg = new PromptMsg();
    public override void OnReceive(SocketMsg msg)
    {
        var code = (AccountCode)msg.SubCode;
        switch (code)
        {
            case AccountCode.Login_Check:
                LoginResponse((AccountCode)msg.State);
                break;
            case AccountCode.Regist_Check:
                RegistResponse((AccountCode)msg.State);
                break;
        }
    }
    /// <summary>
    /// 登录请求处理
    /// </summary>
    /// <param name="code"></param>
    private void LoginResponse(AccountCode code)
    {
        if (code == AccountCode.Success)
        {
            Dispatch(AreaCode.SCENE, UIEvent.Scene, new LoadSceneMsg
            {
                SceneIndex = 1,
                OnSceneLoaded = () =>
                {
                    Dispatch(AreaCode.NET, 0, new SocketMsg
                    {
                        OpCode = MsgType.User,
                        SubCode =UserCode.GetInfoRequest
                    });
                }
            });
            return;
        }
        switch (code)
        {
            case AccountCode.AccountDoesNotExist:
                promptMsg.Text = "账号不存在";
                break;
            case AccountCode.AccountOnline:
                promptMsg.Text = "账号在线";
                break;
            case AccountCode.AccountPasswordDoesNotMatch:
                promptMsg.Text = "账号密码不匹配";
                break;
        }
        Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
    }
    /// <summary>
    /// 注册请求处理
    /// </summary>
    /// <param name="code"></param>
    private void RegistResponse(AccountCode code)
    {
        if (code == AccountCode.Success)
        {
            promptMsg.Text = "注册成功";
            Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
            return;
        }
        switch (code)
        {
            case AccountCode.AccountAlreadyExists:
                promptMsg.Text = "账号已存在";
                break;
            case AccountCode.AccountEntryIsIllegal:
                promptMsg.Text = "账号输入不合法";
                break;
            case AccountCode.ThePasswordIsIllegal:
                promptMsg.Text = "密码不合法,应在4~16个字符之间";
                break;
        }
        Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
    }
}