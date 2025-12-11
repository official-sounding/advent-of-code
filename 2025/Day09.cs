[Slug(2025, 09)]
public class Day202509 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var points = input.Select((s) => Position.FromString(s)).ToList();

        var area = long.MinValue;

        for (int i = 0; i < points.Count; i++)
        {
            for (var j = i + 1; j < points.Count; j++)
            {
                var a = Area(points[i], points[j]);
                if (a > area)
                {
                    area = a;
                }
            }
        }

        return area;
    }

    private static long Area(Position a, Position b)
    {
        return (long)(Math.Max(a.X, b.X) - Math.Min(a.X, b.X) + 1) * (Math.Max(a.Y, b.Y) - Math.Min(a.Y, b.Y) + 1);
    }
}