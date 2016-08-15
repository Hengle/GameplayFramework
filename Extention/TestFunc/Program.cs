using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFunc
{
    class Program
    {
        static void Main(string[] args)
        {
            string test = "stedssdfasfsdff2343sdf*&^&^adf";
            Console.WriteLine(test);
            string temp = test.Encipher();
            Console.WriteLine(temp);
            string original = temp.Decrypt();
            Console.WriteLine(original);
            Console.ReadLine();
        }
    }
}
