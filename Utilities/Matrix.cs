using System.Collections;

public class Matrix : IEnumerable<KeyValuePair<(int, int), char>>
{
    private Dictionary<(int, int), char> _matrix = [];

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
                        return (x, y);
                    }, (t) => t.n
                )
        };

        return result;
    }

    public char this[(int, int) i]
    {
        get => _matrix[i];
        set => _matrix[i] = value;
    }

    public char this[Position i]
    {
        get => _matrix[(i.X, i.Y)];
        set => _matrix[(i.X, i.Y)] = value;
    }

    public bool TryGetValue((int, int) position, out char value)
    {
        return _matrix.TryGetValue(position, out value);
    }

    public bool TryGetValue(Position position, out char value)
    {
        return TryGetValue((position.X, position.Y), out value);
    }

    public bool ValidPosition((int, int) position) => _matrix.ContainsKey(position);

    public Position FindPosition(char x)
    {
        foreach (var (key, value) in _matrix)
        {
            if (value == x)
            {
                return new(key.Item1, key.Item2);
            }
        }

        return new(0, 0);
    }

    public IEnumerator<KeyValuePair<(int, int), char>> GetEnumerator()
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
    Position ApplyOffset(Offset o) => this with { X = X + o.X, Y = Y + o.Y };
    public Position ApplyOffset(Offset o, int scale = 1)
    {
        var scaled = o.Scale(scale);
        return ApplyOffset(scaled);
    }
}

public record Offset(int X, int Y)
{
    public static Offset Nil => new(0, 0);
    public static Offset N => new(0, 1);
    public static Offset S => new(0, -1);
    public static Offset E => new(1, 0);
    public static Offset W => new(-1, 0);
    public static Offset NE => new(1, 1);
    public static Offset SE => new(1, -1);
    public static Offset SW => new(-1, -1);
    public static Offset NW => new(-1, 1);

    public Offset Scale(int factor) => this with { X = X * factor, Y = Y * factor };

    public static IEnumerable<Offset> AllDirections()
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

    public static Offset ToOffset(this Direction dir)
    {
        return dir switch
        {
            Direction.N => Offset.N,
            Direction.NE => Offset.NE,
            Direction.E => Offset.E,
            Direction.SE => Offset.SE,
            Direction.S => Offset.S,
            Direction.SW => Offset.SW,
            Direction.W => Offset.W,
            Direction.NW => Offset.NW,
            _ => throw new Exception($"{dir} invalid")
        };
    }
}