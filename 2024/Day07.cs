[Slug("2024/d07")]
public class Day202407 : Problem
{
    public override long RunPartOne(string[] input)
    {
        return ParseInput(input)
            .Where(IsValidEquation)
            .Sum(p => p.total);

    }

    public override long RunPartTwo(string[] input)
    {
        return ParseInput(input)
            .Where(IsValidEquationPartTwo)
            .Sum(p => p.total);
    }

    static IEnumerable<(long total, long[] nums)> ParseInput(string[] input)
    {
        return input
                   .Select(row => row.Split(":", StringSplitOptions.TrimEntries))
                   .Select(parts =>
                   {
                       var total = Convert.ToInt64(parts[0]);
                       var nums = parts[1].ToLongList().ToArray();
                       return (total, nums);
                   });
    }

    static bool IsValidEquation((long total, long[] nums) parts)
    {
        var (total, nums) = parts;

        return Apply(nums, [Add, Mult]).Any(candidate => candidate == total);
    }

    static bool IsValidEquationPartTwo((long total, long[] nums) parts)
    {
        var (total, nums) = parts;

        return Apply(nums, [Add, Mult, Concat]).Any(candidate => candidate == total);
    }

    static IEnumerable<long> Apply(long[] nums, Func<long, long, long>[] allOps)
    {
        List<long> allTotals = [];

        if (nums is [var head, .. var tail])
        {
            foreach (var op in allOps)
            {
                Apply(tail, head, allTotals, allOps, op);
            }
        }

        return allTotals;
    }


    static void Apply(long[] nums, long runningTotal, List<long> allTotals, Func<long, long, long>[] allOps, Func<long, long, long> op)
    {
        if (nums.Length == 0)
        {
            allTotals.Add(runningTotal);
            return;
        }

        if (nums is [var head, .. var tail])
        {
            var total = op(runningTotal, head);
            foreach (var rOp in allOps)
            {
                Apply(tail, total, allTotals, allOps, rOp);
            }
        }
    }


    static long Mult(long a, long b) => a * b;
    static long Add(long a, long b) => a + b;
    static long Concat(long a, long b) => Convert.ToInt64($"{a}{b}");

}