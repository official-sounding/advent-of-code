[Slug(2022, 12)]
public class Day202212 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var source = Position.Nil;
        var target = Position.Nil;

        foreach (var (pos, value) in matrix)
        {
            if (value == 'E')
            {
                matrix[pos] = 'z';
                target = pos;
            }
            else if (value == 'S')
            {
                matrix[pos] = 'a';
                source = pos;
            }
        }

        return Dijkstra.FindPathLength(matrix, source, target, Neighbors);
    }

    public override long RunPartTwo(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var target = Position.Nil;
        HashSet<Position> starts = [];

        foreach (var (pos, value) in matrix)
        {
            if (value == 'E')
            {
                matrix[pos] = 'z';
                target = pos;
            }
            else if (value == 'S')
            {
                matrix[pos] = 'a';
                starts.Add(pos);
            }
            else if (value == 'a')
            {
                starts.Add(pos);
            }
        }

        return starts.Select(src => Dijkstra.FindPathLength(matrix, src, target, Neighbors)).Min();
    }

    static IEnumerable<(Position,int)> Neighbors(Matrix matrix, Position u)
    {
        if (!matrix.TryGetValue(u, out var cellHeight))
        {
            yield break;
        }

        foreach (var dir in Position.CardinalDirections())
        {
            var newCell = u + dir;
            if (matrix.TryGetValue(newCell, out var nextHeight) && nextHeight <= cellHeight + 1)
            {
                yield return (newCell,1);
            }
        }
    }
}
