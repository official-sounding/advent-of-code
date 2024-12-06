[Slug("2024/d02")]
public class Day202402 : Problem
{
    private IEnumerable<List<int>> ExtractNumbers(string[] input) => input
         .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(c => Convert.ToInt32(c)).ToList());


    private bool SafeValue(int n, int prev, bool rowAsc)
    {
        var diff = prev - n;
        var pairAsc = diff > 0;
        if (diff == 0)
        {
            return false;
        }
        else if (pairAsc != rowAsc)
        {
            return false;
        }
        else if (Math.Abs(diff) > 3)
        {
            return false;
        }

        return true;
    }

    private bool ValidRow(List<int> row)
    {
        var prev = row[0];
        var rowAsc = row[0] - row[1] > 0;
        foreach (var n in row.Skip(1))
        {
            if (!SafeValue(n, prev, rowAsc))
            {
                return false;
            }

            prev = n;
        }
        return true;
    }

    public IEnumerable<List<int>> PermuteRow(List<int> row)
    {
        foreach (var idx in row.Select((_, i) => i))
        {
            var permutation = row.ToList();
            permutation.RemoveAt(idx);
            yield return permutation;
        }
    }


    public override long RunPartOne(string[] input)
    {
        var nums = ExtractNumbers(input);

        var result = nums.Where(ValidRow).Count();

        return result;
    }

    public override long RunPartTwo(string[] input)
    {
        var nums = ExtractNumbers(input);

        var result = nums.Where(row =>
        {
            if (ValidRow(row))
            {
                return true;
            }

            if (PermuteRow(row).Any(ValidRow))
            {
                return true;
            }


            return false;
        }).Count();

        return result;
    }
}