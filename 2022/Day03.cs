[Slug("2022/d03")]
public class Day202203 : SyncProblem
{
    public override string RunPartOneSync(string[] input)
    {
        var result = input.Select(x => SplitLine(x)).Select((halves) =>
{
    var (first, second) = halves;
    return first.Intersect(second).First();
}).Select(Score).Sum();

        return $"{result}";
    }

    public override string RunPartTwoSync(string[] input)
    {
        var result = input
        .Select(x => x.ToCharArray())
        .Chunk(3)
        .Select(x => x.Skip(1).Aggregate(new HashSet<char>(x.First()), (h, e) => { h.IntersectWith(e); return h; }).First())
        .Select(ScorePart2)
        .Sum()
        ;

        return $"{result}";
    }

    static (char[], char[]) SplitLine(string x)
    {
        var midpoint = x.Length / 2;
        return (x.ToCharArray().Take(midpoint).ToArray(), x.ToCharArray().Skip(midpoint).ToArray());
    }

    static int Score(char x)
    {
        int ascii = (int)x;
        if (ascii >= 65 && ascii <= 90)
        {
            return ascii - 38;
        }
        else
        {
            return ascii - 96;
        }
    }

    static int ScorePart2(char x)
    {
        int ascii = (int)x;
        if (ascii <= 90)
        {
            return ascii - 38;
        }
        else
        {
            return ascii - 96;
        }
    }
}