using Common;
using System.Text.RegularExpressions;

namespace Year2023;

internal class Day08 : Day
{
    protected override int TestSolutionOne { get; set; } = 6;
    protected override int TestSolutionTwo { get; set; } = 6;

    protected override long SolveOne(string input)
    {
        var output = 0;
        var pointer = 0;

        var map = new Dictionary<string, List<string>>();

        using var reader = new StreamReader(input);

        var instruction = reader.ReadLine() ?? throw new NullReferenceException();
        reader.ReadLine();

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine() ?? throw new NullReferenceException();
            var theMatch = Regex.Match(line, @"(\w{3}) = \((\w{3})\, (\w{3})\)");
            map.TryAdd(theMatch.Groups[1].Value, new() { theMatch.Groups[2].Value, theMatch.Groups[3].Value });
        }

        var currentLocation = map["AAA"];

        while (true)
        {
            output++;

            string target;
            if (instruction[pointer] == 'L') target = currentLocation[0];
            else target = currentLocation[1];

            if (target == "ZZZ") break;

            currentLocation = map[target];

            pointer++;
            if (pointer >= instruction.Length) pointer = 0;
        }
        return output;
    }

    protected override long SolveTwo(string input)
    {
        var steps = 0;
        var pointer = 0;

        var map = new Dictionary<string, List<string>>();
        var paths = new Dictionary<string, string>();
        var shortestPaths = new Dictionary<string, long>();

        using var reader = new StreamReader(input);

        var instruction = reader.ReadLine() ?? throw new NullReferenceException();
        reader.ReadLine();

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine() ?? throw new NullReferenceException();
            var theMatch = Regex.Match(line, @"(\w{3}) = \((\w{3})\, (\w{3})\)");
            map.TryAdd(theMatch.Groups[1].Value, new() { theMatch.Groups[2].Value, theMatch.Groups[3].Value });
        }

        foreach (var location in map.Keys)
        {
            if (location.EndsWith("A")) paths.TryAdd(location, location);
        }

        while (true)
        {
            foreach (var path in paths)
            {
                if (instruction[pointer] == 'L') paths[path.Key] = map[path.Value][0];
                else paths[path.Key] = map[path.Value][1];

                if (path.Value.EndsWith("Z")) shortestPaths.TryAdd(path.Key, steps);
            }

            if (shortestPaths.Count == paths.Count) break;

            steps++;
            pointer++;
            if (pointer >= instruction.Length) pointer = 0;
        }

        return Utils.LCM(shortestPaths.Values.ToList());
    }
}
