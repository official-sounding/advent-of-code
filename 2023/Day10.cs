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
            pos += dir.ToOffset();
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
        List<(Direction, string)> dirs = [
            (Direction.N, "F|7"),
            (Direction.S, "L|J"),
            (Direction.W, "F-L"),
            (Direction.E, "7-J")
        ];

        return dirs.First((o) =>
        {
            var (dir, valid) = o;
            return matrix.TryGetValue(start + dir.ToOffset(), out var candidate) && valid.Contains(candidate);
        }).Item1;
    }
}