const string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
const string inputPath = @"..\..\..\input.txt";

int totalOne = 0;
int totalTwo = 0;

List<string> sublist = new();

using var reader = new StreamReader(inputPath);
while (!reader.EndOfStream)
{
    string? fullString = reader.ReadLine();
    if (fullString == null) throw new NullReferenceException();

    // Solution for the first part
    int middle = fullString.Length / 2;
    string firstHalf = fullString[..middle];
    string secondHalf = fullString[middle..];

    char item = firstHalf.ToList().Find(
        letter => secondHalf.Contains(letter));

    totalOne += ConvertToPriority(item);

    // Solution for the second part
    sublist.Add(fullString);
    if (sublist.Count < 3) continue;

    char badge = sublist[0].ToList().Find(
        letter => sublist[1].Contains(letter) && sublist[2].Contains(letter));

    totalTwo += ConvertToPriority(badge);
    sublist.Clear();
}

Console.WriteLine($"Part one: {totalOne}\nPart two: {totalTwo}");

static int ConvertToPriority(char letter) => letters.IndexOf(letter) + 1;
