using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Threading
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();

            int pointX = rand.Next(1920);
            int pointY = rand.Next(1080);

            while (true)
            {
                int userX = Cursor.Position.X;
                int userY = Cursor.Position.Y;

                if (userX < pointX)
                {
                    Console.WriteLine("Behind");
                }
                else
                {
                    Console.WriteLine("Past");
                }
            }
            
        }

        //Neither of these functions detect mouse clicks
        private void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Console.WriteLine("Test");
                Console.WriteLine(Cursor.Position);
            }
        }

        private void Control1_MouseCLick(Object sender, MouseEventArgs e)
        {
            Console.WriteLine("test");
        }
    }
}
