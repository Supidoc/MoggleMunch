using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoggleMunch
{
    public class ScoreBoardData :  IComparable<ScoreBoardData>
    {
        public string PlayerName { get; set; } = string.Empty;
        public int Score { get; set;}
        public int TimeStamp { get; set; }

        public override string ToString()
        {
            return $"Player: {PlayerName}  Score: {Score}";
        }

        // Default comparer
        public int CompareTo(ScoreBoardData? compareScore)
        {
            // A null value means that this object is greater.
            if (compareScore == null)
                return 1;

            else
                return compareScore.Score.CompareTo(this.Score); // order correct?
        }

        
    }
}
