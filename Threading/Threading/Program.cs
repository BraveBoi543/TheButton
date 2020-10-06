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
            public static bool hasWritten = true;

            public static int maxTime = 10000;
            public static int currentTime = 0;
            public static int difficultyModifier;

            public static string folderPath = @"C:/Users/Public/Documents/";
            public static string txtPath = @"C:/Users/Public/Documents/myData.txt";
            public static string scorePath = @"C:/Users/Public/Documents/score.txt";
            public static string userName;
            public static string difficulty;
            public static string timerDifficulty;

            public static List<PlayerScore> playerScoresList = new List<PlayerScore>();
        }

        static void Main(string[] args)
        {
            // Point generation
            Random rand = new Random();

            int pointX = rand.Next(1920);
            int pointY = rand.Next(1080);

            // Only execute function 'WriteObjectsToMemory' once
            // or will have duplicates on reply in same session
            if (Globals.hasWritten)
            {
                WriteObjectsToMemory();
                Globals.hasWritten = false;
            }

            // Sort and display full leaderboard
            SortFullLeaderboard();

            // User enters name
            Console.Write("Enter name: ");
            Globals.userName = Console.ReadLine();


            // Size Difficulty Selection
            string userDifficulty = "";
            while (!(userDifficulty == "easy") && !(userDifficulty == "medium") && !(userDifficulty == "hard") && !(userDifficulty == "extreme"))
            {
                Console.Write("Select a size difficulty:\n" +
                    "- Easy\n" +
                    "- Medium\n" +
                    "- Hard\n" +
                    "- Extreme\n");
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

            // Time Difficulty Selection
            while(!(userDifficulty == "sloth") && !(userDifficulty == "giant turtle") && !(userDifficulty == "green iguana") && !(userDifficulty == "cheetah") && !(userDifficulty == "peregrine falcon"))
            {
                Console.Write("Select a time difficulty:\n" +
                    "- Sloth\n" +
                    "- Giant Turtle\n" +
                    "- Green Iguana\n" +
                    "- Cheetah\n" +
                    "- Peregrine falcon\n");
                userDifficulty = Console.ReadLine().ToLower();
            }

            switch (userDifficulty)
            {
                case ("sloth"):
                    Globals.timerDifficulty = "Sloth";
                    Globals.maxTime = 20000;
                    break;

                case ("giant turtle"):
                    Globals.timerDifficulty = "Giant Turtle";
                    Globals.maxTime = 10000;
                    break;

                case ("green iguana"):
                    Globals.timerDifficulty = "Green Iguana";
                    Globals.maxTime = 4000;
                    break;

                case ("cheetah"):
                    Globals.timerDifficulty = "Cheetah";
                    Globals.maxTime = 2000;
                    break;

                case ("peregrine falcon"):
                    Globals.timerDifficulty = "Peregrine Falcon";
                    Globals.maxTime = 1000;
                    break;

                default:
                    Console.WriteLine("Someting went wrong with selecting difficulty. Auto selecting Giant Turtle. Press ENTER to continue. . .");
                    Console.ReadKey();
                    Globals.timerDifficulty = "Giant Turtle";
                    Globals.maxTime = 10000;
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
                        sw.WriteLine($"{Globals.userName} took {ConvertToSeconds(Globals.currentTime)} seconds with {Globals.difficulty} size on {Globals.timerDifficulty} mode.");
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
                        sw.WriteLine($"{Globals.userName},{ConvertToSeconds(Globals.currentTime)},{Globals.difficulty},{Globals.timerDifficulty}");
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
            Globals.playerScoresList.Add(new PlayerScore { Name = Globals.userName, Time = ConvertToSeconds(Globals.currentTime), Difficulty = Globals.difficulty, TimerDifficulty = Globals.timerDifficulty });
        }

        // Creates objects using myData.txt
        public static void WriteObjectsToMemory()
        {
            // Initiallizing values
            string line;
            var names = new List<string>();
            var scores = new List<string>();
            var difficulties = new List<string>();
            var timerDifficulties = new List<string>();

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
                    scores.Add(line.Split(',')[1]);
                    difficulties.Add(line.Split(',')[2]);
                    timerDifficulties.Add(line.Split(',')[3]);
                }

                sw.Close();
            }

            // Iterate through 'names' list and pass info from all lists into 'PlayerScore' objects
            for (int i = 0; i < names.Count; i++)
            {
                Globals.playerScoresList.Add(new PlayerScore { Name = names[i], Time = Convert.ToInt32(scores[i]), Difficulty = difficulties[i], TimerDifficulty = timerDifficulties[i] });
            }
        }

        // Sort Leaderboard fully
        public static void SortFullLeaderboard()
        {
            // Easy Lists
            var easyMaster = new List<PlayerScore>();
            var easySloth = new List<PlayerScore>();
            var easyGiantTurtle = new List<PlayerScore>();
            var easyGreenIguana = new List<PlayerScore>();
            var easyCheetah = new List<PlayerScore>();
            var easyPeregrineFalcon = new List<PlayerScore>();

            // Medium Lists
            var mediumMaster = new List<PlayerScore>();
            var mediumSloth = new List<PlayerScore>();
            var mediumGiantTurtle = new List<PlayerScore>();
            var mediumGreenIguana = new List<PlayerScore>();
            var mediumCheetah = new List<PlayerScore>();
            var mediumPeregrineFalcon = new List<PlayerScore>();

            // Hard Lists
            var hardMaster = new List<PlayerScore>();
            var hardSloth = new List<PlayerScore>();
            var hardGiantTurtle = new List<PlayerScore>();
            var hardGreenIguana = new List<PlayerScore>();
            var hardCheetah = new List<PlayerScore>();
            var hardPeregrineFalcon = new List<PlayerScore>();

            // Extreme Lists
            var extremeMaster = new List<PlayerScore>();
            var extremeSloth = new List<PlayerScore>();
            var extremeGiantTurtle = new List<PlayerScore>();
            var extremeGreenIguana = new List<PlayerScore>();
            var extremeCheetah = new List<PlayerScore>();
            var extremePeregrineFalcon = new List<PlayerScore>();

            // Group by size difficulty
            foreach (var item in Globals.playerScoresList)
            {
                switch (item.Difficulty)
                {
                    case ("Easy"):
                        easyMaster.Add(item);
                        break;

                    case ("Medium"):
                        mediumMaster.Add(item);
                        break;

                    case ("Hard"):
                        hardMaster.Add(item);
                        break;

                    case ("Extreme"):
                        extremeMaster.Add(item);
                        break;
                }
            }

            // Group easy by time difficulty
            foreach (var item in easyMaster)
            {
                switch (item.TimerDifficulty)
                {
                    case ("Sloth"):
                        easySloth.Add(item);
                        break;

                    case ("Giant Turtle"):
                        easyGiantTurtle.Add(item);
                        break;

                    case ("Green Iguana"):
                        easyGreenIguana.Add(item);
                        break;

                    case ("Cheetah"):
                        easyCheetah.Add(item);
                        break;

                    case ("Peregrine Falcon"):
                        easyPeregrineFalcon.Add(item);
                        break;
                }
            }

            // Group medium by time difficulty
            foreach (var item in mediumMaster)
            {
                switch (item.TimerDifficulty)
                {
                    case ("Sloth"):
                        mediumSloth.Add(item);
                        break;

                    case ("Giant Turtle"):
                        mediumGiantTurtle.Add(item);
                        break;

                    case ("Green Iguana"):
                        mediumGreenIguana.Add(item);
                        break;

                    case ("Cheetah"):
                        mediumCheetah.Add(item);
                        break;

                    case ("Peregrine Falcon"):
                        mediumPeregrineFalcon.Add(item);
                        break;
                }
            }

            // Group hard by time difficulty
            foreach (var item in hardMaster)
            {
                switch (item.TimerDifficulty)
                {
                    case ("Sloth"):
                        hardSloth.Add(item);
                        break;

                    case ("Giant Turtle"):
                        hardGiantTurtle.Add(item);
                        break;

                    case ("Green Iguana"):
                        hardGreenIguana.Add(item);
                        break;

                    case ("Cheetah"):
                        hardCheetah.Add(item);
                        break;

                    case ("Peregrine Falcon"):
                        hardPeregrineFalcon.Add(item);
                        break;
                }
            }

            // Group extreme by time difficulty
            foreach (var item in extremeMaster)
            {
                switch (item.TimerDifficulty)
                {
                    case ("Sloth"):
                        extremeSloth.Add(item);
                        break;

                    case ("Giant Turtle"):
                        extremeGiantTurtle.Add(item);
                        break;

                    case ("Green Iguana"):
                        extremeGreenIguana.Add(item);
                        break;

                    case ("Cheetah"):
                        extremeCheetah.Add(item);
                        break;

                    case ("Peregrine Falcon"):
                        extremePeregrineFalcon.Add(item);
                        break;
                }
            }

            // Group easy sloth by score
            var orderedEasySloth = easySloth.OrderBy(x => x.Time).ToList();

            // Group easy giant turtle by score
            var orderedEasyGiantTurtle = easyGiantTurtle.OrderBy(x => x.Time).ToList();

            // Group easy green iguana by score
            var orderedEasyGreenIguana = easyGreenIguana.OrderBy(x => x.Time).ToList();

            // Group easy cheetah by score
            var orderedEasyCheetah = easyCheetah.OrderBy(x => x.Time).ToList();

            // Group easy peregrine falcon by score
            var orderedEasyPeregrineFalcon = easyPeregrineFalcon.OrderBy(x => x.Time).ToList();

            // Group medium sloth by score
            var orderedMediumSloth = mediumSloth.OrderBy(x => x.Time).ToList();

            // Group medium giant turtle by score
            var orderedMediumGiantTurtle = mediumGiantTurtle.OrderBy(x => x.Time).ToList();

            // Group medium green iguana by score
            var orderedMediumGreenIguana = mediumGreenIguana.OrderBy(x => x.Time).ToList();

            // Group medium cheetah by score
            var orderedMediumCheetah = mediumCheetah.OrderBy(x => x.Time).ToList();

            // Group medium peregrine falcon by sore
            var orderedMediumPeregrineFalcon = mediumPeregrineFalcon.OrderBy(x => x.Time).ToList();

            // Group hard sloth by score
            var orderedHardSloth = hardSloth.OrderBy(x => x.Time).ToList();

            // Group hard giant turtle by score 
            var orderedHardGiantTurtle = hardGiantTurtle.OrderBy(x => x.Time).ToList();

            // Group hard green iguana by score
            var orderedHardGreenIguana = hardGreenIguana.OrderBy(x => x.Time).ToList();

            // Group hard cheetah by score
            var orderedHardCheetah = hardCheetah.OrderBy(x => x.Time).ToList();

            // Group hard peregrine falcon by score
            var orderedHardPeregrineFalcone = hardPeregrineFalcon.OrderBy(x => x.Time).ToList();

            // Group extreme sloth by score
            var orderedExtremeSloth = extremeSloth.OrderBy(x => x.Time).ToList();

            // Group extreme giant turtle by score
            var orderedExtremeGiantTurtle = extremeGiantTurtle.OrderBy(x => x.Time).ToList();

            // Group extreme green iguana by score
            var orderedExtremeGreenIguana = extremeGreenIguana.OrderBy(x => x.Time).ToList();

            // Group extreme cheetah by score
            var orderedExtremeCheetah = extremeCheetah.OrderBy(x => x.Time).ToList();

            // Group extreme peregrine falcon by score
            var orderedExtremePeregrineFalcon = extremePeregrineFalcon.OrderBy(x => x.Time).ToList();

            // Put all lists into one list to iterate through
            var allOrderedLists = orderedExtremePeregrineFalcon.Concat(orderedExtremeCheetah).Concat(orderedExtremeGreenIguana).Concat(orderedExtremeGiantTurtle).Concat(orderedExtremeSloth).Concat(orderedHardPeregrineFalcone).Concat(orderedHardCheetah).Concat(orderedHardGreenIguana).Concat(orderedHardGiantTurtle).Concat(orderedHardSloth).Concat(orderedMediumPeregrineFalcon).Concat(orderedMediumCheetah).Concat(orderedMediumGreenIguana).Concat(orderedMediumGiantTurtle).Concat(orderedMediumSloth).Concat(orderedEasyPeregrineFalcon).Concat(orderedEasyCheetah).Concat(orderedEasyGreenIguana).Concat(orderedEasyGiantTurtle).Concat(orderedEasySloth).ToList();

            // Print Leaderboard Header
            Console.WriteLine("Name      | Time     | Size      | Timer");

            // Iterate through full list to display
            for (int i = 0; i < allOrderedLists.Count; i++)
            {
                Console.WriteLine(allOrderedLists[i]);
            }
        }
    }
}
