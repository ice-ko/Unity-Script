using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ChatHandler : HandleBase
{
    public override void OnReceive(SocketMsg msg)
    {
        var code = (ChatCode)msg.SubCode;
        switch (code)
        {
            case ChatCode.Result:
                ChatDto chatDto = msg.value as ChatDto;
                //显示文字
                Dispatch(AreaCode.UI, UIEvent.Player_Chat, chatDto);
                //播放声音
                Dispatch(AreaCode.UI,UIEvent.EffectAudio, "Chat/Chat_" + chatDto.Type);
                break;
            default:
                break;
        }
    }
}