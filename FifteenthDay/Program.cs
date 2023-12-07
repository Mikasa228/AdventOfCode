using System.Diagnostics;
using System.Text.RegularExpressions;

namespace FourteenthDay;

class Program
{
    const string inputPath = @"..\..\..\input.txt";

    static int resultFirst = 0;
    static readonly int resultSecond = 0;

    static int minX = int.MaxValue;
    static int maxX = int.MinValue;
    static int minY = int.MaxValue;
    static int maxY = int.MinValue;


    static void Main()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        List<Beacon> beacons = new();
        List<Sensor> sensors = new();

        List<List<char>> map = new();

        //int targetRowIndex = 10;
        int targetRowIndex = 2000000;

        using var reader = new StreamReader(inputPath);
        while (!reader.EndOfStream)
        {
            var fullString = reader.ReadLine();
            if (string.IsNullOrEmpty(fullString)) throw new NullReferenceException();

            var match = Regex.Match(fullString, @"Sensor at x=(?'Sx'[-\d]+), y=(?'Sy'[-\d]+): closest beacon is at x=(?'Bx'[-\d]+), y=(?'By'[-\d]+)");
            int Bx = int.Parse(match.Groups["Bx"].Value);
            int By = int.Parse(match.Groups["By"].Value);

            var beacon = new Beacon(Bx, By);
            beacons.Add(beacon);

            int Sx = int.Parse(match.Groups["Sx"].Value);
            int Sy = int.Parse(match.Groups["Sy"].Value);

            var sensor = new Sensor(Sx, Sy, beacon);
            sensors.Add(sensor);
        }

        //foreach (var beacon in beacons)
        //{
        //    minX = Math.Min(minX, beacon.baseX);
        //    minY = Math.Min(minY, beacon.baseY);
        //    maxX = Math.Max(maxX, beacon.baseX);
        //    maxY = Math.Max(maxY, beacon.baseY);
        //}

        foreach (var sensor in sensors)
        {
            minX = Math.Min(minX, sensor.BaseX - sensor.Radius);
            minY = Math.Min(minY, sensor.BaseY - sensor.Radius);
            maxX = Math.Max(maxX, sensor.BaseX + sensor.Radius);
            maxY = Math.Max(maxY, sensor.BaseY + sensor.Radius);
        }

        targetRowIndex -= minY;

        List<char> targetRow = new();

        //for (int rowIndex = 0; rowIndex <= maxY - minY; rowIndex++)
        //{
        //    var row = new List<char>();
        //    if (rowIndex == targetRowIndex)
        //    {
        //        targetRow = row;
        //    }
        //    for (int columnIndex = 0; columnIndex <= maxX - minX; columnIndex++)
        //    {
        //        row.Add('.');
        //    }
        //    map.Add(row);
        //}



        foreach (var row in map)
        {
            Console.WriteLine(string.Join("", row));
        }

        //var pointsInRow = new List<Point>();
        //pointsInRow.AddRange(beacons.Where(beacon => beacon.Y == targetRowIndex).Select(beacon => (Point)beacon).ToList());
        //pointsInRow.AddRange(sensors.Where(sensor => sensor.Y == targetRowIndex).Select(sensor => (Point)sensor).ToList());
        for (int _ = 0; _ <= maxX - minX; _++)
        {
            targetRow.Add('.');
        }

        //foreach (var sensor in sensors)
        //{
        //    {
        //        map[sensor.Y][sensor.X] = 'S';
        //    }

        //}

        //foreach (var beacon in beacons)
        //{
        //    {
        //        map[beacon.Y][beacon.X] = 'B';
        //    }
        //}

        Console.Clear();
        foreach (var row in map)
        {
            Console.WriteLine(string.Join("", row));
        }

        foreach (var sensor in sensors)
        {
            if (sensor.Y == targetRowIndex)
            {
                targetRow[sensor.X] = 'S';
            }

        }

        foreach (var beacon in beacons)
        {
            if (beacon.Y == targetRowIndex)
            {
                targetRow[beacon.X] = 'B';
            }
        }
        //Console.WriteLine(string.Join("", targetRow));

        //foreach (var sensor in sensors)
        // {
        //    for (int rowIndex = 0; rowIndex < map.Count; rowIndex++)
        //    {
        //        for (int columnIndex = 0; columnIndex < map[0].Count; columnIndex++)
        //        {
        //            if (map[rowIndex][columnIndex] != '.') continue;

        //            if (sensor.IsInRange(columnIndex, rowIndex))
        //            {
        //                map[rowIndex][columnIndex] = '#';
        //            }
        //        }
        //    }

        //    Console.Clear();
        //    foreach (var row in map)
        //    {
        //        Console.WriteLine(string.Join("", row));
        //    }
        //}

        for ( int columnIndex = 0; columnIndex < targetRow.Count; columnIndex++)
        {
            if (targetRow[columnIndex] != '.') continue;

            foreach (var sensor in sensors)
            {
                if (sensor.IsInRange(columnIndex, targetRowIndex))
                {
                    resultFirst++;
                    targetRow[columnIndex] = '#';
                    break;
                }
                //Console.WriteLine(string.Join("", targetRow));
                //Console.Clear();
                //foreach (var row in map)
                //{
                //    Console.WriteLine(string.Join("", row));
                //}
            }
        }
        //Console.WriteLine(string.Join("", targetRow));

        //foreach (var row in map)
        //{
        //    Console.WriteLine(string.Join("", row));
        //}

        Console.Clear();
        foreach (var row in map)
        {
            Console.WriteLine(string.Join("", row));
        }

        stopwatch.Stop();

        //Console.WriteLine();

        Console.WriteLine($"Part one: {resultFirst}\nPart two: {resultSecond}\nTime elapsed: {stopwatch.Elapsed}");

    }


    class Point
    {
        public int BaseX { get; set; }
        public int BaseY { get; set; }
        public int X { get => BaseX - minX; set => BaseX = value + minX; }
        public int Y { get => BaseY - minY; set => BaseY = value + minY; }

        public Point(int x, int y)
        {
            BaseX = x;
            BaseY = y;
        }

        public Point(Point copy)
        {
            BaseX = copy.BaseX;
            BaseY = copy.BaseY;
        }

        public override string ToString()
        {
            return $"{BaseX} : {BaseY}";
        }
    }

    class Beacon : Point
    {
        public Beacon(int x, int y) : base(x, y) { }
    }

    class Sensor : Point
    {
        public int Radius { get; set; }
        public Sensor(int x, int y, Beacon beacon) : base(x, y)
        {
            Radius = Math.Abs(x - beacon.BaseX) + Math.Abs(y - beacon.BaseY);
        }

        public bool IsInRange(int x, int y)
        {
            return Radius >= Math.Abs(X - x) + Math.Abs(Y - y);
        }
    }
}
