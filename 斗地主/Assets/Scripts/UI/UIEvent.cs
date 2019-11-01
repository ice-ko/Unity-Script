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
    public const int Refresh_Info_Panel= 3;
    /// <summary>
    ///设置创建面板
    /// </summary>
    public const int Create_Panel = 4;
    /// <summary>
    /// 显示进入房间按钮
    /// </summary>
    public const int Show_EnterRoom_Button=5;
    /// <summary>
    /// 提示面板
    /// </summary>
    public const int Prompt_Msg = int.MaxValue;
}