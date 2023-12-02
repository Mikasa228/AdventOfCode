namespace Common;

public abstract class Day
{
    protected string InputOne { get; set; }
    protected string InputTwo { get; set; }
    protected string TestInputOne { get; set; }
    protected string TestInputTwo { get; set; }
    protected abstract int TestSolutionOne { get; set; }
    protected abstract int TestSolutionTwo { get; set; }

    public Day()
    {
        InputOne = $"../../../{GetType().Name}/input1.txt";
        InputTwo = $"../../../{GetType().Name}/input2.txt";
        TestInputOne = $"../../../{GetType().Name}/testInput1.txt";
        TestInputTwo = $"../../../{GetType().Name}/testInput2.txt";
    }

    public object MainSolveOne()
    {
        return SolveOne(InputOne);
    }
    public object MainSolveTwo()
    {
        return SolveTwo(InputTwo);
    }

    protected abstract int SolveTwo(string input);

    public bool ValidateOne()
    {
        var valid = SolveOne(TestInputOne) == TestSolutionOne;
        if (valid)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Part I passes!");
            Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Part I fails :(");
            Console.ForegroundColor = ConsoleColor.White;
        }
        return valid;
    }

    public bool ValidateTwo()
    {

        var valid = SolveTwo(TestInputTwo) == TestSolutionTwo;
        if (valid)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Part II passes!");
            Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Part II fails :(");
            Console.ForegroundColor = ConsoleColor.White;
        }
        return valid;
    }

    protected abstract int SolveOne(string input);
}
