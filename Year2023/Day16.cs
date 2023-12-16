using Common;

namespace Year2023;

internal class Day16 : Day
{
    protected override int TestSolutionOne { get; set; } = 46;
    protected override int TestSolutionTwo { get; set; } = 51;

    private Direction direction;
    private int currentRow;
    private int currentColumn;

    protected override long SolveOne(string input)
    {
        var map = new List<List<char>>();

        using var reader = new StreamReader(input);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine() ?? throw new NullReferenceException();
            map.Add(line.ToList());
        }

        currentRow = 0;
        currentColumn = 0;
        direction = Direction.Right;

        return Travel(map);
    }

    protected override long SolveTwo(string input)
    {
        var output = 0;

        var map = new List<List<char>>();
        var variants = new List<Path>();

        using var reader = new StreamReader(input);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine() ?? throw new NullReferenceException();
            map.Add(line.ToList());
        }

        for (int i = 0; i < map.Count; i++)
        {
            variants.Add(new(i, 0, Direction.Right));
            variants.Add(new(i, map[i].Count - 1, Direction.Left));
        }

        for (int i = 0; i < map[0].Count; i++)
        {
            variants.Add(new(0, i, Direction.Down));
            variants.Add(new(map.Count - 1, i, Direction.Up));
        }

        foreach (var variant in variants)
        {
            currentRow = variant.Row;
            currentColumn = variant.Col;
            direction = variant.OneDirection;

            output = Math.Max(output, Travel(map));
        }

        return output;
    }

    private int Travel(List<List<char>> map)
    {
        var points = new List<List<int>>();

        for (int i = 0; i < map.Count; i++)
        {
            points.Add(new());
            for (int _ = 0; _ < map[0].Count; _++)
            {
                points[i].Add(0);
            }
        }

        var queue = new Queue<Path>();
        queue.Enqueue(new Path(currentRow, currentColumn, direction));

        var pathsTravelled = new List<Path>();

        while (queue.Count > 0)
        {
            var activePath = queue.Dequeue();
            
            currentColumn = activePath.Col;
            currentRow = activePath.Row;
            direction = activePath.OneDirection;

            while (true)
            {
                points[currentRow][currentColumn] = 1;
                AdjustDirection(map, queue, pathsTravelled);
                Move();
                if (currentRow < 0 || currentRow >= map.Count || currentColumn < 0 || currentColumn >= map[0].Count)
                {
                    break;
                }
            }
        }

        return points.Select(row => row.Sum()).Sum();
    }

    private void AdjustDirection(List<List<char>> map, Queue<Path> queue, List<Path> pathsTravelled)
    {
        switch (map[currentRow][currentColumn])
        {
            case '/':
                switch (direction)
                {
                    case Direction.Up:
                        direction = Direction.Right;
                        break;
                    case Direction.Down:
                        direction = Direction.Left;
                        break;
                    case Direction.Left:
                        direction = Direction.Down;
                        break;
                    case Direction.Right:
                        direction = Direction.Up;
                        break;
                    default:
                        break;
                }
                break;
            case '\\':
                switch (direction)
                {
                    case Direction.Up:
                        direction = Direction.Left;
                        break;
                    case Direction.Down:
                        direction = Direction.Right;
                        break;
                    case Direction.Left:
                        direction = Direction.Up;
                        break;
                    case Direction.Right:
                        direction = Direction.Down;
                        break;
                    default:
                        break;
                }
                break;
            case '-':
                if (direction == Direction.Left || direction == Direction.Right)
                {
                    break;
                }
                else
                {
                    if (pathsTravelled.Any(path => path.Col == currentColumn && path.Row == currentRow && path.OneDirection == direction))
                    {
                        currentRow = -1;
                        currentColumn = -1;
                        direction = Direction.Up;
                        break;
                    }

                    direction = Direction.Right;
                    queue.Enqueue(new(currentRow, currentColumn, Direction.Left));

                    pathsTravelled.Add(new Path(currentRow, currentColumn, Direction.Up));
                    pathsTravelled.Add(new Path(currentRow, currentColumn, Direction.Down));
                }
                break;
            case '|':
                if (direction == Direction.Up || direction == Direction.Down)
                {
                    break;
                }
                else
                {
                    if (pathsTravelled.Any(path => path.Col == currentColumn && path.Row == currentRow && path.OneDirection == direction))
                    {
                        currentRow = -1;
                        currentColumn = -1;
                        direction = Direction.Up;
                        break;
                    }

                    direction = Direction.Up;
                    queue.Enqueue(new(currentRow, currentColumn, Direction.Down));

                    pathsTravelled.Add(new Path(currentRow, currentColumn, Direction.Left));
                    pathsTravelled.Add(new Path(currentRow, currentColumn, Direction.Right));
                }
                break;
            default: break;
        }
    }

    private void Move()
    {
        switch (direction)
        {
            case Direction.Up:
                currentRow--;
                break;
            case Direction.Down:
                currentRow++;
                break;
            case Direction.Left:
                currentColumn--;
                break;
            case Direction.Right:
                currentColumn++;
                break;
            default:
                break;
        }
    }

    private class Path
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public Direction OneDirection { get; set; }
        public Path(int row, int col, Direction direction)
        {
            Row = row;
            Col = col;
            OneDirection = direction;
        }

        public override string ToString()
        {
            return $"{Col}, {Row} - {OneDirection}";
        }
    }

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
