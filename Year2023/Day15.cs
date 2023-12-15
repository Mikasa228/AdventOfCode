using Common;
using System.Text;
using System.Text.RegularExpressions;

namespace Year2023;

internal class Day15 : Day
{
    protected override int TestSolutionOne { get; set; } = 1320;
    protected override int TestSolutionTwo { get; set; } = 145;

    protected override long SolveOne(string input)
    {
        var output = 0;

        using var reader = new StreamReader(input);
        var line = reader.ReadLine() ?? throw new NullReferenceException();
        var steps = line.Split(",");

        foreach (var step in steps)
        {
            output += HASH(step);
        }

        return output;
    }

    private static int HASH(string step)
    {
        var output = 0;

        var bytes = Encoding.ASCII.GetBytes(step);
        foreach (var code in bytes)
        {
            output += code;
            output *= 17;
            output %= 256;
        }

        return output;
    }

    protected override long SolveTwo(string input)
    {
        var output = 0;
        var boxes = new Dictionary<int, List<KeyValuePair<string, int>>>();

        using var reader = new StreamReader(input);
        var line = reader.ReadLine() ?? throw new NullReferenceException();
        var steps = line.Split(",");

        foreach (var step in steps)
        {
            var match = Regex.Match(step, @"(\w+)([-=])(\d*)");

            var label = match.Groups[1].Value;
            var operation = match.Groups[2].Value;
            var fLength = match.Groups[3].Value;

            var box = HASH(label);

            boxes.TryAdd(box, new());

            var target = boxes[box].Where(lens => lens.Key == label).FirstOrDefault();
            if (operation == "-")
            {
                boxes[box].Remove(target);
            }
            else
            {
                var index = boxes[box].IndexOf(target);
                var lens = new KeyValuePair<string, int>(label, int.Parse(fLength));
                if (index == -1)
                {
                    boxes[box].Add(lens);
                }
                else
                {
                    boxes[box][index] = lens;
                }
            }
        }

        foreach (var box in boxes)
        {
            for (int i = 0; i < box.Value.Count; i++)
            {
                output += (box.Key + 1) * (i + 1) * box.Value[i].Value;
            }
        }

        return output;
    }
}
