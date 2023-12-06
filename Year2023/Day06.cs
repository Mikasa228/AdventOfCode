using Common;

namespace Year2023;

internal class Day06 : Day
{
    protected override int TestSolutionOne { get; set; } = 288;
    protected override int TestSolutionTwo { get; set; } = 71503;

    protected override long SolveOne(string input)
    {
        var output = 1;
        using var reader = new StreamReader(input);
        
        var taims = reader.ReadLine()?.Split(' ', options: StringSplitOptions.RemoveEmptyEntries)[1..].Select(value => int.Parse(value)).ToList();
        if (taims == null) throw new NullReferenceException(nameof(taims));

        var disses = reader.ReadLine()?.Split(' ', options: StringSplitOptions.RemoveEmptyEntries)[1..].Select(value => int.Parse(value)).ToList();
        if (disses == null) throw new NullReferenceException(nameof(disses));

        for (int i = 0; i < taims.Count; i++)
        {
            output *= CalculateDisses(taims[i]).Count(taim => taim > disses[i]);
        }

        return output;
    }

    private static List<long> CalculateDisses(long time)
    {
        var output = new List<long>();
        for (int i = 1; i < time; i++)
        {
            output.Add((time - i) * i);
        }
        return output;
    }

    protected override long SolveTwo(string input)
    {
        using var reader = new StreamReader(input);
        
        var strTaim = reader.ReadLine()?.Split(' ', options: StringSplitOptions.RemoveEmptyEntries)[1..];
        if (strTaim == null) throw new NullReferenceException(nameof(strTaim));
        var taim = long.Parse(string.Join("", strTaim));

        var strDiss = reader.ReadLine()?.Split(' ', options: StringSplitOptions.RemoveEmptyEntries)[1..];
        if (strDiss == null) throw new NullReferenceException(nameof(strDiss));
        var diss = long.Parse(string.Join("", strDiss));

        return CalculateDisses(taim).Count(val => val > diss);
    }
}
