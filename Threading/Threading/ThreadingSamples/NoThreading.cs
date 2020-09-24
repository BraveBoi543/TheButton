using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threading.ThreadingSamples
{
    class NoThreading
    {
        public static void NoThreadFunctions()
        {
            NoThread1();
            NoThread2();
        }

        static void NoThread1()
        {
            while (true)
            {
                Console.WriteLine("This is THREAD 1");
            }
        }

        static void NoThread2()
        {
            while (true)
            {
                Console.WriteLine("This is THREAD 2");
            }
        }
    }
}
