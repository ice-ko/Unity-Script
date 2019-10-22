using Server;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerPeer serverPeer = new ServerPeer();
            serverPeer.Start(9999,10);


            Console.ReadKey();
        }
       
    }
}
