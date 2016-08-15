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
            Test22 t = Test22.B;
            EnumExtention.Add(ref t, Test22.C);

            bool res = t.Contain(Test22.C);

            EnumExtention.Remove(ref t, Test22.C);

            Console.ReadLine();
        }



    }

    [Flags]
    enum Test22
    {
        A = 0x1,
        B = 0x2,
        C = 0x4,
    }
}
