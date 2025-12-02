public class NumberUtils
{
    public static int Mod(int value, int mod)
    {
        var rem = value % mod;
        return rem < 0 ? rem + mod : rem;
    }

    public static long Mod(long value, long mod)
    {
        var rem = value % mod;
        return rem < 0 ? rem + mod : rem;
    }

    public static IEnumerable<long> InRange(long start, long end)
    {
        var i = start -1;
        while (i++ < end)
        {
            yield return i;
        }
    }

    public static (long, long) SplitRangeString(string rangeStr)
    {
        var split = rangeStr.ToLongList('-').ToArray();
        return (split[0], split[1]);
    }
}