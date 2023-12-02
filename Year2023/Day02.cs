using Common;

namespace Year2023
{
    internal class Day02 : Day
    {
        protected override int TestSolutionOne { get; set; } = 8;
        protected override int TestSolutionTwo { get; set; } = 2286;

        protected override int SolveOne(string input)
        {
            var output = 0;

            using var reader = new StreamReader(input);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line == null) throw new NullReferenceException(line);

                var split = line.Split(':', options: StringSplitOptions.TrimEntries);
                int gameNo = int.Parse(split[0].Split(' ')[1]);
                if (IsPossible(split[1]))
                {
                    output += gameNo;
                }
            }
            return output;
        }

        private static bool IsPossible(string game)
        {
            var caps = new Dictionary<string, int>()
            {
                { "red", 12 },
                { "green", 13 },
                { "blue", 14 }
            };

            var sets = game.Split(';', options: StringSplitOptions.TrimEntries);
            foreach (var set in sets)
            {
                var colors = set.Split(',', options: StringSplitOptions.TrimEntries);
                foreach (var color in colors)
                {
                    var split = color.Split(' ');
                    if (int.Parse(split[0]) > caps[split[1]])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        protected override int SolveTwo(string input)
        {
            var output = 0;
            using var reader = new StreamReader(input);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line == null) throw new NullReferenceException(line);

                var split = line.Split(':', options: StringSplitOptions.TrimEntries);
                int gameNo = int.Parse(split[0].Split(' ')[1]);
                var fewestNumbers = GetFewestNumber(split[1]);

                output += fewestNumbers["red"] * fewestNumbers["green"] * fewestNumbers["blue"];
            }
            return output;
        }

        private static Dictionary<string, int> GetFewestNumber(string game)
        {
            var output = new Dictionary<string, int>()
            {
                { "red", 0 },
                { "green", 0 },
                { "blue", 0 }
            };

            var sets = game.Split(';', options: StringSplitOptions.TrimEntries);
            foreach (var set in sets)
            {
                var colors = set.Split(',', options: StringSplitOptions.TrimEntries);
                foreach (var color in colors)
                {
                    var split = color.Split(' ');
                    var currentColor = split[1];
                    var currentNumber = int.Parse(split[0]);

                    if (currentNumber > output[currentColor])
                    {
                        output[currentColor] = currentNumber;
                    }
                }
            }

            return output;
        }
    }
}
