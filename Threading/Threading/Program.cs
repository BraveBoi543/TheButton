using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threading
{
    class Program
    {
        static void Main(string[] args)
        {
            //RunNoThread();
            //RunThreadingSample1();
            RunPC();
        }

        static void RunNoThread()
        {
            ThreadingSamples.NoThreading.NoThreadFunctions();
        }

        static void RunThreadingSample1()
        {
            ThreadingSamples.ThreadingSample1.DemoThread();
        }

        static void RunPC()
        {
            ThreadingSamples.CrazyPC.CrazyFunctionCall();
        }
    }
}
