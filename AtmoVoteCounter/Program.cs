using Liv.CommandlineArguments;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmoVoteCounter
{
    class Program
    {
        private static ArgsManager<Args> args;

        public enum Args
        {
            [Option(Description = "A file with one level per line, in the following format: <level name> by <designer>", DefaultValue = "levels.txt", ShortName = "l", LongName = "levels", Type = typeof(string))]
            OptionLevelsFile,
            [Option(Description = "A line like so: /<Username> designates a user, followed by their votes in the following format: <vote number> <level name and designer>. If a line starts with a letter or a / it will be ignored. The level will be matched to the levels list with a fuzzy search.", DefaultValue = "votes.txt", ShortName = "v", LongName = "votes", Type = typeof(string))]
            OptionVotesFile,
        }

        public static void PrintColumn(string data, int columnSize)
        {
            if(data.Length > columnSize)
            {
                Console.Write(data.Substring(0, columnSize));
            }
            else
            {
                Console.Write(data);
                for (int i = data.Length; i < columnSize; i++)
                    Console.Write(" ");
            }
        }

        public static void Main(string[] _args)
        {
            var settings = null as Settings;
            if(File.Exists("vote_settings.json"))
            {
                settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("vote_settings.json"));
            }
            else
            {
                settings = new Settings();
                File.WriteAllText("vote_settings.json", JsonConvert.SerializeObject(settings));
            }

            args = new ArgsManager<Args>(_args);

            var levelsFile = args.GetValue(Args.OptionLevelsFile);
            var votesFile = args.GetValue(Args.OptionVotesFile);

            if(!File.Exists(levelsFile))
            {
                Console.WriteLine("Couldn't find levels file: {0}", levelsFile);
                Console.ReadKey();
                return;
            }

            if (!File.Exists(votesFile))
            {
                Console.WriteLine("Couldn't find votes file: {0}", votesFile);
                Console.ReadKey();
                return;
            }

            try
            {
                // load the levels
                var _levels = File.ReadAllLines(levelsFile);
                var levels = new List<Level>();

                foreach (var _level in _levels)
                {
                    levels.Add(new Level(_level, settings));
                }

                // load the votes
                var _votes = File.ReadAllLines(votesFile);
                var voters = new List<Voter>();
                var voter = null as Voter;

                int line = 0;
                foreach (var _vote in _votes)
                {
                    line++;
                    if (Char.IsLetter(_vote[0]))
                        continue;

                    if (_vote[0] == '/')
                    {
                        voter = new Voter(_vote.Substring(1), settings);
                        voters.Add(voter);
                    }
                    else
                        voter.AddVote(new Vote(_vote, line));
                }

                // match the votes to the levels

                Console.WriteLine("Some votes may have been mispelled. Please verify the following fuzzy matches:");
                foreach (var _voter in voters)
                {
                    if (!_voter.IsComplete())
                        throw new Exception("User " + _voter.Username + " voted incorrectly.");

                    foreach(var vote in _voter.Votes)
                    {
                        var match = vote.FindClosestMatch(levels);

                        if (!match.IsExact)
                            Console.WriteLine("Fuzzy Matched: {0}\n" +
                                              "         with: {1}\n", vote.Level, match.Level.FullTitle);

                        match.Level.Vote(vote.VoteNumber);
                    }
                }

                // output the results

                // first calculate the column widths
                int place = 0;
                int[] cols = { 0, 0, 0 };
                foreach (var level in levels)
                {
                    int c1 = (place + ".").Length + 1;
                    int c2 = Math.Min(level.FullTitle.Length+1, 60);
                    int c3 = (level.Points.ToString("0.0") + " points ("+level.NthVotes[0]+" 1st place votes)").Length;

                    if (c1 > cols[0]) cols[0] = c1;
                    if (c2 > cols[1]) cols[1] = c2;
                    if (c3 > cols[2]) cols[2] = c3;

                    place++;
                }

                // now output
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("############  RESULTS  ############");
                levels.Sort(delegate (Level x, Level y) {
                    int cmp = y.Points.CompareTo(x.Points);
                    if (cmp == 0)
                        cmp = y.NthVotes[0].CompareTo(x.NthVotes[0]);

                    return cmp;
                });

                place = 0;
                foreach (var level in levels)
                {
                    place++;
                    PrintColumn(place + ".", cols[0]);
                    PrintColumn(level.FullTitle, cols[1]);
                    PrintColumn(level.Points.ToString("0.0") + " points (" + level.NthVotes[0] + " 1st place votes)", cols[2]);

                    Console.WriteLine();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
