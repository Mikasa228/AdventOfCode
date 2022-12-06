const string inputPath = @"..\..\..\input.txt";

int resultFirst;
int resultSecond;

using var reader = new StreamReader(inputPath);
while (!reader.EndOfStream)
{
    string? fullString = reader.ReadLine();
    if (fullString is null) throw new NullReferenceException();

    resultFirst = CalculateResult(fullString, 4);
    resultSecond = CalculateResult(fullString, 14);

    Console.WriteLine($"Part one: {resultFirst}\nPart two: {resultSecond}\n"); ;
}


static int CalculateResult(string fullString, int count)
{
    var buffer = new Queue<char>();
    for (int i = 0; i < fullString.Length; i++)
    {
        buffer.Enqueue(fullString[i]);
        if (buffer.Count < count) continue;
        if (buffer.Distinct().ToList().Count == count) return i + 1;
        buffer.Dequeue();
    }

    return -1;
}
