using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    /// <summary>
    /// 网络消息
    /// </summary>
    public class SocketMsg
    {
        /// <summary>
        /// 操作码
        /// </summary>
        public int OpCode { get; set; }
        /// <summary>
        /// 子操作
        /// </summary>
        public int SubCode { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public object value { get; set; }

        public SocketMsg()
        {
        }
        public SocketMsg(int opCode, int subCode, object value)
        {
            this.OpCode = opCode;
            this.SubCode = subCode;
            this.value = value;
        }
    }
}
