using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatServer.Server s = new ChatServer.Server();
            
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
                System.Threading.Thread.Sleep(50);
            }
        }
    }
}
