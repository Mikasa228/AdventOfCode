using Common;

namespace Year2023;

internal class Day13 : Day
{
    protected override int TestSolutionOne { get ; set; } = 405;
    protected override int TestSolutionTwo { get ; set ; } = 400;

    protected override long SolveOne(string input)
    {
        var output = 0;
        var fields = new List<List<string>>() { new List<string>()};

        using var reader = new StreamReader(input);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine() ?? throw new NullReferenceException();
            if (string.IsNullOrEmpty(line))
            {
                fields.Add(new());
                continue;
            }
            fields[^1].Add(line);
        }

        foreach (var field in fields)
        {
            output += CalculateScore(field);
        }

        return output;
    }

    private int CalculateScore(List<string> field)
    {
        for (int i = 0; i < field.Count-1; i++)
        {
            if (field[i] == field[i + 1] && SplitAndCompare(field, i))
            {
                return 100 * (i+1);
            }
        }

        var sideways = new List<List<char>>();

        foreach (var _ in field[0])
        {
            sideways.Add(new List<char>());
        }

        for (int i = 0; i < field.Count; i++)
        {
            for (int j = 0; j < field[0].Length; j++)
            {
                sideways[j].Add(field[i][j]);
            }
        }

        var legitSideways = sideways.Select(row => string.Join("", row)).ToList();

        for (int i = 0; i < legitSideways.Count - 1; i++)
        {
            if (legitSideways[i] == legitSideways[i + 1] && SplitAndCompare(legitSideways, i))
            {
                return i+1;
            }
        }

        return 0;
    }

    private bool SplitAndCompare(List<string> field, int i)
    {
        var length = Math.Min(i+1, field.Count - i-1);
        var list1 = field.GetRange(i+1, length);
        var list2 = new List<string>();
        for (int j = i; j >= i-length+1; j--)
        {
            list2.Add(field[j]);
        }

        return list1.SequenceEqual(list2);

    }

    protected override long SolveTwo(string input)
    {
        var output = 0;
        var fields = new List<List<string>>() { new List<string>() };

        using var reader = new StreamReader(input);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine() ?? throw new NullReferenceException();
            if (string.IsNullOrEmpty(line))
            {
                fields.Add(new());
                continue;
            }
            fields[^1].Add(line);
        }

        foreach (var field in fields)
        {
            output += CalculateScoreTwo(field);
        }

        return output;
    }

    private int CalculateScoreTwo(List<string> field)
    {
        for (int i = 0; i < field.Count - 1; i++)
        {
            int difs = 0;
            for (int j = 0; j < field[i].Length; j++)
            {
                if (field[i][j] != field[i + 1][j])
                {
                    if (++difs > 1) break;
                }
            }

            if (SplitAndCompareTwo(field, i)) return 100 * (i + 1);
        }

        var sideways = new List<List<char>>();

        foreach (var _ in field[0])
        {
            sideways.Add(new List<char>());
        }

        for (int i = 0; i < field.Count; i++)
        {
            for (int j = 0; j < field[0].Length; j++)
            {
                sideways[j].Add(field[i][j]);
            }
        }

        var legitSideways = sideways.Select(row => string.Join("", row)).ToList();

        for (int i = 0; i < legitSideways.Count - 1; i++)
        {
            int difs = 0;
            for (int j = 0; j < legitSideways[i].Length; j++)
            {
                if (legitSideways[i][j] != legitSideways[i + 1][j])
                {
                    if (++difs > 1) break;
                }
            }

            if (SplitAndCompareTwo(legitSideways, i)) return i + 1;
        }

        return 0;
    }

    private bool SplitAndCompareTwo(List<string> field, int lineIndex)
    {
        int difs = 0;
        
        var length = Math.Min(lineIndex + 1, field.Count - lineIndex - 1);
        var list1 = field.GetRange(lineIndex + 1, length);
        var list2 = new List<string>();
        for (int i = lineIndex; i >= lineIndex - length + 1; i--)
        {
            list2.Add(field[i]);
        }

        for (int i = 0; i < list1.Count; i++)
        {
            for (int j = 0; j < list1[0].Length; j++)
            {
                if (list1[i][j] != list2[i][j])
                {
                    difs++;
                    if (difs > 1) return false;
                }
            }
        }

        return difs == 1;

    }
}
