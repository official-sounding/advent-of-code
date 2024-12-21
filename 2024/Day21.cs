using System.Text;

[Slug(2024, 21)]
public class Day202421 : Problem
{
    private static readonly KeyPad _numPad = new(Matrix.Parse(["789", "456", "123", "X0A"]));
    private static readonly KeyPad _dirPad = new(Matrix.Parse(["X^A", "<v>"]));

    public override long RunPartOne(string[] input)
    {
        var sum = 0L;

        foreach (var code in input)
        {
            var numeric = Convert.ToInt64(code[0..^1]);
            var count = NumPadMoves(code).Prepend('A').Pairwise((l, n) => DirPadMoves((l, n, 2))).Sum();
            sum += count * numeric;
        }

        return sum;
    }

    public override long RunPartTwo(string[] input)
    {
        var sum = 0L;

        foreach (var code in input)
        {
            var numeric = Convert.ToInt64(code[0..^1]);
            var count = NumPadMoves(code).Prepend('A').Pairwise((l, n) => DirPadMoves((l, n, 25))).Sum();
            sum += count * numeric;
        }

        return sum;
    }

    private string NumPadMoves(string code)
    {
        return string.Concat(code.Prepend('A').Pairwise(_numPad.Transition));
    }

    private long DirPadMoves((char lastPos, char newPos, int level) arg) => this.Memoized(arg, (a) =>
    {
        var (lastPos, newPos, level) = arg;
        var todo = _dirPad.Transition(lastPos, newPos);
        if (level == 1)
            return todo.Length;

        return todo.Prepend('A').Pairwise((l, n) => DirPadMoves((l, n, level - 1))).Sum();
    });

}

class KeyPad
{
    private readonly Matrix matrix;
    private static readonly (char, Position)[] _directions = "<v>^"
        .Select(c => (c, Position.FromArrow(c))).ToArray();

    public KeyPad(Matrix matrix)
    {
        this.matrix = matrix;
        matrix.Remove(matrix.FindPosition('X'));
    }

    public string Transition(char from, char to)
    {
        var sb = new StringBuilder();
        var target = matrix.FindPosition(to);
        var curPos = matrix.FindPosition(from);
        var delta = target - curPos;
        var d = 0;

        while (delta != (0, 0))
        {
            var (dirChar, dir) = _directions[d++ % _directions.Length];
            var magnitude = dir.X == 0 ? delta.Y / dir.Y : delta.X / dir.X;

            var dist = dir * magnitude;
            var dest = curPos + dist;

            if (magnitude <= 0 || !matrix.ValidPosition(dest))
                continue;

            curPos = dest;
            delta -= dist;
            sb.Append(dirChar, magnitude);
        }

        return sb.Append('A').ToString();
    }
}