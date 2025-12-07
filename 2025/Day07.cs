[Slug(2025, 7)]
public class Day202507 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var start = matrix.FindPosition('S');

        return RunBeams(matrix, start);
    }

    public override long RunPartTwo(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var start = matrix.FindPosition('S');

        RunBeams(matrix, start);
        return CountBeams((start, matrix));
    }

    private static long RunBeams(Matrix matrix, Position start)
    {
        matrix[start] = '|';

        var splits = 0;
        HashSet<Position> beams = [start];

        while (matrix.ValidPosition(beams.First() + Position.S))
        {
            HashSet<Position> newBeams = [];

            foreach (var beam in beams)
            {
                var next = beam + Position.S;
                if (matrix[next] == '.')
                {
                    matrix[next] = '|';
                    newBeams.Add(next);
                }
                else if (matrix[next] == '^')
                {
                    var left = next + Position.E;
                    var right = next + Position.W;

                    splits++;
                    matrix[left] = '|';
                    newBeams.Add(left);

                    matrix[right] = '|';
                    newBeams.Add(right);
                }
            }

            beams = newBeams;
        }
        return splits;
    }

    private long CountBeams((Position p, Matrix m) arg) => this.Memoized(arg, WrappedCount);

    private long WrappedCount((Position, Matrix) arg)
    {
        var (p, matrix) = arg;

        if (!matrix.TryGetValue(p, out var value))
        {
            return 1L;
        }

        var next = p + Position.S;

        if (value == '|')
        {
            return CountBeams((next, matrix));
        }
        else if (value == '^')
        {
            return CountBeams((next + Position.E, matrix)) + CountBeams((next + Position.W, matrix));
        }

        return 1L;
    }
}