using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftStartsPanel : StartsPanel
{
    public override void Start()
    {
        base.Start();

        Bind(UIEvent.Set_Left_Player_Data);

        //fix bug
        //如果！= -1 就代表 有角色
        MatchRoomDto room = Data.GameData.MatchRoomDto;
        int leftId = room.LeftId;
        if (leftId != -1)
        {
            this.userCharacterDto = room.UIdClientDict[leftId];
            if (room.ReadyUIdList.Contains(leftId))
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
            case UIEvent.Set_Left_Player_Data:
                this.userCharacterDto = message as UserCharacterDto;
                break;
        }
    }

}
