[Slug(2025, 09)]
public class Day202509 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var points = input.Select((s) => Position.FromString(s)).ToList();
        var squares = BuildSquares(points);

        return squares.First().Area;
    }

    public override long RunPartTwo(string[] input)
    {
        var points = input.Select((s) => Position.FromString(s)).ToList();
        var squares = BuildSquares(points);
        var edges = BuildEdges(points);

        Console.WriteLine(edges.Count);

        return squares.First((s) => TestSquare(s, edges)).Area;

    }

    private static IEnumerable<Square> BuildSquares(List<Position> points)
    {
        return points
            .AllPairs(false)
            .Select((p) => new Square(p.Item1, p.Item2, Area(p.Item1, p.Item2)))
            .OrderByDescending(s => s.Area);
    }

    private static long Area(Position a, Position b)
    {
        return (long)(Math.Max(a.X, b.X) - Math.Min(a.X, b.X) + 1) * (Math.Max(a.Y, b.Y) - Math.Min(a.Y, b.Y) + 1);
    }

    static bool TestSquare(Square s, HashSet<Position> edges)
    {
        var (s1, s2, _) = s;

        var minY = Math.Min(s1.Y, s2.Y);
        var maxY = Math.Max(s1.Y, s2.Y);
        var minX = Math.Min(s1.X, s2.X);
        var maxX = Math.Max(s1.X, s2.X);

        for (int x = minX + 1; x < maxX; x++)
        {
            if (edges.Contains(new Position(x, minY + 1)) || edges.Contains(new Position(x, maxY - 1)))
                return false;
        }
        for (int y = minY + 1; y < maxY; y++)
        {
            if (edges.Contains(new Position(minX + 1, y)) || edges.Contains(new Position(maxX - 1, y)))
                return false;
        }

        return true;
    }

    static HashSet<Position> BuildEdges(List<Position> points)
    {
        HashSet<Position> edges = [];
        foreach (var (p1, idx) in points.WithIndex())
        {
            var p2 = points[(idx + 1) % points.Count];

            if (p1.HorizontalInline(p2))
            {
                var (start, end) = p1.X > p2.X ? (p2, p1) : (p1, p2);
                while (start != end)
                {
                    edges.Add(start);
                    start += Position.E;
                }
                edges.Add(end);
            }

            if (p1.VerticalInline(p2))
            {
                var (start, end) = p1.Y > p2.Y ? (p2, p1) : (p1, p2);
                while (start != end)
                {
                    edges.Add(start);
                    start += Position.N;
                }
                edges.Add(end);
            }
        }
        return edges;
    }

    private record Square(Position A, Position B, long Area)
    {
        public List<Position> Corners => [A, B, A with { Y = B.Y }, A with { X = B.X }];
    }
}