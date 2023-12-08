namespace Common;

public static class Utils
{
    public static long GCD(long n1, long n2)
    {
        if (n2 == 0)
        {
            return n1;
        }
        else
        {
            return GCD(n2, n1 % n2);
        }
    }

    public static long LCM(List<long> numbers)
    {
        return numbers.Aggregate((S, val) => S * val / GCD(S, val));
    }
}
