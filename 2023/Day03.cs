using System.Text;
using System.Text.RegularExpressions;

[Slug(2023, 03)]
public class Day03 : Problem
{
    private static HashSet<char> numbers = new("0987654321".ToCharArray());

    public override long RunPartOne(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var sum = 0L;

        foreach (var (pos, c) in matrix)
        {
            if (!numbers.Contains(c) && c != '.')
            {
                sum += AdjacentNumbers(matrix, pos);
            }
        }

        return sum;
    }

    public override long RunPartTwo(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var sum = 0L;

        foreach (var (pos, c) in matrix)
        {
            if (c == '*')
            {
                sum += GearRatio(matrix, pos);
            }
        }

        return sum;
    }

    long AdjacentNumbers(Matrix matrix, Position pos)
    {
        var sum = 0L;
        foreach (var dir in Position.AllDirections())
        {
            var p = pos + dir;
            var sb = new StringBuilder();
            if (matrix.TryGetValue(p, out var c) && numbers.Contains(c))
            {
                matrix[p] = '.';
                sb.Append(Consume(matrix, p, Position.W, []).Reverse().ToArray());
                sb.Append(c);
                sb.Append(Consume(matrix, p, Position.E, []));
                sum += Convert.ToInt32(sb.ToString());
            }
        }

        return sum;
    }

    long GearRatio(Matrix matrix, Position pos)
    {
        var ratio = 1L;
        var count = 0;

        foreach (var dir in Position.AllDirections())
        {
            var p = pos + dir;
            var sb = new StringBuilder();
            if (matrix.TryGetValue(p, out var c) && numbers.Contains(c))
            {
                matrix[p] = '.';
                sb.Append(Consume(matrix, p, Position.W, []).Reverse().ToArray());
                sb.Append(c);
                sb.Append(Consume(matrix, p, Position.E, []));
                ratio *= Convert.ToInt32(sb.ToString());
                count++;
            }
        }

        return count == 2 ? ratio : 0;
    }

    char[] Consume(Matrix matrix, Position pos, Position dir, char[] values)
    {
        if (!matrix.TryGetValue(pos + dir, out var c) || !numbers.Contains(c))
        {
            return values;
        }

        matrix[pos + dir] = '.';

        return Consume(matrix, pos + dir, dir, [.. values, c]);
    }
}