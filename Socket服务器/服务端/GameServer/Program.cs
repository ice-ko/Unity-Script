using Server;
using SqlSugar;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Utils.Tool;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //var db = DatabaseHelper.GetInstance();
            //db.DbFirst.CreateClassFile(@"E:\Unity\Socket服务器\GameServer\Model", "Model");//生成数据库所有表实体类
            //db.DbFirst.Where("UaseInfo").CreateClassFile(@"E:\Unity\Socket服务器\GameServer\Model", "GameServer.Model");//生成指定表实体类

            ServerPeer serverPeer = new ServerPeer();
            //指定关联的应用
            serverPeer.SetApplication(new NetMsgCenter());
            //设置端口ip
            serverPeer.Start(9999, 10);


            Console.ReadKey();
        }

    }
}
