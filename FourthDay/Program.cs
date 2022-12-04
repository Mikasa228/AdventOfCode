const string inputPath = @"..\..\..\input.txt";

int firstResult = 0;
int secondResult = 0;

using var reader = new StreamReader(inputPath);
while (!reader.EndOfStream)
{
    string? fullString = reader.ReadLine();
    if (fullString == null) throw new NullReferenceException();

    var pairs = fullString.Split(",");
    var firstBorders = pairs[0].Split("-").Select(x => int.Parse(x)).ToList();
    var secondBorders = pairs[1].Split("-").Select(x => int.Parse(x)).ToList();

    var firstRange = new List<int>();
    for (int i = firstBorders[0]; i <= firstBorders[1]; i++)
    {
        firstRange.Add(i);
    }

    var secondRange = new List<int>();
    for (int i = secondBorders[0]; i <= secondBorders[1]; i++)
    {
        secondRange.Add(i);
    }

    if (firstRange.All(x => secondRange.Contains(x))
        || secondRange.All(x => firstRange.Contains(x)))
        firstResult++;

    if (firstRange.Any(x => secondRange.Contains(x))
        || secondRange.Any(x => firstRange.Contains(x)))
        secondResult++;
}

Console.WriteLine($"Part one: {firstResult}\nPart two: {secondResult}");
