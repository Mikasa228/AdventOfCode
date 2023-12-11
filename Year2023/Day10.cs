using Common;

namespace Year2023;

internal class Day10 : Day
{
    protected override int TestSolutionOne { get; set; } = 4;
    protected override int TestSolutionTwo { get; set; } = 4;

    public static List<List<char>> Map { get; set; }

    protected override long SolveOne(string input)
    {
        using var reader = new StreamReader(input);
        Map = new();
        var path = new List<char>() { 'S' };
        Point previous = new(-1, -1);
        Point current = new(-1, -1);
        Point next = new(-1, -1);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine() ?? throw new NullReferenceException();
            Map.Add(line.ToList());
        }

        for (int i = 0; i < Map.Count; i++)
        {
            if (Map[i].Contains('S'))
            {
                previous = new(Map[i].IndexOf('S'), i);
                break;
            }
        }
        if (Map[previous.row - 1][previous.col] == 'F' || Map[previous.row - 1][previous.col] == '7' || Map[previous.row - 1][previous.col] == '|') current = new Point(previous.col, previous.row - 1);
        else if (Map[previous.row + 1][previous.col] == 'J' || Map[previous.row + 1][previous.col] == 'L' || Map[previous.row + 1][previous.col] == '|') current = new Point(previous.col, previous.row + 1);
        else if (Map[previous.row][previous.col - 1] == 'F' || Map[previous.row][previous.col - 1] == 'L' || Map[previous.row][previous.col - 1] == '-') current = new Point(previous.col - 1, previous.row);
        else if (Map[previous.row][previous.col + 1] == 'J' || Map[previous.row][previous.col + 1] == '7' || Map[previous.row][previous.col + 1] == '-') current = new Point(previous.col + 1, previous.row);

        path.Add(GetValue(current));

        while (true)
        {
            next = Trailblaze(previous, current);
            if (GetValue(next) == 'S')
            {
                break;
            }
            path.Add(GetValue(next));
            previous = current;
            current = next;
        }

        return path.Count / 2;
    }

    private Point Trailblaze(Point previous, Point current)
    {
        Point next = new(-1, -1);
        int col, row;

        switch (GetValue(current))
        {
            case 'F':
                col = current.col + (current.col - previous.col + 1);
                row = current.row + (current.row - previous.row + 1);
                next = new Point(col, row);
                break;
            case 'L':
                col = current.col + (current.col - previous.col + 1);
                row = current.row + (current.row - previous.row - 1);
                next = new Point(col, row);
                break;
            case 'J':
                col = current.col + (current.col - previous.col - 1);
                row = current.row + (current.row - previous.row - 1);
                next = new Point(col, row);
                break;
            case '7':
                col = current.col + (current.col - previous.col - 1);
                row = current.row + (current.row - previous.row + 1);
                next = new Point(col, row);
                break;
            case '-':
                col = current.col + (current.col - previous.col);
                row = current.row;
                next = new Point(col, row);
                break;
            case '|':
                col = current.col;
                row = current.row + (current.row - previous.row);
                next = new Point(col, row);
                break;
            default: break;
        }


        return next;
    }

    private static char GetValue(Point point)
    {
        return Map[point.row][point.col];
    }

    protected override long SolveTwo(string input)
    {
        var output = 0;
        using var reader = new StreamReader(input);
        Map = new();
        var path = new List<Point>();
        Point previous = new(-1, -1);
        Point current = new(-1, -1);
        Point next = new(-1, -1);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine() ?? throw new NullReferenceException();
            Map.Add(line.ToList());
        }

        for (int i = 0; i < Map.Count; i++)
        {
            if (Map[i].Contains('S'))
            {
                previous = new(Map[i].IndexOf('S'), i);
                path.Add(previous);
                break;
            }
        }
        if (Map[previous.row - 1][previous.col] == 'F' || Map[previous.row - 1][previous.col] == '7' || Map[previous.row - 1][previous.col] == '|') current = new Point(previous.col, previous.row - 1);
        else if (Map[previous.row + 1][previous.col] == 'J' || Map[previous.row + 1][previous.col] == 'L' || Map[previous.row + 1][previous.col] == '|') current = new Point(previous.col, previous.row + 1);
        else if (Map[previous.row][previous.col - 1] == 'F' || Map[previous.row][previous.col - 1] == 'L' || Map[previous.row][previous.col - 1] == '-') current = new Point(previous.col - 1, previous.row);
        else if (Map[previous.row][previous.col + 1] == 'J' || Map[previous.row][previous.col + 1] == '7' || Map[previous.row][previous.col + 1] == '-') current = new Point(previous.col + 1, previous.row);

        path.Add(current);

        while (true)
        {
            next = Trailblaze(previous, current);
            if (GetValue(next) == 'S')
            {
                break;
            }
            path.Add(next);
            previous = current;
            current = next;
        }
        Console.Clear();
        for (int row = 1; row < Map.Count - 1; row++)
        {
            for (int col = 1; col < Map[row].Count - 1; col++)
            {
                if (path.Where(point => point.col == col && point.row == row).Count() > 0) Console.Write(GetValue(new Point(col, row)));
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write('.');
                    Console.ForegroundColor = ConsoleColor.White;
                }

            }
            Console.WriteLine(  );
        }

        for (int row = 1; row < Map.Count - 1; row++)
        {
            for (int col = 1; col < Map[row].Count - 1; col++)
            {
                if (path.Where(point => point.col == col && point.row == row).Count() > 0) continue;
                var hyphens = path.Where(point => (GetValue(point) == '-' && point.col == col && point.row < row));
                var Fs = path.Where(point => (GetValue(point) == 'F' && point.col == col && point.row < row));
                var Sevens = path.Where(point => (GetValue(point) == '7' && point.col == col && point.row < row));
                var Js = path.Where(point => (GetValue(point) == 'J' && point.col == col && point.row < row));
                var Ls = path.Where(point => (GetValue(point) == 'L' && point.col == col && point.row < row));
                if ((hyphens.Count() + Math.Min(Fs.Count(), Js.Count()) + Math.Min(Sevens.Count(), Ls.Count())) % 2 == 1)
                {
                    output++;
                }
            }
        }

        return output;
    }

    private class Point
    {
        public int col, row;
        public Point(int col, int row)
        {
            this.col = col;
            this.row = row;
        }

        public override string ToString()
        {
            return $"({col},{row}) - {GetValue(this)}";
        }
    }
}
