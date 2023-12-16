using Common;
using System.Diagnostics;
using Year2023;

var timer = new Stopwatch();
timer.Start();

Day currentDay = new Day16();

if (!Directory.Exists($"../../../{currentDay.GetType().Name}"))
{
    PrepScript.Run("2023", currentDay.GetType().Name[^2..]);
}

try
{
    if (currentDay.ValidateOne()) Console.WriteLine("Solution: " + currentDay.MainSolveOne() + "\n");
    if (currentDay.ValidateTwo()) Console.WriteLine("Solution: " + currentDay.MainSolveTwo() + "\n");
}
catch (NotImplementedException)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Solution is not implemented yet...\n");
    Console.ForegroundColor = ConsoleColor.White;
}

timer.Stop();
Console.WriteLine($"Runtime: {timer.ElapsedMilliseconds / 1000}s {timer.ElapsedMilliseconds % 1000}ms");
