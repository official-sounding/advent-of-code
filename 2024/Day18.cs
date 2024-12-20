[Slug(2024, 18)]
public class Day202418 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var matrix = Matrix.FromDimensions(70,70);
        foreach(var block in input.Take(1024).Select((s) => Position.FromString(s))) {
            matrix[block] = '#';
        }

        return Dijkstra.FindPathLength(matrix, (0,0), (70,70));
    }

    public override Task<string> RunPartTwoAsync(string[] input)
    {
        var matrix = Matrix.FromDimensions(70,70);
        foreach(var block in input.Take(1024).Select((s) => Position.FromString(s))) {
            matrix[block] = '#';
        }

        var blocker = new Position(-1,-1);
        for(var i = 1024; ;i++) {
            blocker = Position.FromString(input[i]);
            matrix[blocker] = '#';
            var dist = Dijkstra.FindPathLength(matrix, (0,0), (70,70));
            if(dist == int.MaxValue) {
                break;
            }
        }

        return Task.FromResult($"{blocker}");
    }

    static int FindShortestPath(Matrix matrix, Position source, Position target)
    {
        Dictionary<Position, int> distances = [];
        PriorityQueue<Position, int> Q = new();

        distances[source] = 0;
        Q.Enqueue(source, 0);

        while (Q.TryDequeue(out var u, out _) && u != target)
        {
            foreach (var v in Neighbors(matrix, u))
            {
                var alt = distances[u] + 1;
                if (alt < distances.GetValueOrDefault(v, int.MaxValue))
                {
                    distances[v] = alt;
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

    static IEnumerable<Position> Neighbors(Matrix matrix, Position u)
    {
        foreach (var dir in Position.CardinalDirections())
        {
            var newCell = u + dir;
            if (matrix.TryGetValue(newCell, out var value) && value != '#')
            {
                yield return newCell;
            }
        }
    }
}