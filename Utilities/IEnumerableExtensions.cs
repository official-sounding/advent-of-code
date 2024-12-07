public static class IEnumerableExtensions
{
    public static long GreatestCommonDenominator(long n1, long n2)
    {
        if (n2 == 0)
        {
            return n1;
        }
        else
        {
            return GreatestCommonDenominator(n2, n1 % n2);
        }
    }

    public static long LeastCommonMultiple(this IEnumerable<long> numbers)
    {
        return numbers.Aggregate((S, val) => S * val / GreatestCommonDenominator(S, val));
    }
}