using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MatchHandler : HandleBase
{

    PromptMsg promptMsg = new PromptMsg();
    public override void OnReceive(SocketMsg msg)
    {
        switch ((MatchCode)msg.SubCode)
        {
            case MatchCode.EnterMatch_Result:
                MyEnterBroadcastResult(msg);
                break;
            case MatchCode.EnterMatch_Broadcast_Result:
                EnterBroadcastResult(msg);
                break;
            case MatchCode.LeaveMatch_Broadcast_Result:
                LeaveBroadcastResult(msg);
                break;
            case MatchCode.Ready_Broadcast_Result:
                ReadyBroadcastResult(msg);
                break;
            case MatchCode.Start_Broadcast_Requst:
                StartBroadcastResult(msg);
                break;
        }
    }
    /// <summary>
    /// 他人进入房间广播
    /// </summary>
    /// <param name="socketMsg"></param>
    private void EnterBroadcastResult(SocketMsg socketMsg)
    {
        if ((MatchCode)socketMsg.State == MatchCode.Success)
        {
            var newUser = socketMsg.value as UserCharacterDto;
            //更新房间数据
            MatchRoomDto room = Data.GameData.MatchRoomDto;
            room.UIdClientDict.Add(newUser.Id, newUser);
            room.UIdList.Add(newUser.Id);

            ResetPosition();

            //给UI绑定数据
            if (room.LeftId != -1)
            {
                UserCharacterDto leftUserDto = room.UIdClientDict[room.LeftId];
                Dispatch(AreaCode.UI, UIEvent.Set_Left_Player_Data, leftUserDto);
            }
            if (room.RightId != -1)
            {
                UserCharacterDto rightUserDto = room.UIdClientDict[room.RightId];
                Dispatch(AreaCode.UI, UIEvent.Set_Right_Player_Data, rightUserDto);
            }
            //发消息 显示玩家的状态面板所有游戏物体
            Dispatch(AreaCode.UI, UIEvent.Player_Enter, newUser.Id);
            //给用户一个提示
            promptMsg.Text="有新玩家 ( " + newUser.Name + " )进入";
            Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
        }
    }
    /// <summary>
    /// 自己进入房间广播
    /// </summary>
    /// <param name="socketMsg"></param>
    private void MyEnterBroadcastResult(SocketMsg socketMsg)
    {
        if ((MatchCode)socketMsg.State == MatchCode.Success)
        {
            Data.GameData.MatchRoomDto = socketMsg.value as MatchRoomDto;
            ResetPosition();
            MatchRoomDto room = Data.GameData.MatchRoomDto;
            if (room.LeftId != -1)
            {
                UserCharacterDto leftUserDto = room.UIdClientDict[room.LeftId];
                Dispatch(AreaCode.UI, UIEvent.Set_Left_Player_Data, leftUserDto);
            }
            if (room.RightId != -1)
            {
                UserCharacterDto rightUserDto = room.UIdClientDict[room.RightId];
                Dispatch(AreaCode.UI, UIEvent.Set_Right_Player_Data, rightUserDto);
            }
            //显示进入房间的玩家对象
            Dispatch(AreaCode.UI, UIEvent.Show_EnterRoom_Button, null);
        }
    }
    /// <summary>
    /// 重置位置 更新左右玩家显示
    /// </summary>
    private void ResetPosition()
    {
        GameData data = Data.GameData;
        MatchRoomDto matchRoom = data.MatchRoomDto;

        //重置一下玩家的位置
        matchRoom.ResetPosition(data.UserCharacterDto.Id);
    }
    /// <summary>
    /// 离开匹配广播
    /// </summary>
    /// <param name="socketMsg"></param>
    private void LeaveBroadcastResult(SocketMsg socketMsg)
    {
        if ((MatchCode)socketMsg.State == MatchCode.Success)
        {
            var userId = (int)socketMsg.value;
            var userInfo = Data.GameData.MatchRoomDto.UIdClientDict[userId];
            //
            Data.GameData.MatchRoomDto.UIdClientDict.Remove(userId);
            Data.GameData.MatchRoomDto.UIdList.Remove(userId);
            //玩家离开隐藏当前玩家所有游戏对象
            Dispatch(AreaCode.UI, UIEvent.Player_Leave, userId);
            promptMsg.Text = string.Format("玩家（{0}）离开房间", userInfo.Name);
            Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
        }
    }
    /// <summary>
    /// 玩家准备广播
    /// </summary>
    /// <param name="socketMsg"></param>
    private void ReadyBroadcastResult(SocketMsg socketMsg)
    {
        if ((MatchCode)socketMsg.State == MatchCode.Success)
        {
            var userId = (int)socketMsg.value;
            Data.GameData.MatchRoomDto.ReadyUIdList.Add(userId);
            //显示玩家已准备文本框
            Dispatch(AreaCode.UI, UIEvent.Player_Ready, userId);
            //隐藏准备按钮
            if (userId == Data.GameData.UserCharacterDto.Id)
            {
                Dispatch(AreaCode.UI, UIEvent.Hide_Ready_Button, null);
            }
           
        }
    }
    /// <summary>
    /// 开始游戏广播
    /// </summary>
    /// <param name="socketMsg"></param>
    private void StartBroadcastResult(SocketMsg socketMsg)
    {
        if ((MatchCode)socketMsg.State == MatchCode.Success)
        {
            promptMsg.Text = "所有玩家已准备";
            Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
            Dispatch(AreaCode.UI, UIEvent.Player_Hide_State, null);
        }
    }
}