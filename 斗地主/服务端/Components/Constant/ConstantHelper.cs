using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 通用常量类
/// </summary>
public static class ConstantHelper
{
    /// <summary>
    /// 存储的是聊天内容格式：类型->聊天内容
    /// </summary>
    private static Dictionary<int, string> typeTextDict = new Dictionary<int, string>();
    static ConstantHelper()
    {
        typeTextDict.Add(1, "大家好，很高兴见到各位~");
        typeTextDict.Add(2, "和你合作真是太愉快了！");
        typeTextDict.Add(3, "快点吧，我等到花都谢了。");
        typeTextDict.Add(4, "你的牌打的太好了！");
        typeTextDict.Add(5, "不要吵了，有什么好吵的，专心玩游戏吧！");
        typeTextDict.Add(6, "不要走，决战到天亮！");
        typeTextDict.Add(7, "再见了，我会想念大家的~");
    }
    /// <summary>
    /// 获取聊天内容
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetChatText(int type)
    {
        return typeTextDict[type];
    }
}