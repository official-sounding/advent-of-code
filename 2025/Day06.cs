[Slug(2025, 06)]
public class Day202506 : Problem
{
    public override long RunPartOne(string[] input)
    {
        List<string[]> parts = [];

        foreach (var row in input)
        {
            parts.Add(row.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }

        var ops = parts[^1];
        parts.Reverse();

        var nums = parts.Skip(1).Select(n => n.Select(long.Parse).ToArray()).ToList();


        return ops.WithIndex().Sum((t) =>
        {
            var (op, idx) = t;
            var opFn = ParseOp(op);
            return nums.Skip(1).Select(n => n[idx]).Aggregate(nums[0][idx], opFn);
        });
    }

    public override long RunPartTwo(string[] input)
    {
        var columns = input
            .AsColumns()
            .Select(c => c.Trim())
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .ToArray();

        long res = 0;
        long pAns = 0;

        var op = ParseOp('+');

        foreach (var col in columns)
        {
            if (!char.IsDigit(col[^1]))
            {

                res += pAns;
                op = ParseOp(col[^1]);
                pAns = long.Parse(col[..^1]);
            }
            else
            {
                pAns = op(pAns, long.Parse(col));
            }
        }

        return res + pAns;
    }

    private static Func<long, long, long> ParseOp(string op) => ParseOp(op[0]);
    private static Func<long, long, long> ParseOp(char op)
    {
        return op switch
        {
            '+' => (prev, curr) => prev + curr,
            '*' => (prev, curr) => prev * curr,
            _ => throw new Exception($"{op}??")
        };
    }
}