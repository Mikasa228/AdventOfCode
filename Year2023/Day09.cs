using Common;

namespace Year2023;

internal class Day09 : Day
{
    protected override int TestSolutionOne { get; set; } = 114;
    protected override int TestSolutionTwo { get; set; } = 2;

    protected override long SolveOne(string input)
    {
        return Solve(input, reversed: false);
    }

    protected override long SolveTwo(string input)
    {
        return Solve(input, reversed: true);
    }

    private static long Solve(string input, bool reversed)
    {
        var output = 0;

        using var reader = new StreamReader(input);
        while (!reader.EndOfStream)
        {
            var storage = new List<List<int>>();

            var line = reader.ReadLine() ?? throw new NullReferenceException();

            if (reversed) storage.Add(line.Split(' ').Select(val => int.Parse(val)).Reverse().ToList());
            else storage.Add(line.Split(' ').Select(val => int.Parse(val)).ToList());

            while (true)
            {
                var newRow = new List<int>();

                for (int i = 0; i < storage[^1].Count - 1; i++)
                {
                    newRow.Add(storage[^1][i + 1] - storage[^1][i]);
                }
                
                storage.Add(newRow);

                if (newRow.All(val => val == 0)) break;
            }

            for (int i = storage.Count - 1; i > 0; i--)
            {
                storage[i - 1].Add(storage[i - 1][^1] + storage[i][^1]);
            }

            output += storage[0][^1];
        }

        return output;
    }
}
