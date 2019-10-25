using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
    class Program
    {
        private static Socket clientSocket = null;
        static void Main(string[] args)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            //指定服务端地址
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"),9999);
            //建立连接
            clientSocket.Connect(remoteEP);
            Console.WriteLine("连接到远程服务器");

            byte[] buffer = new byte[1024];
            //接收到的数据长度
            int length = clientSocket.Receive(buffer);

            Console.WriteLine("收到来自服务端的消息：" + Encoding.Default.GetString(buffer, 0, length));

            clientSocket.Send(Encoding.Default.GetBytes("客户端给服务器发送的消息"));

            Console.ReadLine();
        }
        
    }
}
