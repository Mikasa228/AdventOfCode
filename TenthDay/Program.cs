using System.Text;

namespace TenthDay;

class Program
{
    const string inputPath = @"..\..\..\input.txt";

    static int resultFirst = 0;

    static int cycleNumber = 0;
    static int x = 1;

    static readonly List<string> imageLines = new();
    static readonly StringBuilder currentImageLine = new();

    static void Main()
    {
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
            IncreaseCycleAndProcess();
            x += int.Parse(command[1]);
        }

        Console.WriteLine($"Part one: {resultFirst}\nPart two:");
        imageLines.ForEach(line => Console.WriteLine(line));
    }

    static void IncreaseCycleAndProcess()
    {
        cycleNumber++;
        Draw();
        if ((cycleNumber + 20) % 40 == 0)
        {
            resultFirst += (x * cycleNumber);
        }
    }

    static void Draw()
    {
        var position = (cycleNumber - 1) % 40;

        var symbol = Math.Abs(position - x) <= 1 ? '#' : ' ';
        currentImageLine.Append(symbol);

        if (currentImageLine.Length == 40)
        {
            imageLines.Add(currentImageLine.ToString());
            currentImageLine.Clear();
        }
    }
}
