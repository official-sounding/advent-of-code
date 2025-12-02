using System.Text.RegularExpressions;

[Slug(2025, 2)]
public partial class Day202502 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var line = input[0];

        var ranges = line.Split(",");

        return ranges.Sum(range =>
        {
            var (start, end)  = NumberUtils.SplitRangeString(range);
            return NumberUtils.InRange(start, end).Where(IsDuped).Sum();
        });
    }

    public override long RunPartTwo(string[] input)
    {
        var line = input[0];

        var ranges = line.Split(",");
        return ranges.Sum(range =>
        {
            var (start, end)  = NumberUtils.SplitRangeString(range);
            return NumberUtils.InRange(start, end).Where(IsRepeating).Sum();
        });

    }

    private bool IsDuped(long i)
    {
        return RepeatsOnce().IsMatch($"{i}");
    }

    private bool IsRepeating(long i)
    {
        
        var asStr = $"{i}";
        return RepeatingSeq().IsMatch(asStr);
    }

    [GeneratedRegex(@"^(\d+)\1$")]
    private static partial Regex RepeatsOnce();

    [GeneratedRegex(@"^(\d+)\1+$")]
    private static partial Regex RepeatingSeq();
}