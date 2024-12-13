[Slug("2022/d12")]
public class Day202212 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var source = Position.Nil;
        var target = Position.Nil;

        foreach (var (pos, value) in matrix)
        {
            if (value == 'E')
            {
                matrix[pos] = 'z';
                target = pos;
            }
            else if (value == 'S')
            {
                matrix[pos] = 'a';
                source = pos;
            }
        }

        return FindShortestPath(matrix, source, target);
    }

    public override long RunPartTwo(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var target = Position.Nil;
        HashSet<Position> starts = [];

        foreach (var (pos, value) in matrix)
        {
            if (value == 'E')
            {
                matrix[pos] = 'z';
                target = pos;
            }
            else if (value == 'S')
            {
                matrix[pos] = 'a';
                starts.Add(pos);
            }
            else if (value == 'a')
            {
                starts.Add(pos);
            }
        }

        return starts.Select(src => FindShortestPath(matrix, src, target)).Min();
    }

    static int FindShortestPath(Matrix matrix, Position source, Position target)
    {
        Dictionary<Position, int> distances = [];
        Dictionary<Position, Position> prevs = [];
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

    static IEnumerable<Position> Neighbors(Matrix matrix, Position u)
    {
        if (!matrix.TryGetValue(u, out var cellHeight))
        {
            yield break;
        }

        foreach (var dir in Position.CardinalDirections())
        {
            var newCell = u + dir;
            if (matrix.TryGetValue(newCell, out var nextHeight) && nextHeight <= cellHeight + 1)
            {
                yield return newCell;
            }
        }
    }
}
