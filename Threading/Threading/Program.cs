using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Timers;

namespace Threading
{
    class Program
    {
        // Construct for Timer object
        private static System.Timers.Timer aTimer;
       
        // Global Variables
        public static class Globals
        {
            public static bool gameLoop = true;
        }


        static void Main(string[] args)
        {
            // Point generation
            Random rand = new Random();

            int pointX = rand.Next(1920);
            int pointY = rand.Next(1080);

            // Game loop
            while (Globals.gameLoop)
            {
                // Creating and starting timer
                aTimer = new System.Timers.Timer(100000);
                aTimer.Elapsed += OnTimedEvent;
                aTimer.Enabled = true;

                // Gather current cursor x and y values
                int userX = Cursor.Position.X;
                int userY = Cursor.Position.Y;

                // Get distance to point
                int distance = SlopeDistance(pointX, pointY, userX, userY);
                // Get distance converted to name
                string displayName = DisplayDistance(distance);
                // Print name on single line
                Console.Write($"\r{displayName}");
                
                if (displayName == "Hit     ")
                {
                    aTimer.Enabled = false;
                    Globals.gameLoop = false;
                }

            }

            Console.WriteLine("Game over");
            Console.ReadKey();
            
        }

        public static int SlopeDistance(int pointX, int pointY, int userX, int userY)
        {
            int a = pointX - userX;
            int b = pointY - userY;
            int c = (a * a) + (b * b);
            return Convert.ToInt32(Math.Round(Math.Sqrt(c)));
        }

        public static string DisplayDistance(int distance)
        {
            string distanceName = "";
            //Can't use operators in switch statements
            //About to Yandere Simulator this
            if (distance > 800)
            {
                distanceName = "Frozen  ";
            }
            if (distance < 801 && distance > 400)
            {
                distanceName = "Ice Cold";
            }
            if (distance < 401 && distance > 200)
            {
                distanceName = "Cold    ";
            }
            if (distance < 201 && distance > 80)
            {
                distanceName = "Warm    ";
            }
            if (distance < 81 && distance > 40)
            {
                distanceName = "Hot     ";
            }
            if (distance < 41 && distance > 5)
            {
                distanceName = "On Fire ";
            }
            if (distance < 6)
            {
                distanceName = "Hit     ";
            }

            return distanceName;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Globals.gameLoop = false;
        }
    }
}
