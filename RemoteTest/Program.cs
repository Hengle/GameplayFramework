using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMONet;
using ProtoBuf;
using System.Net;
using System.Threading;

namespace RemoteTest
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomRemote cl = new CustomRemote();

            var list = Dns.GetHostAddresses("www.mikumikufight.top");

            IPAddress ip = list[0];
            cl.BeginConnect(ip, 39393, callback, cl);
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.F1)
                {
                    var msg = new QLogin()
                    {
                        Note = "test"
                    };
                    cl.Write(msg);
                    Console.WriteLine("fasong");
                }
                Thread.Sleep(1);
            }
            Console.ReadLine();
        }

        private static void callback(IAsyncResult ar)
        {
            CustomRemote cl = ar.AsyncState as CustomRemote;
            cl.EndConnect(ar);

            var msg = new QLogin()
            {
                Note = "test"
            };
            cl.Write(msg);
            Console.WriteLine("fasong");
        }
    }
}
