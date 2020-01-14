using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UIEvent
{
    /// <summary>
    /// 设置开始面板
    /// </summary>
    public const int Start_Code = 0;
    /// <summary>
    /// 设置注册面板的显示
    /// </summary>
    public const int Regist_Code = 1;
    /// <summary>
    /// 场景
    /// </summary>
    public const int Scene = 2;
    /// <summary>
    /// 刷新信息面板
    /// </summary>
    public const int Refresh_Info_Panel = 3;
    /// <summary>
    ///设置创建面板
    /// </summary>
    public const int Create_Panel = 4;
    /// <summary>
    /// 显示进入房间按钮
    /// </summary>
    public const int Show_EnterRoom_Button = 5;
    /// <summary>
    ///设置底牌
    /// </summary>
    public const int Set_Cards = 6;
    /// <summary>
    ///设置左边的角色数据
    /// </summary>
    public const int Set_Left_Player_Data = 7;
    /// <summary>
    ///设置右边的角色数据
    /// </summary>
    public const int Set_Right_Player_Data = 8;
    /// <summary>
    /// 角色准备
    /// </summary>
    public const int Player_Ready = 9;
    /// <summary>
    /// 角色进入
    /// </summary>
    public const int Player_Enter = 10;
    /// <summary>
    /// 角色离开
    /// </summary>
    public const int Player_Leave = 11;
    /// <summary>
    /// 角色聊天
    /// </summary>
    public const int Player_Chat = 12;
    /// <summary>
    /// 角色身份更改
    /// </summary>
    public const int Player_Change_Identity = 13;
    /// <summary>
    /// 开始游戏角色隐藏准备面板
    /// </summary>
    public const int Player_Hide_State = 14;
    /// <summary>
    /// 显示抢地主按钮
    /// </summary>
    public const int Show_Grab_Button = 15;
    /// <summary>
    /// 显示出牌按钮
    /// </summary>
    public const int Show_Deal_Button = 16;
    /// <summary>
    /// 设置自己角色数据
    /// </summary>
    public const int Set_MyPlayer_Data = 17;
    /// <summary>
    /// 隐藏准备按钮
    /// </summary>
    public const int Hide_Ready_Button = 18;
    /// <summary>
    /// 播放音效
    /// </summary>
    public const int EffectAudio = 19;
    /// <summary>
    /// 改变倍数
    /// </summary>
    public const int Change_Mutiple = 20;
    /// <summary>
    /// 刷新豆子
    /// </summary>
    public const int Change_Been = 21;
    /// <summary>
    /// 提示面板
    /// </summary>
    public const int Prompt_Msg = int.MaxValue;
}