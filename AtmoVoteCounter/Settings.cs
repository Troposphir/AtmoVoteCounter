using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmoVoteCounter
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Settings
    {
        [JsonProperty]
        public int VotesPerVoter { get; set; }

        [JsonProperty]
        public double[] VoteBias { get; set; }
        
        public Settings()
        {
            VotesPerVoter = 3;
            VoteBias = new double[] { 3, 2, 1 };
        }
    }
}
