using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks.Sources;

namespace TwelfthDay;

class Program
{
    const string inputPath = @"..\..\..\input.txt";

    static int resultFirst = -1;
    static readonly int resultSecond = -1;

    static void Main()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        var map = new List<List<char>>();
        var pathNodes = new List<PathNode>();

        var distances = new List<int>();
        var chekked = new List<PathNode>();
        PathNode? end = null;

        using var reader = new StreamReader(inputPath);
        while (!reader.EndOfStream)
        {
            var fullString = reader.ReadLine() ?? throw new NullReferenceException();
            var row = new List<char>();
            map.Add(row);
            foreach (var letter in fullString)
            {
                row.Add(letter);
            }
        }
        var nodeMap = new List<List<PathNode>>();
        foreach (var row in map)
        {
            var nodeRow = new List<PathNode>();
            foreach (var letter in row)
            {
                var node = new PathNode(letter);

                PathNode? start;
                if (letter == 'S') start = node;
                if (letter == 'E') end = node;
                pathNodes.Add(node);
                nodeRow.Add(node);
            }
            nodeMap.Add(nodeRow);
        }

        for (int rowIndex = 0; rowIndex < nodeMap.Count; rowIndex++)
        {
            for (int columnIndex = 0; columnIndex < nodeMap[0].Count; columnIndex++)
            {
                PathNode currentNode = nodeMap[rowIndex][columnIndex];
                PathNode inspectedNode;
                if (rowIndex - 1 >= 0)
                {
                    inspectedNode = nodeMap[rowIndex - 1][columnIndex];
                    if (inspectedNode.Elevation - currentNode.Elevation < 2)
                    {
                        currentNode.Neighbors.Add(inspectedNode);
                    }
                }
                if (rowIndex + 1 < nodeMap.Count)
                {
                    inspectedNode = nodeMap[rowIndex + 1][columnIndex];
                    if (inspectedNode.Elevation - currentNode.Elevation < 2)
                    {
                        currentNode.Neighbors.Add(inspectedNode);
                    }
                }
                if (columnIndex - 1 >= 0)
                {
                    inspectedNode = nodeMap[rowIndex][columnIndex - 1];
                    if (inspectedNode.Elevation - currentNode.Elevation < 2)
                    {
                        currentNode.Neighbors.Add(inspectedNode);
                    }
                }
                if (columnIndex + 1 < nodeMap[0].Count)
                {
                    inspectedNode = nodeMap[rowIndex][columnIndex + 1];
                    if (inspectedNode.Elevation - currentNode.Elevation < 2)
                    {
                        currentNode.Neighbors.Add(inspectedNode);
                    }
                }
            }
        }

        foreach (var pathNode in pathNodes)
        {
            if (pathNode.Letter == 'S' || pathNode.Letter == 'a')
            {
                distances.Add(0);
            }
            else
            {
                distances.Add(int.MaxValue);
            }
        }

        for (int _ = 0; _ < distances.Count; _++)
        {
            try
            {
                var minDistance = int.MaxValue;
                PathNode? targetNode = null;
                for (int i = 0; i < distances.Count; i++)
                {
                    if (chekked.Contains(pathNodes[i])) continue;
                    if (distances[i] < minDistance)
                    {
                        minDistance = distances[i];
                        targetNode = pathNodes[i];
                    }


                }
                if (targetNode is null) throw new NullReferenceException();
                foreach (var node in targetNode.Neighbors)
                {
                    var index = pathNodes.IndexOf(node);
                    if (minDistance + 1 < distances[index])
                    {
                        distances[index] = minDistance + 1;
                    }
                }
                chekked.Add(targetNode);
            }
            catch (NullReferenceException)
            {

                break;
            }

        }

        if (end is null) throw new NullReferenceException();
        resultFirst = distances[pathNodes.IndexOf(end)];
        stopwatch.Stop();

        Console.WriteLine($"Part one: {resultFirst}\nPart two: {resultSecond}\nTime elapsed: {stopwatch.Elapsed}");


    }

    class PathNode
    {
        public int Elevation { get => GetElevation(Letter); }
        public char Letter { get; set; }
        public List<PathNode> Neighbors { get; set; } = new();
        public PathNode(char letter)
        {
            Letter = letter;
        }

        public override string ToString()
        {
            return Letter.ToString();
        }

        static int GetElevation(char letter)
        {
            if (letter == 'S') return 'a';
            if (letter == 'E') return 'z';
            return letter;
        }
    }
}
