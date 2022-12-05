using System;
using System.Text;
using System.Text.RegularExpressions;

const string inputPath = @"..\..\..\input.txt";

StringBuilder result = new();
var stacks = new Dictionary<int, Stack<char>>();
bool first = true;

// 0 - 1
// 1 - 5
// 2 - 9
using var reader = new StreamReader(inputPath);
while (!reader.EndOfStream)
{
    string? fullString = reader.ReadLine();
    if (string.IsNullOrEmpty(fullString)) break;
    if (first)
    {
        for (int i = 0; i <= fullString.Length / 4; i++)
        {
            stacks[i] = new Stack<char>();
        }
        first = false;
    }

    for (int i = 1; i < fullString.Length; i+=4)
    {
        var index = (i - 1) / 4;
        if (char.IsLetter(fullString[i])) stacks[index].Push(fullString[i]);
    }
}
for (int i = 0; i < stacks.Count; i++)
{
    var reversed = stacks[i];
    stacks[i] = new();
    foreach (var item in reversed)
    {
        stacks[i].Push(item);
    }
}
while (!reader.EndOfStream)
{
    string? fullString = reader.ReadLine();
    if (fullString is null) throw new NullReferenceException();

    var list = Regex.Split(fullString, @"\D+");
    var amount = int.Parse(list[1]);
    var from = int.Parse(list[2]);
    var to = int.Parse(list[3]);
    var temp = new Stack<char>();

    for (int i = 0; i < amount; i++)
    {
        temp.Push(stacks[from - 1].Pop());
    }
    for (int i = 0; i < amount; i++)
    {
        stacks[to-1].Push(temp.Pop());
    }
}
for (int i = 0; i < stacks.Count; i++)
{
    result.Append(stacks[i].Pop());
}
Console.WriteLine(result);
