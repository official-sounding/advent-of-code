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

    public static IEnumerable<(T, T)> AllCombinations<T>(this IEnumerable<T> elements, bool includeIdentities = true, bool orderSensitive = false)
    {
        foreach (var e1 in elements)
        {
            foreach (var e2 in elements)
            {
                if (includeIdentities || !EqualityComparer<T>.Default.Equals(e1, e2))
                {
                    yield return (e1, e2);
                }
            }
        }
    }
}