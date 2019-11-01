using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GameServer
{
    class ProgramS
    {
        static Socket serverSocket = null;
        static void Maina(string[] args)
        {
            //AddressFamily.InterNetwork 地址族
            //SocketType.Stream 指定类型
            //ProtocolType.Tcp 指定协议
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //绑定端口号
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 9999);
            serverSocket.Bind(endPoint);
            //连接队列的长度
            serverSocket.Listen(10);
            Console.WriteLine("开始监听");
            //开启新的线程监听客户端连接信息
            Thread thread = new Thread(ListenClientConnect);
            thread.Start();

            Console.ReadLine();
        }
        /// <summary>
        /// 监听客户端连接
        /// </summary>
        private static void ListenClientConnect()
        {
            //等待客户端接入，接入成功触发 返回客户端Socket对象
            Socket clientSocket = serverSocket.Accept();
            Console.WriteLine("客户端连接成功,信息:"+clientSocket.AddressFamily.ToString());
            //给客户端发送消息
            clientSocket.Send(Encoding.Default.GetBytes("连接成功"));
            //开启新的线程监听客户端连接信息
            Thread recThread = new Thread(ReceiveClientMessage);
            recThread.Start(clientSocket);
        }
        /// <summary>
        /// 接收来自客户端的消息
        /// </summary>
        private static void ReceiveClientMessage(object clientSocket)
        {
            Socket socket = clientSocket as Socket;
            byte[] buffer = new byte[1024];
            //接收到的数据长度
            int length = socket.Receive(buffer);

            Console.WriteLine(Encoding.Default.GetString(buffer, 0, length));
        }
    }
}
