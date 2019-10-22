using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    /// <summary>
    /// 服务端
    /// </summary>
    public class ServerPeer
    {
        /// <summary>
        /// 服务端的socket对象
        /// </summary>
        private static Socket serverSocket;
        /// <summary>
        /// 限制客户端连接数量的信号量
        /// </summary>
        private Semaphore acceptSemaphore;
        /// <summary>
        /// 客户端连接池
        /// </summary>
        private ClientPeerPool clientPeerPool;
        /// <summary>
        /// 开启服务器
        /// </summary>
        /// <param name="port">端口号</param>
        /// <param name="maxCount">最大连接数</param>
        public void Start(int port, int maxCount)
        {
            try
            {
                //初始化连接池
                clientPeerPool = new ClientPeerPool(maxCount);
                ClientPeer tmpClientPeer;
                for (int i = 0; i < maxCount; i++)
                {
                    tmpClientPeer = new ClientPeer();
                    tmpClientPeer.receiveArgs = new SocketAsyncEventArgs();

                    tmpClientPeer.receiveCompleted += ReceiveCompleted;
                    tmpClientPeer.receiveArgs.Completed += Receive_Completed;

                    tmpClientPeer.receiveArgs.UserToken = tmpClientPeer;
                    clientPeerPool.Enqueue(tmpClientPeer);
                }
                //创建socket
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //限制访问线程数
                acceptSemaphore = new Semaphore(maxCount, maxCount);
                //绑定ip端口号
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                //连接数量
                serverSocket.Listen(10);
                Console.WriteLine("服务器启动...");
                StartAccept(null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #region 接收连接
        /// <summary>
        /// 开始等待客户端连接
        /// </summary>
        private void StartAccept(SocketAsyncEventArgs e)
        {
            if (e == null)
            {
                e = new SocketAsyncEventArgs();
                e.Completed += AcceptCompleted;
            }
            //限制线程的访问
            acceptSemaphore.WaitOne();
            //返回执行状态
            bool result = serverSocket.AcceptAsync(e);
            if (result == false)
            {
                ProcessAccept(e);
            }
        }
        /// <summary>
        /// 接受连接请求异步事件完成时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }
        /// <summary>
        /// 处理连接请求
        /// </summary>
        /// <param name="e"></param>
        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            ClientPeer clientPeer = clientPeerPool.Dequeue();
            clientPeer.ClientSocket = e.AcceptSocket;
            //开始接收数据
            StartReceive(clientPeer);
            //清空
            e.AcceptSocket = null;
            StartAccept(e);
        }
        #endregion
        #region 接收数据
        private void StartReceive(ClientPeer clientPeer)
        {
            try
            {
                bool result = clientPeer.ClientSocket.ReceiveAsync(clientPeer.receiveArgs);
                if (result == false)
                {
                    ProcessReceive(clientPeer.receiveArgs);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// 处理接收的请求
        /// </summary>
        /// <param name="e"></param>
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            ClientPeer client = e.UserToken as ClientPeer;
            //判断网络消息是否接收成功
            if (client.receiveArgs.SocketError == SocketError.Success && client.receiveArgs.BytesTransferred > 0)
            {
                byte[] packet = new byte[client.receiveArgs.BytesTransferred];
                Buffer.BlockCopy(client.receiveArgs.Buffer, 0, packet, 0, client.receiveArgs.BytesTransferred);
                //让客户端自身处理数据
                client.StartReceive(packet);
                //
                StartReceive(client);
            }
            else
            {
                //如果没有传输的字节数，就表示断开连接
                if (client.receiveArgs.BytesTransferred == 0)
                {
                    if (client.receiveArgs.SocketError == SocketError.Success)
                    {
                        //客户端主动断开连接
                    }
                    else
                    {
                        //由于网络异常 客户端断开连接
                    }
                }
            }
        }
        /// <summary>
        /// 当接收完成时 触发的事件
        /// </summary>
        /// <param name="e"></param>
        private void Receive_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessReceive(e);
        }
        /// <summary>
        /// 数据解析完成后的处理
        /// </summary>
        /// <param name="client"></param>
        /// <param name="value"></param>
        private void ReceiveCompleted(ClientPeer client, SocketMsg value)
        {

        }
        #endregion

        #region 发送数据

        #endregion
        #region 断开连接
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="client"></param>
        /// <param name="reason">断开的原因</param>
        public void DisConnect(ClientPeer client,string reason)
        {
            try
            {
                if (client==null)
                {
                    throw new Exception("当前指定的客户端不存在，无法断开连接");
                }
                client.Disconnect();
                //回收当前客户端
                clientPeerPool.Enqueue(client);
                //退出信号量并返回上一个计数。
                acceptSemaphore.Release();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

    }
}
