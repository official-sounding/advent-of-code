using System.Collections;

public class NumMatrix : IEnumerable<KeyValuePair<Position, int>>
{
    private Dictionary<Position, int> _matrix = [];

    // from input, generates a matrix where (0,0) is the bottom left
    public static NumMatrix Parse(IEnumerable<string> input, string separator = " ")
    {
        var maxX = int.MinValue;
        var maxY = int.MinValue;
        var dict = input
                .Reverse()
                .SelectMany((l, y) => l.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select((n, x) => (x, y, n: int.Parse(n))))
                .ToDictionary((t) =>
                    {
                        var (x, y, _) = t;
                        maxX = Math.Max(maxX, x);
                        maxY = Math.Max(maxY, y);
                        return new Position(x, y);
                    }, (t) => t.n
                );

        return new NumMatrix
        {
            _matrix = dict,
            MaxX = maxX,
            MaxY = maxY
        };
    }

    public static NumMatrix FromDimensions(int maxX, int maxY, int c = 0)
    {
        var dict = new Dictionary<Position, int>();
        for (var x = 0; x <= maxX; x++)
        {
            for (var y = 0; y <= maxY; y++)
            {
                dict[(x, y)] = c;
            }
        }

        return new NumMatrix
        {
            _matrix = dict,
            MaxX = maxX,
            MaxY = maxY
        };
    }

    public int this[Position i]
    {
        get => _matrix[i];
        set => _matrix[i] = value;
    }

    public int MaxX { get; init; }
    public int MaxY { get; init; }

    public IEnumerable<int> XValues => Enumerable.Range(0, MaxX + 1);
    public IEnumerable<int> YValues => Enumerable.Range(0, MaxY + 1);

    public bool TryGetValue(Position position, out int value)
    {
        return _matrix.TryGetValue(position, out value);
    }

    public bool ValidPosition(Position position) => _matrix.ContainsKey(position);
    public bool Remove(Position position) => _matrix.Remove(position);

    public Position FindPosition(int x)
    {
        foreach (var (key, value) in _matrix)
        {
            if (value == x)
            {
                return key;
            }
        }

        return new(0, 0);
    }

    public IEnumerator<KeyValuePair<Position, int>> GetEnumerator()
    {
        return _matrix.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _matrix.GetEnumerator();
    }

    public void WriteToConsole()
    {
        var grid = _matrix
            .GroupBy((kv) => kv.Key.Y)
            .OrderByDescending(grp => grp.Key)
            .Select(grp => grp.OrderBy(k => k.Key.X).Select(kv => kv.Value).ToArray())
            .ToArray();


        foreach (var row in grid)
        {
            foreach (var col in row)
            {
                Console.Write(col);
            }
            Console.WriteLine();
        }
    }
}