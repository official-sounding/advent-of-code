using System.Collections;

public class Matrix : IEnumerable<KeyValuePair<Position, char>>
{
    private Dictionary<Position, char> _matrix = [];

    // from input, generates a matrix where (0,0) is the bottom left
    public static Matrix Parse(string[] input)
    {
        var result = new Matrix
        {
            _matrix = input
                .Reverse()
                .SelectMany((l, y) => l.ToCharArray().Select((n, x) => (x, y, n)))
                .ToDictionary((t) =>
                    {
                        var (x, y, _) = t;
                        return new Position(x, y);
                    }, (t) => t.n
                )
        };

        return result;
    }

    public char this[Position i]
    {
        get => _matrix[i];
        set => _matrix[i] = value;
    }

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
}

public record Position(int X, int Y)
{
    public static Position Nil => new(0, 0);
    public static Position N => new(0, 1);
    public static Position S => new(0, -1);
    public static Position E => new(1, 0);
    public static Position W => new(-1, 0);
    public static Position NE => new(1, 1);
    public static Position SE => new(1, -1);
    public static Position SW => new(-1, -1);
    public static Position NW => new(-1, 1);

    public Position Scale(int factor) => this with { X = X * factor, Y = Y * factor };

    public (int, int) Tuple => (X, Y);

    public static Position operator -(Position me, Position other) => new(Y: me.Y - other.Y, X: me.X - other.X);
    public static Position operator +(Position me, Position other) => new(Y: me.Y + other.Y, X: me.X + other.X);

    public static implicit operator Position((int, int) b) => FromTuple(b);

    public static Position FromTuple((int, int) b) => new(b.Item1, b.Item2);

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

    public override string ToString() => $"({X},{Y})";
}

public enum Direction
{
    N = 0,
    NE = 1,
    E = 2,
    SE = 3,
    S = 4,
    SW = 5,
    W = 6,
    NW = 7
}

public static class DirectionExtensions
{
    public static Direction RotateLeft90(this Direction dir)
    {
        return (Direction)(((int)dir + 2) % 8);
    }

    public static Direction RotateRight90(this Direction dir)
    {
        return (Direction)(((int)dir + 8 - 2) % 8);
    }

    public static Direction RotateLeft45(this Direction dir)
    {
        return (Direction)(((int)dir + 1) % 8);
    }

    public static Direction RotateRight45(this Direction dir)
    {
        return (Direction)(((int)dir + 8 - 1) % 8);
    }

    public static Position ToOffset(this Direction dir)
    {
        return dir switch
        {
            Direction.N => Position.N,
            Direction.NE => Position.NE,
            Direction.E => Position.E,
            Direction.SE => Position.SE,
            Direction.S => Position.S,
            Direction.SW => Position.SW,
            Direction.W => Position.W,
            Direction.NW => Position.NW,
            _ => throw new Exception($"{dir} invalid")
        };
    }
}