using Common;

namespace Year2023;

internal class Day01 : Day
{
    protected override int TestSolutionOne { get; set; } = 142;
    protected override int TestSolutionTwo { get; set; } = 281;

    protected override int SolveOne(string input)
    {
        int output = 0;
        using var reader = new StreamReader(input);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var numbers = line.Where(c => char.IsDigit(c)).ToList();
            output += int.Parse(numbers[0].ToString() + numbers[^1].ToString());
        }
        return output;
    }

    protected override int SolveTwo(string input)
    {
        var output = 0;
        var wordy = new Dictionary<string, string>()
        {
            { "one",    "1" },
            { "two",    "2" },
            { "three",  "3" },
            { "four",   "4" },
            { "five",   "5" },
            { "six",    "6" },
            { "seven",  "7" },
            { "eight",  "8" },
            { "nine",   "9" }
        };
        using var reader = new StreamReader(input);
        while (!reader.EndOfStream)
        {
            var numbers = new Dictionary<int, string>();
            var line = reader.ReadLine();
            foreach (var pair in wordy)
            {
                var pos = line.IndexOf(pair.Key);
                if (pos != -1)
                {
                    numbers.Add(pos, pair.Value);
                }
                pos = line.LastIndexOf(pair.Key);
                if (pos != -1)
                {
                    numbers.TryAdd(pos, pair.Value);
                }
            }
            for (int i = 0; i < line.Length; i++)
            {
                if (char.IsDigit(line[i]))
                {
                    numbers.Add(i, line[i].ToString());
                }
            }
            var minKey = numbers.Keys.Min();
            var fNumber = numbers[minKey];
            var maxKey = numbers.Keys.Max();
            var sNumber = numbers[maxKey];
            var sOutput = fNumber + sNumber;
            output += int.Parse(sOutput);
        }
        return output;
    }
}
