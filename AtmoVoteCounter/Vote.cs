using DuoVia.FuzzyStrings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AtmoVoteCounter
{
    public class Vote
    {
        public int VoteNumber { get; private set; }
        public string Level { get; private set; }
        public int LineNumber { get; private set; }

        public Vote(string voteString, int lineNumber)
        {
            var matches = Regex.Matches(voteString, @"^[\.\#\(\)]*(\d)[\.\#\(\)]*\s+(.*)$");
            VoteNumber = Int32.Parse(matches[0].Groups[1].Value);
            Level = matches[0].Groups[2].Value;
            LineNumber = lineNumber;
        }

        public LevelMatch FindClosestMatch(List<Level> levels)
        {
            var bestMatch = null as Level;
            var bestMatchValue = 0.0;

            foreach (var level in levels)
            {
                // check for exact matches
                if (level.Name.ToLower() == Level.ToLower())
                    return new LevelMatch(level, true);
                if (level.FullTitle.ToLower() == Level.ToLower())
                    return new LevelMatch(level, true);

                // if no exact match, continue with fuzzy search
                var matchValue = Math.Max(
                    Level.DiceCoefficient(level.FullTitle),
                    Level.DiceCoefficient(level.Name)
                );

                if(matchValue > bestMatchValue)
                {
                    bestMatchValue = matchValue;
                    bestMatch = level;
                }
            }

            return new LevelMatch(bestMatch, false);
        }
    }
}
