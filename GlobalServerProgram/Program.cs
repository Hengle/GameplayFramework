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
            ConsoleKey exit = ConsoleKey.F8;
            Console.WriteLine($"**********************************************");
            Console.WriteLine($"    服务器已经启动，按{exit}键关闭所有服务器进程……");
            Console.WriteLine($"**********************************************");
            GlobalServer.Server s = new GlobalServer.Server();
            s.Init(args);
            var t = new Thread(s.Run);
            t.Start();

            //MMONet.Client client = new MMONet.Client();
            //client.BeginConnect(IPAddress.Loopback, 40000, callback, client);
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == exit)
                {
                    s.Stop();
                    t.Abort();
                    t.DisableComObjectEagerCleanup();
                    break;
                }
                Thread.Sleep(1);
            }
        }
    }
}
