[Slug(2024, 16)]
public class Day202416 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var pos = matrix.FindPosition('S');
        var end = matrix.FindPosition('E');
        var (part1, _) = FindShortestPath(matrix, new(pos, Position.E, [pos]), end);
        return part1;
    }

    public override long RunPartTwo(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var pos = matrix.FindPosition('S');
        var end = matrix.FindPosition('E');
        var (_, part2) = FindShortestPath(matrix, new(pos, Position.E, [pos]), end);

        return part2;
    }

    static (int, int) FindShortestPath(Matrix matrix, MazeState source, Position target)
    {
        Dictionary<(Position pos, Position dir), int> distances = [];
        PriorityQueue<MazeState, int> Q = new();
        distances[source.state] = 0;
        Q.Enqueue(source, 0);


        HashSet<Position> allPaths = [];
        var shortestPathLength = 0;

        while (Q.TryDequeue(out var u, out var qCost))
        {
            if (u.pos == target)
            {
                if (shortestPathLength == 0)
                {
                    shortestPathLength = qCost;
                }

                if (qCost > shortestPathLength)
                {
                    break;
                }


                allPaths.UnionWith(u.paths);
            }

            if (distances.GetValueOrDefault(u.state, int.MaxValue) < qCost)
            {
                continue;
            }

            distances[u.state] = qCost;

            foreach (var (v, incCost) in Neighbors(matrix, u))
            {
                var alt = qCost + incCost;
                Q.Enqueue(v, alt);
            }
        }

        return (shortestPathLength, allPaths.Count);
    }

    static IEnumerable<(MazeState p, int cost)> Neighbors(Matrix matrix, MazeState u)
    {
        var (pos, dir, paths) = u;
        var possibles = new[] { (dir, 1), (dir.RotateLeft(), 1001), (dir.RotateRight(), 1001) };

        foreach (var (d, cost) in possibles)
        {
            if (matrix.TryGetValue(pos + d, out var c) && c != '#')
            {
                yield return (new(pos + d, d, [.. paths, pos + d]), cost);
            }
        }
    }
}

public record MazeState(Position pos, Position dir, HashSet<Position> paths)
{
    public (Position, Position) state => (pos, dir);
}