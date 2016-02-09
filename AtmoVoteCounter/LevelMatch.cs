using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmoVoteCounter
{
    public class LevelMatch
    {
        public Level Level { get; private set; }
        public bool IsExact { get; private set; }
        public LevelMatch(Level level, bool isExact)
        {
            Level = level;
            IsExact = isExact;
        }
    }
}
