using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmoVoteCounter
{
    public class Voter
    {
        public string Username { get; private set; }
        public Vote[] Votes { get; private set; }
        public Settings Settings { get; private set; }

        public Voter(string username, Settings settings)
        {
            Username = username;
            Votes = new Vote[settings.VotesPerVoter];
            Settings = settings;
        }

        public void AddVote(Vote v)
        {
            if (v.VoteNumber > Votes.Length)
                throw new Exception(Username + " tried to have a #"+v.VoteNumber + " vote.");

            Votes[v.VoteNumber - 1] = v;
        }

        public bool IsComplete()
        {
            for(var i = 0; i < Votes.Length; i++)
            {
                if (Votes[i] == null)
                    return false;
            }

            return true;
        }
    }
}
