[Slug(2024, 19)]
public class Day202419 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var towels = input[0].Split(",", StringSplitOptions.TrimEntries);

        var achievable = 0;
        foreach (var pattern in input.Skip(2))
        {
            if (AllValidCombinations((pattern, towels)) > 0)
            {
                achievable++;
            }
        }

        return achievable;
    }

    public override long RunPartTwo(string[] input)
    {
        var towels = input[0].Split(",", StringSplitOptions.TrimEntries);

        var achievable = 0L;
        foreach (var pattern in input.Skip(2))
        {
            var patterns = AllValidCombinations((pattern, towels));
            achievable += patterns;
        }

        return achievable;
    }

    long AllValidCombinations((string combination, string[] towels) arg) => this.Memoized(arg, WrappedAllCombos);
    long WrappedAllCombos((string, string[]) arg)
    {
        var (remaining, towels) = arg;
        if (remaining.Length == 0)
        {
            return 1;
        }
        var count = 0L;
        foreach (var towel in towels)
        {
            if (remaining.StartsWith(towel))
            {
                count += AllValidCombinations((remaining[towel.Length..], towels));
            }
        }

        return count;
    }
}