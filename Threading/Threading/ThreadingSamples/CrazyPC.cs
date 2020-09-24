using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Threading.ThreadingSamples
{
    class CrazyPC
    {
        public static Random _rand = new Random();
        public static void CrazyFunctionCall()
        {
            Thread mouseThread = new Thread(new ThreadStart(MouseThread));
            Thread keyboardThread = new Thread(new ThreadStart(KeyboardThread));
            Thread soundThread = new Thread(new ThreadStart(SoundThread));
            Thread popThread = new Thread(new ThreadStart(PopThread));

            mouseThread.Start();
            keyboardThread.Start();
            soundThread.Start();
            popThread.Start();

            //Console.ReadKey();
            //mouseThread.Abort();
            //keyboardThread.Abort();
            //soundThread.Abort();
            //popThread.Abort();
        }

        static void MouseThread()
        {
            int moveX = 0;
            int moveY = 0;

            while (true)
            {
                moveX = _rand.Next(30) - 15;
                moveY = _rand.Next(30) - 15;

                Cursor.Position = new System.Drawing.Point(Cursor.Position.X + moveX, Cursor.Position.Y + moveY);
                Thread.Sleep(50);
            }
        }

        static void KeyboardThread()
        {
            while (true)
            {
                char key = (char)(_rand.Next(50) + 45);
                //SendKeys.SendWait("A");

                if (_rand.Next(2) == 0)
                {
                    key = Char.ToLower(key);
                }
                SendKeys.SendWait(key.ToString());
                Thread.Sleep(50);
            }
        }

        static void SoundThread()
        {

        }

        static void PopThread()
        {

        }
    }
}
