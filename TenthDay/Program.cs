using System.Text;

namespace TenthDay;

class Program
{
    static int cycleNumber = 1;
    static int resultFirst = 0;

    static int x = 1;
    static int counter = 1;

    static readonly List<string> imageLines = new();
    static readonly StringBuilder currentLine = new();

    static void Main()
    {
        const string inputPath = @"..\..\..\input.txt";

        using var reader = new StreamReader(inputPath);
        Draw();
        while (!reader.EndOfStream)
        {
            string? fullString = reader.ReadLine();
            if (fullString == null) throw new NullReferenceException();

            var command = fullString.Split(" ");
            if (command[0] == "noop")
            {
                IncreaseCycleAndProcess();
                continue;
            }

            IncreaseCycleAndProcess();
            x += int.Parse(command[1]);
            IncreaseCycleAndProcess();

        }
        Console.WriteLine($"Part one: {resultFirst}\nPart two:");
        foreach (var line in imageLines)
        {
            Console.WriteLine(line);
        }
    }

    static void IncreaseCycleAndProcess()
    {
        cycleNumber++;
        Draw();
        if (cycleNumber == 20 || (cycleNumber + 20) % (counter * 40) == 0)
        {
            resultFirst += (x * cycleNumber);
            counter++;
        }
    }

    static void Draw()
    {
        var position = (cycleNumber - 1) % 40;
        if (position == x - 1
            || position == x
            || position == x + 1)
        {
            currentLine.Append('#');
        }
        else
        {
            currentLine.Append('.');
        }
        if (currentLine.Length == 40)
        {
            imageLines.Add(currentLine.ToString());
            currentLine.Clear();
        }
    }
}
