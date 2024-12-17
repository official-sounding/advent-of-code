[Slug(2024, 01)]
public class Day202401 : Problem
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

    public override long RunPartOne(string[] input)
    {
        var (left, right) = ExtractNumbers(input);


        left = left.Order().ToList();
        right = right.Order().ToList();

        return left.Select((l, i) => Math.Abs(l - right[i])).Sum();
    }

    public override long RunPartTwo(string[] input)
    {
        var (left, right) = ExtractNumbers(input);

        var rDict = right.GroupBy(r => r).ToDictionary(grp => grp.Key, grp => grp.Count());

        return left.Sum((l) =>
            l * rDict.GetValueOrDefault(l, 0)
        );
    }
}