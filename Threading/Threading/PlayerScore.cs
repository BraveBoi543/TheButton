using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threading
{
    class PlayerScore
    {
        // Initializing values
        // Without these it reads the value as 'Threading.PlayerScore' instead of 'string' or 'int'
        private string name;
        private int time;
        private string difficulty;
        private string timerDifficulty;

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public int Time
        {
            get
            {
                return this.time;
            }
            set
            {
                this.time = value;
            }
        }

        public string Difficulty
        {
            get
            {
                return this.difficulty;
            }
            set
            {
                this.difficulty = value;
            }
        }

        public string TimerDifficulty
        {
            get
            {
                return this.timerDifficulty;
            }
            set
            {
                this.timerDifficulty = value;
            }
        }

        public override string ToString()
        {
            return String.Format("{0,-10}|{1,-10}| {2,-10}|{3,-10}", name, time, difficulty, timerDifficulty);
        }
    }
}
