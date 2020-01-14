using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class FightHandler : HandleBase
{
    public override void OnReceive(SocketMsg msg)
    {
        var code = (FightCode)msg.SubCode;
        switch (code)
        {
            case FightCode.Get_Card_Result:
                GetCard(msg);
                break;
            case FightCode.Turn_Grab_Bro:
                TurnGrabBro(msg);
                break;
            case FightCode.Grab_Landlord_Bro:
                GrabLandlordBro(msg); break;
            default:
                break;
        }
    }
    /// <summary>
    /// 是否第一个玩家抢地主，而不是别的玩家不叫而到他
    /// </summary>
    private bool isFirst = true;
    /// <summary>
    /// 转换抢地主
    /// </summary>
    /// <param name="msg"></param>
    private void TurnGrabBro(SocketMsg msg)
    {
        if (isFirst == true)
        {
            isFirst = false;
        }
        else
        {
            Dispatch(AreaCode.UI, UIEvent.EffectAudio, "Fight/Woman_NoOrder");
        }
        var userId = (int)msg.value;
        if (userId == Data.GameData.UserCharacterDto.Id)
        {
            Dispatch(AreaCode.UI, UIEvent.Show_Grab_Button, true);
        }
    }
    /// <summary>
    /// 抢地主成功的处理
    /// </summary>
    /// <param name="msg"></param>
    private void GrabLandlordBro(SocketMsg msg)
    {
        var dto = msg.value as GrabDto;
        Dispatch(AreaCode.UI, UIEvent.Player_Change_Identity, dto.UserId);
        //
        Dispatch(AreaCode.UI, UIEvent.EffectAudio, "Fight/Woman_Order");
        //显示底牌
        Dispatch(AreaCode.UI, UIEvent.Set_Cards, dto.TableCardList);
        //给地主玩家添加底牌
        int eventCode = -1;
        if (dto.UserId == Data.GameData.MatchRoomDto.LeftId)
        {
            eventCode = CharacterEvent.Add_LeftCard;
        }
        else if (dto.UserId == Data.GameData.MatchRoomDto.RightId)
        {
            eventCode = CharacterEvent.Add_RightCard;
        }
        else if (dto.UserId == Data.GameData.UserCharacterDto.Id)
        {
            eventCode = CharacterEvent.Add_MyCard;
        }
        Dispatch(AreaCode.CHARACTER, eventCode, dto);
    }
    /// <summary>
    /// 获取卡牌
    /// </summary>
    /// <param name="msg"></param>
    private void GetCard(SocketMsg msg)
    {
        //设置玩家卡牌
        Dispatch(AreaCode.CHARACTER, CharacterEvent.Init_MyCard, msg.value);
        Dispatch(AreaCode.CHARACTER, CharacterEvent.Init_LeftCard, null);
        Dispatch(AreaCode.CHARACTER, CharacterEvent.Init_RightCard, null);
        //设置倍数
        Dispatch(AreaCode.UI, UIEvent.Change_Mutiple, 1);
    }
}