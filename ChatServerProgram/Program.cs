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

            MMONet.MMOClient client = new MMONet.MMOClient();
            client.BeginConnect(IPAddress.Loopback, 40000, callback, client);
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
                MMOClient cl = ar.AsyncState as MMOClient;
                cl.EndConnect(ar);

                //Stopwatch s = new Stopwatch();
                //s.Start();

                ChatMsg msg = new ChatMsg() { CharacterID = 1, Context = "test" };
                for (int i = 0; i < 10000; i++)
                {
                    msg = new ChatMsg();
                    msg.CharacterID = i;
                    cl.Write(msg);
                }
                //s.Stop();
                //Console.WriteLine("-----------" + s.ElapsedMilliseconds);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
