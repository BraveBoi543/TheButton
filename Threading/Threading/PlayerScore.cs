using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threading
{
    class PlayerScore
    {
        private string name;
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

        private int time;
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

        private string difficulty;
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

        public override string ToString()
        {
            return String.Format($"{name} {time} {difficulty}");
        }
    }
}
