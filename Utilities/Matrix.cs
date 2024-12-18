using System.Collections;

public class Matrix : IEnumerable<KeyValuePair<Position, char>>
{
    private Dictionary<Position, char> _matrix = [];

    // from input, generates a matrix where (0,0) is the bottom left
    public static Matrix Parse(IEnumerable<string> input)
    {
        var maxX = int.MinValue;
        var maxY = int.MinValue;
        var dict = input
                .Reverse()
                .SelectMany((l, y) => l.ToCharArray().Select((n, x) => (x, y, n)))
                .ToDictionary((t) =>
                    {
                        var (x, y, _) = t;
                        maxX = Math.Max(maxX, x);
                        maxY = Math.Max(maxY, y);
                        return new Position(x, y);
                    }, (t) => t.n
                );

        return new Matrix
        {
            _matrix = dict,
            MaxX = maxX,
            MaxY = maxY
        };
    }

    public static Matrix FromDimensions(int maxX, int maxY, char c = '.') {
        var dict = new Dictionary<Position, char>();
        for (var x = 0; x <= maxX; x++) {
            for(var y = 0; y <= maxY; y++) {
                dict[(x,y)] = c;
            }
        }

        return new Matrix {
            _matrix = dict,
            MaxX = maxX,
            MaxY = maxY
        };
    }

    public char this[Position i]
    {
        get => _matrix[i];
        set => _matrix[i] = value;
    }

    public int MaxX { get; init; }
    public int MaxY { get; init; }

    public bool TryGetValue(Position position, out char value)
    {
        return _matrix.TryGetValue(position, out value);
    }

    public bool ValidPosition(Position position) => _matrix.ContainsKey(position);

    public Position FindPosition(char x)
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

    public IEnumerator<KeyValuePair<Position, char>> GetEnumerator()
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

public record Position(int X, int Y)
{
    public bool IsUnitVector => (Math.Abs(X) == 1 && Y == 0) || (X == 0 && Math.Abs(Y) == 1);
    public (int, int) Tuple => (X, Y);

    public Position Scale(int factor) => this with { X = X * factor, Y = Y * factor };

    public Position RotateLeft(int quadrant = 1)
    {
        if (!IsUnitVector)
        {
            throw new Exception("Cannot Rotate non-unit vector");
        }

        var dirs = CardinalDirections().ToArray();
        var start = Array.IndexOf(dirs, this);

        return dirs[(start + quadrant) % 4];
    }

    public Position RotateRight(int quadrant = 1)
    {
        if (!IsUnitVector)
        {
            throw new Exception("Cannot Rotate non-unit vector");
        }
        var dirs = CardinalDirections().ToArray();
        var start = Array.IndexOf(dirs, this);

        return dirs[(start - quadrant + 4) % 4];
    }

    public override string ToString() => $"({X},{Y})";

    public static Position operator -(Position me, Position other) => new(Y: me.Y - other.Y, X: me.X - other.X);
    public static Position operator +(Position me, Position other) => new(Y: me.Y + other.Y, X: me.X + other.X);

    public static implicit operator Position((int, int) b) => FromTuple(b);

    public static Position FromTuple((int, int) b) => new(b.Item1, b.Item2);
    public static Position FromString(string str, string separator = ",")
    {
        if (str.Split(separator) is [var xStr, var yStr] && int.TryParse(xStr, out var x) && int.TryParse(yStr, out var y))
        {
            return new(x, y);
        }

        throw new ArgumentException(str);
    }

    public static IEnumerable<Position> AllDirections()
    {
        yield return N;
        yield return NE;
        yield return E;
        yield return SE;
        yield return S;
        yield return SW;
        yield return W;
        yield return NW;
    }

    public static IEnumerable<Position> CardinalDirections()
    {
        yield return N;
        yield return E;
        yield return S;
        yield return W;
    }

    public static Position Nil => new(0, 0);
    public static Position N => new(0, 1);
    public static Position S => new(0, -1);
    public static Position E => new(1, 0);
    public static Position W => new(-1, 0);
    public static Position NE => new(1, 1);
    public static Position SE => new(1, -1);
    public static Position SW => new(-1, -1);
    public static Position NW => new(-1, 1);
}