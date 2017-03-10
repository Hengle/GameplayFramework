using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using MMONet;
using ProtoBuf;
using System.Diagnostics;

namespace ChatServerProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatServer.Server s = new ChatServer.Server();
            var t = new Thread(s.Run);
            t.Start();

            //MMONet.Client client = new MMONet.Client();
            //client.BeginConnect(IPAddress.Loopback, 40000, callback, client);
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    t.Abort();
                    t.DisableComObjectEagerCleanup();
                    break;
                }
                Thread.Sleep(50);
            }
        }

        private static void callback(IAsyncResult ar)
        {
            try
            {
                Client cl = ar.AsyncState as Client;
                cl.EndConnect(ar);
                List<ChatMsg> list = new List<ChatMsg>();
                var msg = new ChatMsg();
                for (int i = 0; i < 10000; i++)
                {
                    msg = new ChatMsg();
                    msg.CharacterID = i + 1;
                    cl.Write(msg);
                    Console.WriteLine("发送消息----------------------------------" + msg.CharacterID);
                }

                //foreach (var item in list)
                //{
                //    cl.Write(item);
                //}
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
