using Common;

namespace Year2023;

internal class Day11 : Day
{
    protected override int TestSolutionOne { get; set; } = 374;
    protected override int TestSolutionTwo { get; set; } = 82000210;

    protected override long SolveOne(string input)
    {
        return Solve(input, 2);
    }

    protected override long SolveTwo(string input)
    {
        return Solve(input, 1000000);
    }

    private static long Solve(string input, int multiplier)
    {
        long output = 0;
        var map = new List<List<char>>();

        using var reader = new StreamReader(input);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine() ?? throw new NullReferenceException();
            map.Add(line.ToList());
        }

        var emptyCols = new List<int>();
        var emptyRows = new List<int>();
        for (int col = 0; col < map[0].Count; col++)
        {
            if (map.All(row => row[col] == '.')) emptyCols.Add(col);
        }

        for (int row = 0; row < map.Count; row++)
        {
            if (!map[row].Contains('#')) emptyRows.Add(row);
        }

        var galaxies = new List<Point>();

        for (int row = 0; row < map.Count; row++)
        {
            for (int col = 0; (col < map[row].Count); col++)
            {
                if (map[row][col] == '#')
                {
                    galaxies.Add(new(row, col));
                }
            }
        }

        for (int i = 0; i < galaxies.Count - 1; i++)
        {
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                output += Math.Abs(galaxies[i].row - galaxies[j].row) + Math.Abs(galaxies[i].col - galaxies[j].col);
                foreach (var emptyRow in emptyRows)
                {
                    if (emptyRow < Math.Max(galaxies[i].row, galaxies[j].row) && emptyRow > Math.Min(galaxies[i].row, galaxies[j].row))
                    {
                        output += (multiplier - 1);
                    }
                }
                foreach (var emptyCol in emptyCols)
                {
                    if (emptyCol < Math.Max(galaxies[i].col, galaxies[j].col) && emptyCol > Math.Min(galaxies[i].col, galaxies[j].col))
                    {
                        output += (multiplier - 1);
                    }
                }
            }
        }

        return output;
    }

    private class Point
    {
        public int row;
        public int col;

        public Point(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }
}
