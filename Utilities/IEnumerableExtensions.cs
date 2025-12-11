using System.Text;

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

    public static HashSet<(T, T)> AllPairs<T>(this IEnumerable<T> elements, bool includeIdentities = true)
    {
        var result = new HashSet<(T, T)>();

        foreach (var e1 in elements)
        {
            foreach (var e2 in elements)
            {
                if (includeIdentities || !EqualityComparer<T>.Default.Equals(e1, e2))
                {
                    if (!result.Contains((e1, e2)) && !result.Contains((e2, e1)))
                    {
                        result.Add((e1, e2));
                    }
                }
            }
        }

        return result;
    }

    public static IEnumerable<(T value, int idx)> WithIndex<T>(this IEnumerable<T> elements)
    {
        return elements.Select((n, idx) => (n, idx));
    }

    public static IEnumerable<TRes> Pairwise<T, TRes>(this IEnumerable<T> items, Func<T, T, TRes> selector)
    {
        using var it = items.GetEnumerator();
        if (!it.MoveNext())
            yield break;
        T last = it.Current;

        while (it.MoveNext())
        {
            T cur = it.Current;
            yield return selector(last, cur);
            last = cur;
        }
    }

    public static IEnumerable<string> AsColumns(this IEnumerable<string> rows)
    {
        int numColumns = rows.Max(x => x.Length);

        var res = new string[numColumns];
        for (int i = 0; i < numColumns; i++)
        {
            StringBuilder sb = new();
            foreach (var row in rows)
            {
                try
                {
                    sb.Append(row[i]);
                }
                catch (IndexOutOfRangeException)
                {
                    sb.Append(' ');
                }
            }
            res[i] = sb.ToString();
        }
        return res;
    }

    public static IEnumerable<T> CountIterations<T>(this IEnumerable<T> e, bool print)
    {
        foreach (var (item, idx) in e.WithIndex())
        {
            if (print)
            {
                if (idx % 1000 == 0)
                {
                    Console.WriteLine("!");
                }
                else if (idx % 100 == 0)
                {
                    Console.Write("+");
                }
                else if (idx % 10 == 0)
                {
                    Console.Write(".");
                }
            }
            yield return item;
        }
        if (print)
        {
            Console.WriteLine();
        }
    }
}