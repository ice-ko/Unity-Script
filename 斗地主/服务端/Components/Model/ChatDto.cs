using System;
using System.Collections.Generic;
using System.Text;
/// <summary>
/// 聊天信息
/// </summary>
[Serializable]
public class ChatDto
{
    /// <summary>
    /// 发送人
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// 类型
    /// </summary>
    public int Type { get; set; }
    /// <summary>
    /// 消息
    /// </summary>
    public string Text { get; set; }
}
