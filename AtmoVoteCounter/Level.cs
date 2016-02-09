using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AtmoVoteCounter
{
    public class Level
    {
        public string Name { get; private set; }
        public string Designer { get; private set; }

        public string FullTitle
        {
            get
            {
                return Name + " by " + Designer;
            }
        }

        public int[] NthVotes { get; private set; }
        public double Points { get; private set; }

        public Settings Settings { get; private set; }

        public Level(string levelString, Settings settings)
        {
            if(!levelString.Contains(" by "))
                throw new ArgumentException("Invalid level from levels file: " + levelString);

            var _parts = Regex.Matches(levelString, @"^(.*[^\\]) by (.*)$");

            Name = _parts[0].Groups[1].Value;
            Designer = _parts[0].Groups[2].Value;

            Points = 0;
            Settings = settings;

            NthVotes = new int[Settings.VotesPerVoter];
        }

        public void Vote(int voteNumber)
        {
            NthVotes[voteNumber - 1]++;

            Points += Settings.VoteBias[voteNumber - 1];
        }
    }
}
