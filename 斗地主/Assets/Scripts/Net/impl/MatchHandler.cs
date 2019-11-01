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
                EnterMatchResult(msg);
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
    /// 进入匹配返回
    /// </summary>
    /// <param name="socketMsg"></param>
    private void EnterMatchResult(SocketMsg socketMsg)
    {
        if ((MatchCode)socketMsg.State == MatchCode.Success)
        {
            //保存数据
            Data.GameData.MatchRoomDto = socketMsg.value as MatchRoomDto;
            //显示进入房间按钮
            Dispatch(AreaCode.UI, UIEvent.Show_EnterRoom_Button, null);
        }

    }
    /// <summary>
    /// 进入房间广播
    /// </summary>
    /// <param name="socketMsg"></param>
    private void EnterBroadcastResult(SocketMsg socketMsg)
    {
        if ((MatchCode)socketMsg.State == MatchCode.Success)
        {
            //更新房间数据
            UserCharacterDto user = socketMsg.value as UserCharacterDto;
            Data.GameData.MatchRoomDto.UIdClientDict.Add(user.Id, user);
            //房间信息提示
            promptMsg.Text = "有新的玩家（" + user.Name + "）进入房间";
            Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
            //更新ui
            // TODO 
        }
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
            Data.GameData.MatchRoomDto.UIdClientDict.Remove(userId);
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
        }
    }
}