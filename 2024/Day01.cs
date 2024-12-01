[Slug("2024/d01")]
public class Day202401 : SyncProblem
{
    private (List<int>, List<int>) ExtractNumbers(string[] input) => input
         .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
         .Aggregate<string[], (List<int>, List<int>)>(([], []), (agg, parts) =>
         {
             var (a, b) = agg;
             a.Add(Convert.ToInt32(parts[0]));
             b.Add(Convert.ToInt32(parts[1]));

             return (a, b);
         });

    public override string RunPartOneSync(string[] input)
    {
        var (left, right) = ExtractNumbers(input);


        left = left.Order().ToList();
        right = right.Order().ToList();

        var result = left.Select((l, i) => Math.Abs(l - right[i])).Sum();

        return $"{result}";
    }

    public override string RunPartTwoSync(string[] input)
    {
        var (left, right) = ExtractNumbers(input);

        var rDict = right.GroupBy(r => r).ToDictionary(grp => grp.Key, grp => grp.Count());

        var result = left.Sum((l) =>
            l * rDict.GetValueOrDefault(l, 0)
        );

        return $"{result}";
    }
}