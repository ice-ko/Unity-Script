using System;
using System.Collections.Generic;
using System.Text;


/// <summary>
/// 网络消息
/// </summary>
[Serializable]
public class SocketMsg 
{
    /// <summary>
    /// 操作码
    /// </summary>
    public MsgType OpCode { get; set; }
    /// <summary>
    /// 子操作
    /// </summary>
    public Enum SubCode { get; set; }
    /// <summary>
    /// 参数
    /// </summary>
    public object value { get; set; }
    /// <summary>
    /// 状态
    /// </summary>
    public Enum State { get; set; }

    public SocketMsg()
    {
    }
    public SocketMsg(Enum state)
    {
        this.State = state;
    }
    public SocketMsg(MsgType opCode, Enum subCode, Enum state, object value = null)
    {
        this.OpCode = opCode;
        this.SubCode = subCode;
        this.value = value;
        this.State = state;
    }
}
