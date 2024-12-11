public static class StringExtensions
{
    public static IEnumerable<int> ToIntList(this string str, char separator = ' ')
    {
        return
            str.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(c => Convert.ToInt32(c));
    }

    public static IEnumerable<long> ToLongList(this string str, char separator = ' ')
    {
        return
            str.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(c => Convert.ToInt64(c));
    }
}