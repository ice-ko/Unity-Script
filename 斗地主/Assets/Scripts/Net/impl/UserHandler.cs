using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 用户角色信息
/// </summary>
public class UserHandler : HandleBase
{
    public override void OnReceive(SocketMsg msg)
    {
        var code = (UserCode)msg.SubCode;
        switch (code)
        {
            case UserCode.CreateCharacterResult:
                CreateCharacterResult(msg);
                break;
            case UserCode.GetInfoResult:
                GetInfoResult(msg);
                break;
            case UserCode.OnlineResult:
                OnlineResult(msg);
                break;
        }
    }
    SocketMsg socketMsg = new SocketMsg();
    /// <summary>
    /// 获取用户角色信息
    /// </summary>
    /// <param name="p"></param>
    private void GetInfoResult(SocketMsg p)
    {
        if ((UserCode)p.State != UserCode.Success)
        {
            Debug.Log("获取用户角色信息");
            //显示创建面板
            Dispatch(AreaCode.UI, UIEvent.Create_Panel, true);
        }
        else
        {
            //隐藏创建面板
            Dispatch(AreaCode.UI, UIEvent.Create_Panel, false);
            //保存数据
            Data.GameData.UserCharacterDto = p.value as UserCharacterDto;
            //刷新数据
            Dispatch(AreaCode.UI, UIEvent.Refresh_Info_Panel, p.value);
        }
    }
    /// <summary>
    /// 用户上线
    /// </summary>
    /// <param name="p"></param>
    private void OnlineResult(SocketMsg p)
    {
        var state = (UserCode)p.State;
        if (state == UserCode.Success)
        {
            //隐藏创建面板
            Dispatch(AreaCode.UI, UIEvent.Create_Panel, false);
            Debug.Log("上线成功");
        }
        //账号未上线 非法操作
        if (state == UserCode.AccountNotOnline)
        {
            Debug.Log("账号未上线 非法操作");
        }
        //用户角色存在
        if (state == UserCode.UserExist)
        {
            Debug.Log("用户角色存在");
        }
    }
    /// <summary>
    /// 创建角色名称 成功重新获取角色数据
    /// </summary>
    /// <param name="socketMsg"></param>
    void CreateCharacterResult(SocketMsg socketMsg)
    {
        if ((UserCode)socketMsg.State == UserCode.Success)
        {
            //创建用户名称
            Dispatch(AreaCode.NET, 0, new SocketMsg
            {
                OpCode = MsgType.User,
                SubCode = UserCode.GetInfoRequest
            });
        }

    }
}