using System.Text.RegularExpressions;

const string inputPath = @"..\..\..\input.txt";

var stacksFirst = new List<Stack<char>>();
var stacksSecond = new List<Stack<char>>();

bool first = true;

using var reader = new StreamReader(inputPath);
while (!reader.EndOfStream)
{
    string? fullString = reader.ReadLine();
    if (string.IsNullOrEmpty(fullString)) break;

    // Initializing necessary amount of stacks
    if (first)
    {
        for (int _ = 0; _ <= fullString.Length / 4; _++)
        {
            stacksFirst.Add(new Stack<char>());
            stacksSecond.Add(new Stack<char>());
        }

        first = false;
    }

    for (int i = 1; i < fullString.Length; i += 4)
    {
        var index = (i - 1) / 4;

        if (char.IsLetter(fullString[i]))
            stacksFirst[index].Push(fullString[i]);
    }
}

// Reversing the stacks
for (int i = 0; i < stacksFirst.Count; i++)
{
    var reversedStack = stacksFirst[i];
    stacksFirst[i] = new(reversedStack);
    stacksSecond[i] = new(reversedStack);
}

while (!reader.EndOfStream)
{
    string? fullString = reader.ReadLine();
    if (fullString is null) throw new NullReferenceException();

    var list = Regex.Split(fullString, @"\D+");
    var amount = int.Parse(list[1]);
    var from = int.Parse(list[2]);
    var to = int.Parse(list[3]);

    // Solution for part one
    for (int i = 0; i < amount; i++) stacksFirst[to - 1].Push(stacksFirst[from - 1].Pop());

    // Solution for part two
    var temp = new Stack<char>();
    for (int i = 0; i < amount; i++) temp.Push(stacksSecond[from - 1].Pop());
    for (int i = 0; i < amount; i++) stacksSecond[to - 1].Push(temp.Pop());
}

var lettasFirst = stacksFirst.Select(stack => stack.Pop());
var lettasSecond = stacksSecond.Select(stack => stack.Pop());

var resultFirst = string.Concat(lettasFirst);
var resultSecond = string.Concat(lettasSecond);

Console.WriteLine($"Part one: {resultFirst}\nPart two: {resultSecond}");
