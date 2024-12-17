using System.Collections;

[Slug(2023, 10)]
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
            pos += dir;
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

    static Position CheckDirectionChange(char value, Position dir)
    {
        return value switch
        {
            'F' => dir == Position.N ? Position.E : Position.S,
            '7' => dir == Position.N ? Position.W : Position.S,
            'L' => dir == Position.S ? Position.E : Position.N,
            'J' => dir == Position.S ? Position.W : Position.N,
            _ => dir
        };
    }

    static Position FindInitialDirection(Matrix matrix, Position start)
    {
        List<(Position, string)> dirs = [
            (Position.N, "F|7"),
            (Position.S, "L|J"),
            (Position.W, "F-L"),
            (Position.E, "7-J")
        ];

        return dirs.First((o) =>
        {
            var (dir, valid) = o;
            return matrix.TryGetValue(start + dir, out var candidate) && valid.Contains(candidate);
        }).Item1;
    }
}