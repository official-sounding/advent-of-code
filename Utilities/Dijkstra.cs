public static class Dijkstra
{
    public static int FindPathLength(Matrix matrix, Position source, Position target, Func<Matrix, Position, IEnumerable<(Position, int)>>? neighbors = null)
    {
        neighbors ??= Neighbors;
        Dictionary<Position, int> distances = [];
        PriorityQueue<Position, int> Q = new();

        distances[source] = 0;
        Q.Enqueue(source, 0);

        while (Q.TryDequeue(out var u, out _) && u != target)
        {
            foreach (var (v, cost) in neighbors(matrix, u))
            {
                var alt = distances[u] + cost;
                if (alt < distances.GetValueOrDefault(v, int.MaxValue))
                {
                    distances[v] = alt;
                    Q.Enqueue(v, alt);
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

    public static Dictionary<Position, int> FindAllDistances(Matrix matrix, Position source, Func<Matrix, Position, IEnumerable<(Position, int)>>? neighbors = null)
    {
        neighbors ??= Neighbors;
        Dictionary<Position, int> distances = [];
        PriorityQueue<Position, int> Q = new();

        distances[source] = 0;
        Q.Enqueue(source, 0);

        while (Q.TryDequeue(out var u, out _))
        {
            foreach (var (v, cost) in neighbors(matrix, u))
            {
                var alt = distances[u] + cost;
                if (alt < distances.GetValueOrDefault(v, int.MaxValue))
                {
                    distances[v] = alt;
                    Q.Enqueue(v, alt);
                }
            }
        }

        return distances;
    }

    public static IEnumerable<(Position v, int cost)> Neighbors(Matrix matrix, Position u)
    {
        foreach (var dir in Position.CardinalDirections())
        {
            var newCell = u + dir;
            if (matrix.TryGetValue(newCell, out var value) && value != '#')
            {
                yield return (newCell, 1);
            }
        }
    }
}