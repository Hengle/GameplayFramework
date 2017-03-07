using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServerProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatServer.Server s = new ChatServer.Server();
            var t = new Thread(s.Run);
            t.Start();
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
