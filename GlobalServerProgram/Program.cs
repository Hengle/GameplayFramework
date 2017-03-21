using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalServerProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalServer.Server s = new GlobalServer.Server();
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
    }
}
