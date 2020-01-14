using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightStartsPanel : StartsPanel
{

    public override void Start()
    {
        base.Start();
        Bind(UIEvent.Set_Right_Player_Data);

        //如果！= -1 就代表 有角色
        MatchRoomDto room = Data.GameData.MatchRoomDto;
        int rightId = room.RightId;
        if (rightId != -1)
        {
            this.userCharacterDto = room.UIdClientDict[rightId];
            if (room.ReadyUIdList.Contains(rightId))
            {
                ReadyState(true);
            }
            else
            {
                //nothing
            }
        }
        else
        {
            SetPanelActive(false);
        }
    }
    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            case UIEvent.Set_Right_Player_Data:
                this.userCharacterDto = message as UserCharacterDto;
                break;
           
        }
    }
}
