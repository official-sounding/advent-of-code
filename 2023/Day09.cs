[Slug(2023, 09)]
public class Day202309 : Problem
{
    public override long RunPartOne(string[] input)
    {
        return input.Select(PredictNextValue).Sum();
    }

    public override long RunPartTwo(string[] input)
    {
        return input.Select(PredictPrevValue).Sum();
    }

    private long PredictNextValue(string input)
    {
        var values = input.ToIntList().ToArray();
        var next = new List<int>() { values[^1] };

        while (values.Any(d => d != 0))
        {
            values = [.. values.Pairwise((a, b) => b - a)];
            next.Add(values[^1]);
        }

        return next.Sum();
    }

    private long PredictPrevValue(string input)
    {
        var values = input.ToIntList().ToArray();
        var next = new List<int>() { values[0] };

        while (values.Any(d => d != 0))
        {
            values = [.. values.Pairwise((a, b) => a - b)];
            next.Add(values[0]);
        }

        return next.Sum();
    }
}