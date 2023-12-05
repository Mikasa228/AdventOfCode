using Common;

namespace Year2023;

internal class Day04 : Day
{
    protected override int TestSolutionOne { get; set; } = 13;
    protected override int TestSolutionTwo { get; set; } = 30;

    protected override long SolveOne(string input)
    {
        var output = 0;
        using var reader = new StreamReader(input);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine() ?? throw new NullReferenceException();
            var notJunk = line.Split(':', options: StringSplitOptions.TrimEntries)[1];

            var oneTwo = notJunk.Split('|', options: StringSplitOptions.TrimEntries);
            var winning = oneTwo[0].Split(' ', options: StringSplitOptions.RemoveEmptyEntries);
            var actual = oneTwo[1].Split(' ', options: StringSplitOptions.RemoveEmptyEntries);

            var winCount = actual.Count(num => winning.Contains(num));
            output += winCount >= 0 ? Convert.ToInt32(Math.Pow(2, winCount - 1)) : 0;
        }
        return output;
    }

    protected override long SolveTwo(string input)
    {
        var pointer = -1;
        var results = new Dictionary<int, int>();
        using var reader = new StreamReader(input);
        while (!reader.EndOfStream)
        {
            pointer++;
            results.TryAdd(pointer, 1);

            var line = reader.ReadLine() ?? throw new NullReferenceException();
            var notJunk = line.Split(':', options: StringSplitOptions.TrimEntries)[1];

            var oneTwo = notJunk.Split('|', options: StringSplitOptions.TrimEntries);
            var winning = oneTwo[0].Split(' ', options: StringSplitOptions.RemoveEmptyEntries);
            var actual = oneTwo[1].Split(' ', options: StringSplitOptions.RemoveEmptyEntries);

            var winCount = actual.Count(num => winning.Contains(num));

            for (int i = 1; i <= winCount; i++)
            {
                results.Increment(pointer + i, results[pointer]);
            }
        }

        return results.Sum(pair => pair.Value);
    }
}

internal static class DiccExtensions
{
    internal static void Increment(this Dictionary<int, int> dicc, int position, int toAdd)
    {
        dicc.TryAdd(position, 1);
        dicc[position] += toAdd;
    }
}
