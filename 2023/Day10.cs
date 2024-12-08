using System.Collections;

[Slug("2023/d10")]


public class Day202310 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var start = matrix.FindPosition('S');

        return WalkLoop(matrix, start).Count / 2;
    }

    public override long RunPartTwo(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var start = matrix.FindPosition('S');
        var positions = WalkLoop(matrix, start);

        var halfB = positions.Count / 2;
        var A = ShoelaceArea(matrix, positions);

        // Pick's Theorem: https://en.wikipedia.org/wiki/Pick%27s_theorem
        // i = A - (b/2) + 1;
        return A - halfB + 1;
    }

    static List<Position> WalkLoop(Matrix matrix, Position start)
    {
        var dir = FindInitialDirection(matrix, start);
        var pos = start;
        var value = 'x';
        List<Position> steps = [];

        while (value != 'S')
        {
            pos = pos.ApplyOffset(dir.ToOffset());
            steps.Add(pos);
            value = matrix[pos];
            dir = CheckDirectionChange(value, dir);
        }

        return steps;
    }

    static int ShoelaceArea(Matrix m, List<Position> positions)
    {
        var vertices = positions.Where(p => !"-|".Contains(m[p])).ToList();
        var count = vertices.Count;
        var area = 0;
        for (var i = 0; i < count; i++)
        {
            var nextIndex = (i + 1) % count;
            var (x, y) = vertices[i];
            var (x1, y1) = vertices[nextIndex];

            area += x * y1 - y * x1;
        }
        return Math.Abs(area) / 2;
    }

    static Direction CheckDirectionChange(char value, Direction dir)
    {
        return value switch
        {
            'F' => dir == Direction.N ? Direction.E : Direction.S,
            '7' => dir == Direction.N ? Direction.W : Direction.S,
            'L' => dir == Direction.S ? Direction.E : Direction.N,
            'J' => dir == Direction.S ? Direction.W : Direction.N,
            _ => dir
        };
    }

    static Direction FindInitialDirection(Matrix matrix, Position start)
    {
        var nValid = "F|7";
        var sValid = "L|J";
        var wValid = "F-L";
        var eValid = "7-J";
        char candidate;
        if (matrix.TryGetValue(start.ApplyOffset(Offset.N), out candidate) && nValid.Contains(candidate))
        {
            return Direction.N;
        }

        if (matrix.TryGetValue(start.ApplyOffset(Offset.S), out candidate) && sValid.Contains(candidate))
        {
            return Direction.S;
        }

        if (matrix.TryGetValue(start.ApplyOffset(Offset.E), out candidate) && eValid.Contains(candidate))
        {
            return Direction.E;
        }

        if (matrix.TryGetValue(start.ApplyOffset(Offset.W), out candidate) && wValid.Contains(candidate))
        {
            return Direction.W;
        }

        throw new Exception("invalid puzzle");
    }
}