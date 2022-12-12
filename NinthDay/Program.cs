using System.Collections.Concurrent;

namespace NinthDay;

class Program
{
    static bool fastForward = false;
    static readonly object _locker = new();
    private static ConcurrentQueue<Draw> queue = new ConcurrentQueue<Draw>();

    static void Main()
    {
        Console.Write("Press Enter to start...");
        Console.ReadLine();
        Console.Clear();

        var resultsTask = Task.Factory.StartNew(() => Run(queue));

        Console.WriteLine("Press Enter to fast-forward...");
        Console.CursorVisible = false;

        while (!resultsTask.IsCompleted)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey();
                if (key.Key.ToString() == "Enter")
                {
                    fastForward = !fastForward;
                }
            }

            if (!queue.TryDequeue(out var message))
            {
                Thread.Sleep(10);
                continue;
            }
            Console.SetCursorPosition(message.Y, message.X);
            Console.Write(message.Text);
        }

        Render();

        var results = resultsTask.Result;

        Console.SetCursorPosition(0, 0);
        Console.Write("Press Enter to see the results...");
        Console.ReadLine();

        Console.Clear();
        Console.WriteLine($"Part one: {results[0]}\nPart two: {results[1]}\n");
    }

    static void Render()
    {
        while (queue.TryDequeue(out var message))
        {
            Console.SetCursorPosition(message.Y, message.X);
            Console.Write(message.Text);
        }
    }

    static List<int> Run(ConcurrentQueue<Draw> queue)
    {
        const string inputPath = @"..\..\..\input.txt";

        // Initialization for part one
        var visitedFirst = new List<string>();

        var firstHeadPosition = new Point(0, 0);
        var firstTailPosition = new Point(0, 0);

        visitedFirst.Add(firstTailPosition.ToString());

        // Initialization for part two
        var visitedSecond = new List<string>();

        var secondHeadPosition = new Point(0, 0);
        var secondTailPosition = new Point(0, 0);

        var rope = new List<Point>();
        rope.Add(secondHeadPosition);
        for (int _ = 0; _ < 8; _++)
        {
            rope.Add(new Point(0, 0));
        }
        rope.Add(secondTailPosition);

        var renderer = new RopeRenderer(rope);
        foreach (var draw in renderer.RenderRope())
        {
            queue.Enqueue(draw);
        }

        visitedSecond.Add(secondTailPosition.ToString());

        using var reader = new StreamReader(inputPath);
        while (!reader.EndOfStream)
        {
            string? fullString = reader.ReadLine();
            if (fullString is null) throw new NullReferenceException();

            var input = fullString.Split(' ');
            var direction = input[0];
            var distance = int.Parse(input[1]);

            for (int i = 0; i < distance; i++)
            {
                // Solution for part one
                firstHeadPosition.Move(direction);
                firstTailPosition.CatchUp(firstHeadPosition);
                if (!visitedFirst.Contains(firstTailPosition.ToString())) visitedFirst.Add(firstTailPosition.ToString());

                // Solution for part two
                secondHeadPosition.Move(direction);
                for (int j = 1; j < rope.Count; j++) rope[j].CatchUp(rope[j - 1]);
                foreach (var draw in renderer.RenderRope())
                {
                    queue.Enqueue(draw);
                }
                if (!fastForward) Thread.Sleep(1);
                if (!visitedSecond.Contains(secondTailPosition.ToString())) visitedSecond.Add(secondTailPosition.ToString());
            }
        }

        return new List<int>()
        {
            visitedFirst.Count,
            visitedSecond.Count
        };
    }
}


public class Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void Move(string direction)
    {
        var _ = direction switch
        {
            "L" => X--,
            "R" => X++,
            "U" => Y++,
            "D" => Y--,
            _ => throw new ArgumentException("Invalid parameter value.", nameof(direction))
        };
    }

    internal void CatchUp(Point coordinate)
    {
        var vectorX = coordinate.X - X;
        var vectorY = coordinate.Y - Y;

        var directionX = Math.Sign(vectorX) == -1 ? -1 : 1;
        var directionY = Math.Sign(vectorY) == -1 ? -1 : 1;

        if (directionX * vectorX > 1 && coordinate.Y != Y
            || directionY * vectorY > 1 && coordinate.X != X)
        {
            X += directionX;
            Y += directionY;
        }

        else if (directionX * vectorX > 1)
        {
            X += directionX;
        }

        else if (directionY * vectorY > 1)
        {
            Y += directionY;
        }
    }

    public override string ToString()
    {
        return $"{X} : {Y}";
    }
}

public record Draw(int Y, int X, string Text)
{
}