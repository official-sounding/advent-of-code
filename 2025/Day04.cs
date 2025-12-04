[Slug(2025, 04)]
public class Day202504 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var matrix = Matrix.Parse(input);
        return RemoveRolls(matrix);
    }

    public override long RunPartTwo(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var removed = 1;
        var total = 0;
        while (removed > 0)
        {
            removed = RemoveRolls(matrix);
            total += removed;
        }

        return total;
    }

    private static int RemoveRolls(Matrix matrix)
    {
        var moveable = 0;

        foreach (var (pos, _) in matrix.Where(kv => kv.Value == '@'))
        {
            var canMove = true;
            var adjacent = 0;
            foreach (var dir in Position.AllDirections().Where(d => matrix.ValidPosition(pos + d)))
            {
                var x = matrix[pos + dir];
                if (x == '@')
                {
                    adjacent++;
                }

                if (adjacent >= 4)
                {
                    canMove = false;
                    break;
                }
            }

            if (canMove)
            {
                matrix[pos] = '.';
                moveable++;
            }
        }

        return moveable;
    }
}