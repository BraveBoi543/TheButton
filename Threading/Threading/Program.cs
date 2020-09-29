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
            string displayName = "";
            string tempDisplayName = "";


            // Point generation
            Random rand = new Random();

            int pointX = rand.Next(1920);
            int pointY = rand.Next(1080);

            // Game loop
            while (true)
            {
                // Gather current cursor x and y values
                int userX = Cursor.Position.X;
                int userY = Cursor.Position.Y;

                // Get distance to point
                int distance = SlopeDistance(pointX, pointY, userX, userY);
                // Get distance converted to name
                displayName = DisplayDistance(distance);

                if (!(displayName == tempDisplayName))
                {
                    Console.WriteLine(displayName);
                }
                else
                {
                    Console.Clear();
                }



                tempDisplayName = DisplayDistance(distance);
                
                
            }
            
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
                distanceName = "Frozen";
            }
            if (distance < 801 && distance > 400)
            {
                distanceName = "Ice Cold";
            }
            if (distance < 401 && distance > 200)
            {
                distanceName = "Cold";
            }
            if (distance < 201 && distance > 80)
            {
                distanceName = "Warm";
            }
            if (distance < 81 && distance > 40)
            {
                distanceName = "Hot";
            }
            if (distance < 41 && distance > 5)
            {
                distanceName = "On Fire";
            }
            if (distance < 6)
            {
                distanceName = "Hit";
            }

            return distanceName;
        }
    }
}
