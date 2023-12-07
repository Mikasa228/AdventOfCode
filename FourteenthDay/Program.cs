using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Sources;

namespace FourteenthDay;

class Program
{
    const string inputPath = @"..\..\..\input.txt";

    static int resultFirst = 0;
    static int resultSecond = 0;

    static int minX = 0;
    static int maxX = 0;
    static int minY = 0;
    static int maxY = 0;

    static readonly Point start = new(500, 0);

    static void Main()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        List<List<Point>> paths = new();
        List<List<char>> map = new();

        using var reader = new StreamReader(inputPath);
        while (!reader.EndOfStream)
        {
            var fullString = reader.ReadLine();
            if (string.IsNullOrEmpty(fullString)) throw new NullReferenceException();

            var pointStrings = fullString.Split(" -> ");
            var path = new List<Point>();
            foreach (var pointString in pointStrings)
            {
                var coords = pointString.Split(",");
                path.Add(new Point(int.Parse(coords[0]), int.Parse(coords[1])));
            }
            paths.Add(path);
        }

        minY = Math.Min(0, paths.Min(path => path.Min(point => point.BaseY)));
        maxY = paths.Max(path => path.Max(point => point.BaseY));

        var height = maxY - minY + 2;

        minX = paths.Min(path => path.Min(point => point.BaseX)) - height;
        maxX = paths.Max(path => path.Max(point => point.BaseX)) + height;

        for (int rowIndex = 0; rowIndex <= maxY - minY; rowIndex++)
        {
            var row = new List<char>();
            for (int columnIndex = 0; columnIndex <= maxX - minX; columnIndex++)
            {
                row.Add('.');
            }
            map.Add(row);
        }

        SetStart(map);

        foreach (var path in paths)
        {
            DrawLine(path, map);
        }

        while (DropSandFirst(start, map)) ;

        foreach (var row in map)
        {
            row.ForEach(point => Console.Write(point));
            Console.WriteLine();
        }

        ClearMap(map);
        Console.WriteLine();

        var emptyRow = new List<char>();
        var groundRow = new List<char>();

        for (int _ = 0; _ < map[0].Count; _++)
        {
            emptyRow.Add('.');
            groundRow.Add('#');
        }

        map.Add(emptyRow);
        map.Add(groundRow);

        foreach (var row in map)
        {
            row.ForEach(point => Console.Write(point));
            Console.WriteLine();
        }

        while (DropSandSecond(start, map)) ;

        Console.WriteLine();

        foreach (var row in map)
        {
            row.ForEach(point => Console.Write(point));
            Console.WriteLine();
        }

        stopwatch.Stop();

        Console.WriteLine($"Part one: {resultFirst}\nPart two: {resultSecond}\nTime elapsed: {stopwatch.Elapsed}");

        static void DrawLine(List<Point> path, List<List<char>> map)
        {
            for (int i = 0; i < path.Count-1; i++)
            {
                var start = path[i];
                var end = path[i + 1];

                if (start.BaseX != end.BaseX)
                {
                    var rowIndex = start.Y;
                    for (int columnIndex = Math.Min(start.X, end.X); columnIndex <= Math.Max(start.X, end.X); columnIndex++)
                    {
                        map[rowIndex][columnIndex] = '#';
                    }
                }

                if (start.BaseY != end.BaseY)
                {
                    var columnIndex = start.X;
                    for (int rowIndex = Math.Min(start.Y, end.Y); rowIndex <= Math.Max(start.Y, end.Y); rowIndex++)
                    {
                        map[rowIndex][columnIndex] = '#';
                    }
                }
            }
        }

        static void SetStart(List<List<char>> map)
        {
            map[start.Y][start.X] = '+';
        }

        static bool DropSandFirst(Point startPoint, List<List<char>> map)
        {
            Point target = new(startPoint.BaseX, startPoint.BaseY+1);

            if (target.Y >= map.Count) return false;


            while (map[target.Y][target.X] == '.')
            {
                int targetY = target.Y + 1;
                if (targetY >= map.Count) return false;
                target.Y = targetY;
            }
            if (target.X - 1 < 0) return false;
            if (map[target.Y][target.X-1] == '.') return DropSandFirst(new Point(target.BaseX - 1, target.BaseY), map);

            if (target.X + 1 >= map[0].Count) return false;
            if (map[target.Y][target.X+1] == '.') return DropSandFirst(new Point(target.BaseX + 1, target.BaseY), map);

            map[target.Y - 1][target.X] = 'o';
            resultFirst++;

            return true;
        }

        static bool DropSandSecond(Point startPoint, List<List<char>> map)
        {
            Point target = new(startPoint.BaseX, startPoint.BaseY + 1);

            if (target.Y >= map.Count) return false;


            while (map[target.Y][target.X] == '.')
            {
                int targetY = target.Y + 1;
                if (targetY >= map.Count) return false;
                target.Y = targetY;
            }
            if (target.X - 1 < 0) return false;
            if (map[target.Y][target.X - 1] == '.') return DropSandSecond(new Point(target.BaseX - 1, target.BaseY), map);

            if (target.X + 1 >= map[0].Count) return false;
            if (map[target.Y][target.X + 1] == '.') return DropSandSecond(new Point(target.BaseX + 1, target.BaseY), map);

            map[target.Y - 1][target.X] = 'o';
            resultSecond++;

            if (target.X == start.X && target.Y - 1 == start.Y) return false;

            return true;
        }

        static void ClearMap(List<List<char>> map)
        {
            foreach (var row in map)
            {
                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i] == 'o') row[i] = '.';
                }
            }
        }

        //static void RenderField(int width, int height)
        //{
        //    for (int i = 0; i < height; i++)
        //    {
        //        for (int j = 0; j < width; j++)
        //        {

        //        }
        //    }
        //}
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
}
