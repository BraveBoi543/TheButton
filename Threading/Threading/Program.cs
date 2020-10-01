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
            public static int difficultyModifier;

            public static string folderPath = @"C:/Users/Public/Documents/";
            public static string txtPath = @"C:/Users/Public/Documents/myData.txt";
            public static string scorePath = @"C:/Users/Public/Documents/score.txt";
            public static string userName;
            public static string difficulty;

            public static List<PlayerScore> playerScoresList = new List<PlayerScore>();
        }


        static void Main(string[] args)
        {
            // Point generation
            Random rand = new Random();

            int pointX = rand.Next(1920);
            int pointY = rand.Next(1080);

            /*  Keeping for saftey for now
             * 
            // Test to see if file exists
            if (File.Exists(Globals.txtPath))
            {
                using (StreamReader sr = File.OpenText(Globals.txtPath))
                {
                    // Iterates through each line and write the last one
                    for (int i = 1; i < File.ReadLines(Globals.txtPath).Count(); i++)
                    {
                        sr.ReadLine();
                    }
                    Console.WriteLine(sr.ReadLine());
                }
            }
            */
            WriteObjectsToMemory();
            WriteLeaderboard();

            // User enters name
            Console.Write("Enter name: ");
            Globals.userName = Console.ReadLine();


            // Difficulty Selection
            string userDifficulty = "";
            while (!(userDifficulty == "easy") && !(userDifficulty == "medium") && !(userDifficulty == "hard") && !(userDifficulty == "extreme"))
            {
                Console.Write("Select a difficulty:\n- Easy\n- Medium\n- Hard\n- Extreme\n");
                userDifficulty = Console.ReadLine().ToLower();
            }

            switch (userDifficulty)
            {
                case ("easy"):
                    Globals.difficulty = "Easy";
                    Globals.difficultyModifier = 25;
                    break;

                case ("medium"):
                    Globals.difficulty = "Medium";
                    Globals.difficultyModifier = 12;
                    break;

                case ("hard"):
                    Globals.difficulty = "Hard";
                    Globals.difficultyModifier = 5;
                    break;

                case ("extreme"):
                    Globals.difficulty = "Extreme";
                    Globals.difficultyModifier = 1;
                    break;

                default:
                    Console.WriteLine("Something went wrong selecting difficulty. Auto selecting Medium. Press ENTER to continue. . .");
                    Globals.difficulty = "Medium";
                    Globals.difficultyModifier = 12;
                    break;
            }

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
            Console.WriteLine("Press ENTER to continue. . .");
            Console.ReadKey();
            // Record score on text file
            WriteToTxt();

            // Play again
            bool confirmed = false;
            bool playAgain = false;
            while (!confirmed)
            {
                Console.Write("Would you like to play again(Y/N)? ");
                string userInput = Console.ReadLine().ToLower();

                switch (userInput)
                {
                    case ("y"):
                    case ("yes"):
                        playAgain = true;
                        confirmed = true;
                        break;
                    case ("n"):
                    case ("no"):
                        confirmed = true;
                        break;

                    default:
                        confirmed = false;
                        break;
                }
            }

            // Calls main if user wants to play again
            if (playAgain)
            {
                Globals.gameLoop = true;
                Globals.hasHit = false;
                Globals.currentTime = 0;
                Main(null);
            }
            else
            {
                // Open 
                if (Globals.hasHit)
                {
                    System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=mnpjpdhUNjY");
                }
                else
                {
                    System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=dQw4w9WgXcQ ");
                    Console.WriteLine("You couldn't hit the borad side of a barn with a bowling ball. Maybe try again.");
                    Console.WriteLine("Press ENTER to exit. . .");
                    Console.ReadKey();
                }
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
            if (distance < 41 && distance > Globals.difficultyModifier)
            {
                distanceName = "On Fire ";
            }
            if (distance <= Globals.difficultyModifier)
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
                if (!File.Exists(Globals.txtPath))      //myData.txt file
                {
                    using (StreamWriter sw = File.CreateText(Globals.txtPath)) ;
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(Globals.txtPath, true))
                    {
                        sw.WriteLine($"{Globals.userName} took {ConvertToSeconds(Globals.currentTime)} seconds on {Globals.difficulty} mode.");
                        sw.Close();
                    }
                }

                //CREATE A SEPARATE FILE IN ORDER TO ORDER SCORES CORRECTLY
                //https://stackoverflow.com/questions/29897306/how-can-i-read-a-specific-part-of-a-text-file

                if (!File.Exists(Globals.scorePath))        //score.txt file
                {
                    using (StreamWriter sw = File.CreateText(Globals.scorePath)) ;
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(Globals.scorePath, true))
                    {
                        sw.WriteLine($"{Globals.userName},{ConvertToSeconds(Globals.currentTime)},{Globals.difficulty}");
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

            // Instantiate new object and add to list
            Globals.playerScoresList.Add(new PlayerScore { Name = Globals.userName, Time = ConvertToSeconds(Globals.currentTime), Difficulty = Globals.difficulty });
        }

        // Display leaderboard in order
        public static void WriteLeaderboard()
        {
            var orderedScores = Globals.playerScoresList.OrderByDescending(x => x.Time).ToList();

            for (int i = 0; i < orderedScores.Count; i++)
            {
                Console.WriteLine(orderedScores[i]);
            }
        }

        // Creates objects using myData.txt
        public static void WriteObjectsToMemory()
        {
            // Initiallizing values
            string line;
            var names = new List<string>();
            var scores = new List<int>();
            var difficulties = new List<string>();

            if (!File.Exists(Globals.scorePath))
            {
                using (StreamWriter sw = File.CreateText(Globals.scorePath)) ;
            }
            else
            {
                StreamReader sw = new StreamReader(Globals.scorePath);
                // Iterate through each line
                while ((line = sw.ReadLine()) != null)
                {
                    // Grabs all names and put them into list 'names'
                    names.Add(line.Split(',')[0]);
                    scores.Add(Convert.ToString(line.Split(','))[1]);
                    difficulties.Add(line.Split(',')[2]);
                }

                sw.Close();
            }

            // Iterate through 'names' list and pass info from all lists into 'PlayerScore' objects
            for (int i = 0; i < names.Count; i++)
            {
                Globals.playerScoresList.Add(new PlayerScore { Name = names[i], Time = ConvertToSeconds(scores[i]), Difficulty = difficulties[i] });
            }

            for (int i = 0; i < scores.Count; i++)
            {
                Console.WriteLine(scores[i]);
                Console.WriteLine(scores.Count);
            }
        }
    }
}
