const string inputPath = @"..\..\..\input.txt";

var result = 0;

using var reader = new StreamReader(inputPath);
while (!reader.EndOfStream)
{
    string? fullString = reader.ReadLine();
    if (fullString is null) throw new NullReferenceException();

    var buffer = new Queue<char>();
    for (int i = 0; i < fullString.Length; i++)
    {
        if (buffer.Count == 14)
        {
            buffer.Dequeue();

        }

        buffer.Enqueue(fullString[i]);
        if (buffer.Count == 14)
        {
            if (buffer.Distinct().ToList().Count == 14)
            {
                result = i + 1;
                break;
            }
        }
    }
}

Console.WriteLine(result); ;
