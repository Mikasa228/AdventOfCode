using System.Diagnostics;

namespace SeventeenthDay;

class Program
{
    const string inputPath = @"..\..\..\input.txt";

    static int resultFirst = 0;
    static int resultSecond = 0;

    static int maxX = int.MinValue;
    static int maxY = int.MinValue;
    static int maxZ = int.MinValue;

    static int minX = int.MaxValue;
    static int minY = int.MaxValue;
    static int minZ = int.MaxValue;

    static void Main()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        Dictionary<List<int>, int> dots = new();
        Dictionary<Dictionary<List<int>, int>, int> innerShapes = new();

        using var reader = new StreamReader(inputPath);
        while (!reader.EndOfStream)
        {
            var fullString = reader.ReadLine();
            if (string.IsNullOrEmpty(fullString)) throw new NullReferenceException();

            dots[fullString.Split(",").Select(n => int.Parse(n)).ToList()] = 6;
        }

        GetMinMax(dots.Keys);

        var emptyDots = GetInnerDots(dots.Keys);

        CalculateFreeSides(dots);

        resultFirst = dots.Values.Sum();
        resultSecond = resultFirst;

        while (emptyDots.Count > 0)
        {
            var innerShape = new Dictionary<List<int>, int>();
            var first = emptyDots[0];

            var additionQueue = new Queue<List<int>>();
            additionQueue.Enqueue(first);

            while (additionQueue.Count > 0)
            {
                var dot = additionQueue.Dequeue();
                emptyDots.Remove(dot);
                var dotNeighbors = emptyDots.Where(d => AreNeighbors(d, dot)).ToList();
                innerShape[dot] = 6;
                foreach (var neighbor in dotNeighbors)
                {
                    additionQueue.Enqueue(neighbor);
                    emptyDots.Remove(neighbor);
                }
            }

            CalculateFreeSides(innerShape);

            innerShapes[innerShape] = innerShape.Values.Sum();
        }

        var pp = innerShapes.OrderByDescending(pair => pair.Key.Count);

        foreach (var shape in innerShapes)
        {
            var gut = true;
            foreach (var dot in shape.Key)
            {
                if (dots.Where(d => AreNeighbors(d.Key, dot.Key)).Count() != dot.Value)
                {
                    gut = false;
                }
            }
            if (gut) resultSecond -= shape.Value;
        }

        stopwatch.Stop();

        Console.WriteLine($"Part one: {resultFirst}\nPart two: {resultSecond}\nTime elapsed: {stopwatch.Elapsed}");
    }

    static bool AreNeighbors(List<int> list1, List<int> list2)
    {
        return list1[0] == list2[0] && list1[1] == list2[1] && Math.Abs(list1[2] - list2[2]) == 1
            || list1[0] == list2[0] && list1[2] == list2[2] && Math.Abs(list1[1] - list2[1]) == 1
            || list1[1] == list2[1] && list1[2] == list2[2] && Math.Abs(list1[0] - list2[0]) == 1;
    }

    private static void CalculateFreeSides(Dictionary<List<int>, int> dots)
    {
        foreach (var item in dots)
        {
            foreach (var key in dots.Keys)
            {
                if (AreNeighbors(item.Key, key))
                {
                    dots[item.Key] = dots[item.Key] - 1;
                    if (dots[item.Key] == 0) break;
                }
            }
        }
    }

    private static List<List<int>> GetInnerDots(Dictionary<List<int>, int>.KeyCollection dots)
    {
        List<List<int>> emptyDots = new();

        for (int x = minX + 1; x < maxX; x++)
        {
            for (int y = minY + 1; y < maxY; y++)
            {
                for (int z = minZ + 1; z < maxZ; z++)
                {
                    var dot = new List<int>() { x, y, z };
                    if (dots.SingleOrDefault(list =>
                    {
                        return list[0] == x && list[1] == y && list[2] == z;
                    }) == default)
                    {
                        emptyDots.Add(dot);
                    }
                }
            }
        }

        return emptyDots;
    }

    private static void GetMinMax(Dictionary<List<int>, int>.KeyCollection keys)
    {
        foreach (var key in keys)
        {
            maxX = Math.Max(maxX, key[0]);
            maxY = Math.Max(maxY, key[1]);
            maxZ = Math.Max(maxZ, key[2]);

            minX = Math.Min(minX, key[0]);
            minY = Math.Min(minY, key[1]);
            minZ = Math.Min(minZ, key[2]);
        }
    }
}