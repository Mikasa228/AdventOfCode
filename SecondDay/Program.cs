namespace SecondDay;

public class Program
{
    public static void Main()
    {
        var inputPath = @"..\..\..\input.txt";

        var firstMethod = CalculatePartOne(inputPath);
        var secondMethod = CalculatePartTwo(inputPath);

        Console.WriteLine($"First method: {firstMethod}\nSecond method: {secondMethod}");
    }

    public static int CalculatePartOne(string path)
    {
        var total = 0;

        using var reader = new StreamReader(path);
        while (!reader.EndOfStream)
        {
            var readString = reader.ReadLine();
            if (readString == null) throw new NullReferenceException();

            var values = readString.Split(' ');

            var enemy = ToShape(values[0]);
            var you = ToShape(values[1]);

            total += (int)you;
            total += ((int)you - (int)enemy) switch
            {
                -2 => 6,
                0 => 3,
                1 => 6,
                _ => 0
            };
        }

        return total;
    }

    public static int CalculatePartTwo(string path)
    {
        var total = 0;

        using var reader = new StreamReader(path);
        while (!reader.EndOfStream)
        {
            var readString = reader.ReadLine();
            if (readString == null) throw new NullReferenceException();

            var values = readString.Split(' ');

            var enemy = ToShape(values[0]);
            var you = values[1];

            total += you switch
            {
                "X" => 0,
                "Y" => 3,
                "Z" => 6,
                _ => throw new ArgumentException(values[1])
            };

            if (you == "X")
            {
                if (enemy == Shape.Rock) total += 3;
                else total += (int)enemy - 1;
            }
            else if (you == "Y") total += (int)enemy;
            else if (you == "Z")
            {
                if (enemy == Shape.Scissors) total += 1;
                else total += (int)enemy + 1;
            }
            else throw new ArgumentException(you);
        }

        return total;
    }

    private static Shape ToShape(string code)
    {
        return code switch
        {
            "A" => Shape.Rock,
            "B" => Shape.Paper,
            "C" => Shape.Scissors,
            "X" => Shape.Rock,
            "Y" => Shape.Paper,
            "Z" => Shape.Scissors,
            _ => throw new ArgumentException(code)
        };
    }
}

public enum Shape
{
    Rock = 1,
    Paper = 2,
    Scissors = 3
}