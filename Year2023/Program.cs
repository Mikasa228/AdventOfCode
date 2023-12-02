using Common;
using System.Diagnostics;
using Year2023;

var timer = new Stopwatch();
timer.Start();

Day currentDay = new Day01();
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

timer.Stop();
Console.WriteLine("Runtime: " + timer.ElapsedMilliseconds + "ms");
