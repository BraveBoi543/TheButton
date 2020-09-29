using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.IO;

namespace Threading
{
    class Program
    {
        // Global Variables
        public static class Globals
        {
            public static bool gameLoop = true;
            public static bool hasHit = false;

            public static int maxTime = 10000;
            public static int currentTime = 0;

            public static string folderPath = @"C:/Users/Public/Documents/";
            public static string txtPath = @"C:/Users/Public/Documents/myData.txt";
            public static string userName;
        }


        static void Main(string[] args)
        {
            // Point generation
            Random rand = new Random();

            int pointX = rand.Next(1920);
            int pointY = rand.Next(1080);

            Console.Write("Enter name: ");
            Globals.userName = Console.ReadLine();

            // Game loop
            while (Globals.gameLoop)
            {
                // Tick timer
                Timer();

                // Gather current cursor x and y values
                int userX = Cursor.Position.X;
                int userY = Cursor.Position.Y;

                // Get distance to point
                int distance = SlopeDistance(pointX, pointY, userX, userY);
                // Get distance converted to name
                string displayName = DisplayDistance(distance);
                // Print name on single line
                Console.Write($"\r{displayName}, {Globals.currentTime}");
                
                // Detect hit and exit loop
                if (displayName == "Hit     ")
                {
                    Globals.gameLoop = false;
                    Globals.hasHit = true;
                }

            }

            // Output 
            Console.WriteLine("\nGame over");
            Console.WriteLine($"You took {ConvertToSeconds(Globals.currentTime)} seconds");
            Console.WriteLine("Press ENTER to exit. . .");
            Console.ReadKey();
            // Record score on text file
            WriteToTxt();

            // Open 
            if (Globals.hasHit)
            {
                System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=mnpjpdhUNjY");
            }
            else
            {
                System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=dQw4w9WgXcQ ");
                Console.WriteLine("You couldn't hit the borad side of a barn with a bowling ball. Maybe try again.");
                Console.ReadKey();
            }
            
        }

        // Calculate distance
        public static int SlopeDistance(int pointX, int pointY, int userX, int userY)
        {
            int a = pointX - userX;
            int b = pointY - userY;
            int c = (a * a) + (b * b);
            return Convert.ToInt32(Math.Round(Math.Sqrt(c)));
        }

        // Calculate appropriate string
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

        // Timer function
        public static void Timer()
        {
            if (Globals.currentTime >= Globals.maxTime)
            {
                Globals.gameLoop = false;
            }
            Globals.currentTime++;
            Thread.Sleep(10);
        }

        // Calculate readable time
        public static int ConvertToSeconds(int totalTime)
        {
            int totalSeconds = totalTime / 100;
            return totalSeconds;
        }

        // Write score to text file
        public static void WriteToTxt()
        {
            // Creates folder directory
            System.IO.Directory.CreateDirectory(Globals.folderPath);

            try
            {
                // Tests to see if file exits
                // If not, creates file
                if (!File.Exists(Globals.txtPath))
                {
                    using (StreamWriter sw = File.CreateText(Globals.txtPath)) ;
                }
                else
                {
                    using (StreamReader sr = File.OpenText(Globals.txtPath))
                    {
                        //OUTPUT TEXT IN FILE
                        sr.Close();

                        // 'true' modifier at end appends the text file
                        StreamWriter sw = new StreamWriter(Globals.txtPath, true);
                        sw.WriteLine($"{Globals.userName} took {ConvertToSeconds(Globals.currentTime)} second(s)");
                        sw.Close();
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"The file was not found: {e}");
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine($"The directory was not found: {e}");
            }
            catch (IOException e)
            {
                Console.WriteLine($"The file could not be opened: {e}");
            }
        }
    }
}
