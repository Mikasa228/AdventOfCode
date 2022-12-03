var inputPath = @"..\..\..\input.txt";

List<int> sums = new();
int temp = 0;

using var reader = new StreamReader(inputPath);
while (!reader.EndOfStream)
{
    var inputString = reader.ReadLine();
    if (string.IsNullOrEmpty(inputString))
    {
        sums.Add(temp);
        temp = 0;
    }
    else temp += int.Parse(inputString);
}

var ordered = sums.OrderByDescending(x => x).ToList();
Console.WriteLine($"Top-1: {ordered[0]}\nTop-3: {ordered.GetRange(0, 3).Sum()}");
