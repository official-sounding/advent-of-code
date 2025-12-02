[Slug(2025, 2)]
public class Day202502 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var line = input[0];

        var ranges = line.Split(",");

        var total = 0L;
        foreach (var range in ranges)
        {
            var split = range.ToLongList('-').ToArray();
            var i = split[0] - 1;
            var max = split[1];


            while (i++ < max)
            {
                if (IsDuped(i))
                {
                    total += i;
                }
            }
        }

        return total;
    }

    private bool IsDuped(long i)
    {
        var asStr = $"{i}";
        if (asStr.Length % 2 != 0)
        {
            return false;
        }

        var start = asStr[..(asStr.Length / 2)];
        var end = asStr[(asStr.Length / 2)..];

        return start == end;
    }
}