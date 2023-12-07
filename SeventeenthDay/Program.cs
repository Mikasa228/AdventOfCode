using System;
using System.Diagnostics;

namespace SeventeenthDay;

class Program
{
    const string inputPath = @"..\..\..\input.txt";

    static int resultFirst = 0;
    static readonly int resultSecond = 0;

    static string? jetString;
    static int jetCursor = 0;

    static int currentPosition = 0;
    static Shape? currentShape;

    static readonly List<List<char>> cave = new()
        {
            "#######".ToCharArray().ToList()
        };

    static readonly List<char> emptyRow = new() { '.', '.', '.', '.', '.', '.', '.' };

    static readonly Shape hLine = new(new()
        {
            "..@@@@.".ToCharArray().ToList()
        });

    static readonly Shape cross = new(new()
        {
            "...@...".ToCharArray().ToList(),
            "..@@@..".ToCharArray().ToList(),
            "...@...".ToCharArray().ToList()
        });

    static readonly Shape corner = new(new()
        {
            "..@@@..".ToCharArray().ToList(),
            "....@..".ToCharArray().ToList(),
            "....@..".ToCharArray().ToList()
        });

    static readonly Shape vLine = new(new()
        {
            "..@....".ToCharArray().ToList(),
            "..@....".ToCharArray().ToList(),
            "..@....".ToCharArray().ToList(),
            "..@....".ToCharArray().ToList()
        });

    static readonly Shape square = new(new()
        {
            "..@@...".ToCharArray().ToList(),
            "..@@...".ToCharArray().ToList()
        });

    static readonly List<Shape> shapeRotation = new()
        {
            hLine,
            cross,
            corner,
            vLine,
            square
        };
    static int shapeCursor = 0;

    static void Main()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        using var reader = new StreamReader(inputPath);
        jetString = reader.ReadLine();
        if (string.IsNullOrEmpty(jetString)) throw new NullReferenceException();

        for (int _ = 0; _ < 2022; _++)
        {
            SpawnShape();

            while (true)
            {
                //PrintMap();

                MoveSide();

                //PrintMap();

                if (!MoveDown()) break;
            }

            //PrintMap();
        }

        PrintMap();

        resultFirst = cave.Count - 1;

        stopwatch.Stop();

        Console.WriteLine($"Part one: {resultFirst}\nPart two: {resultSecond}\nTime elapsed: {stopwatch.Elapsed}");

        static void MoveSide()
        {
            var direction = jetString?[jetCursor];
            var directionInt = direction == '>' ? 1 : -1;

            jetCursor = jetCursor + 1 >= jetString?.Length ? 0 : jetCursor + 1;

            for (int i = 0; i < currentShape?.Height; i++)
            {
                if (direction == '>' && cave[currentPosition - i][6] == '@'
                 || direction == '<' && cave[currentPosition - i][0] == '@')
                {
                    return;
                }

                for (int position = 1; position < cave[0].Count - 1; position++)
                {
                    if (cave[currentPosition - i][position + directionInt] == '#'
                        && cave[currentPosition - i][position] == '@')
                    {
                        return;
                    }
                }
            }

            if (direction == '>')
            {

                if (currentShape is null)
                {
                    throw new NullReferenceException();
                }
                for (int i = currentShape.Height - 1; i >= 0; i--)
                {
                    for (int position = cave[0].Count - 1; position >= 0; position--)
                    {
                        if (cave[currentPosition - i][position] == '@')
                        {
                            cave[currentPosition - i][position + 1] = '@';
                            cave[currentPosition - i][position] = '.';
                        }
                    }

                    //PrintMap();


                }
            }

            else
            {
                if (currentShape is null)
                {
                    throw new NullReferenceException();
                }
                for (int i = currentShape.Height - 1; i >= 0; i--)
                {
                    for (int position = 0; position < cave[0].Count; position++)
                    {
                        if (cave[currentPosition - i][position] == '@')
                        {
                            cave[currentPosition - i][position - 1] = '@';
                            cave[currentPosition - i][position] = '.';
                        }
                    }

                    //PrintMap();


                }
            }

        }

        static bool MoveDown()
        {
            for (int i = 0; i <= currentShape?.Height; i++)
            {
                if (i > cave.Count) break;
                for (int position = 0; position < cave[0].Count; position++)
                {
                    if (cave[currentPosition - i - 1][position] == '#'
                        && cave[currentPosition - i][position] == '@')
                    {
                        Stabilize();
                        return false;
                    }
                }
            }
            if (currentShape is null)
            {
                throw new NullReferenceException();
            }
            for (int i = currentShape.Height - 1; i >= 0 ; i--)
            {
                for (int position = 0; position < cave[0].Count; position++)
                {
                    if (cave[currentPosition - i][position] == '@')
                    {
                        cave[currentPosition - i - 1][position] = '@';
                        cave[currentPosition - i][position] = '.';
                    }
                }

                //PrintMap();


            }

            currentPosition--;
            var lastRow = cave[^1];
            if (!lastRow.Contains('#')) cave.Remove(lastRow);

            return true;
        }

        static void Stabilize()
        {
            foreach (var row in cave)
            {
                for (int position = 0; position < row.Count; position++)
                {
                    if (row[position] == '@') row[position] = '#';
                }
            }
        }
    }

    private static void SpawnShape()
    {
        for (int _ = 0; _ < 3; _++)
        {
            cave.Add(new(emptyRow));
        }
        currentShape = shapeRotation[shapeCursor];
        foreach (var row in currentShape.Rows)
        {
            cave.Add(new(row));
        }
        shapeCursor = shapeCursor + 1 >= shapeRotation.Count ? 0 : shapeCursor + 1;
        currentPosition = cave.Count - 1;
    }

    private static void PrintMap()
    {
        Console.Clear();
        for (int i = cave.Count - 1; i >= 0; i--)
        {
            Console.WriteLine(string.Join("", cave[i]));
        }
        Thread.Sleep(500);

    }

    class Shape
    {
        public List<List<char>> Rows { get; set; }
        public int Height { get => Rows.Count; }

        public Shape(List<List<char>> rows)
        {
            Rows = rows;
        }
    }
}