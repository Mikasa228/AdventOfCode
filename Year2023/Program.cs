using Common;
using System.Diagnostics;
using Year2023;

var timer = new Stopwatch();
timer.Start();

Day currentDay = new Day02();

if (!Directory.Exists($"../../../{currentDay.GetType().Name}"))
{
    PrepScript.Run("2023", currentDay.GetType().Name[^2..]);
}

try
{
    if (currentDay.ValidateOne())
    {
        Console.WriteLine("Solution: " + currentDay.MainSolveOne());
        Console.WriteLine();
    }
    if (currentDay.ValidateTwo())
    {
        Console.WriteLine("Solution: " + currentDay.MainSolveTwo());
        Console.WriteLine();
    }
}
catch (NotImplementedException)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Solution is not implemented yet...");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine();
}


timer.Stop();
Console.WriteLine("Runtime: " + timer.ElapsedMilliseconds + "ms");
