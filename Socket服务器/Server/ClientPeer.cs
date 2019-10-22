using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    /// <summary>
    /// 封装客户端
    /// </summary>
    public class ClientPeer
    {
        public Socket ClientSocket { get; set; }
        /// <summary>
        /// 数据解析完成的回调 委托
        /// </summary>
        /// <param name="client"></param>
        /// <param name="value"></param>
        public delegate void ReceiveCompleted(ClientPeer client, SocketMsg value);
        public ReceiveCompleted receiveCompleted;
        /// <summary>
        /// 接收的异步套接字请求
        /// </summary>
        public SocketAsyncEventArgs receiveArgs;
        /// <summary>
        /// 接收到的数据，存放在数组中
        /// </summary>
        private List<byte> dataCache = new List<byte>();
        /// <summary>
        /// 是否正在处理接收的数据
        /// </summary>
        private bool isProcess = false;
        #region 接收数据
        /// <summary>
        /// 处理数据包
        /// </summary>
        /// <param name="packet"></param>
        public void StartReceive(byte[] packet)
        {
            dataCache.AddRange(packet);
            if (!isProcess)
            {
                ProcessReceive();
            }
        }
        /// <summary>
        /// 处理接收的数据
        /// </summary>
        private void ProcessReceive()
        {
            isProcess = true;
            //解析数据包
            byte[] data = EncodeHelper.DecodePacket(ref dataCache);
            if (data == null)
            {
                isProcess = false;
                return;
            }
            SocketMsg msg = EncodeHelper.DecodeMsg(data);
            //回调给上层
            receiveCompleted?.Invoke(this, msg);
            //伪递归
            ProcessReceive();
        }
        #endregion
        #region 断开连接
        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            //清空数据
            dataCache.Clear();
            isProcess = false;

            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            ClientSocket = null;

        }
        #endregion
    }
}
