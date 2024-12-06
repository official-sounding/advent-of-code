using System.Collections;

public class Matrix : IEnumerable<KeyValuePair<(int, int), char>>
{
    private Dictionary<(int, int), char> _matrix = [];

    // from input, generates a matrix where (0,0) is the bottom left
    public static Matrix Parse(string[] input)
    {
        var result = new Matrix
        {
            _matrix = input.Reverse().SelectMany((l, y) => l.ToCharArray().Select((n, x) => (x, y, n))).ToDictionary((t) =>
        {
            var (x, y, _) = t;
            return (x, y);
        }, (t) => t.n)
        };

        return result;
    }

    public char this[(int, int) i]
    {
        get => _matrix[i];
        set => _matrix[i] = value;
    }

    public bool TryGetValue((int, int) position, out char value)
    {
        return _matrix.TryGetValue(position, out value);
    }

    public bool ContainsKey((int, int) position) => _matrix.ContainsKey(position);

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

}

public record Offset(int X, int Y)
{
    public static Offset N => new(0, 1);
    public static Offset S => new(0, -1);
    public static Offset E => new(1, 0);
    public static Offset W => new(-1, 0);
    public static Offset NE => new(1, 1);
    public static Offset SE => new(1, -1);
    public static Offset SW => new(-1, -1);
    public static Offset NW => new(-1, 1);

    public Offset Scale(int factor) => this with { X = X * factor, Y = Y * factor };
}