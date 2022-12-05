using Common;
using System.Text.RegularExpressions;

const string inputPath = @"..\..\..\input.txt";

var stacksFirst = new List<Stack<char>>();
var stacksSecond = new List<Stack<char>>();

bool isInitialized = false;

using var reader = new StreamReader(inputPath);
while (!reader.EndOfStream)
{
    string? fullString = reader.ReadLine();
    if (string.IsNullOrEmpty(fullString)) break;

    // Initializing necessary amount of stacks
    if (!isInitialized)
    {
        for (int _ = 0; _ <= fullString.Length / 4; _++)
        {
            stacksFirst.Add(new Stack<char>());
            stacksSecond.Add(new Stack<char>());
        }

        isInitialized = true;
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
    stacksSecond[i] = new(stacksFirst[i]);
    stacksFirst[i] = new(stacksFirst[i]);
}

while (!reader.EndOfStream)
{
    string? fullString = reader.ReadLine();
    if (fullString is null) throw new NullReferenceException();

    var instruction = Regex.Match(fullString, @"move (?'amount'\d+) from (?'from'\d+) to (?'to'\d+)");

    var amount = instruction.GetIntValue("amount");
    var from = instruction.GetIntValue("from") - 1;
    var to = instruction.GetIntValue("to") - 1;

    // Solution for part one
    for (int _ = 0; _ < amount; _++) stacksFirst[to].Push(stacksFirst[from].Pop());

    // Solution for part two
    var temp = new Stack<char>();
    for (int _ = 0; _ < amount; _++) temp.Push(stacksSecond[from].Pop());
    for (int _ = 0; _ < amount; _++) stacksSecond[to].Push(temp.Pop());
}

Console.WriteLine($"Part one: {GetAnswer(stacksFirst)}\nPart two: {GetAnswer(stacksSecond)}");

static string GetAnswer(List<Stack<char>> stacks) => string.Concat(stacks.Select(stack => stack.Pop()));
