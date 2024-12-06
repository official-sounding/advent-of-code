[Slug("2022/d12")]
public class Day202212 : SyncProblem
{
    public override string RunPartOneSync(string[] input)
    {
        int[,] heights = new int[input.Length, input[0].Length];
        var vertexCount = input.Length * input[0].Length;

        (int, int) source = (0, 0);
        (int, int) target = (0, 0);

        HashSet<(int, int)> aValues = new();

        foreach (var (line, row) in input.Select((l, idx) => (l, idx)))
        {
            foreach (var (ch, col) in line.Select((c, idx) => (c, idx)))
            {
                heights[row, col] = (int)ch;

                if (ch == 'E')
                {
                    target = (row, col);
                    heights[row, col] = (int)'z';
                }
                else if (ch == 'S')
                {
                    source = (row, col);
                    heights[row, col] = (int)'a';
                    aValues.Add((row, col));
                }
                else if (ch == 'a')
                {
                    aValues.Add((row, col));
                }
            }
        }

        var grid = new Grid(heights, input.Length, input[0].Length);

        var part1 = FindShortedPath(grid, source, target);

        return $"{part1}";
    }

    public override string RunPartTwoSync(string[] input)
    {
        int[,] heights = new int[input.Length, input[0].Length];
        var vertexCount = input.Length * input[0].Length;

        (int, int) target = (0, 0);

        HashSet<(int, int)> aValues = new();

        foreach (var (line, row) in input.Select((l, idx) => (l, idx)))
        {
            foreach (var (ch, col) in line.Select((c, idx) => (c, idx)))
            {
                heights[row, col] = (int)ch;

                if (ch == 'E')
                {
                    target = (row, col);
                    heights[row, col] = (int)'z';
                }
                else if (ch == 'S')
                {
                    heights[row, col] = (int)'a';
                    aValues.Add((row, col));
                }
                else if (ch == 'a')
                {
                    aValues.Add((row, col));
                }
            }
        }

        var grid = new Grid(heights, input.Length, input[0].Length);
        var part2 = aValues.Select(src => FindShortedPath(grid, src, target)).Min();

        return $"{part2}";


    }

    static int FindShortedPath(Grid grid, (int, int) source, (int, int) target)
    {
        Dictionary<(int, int), int> distances = new();
        Dictionary<(int, int), (int, int)> previous = new();
        PriorityQueue<(int, int), int> Q = new();

        distances[source] = 0;

        Q.Enqueue(source, 0);

        while (Q.TryDequeue(out var u, out var _) && u != target)
        {
            foreach (var v in grid.Neighbours(u))
            {
                var alt = distances[u] + 1;
                if (alt < distances.GetValueOrDefault(v, int.MaxValue))
                {
                    distances[v] = alt;
                    previous[v] = u;
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


    static IEnumerable<(int, int)> Path((int, int) target, Dictionary<(int, int), (int, int)> prev)
    {
        var node = target;
        while (prev.TryGetValue(node, out node))
        {
            yield return node;
        }
    }
}

record Grid(int[,] heights, int height, int width)
{
    bool ValidCell((int x, int y) cell) => cell.x >= 0 && cell.y >= 0 && cell.x < height && cell.y < width;

    public IEnumerable<(int, int)> Neighbours((int, int) u)
    {
        var (row, col) = u;
        var cellHeight = heights[row, col];
        foreach (var (x, y) in Offsets())
        {
            var newCell = (row + x, col + y);
            if (ValidCell(newCell) && heights[newCell.Item1, newCell.Item2] <= cellHeight + 1)
            {
                yield return newCell;
            }
        }
    }

    private static IEnumerable<(int x, int y)> Offsets()
    {
        yield return (0, -1);
        yield return (0, 1);
        yield return (1, 0);
        yield return (-1, 0);
    }
}