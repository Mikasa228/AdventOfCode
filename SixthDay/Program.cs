const string inputPath = @"..\..\..\input.txt";

int resultFirst;
int resultSecond;

using var reader = new StreamReader(inputPath);
while (!reader.EndOfStream)
{
    string? fullString = reader.ReadLine();
    if (fullString is null) throw new NullReferenceException();

    resultFirst = CalculateResult(fullString, MarkerType.StartOfPacket);
    resultSecond = CalculateResult(fullString, MarkerType.StartOfMessage);

    Console.WriteLine($"Part one: {resultFirst}\nPart two: {resultSecond}\n"); ;
}


static int CalculateResult(string dataStreamBuffer, MarkerType markerType)
{
    var buffer = new Queue<char>();
    for (int markerPosition = 0; markerPosition < dataStreamBuffer.Length; markerPosition++)
    {
        buffer.Enqueue(dataStreamBuffer[markerPosition]);
        if (buffer.Count < (int)markerType) continue;
        if (buffer.Distinct().ToList().Count == (int)markerType) return markerPosition + 1;
        buffer.Dequeue();
    }

    return -1;
}

enum MarkerType
{
    StartOfPacket = 4,
    StartOfMessage = 14
}
