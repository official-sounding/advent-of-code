[Slug("2024/d16")]
public class Day202416 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var pos = matrix.FindPosition('S');
        var end = matrix.FindPosition('E');

        return FindShortestPath(matrix, new(pos, Position.E), end);
    }

    static int FindShortestPath(Matrix matrix, MazeState source, Position target)
    {
        Dictionary<Position, int> distances = [];
        Dictionary<MazeState, MazeState> prevs = [];
        PriorityQueue<MazeState, int> Q = new();

        distances[source.pos] = 0;
        Q.Enqueue(source, 0);

        while (Q.TryDequeue(out var u, out _) && u.pos != target)
        {
            foreach (var (v, cost) in Neighbors(matrix, u))
            {
                var alt = distances[u.pos] + cost;
                if (alt < distances.GetValueOrDefault(v.pos, int.MaxValue))
                {
                    distances[v.pos] = alt;
                    prevs[v] = u;
                    if (!Q.UnorderedItems.Any((x) => x.Element == v))
                    {
                        Q.Enqueue(v, alt);
                    }
                }
            }
        }

        if (distances.TryGetValue(target, out var result))
        {
            return result;
        }
        else
        {
            return int.MaxValue;
        }
    }

    static IEnumerable<(MazeState p, int cost)> Neighbors(Matrix matrix, MazeState u)
    {
        var (pos, dir) = u;
        var possibles = new[] { (dir, 1), (dir.RotateLeft(), 1001), (dir.RotateRight(), 1001) };

        foreach (var (d, cost) in possibles)
        {
            if (matrix.TryGetValue(pos + d, out var c) && c != '#')
            {
                yield return (new(pos + d, d), cost);
            }
        }
    }
}

public record MazeState(Position pos, Position dir);