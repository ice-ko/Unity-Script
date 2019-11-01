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

        public ClientPeer()
        {
            this.receiveArgs = new SocketAsyncEventArgs();
            this.receiveArgs.UserToken = this;
            this.receiveArgs.SetBuffer(new byte[1024], 0, 1024);
            this.sendArge = new SocketAsyncEventArgs();
            this.sendArge.Completed += SendArgs_Completed;
        }

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
            sendQueue.Clear();
            isSendProcess = false;

            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            ClientSocket = null;

        }
        #endregion
        #region 发送数据
        /// <summary>
        /// 发送的消息队列
        /// </summary>
        private Queue<byte[]> sendQueue = new Queue<byte[]>();
        /// <summary>
        /// 是否已处理消息队列
        /// </summary>
        private bool isSendProcess = false;
        /// <summary>
        /// 发送的异步套接字
        /// </summary>
        private SocketAsyncEventArgs sendArge;
        /// <summary>
        /// 发送数据的时候，发现断开连接的回调
        /// </summary>
        /// <param name="client"></param>
        /// <param name="reason"></param>
        public delegate void SendDisconnect(ClientPeer client, string reason);
        /// <summary>
        /// 委托方法
        /// </summary>
        public SendDisconnect sendDisconnect;
        /// <summary>
        /// 发送网络数据包
        /// </summary>
        /// <param name="packet"></param>
        public void Send(byte[] packet)
        {
            sendQueue.Enqueue(packet);
            if (!isSendProcess)
            {
                ProcessSend();
            }
        }
        /// <summary>
        /// 发送网络消息
        /// </summary>
        /// <param name="msg"></param>
        public void Send(SocketMsg msg)
        {
            byte[] data = EncodeHelper.EncodeMsg(msg);
            byte[] packet = EncodeHelper.EncodePacket(data);
            Send(packet);
        }
        /// <summary>
        /// 处理发送的消息
        /// </summary>
        private void SendMsg()
        {
            isSendProcess = true;
            if (sendQueue.Count == 0)
            {
                isSendProcess = false;
                return;
            }
            byte[] packet = sendQueue.Dequeue();
            //设置消息发送异步套接字操作的发送数据缓冲区
            sendArge.SetBuffer(packet, 0, packet.Length);
            //
            bool result = ClientSocket.SendAsync(sendArge);
            if (result == false)
            {
                ProcessSend();
            }
        }
        /// <summary>
        /// 异步发送请求完成调用
        /// </summary>
        private void ProcessSend()
        {
            if (sendArge.SocketError != SocketError.Success)
            {
                //发送错误 客户端断开连接
                sendDisconnect(this, SocketError.SocketError.ToString());
            }
            else
            {
                SendMsg();
            }
        }
        /// <summary>
        /// 异步发送请求完成调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SendArgs_Completed(object sender, SocketAsyncEventArgs args)
        {
            ProcessSend();
        }
        #endregion
    }
}
