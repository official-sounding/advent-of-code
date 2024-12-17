using System.Data.Common;

[Slug(2020, 11)]
public class Day202011 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var matrix = Matrix.Parse(input).ToDictionary();
        var stable = false;
        while (!stable)
        {
            var next = Iterate(matrix);
            stable = matrix.All((kv) => next[kv.Key] == kv.Value);
            matrix = next;
        }

        return matrix.Count(kv => kv.Value == '#');
    }

    public Dictionary<Position, char> Iterate(Dictionary<Position, char> matrix)
    {
        var result = new Dictionary<Position, char>();

        foreach (var (pos, value) in matrix)
        {
            // if chair is empty & no adjacent chairs are occupied, chair becomes occupied
            if (value == 'L' && !AdjacentValues(matrix, pos).Any(v => v == '#'))
            {
                result[pos] = '#';
            }
            // if char is occupied & 4 or more chairs are occupied, chair becomes unoccupied
            else if (value == '#' && AdjacentValues(matrix, pos).Count(v => v == '#') >= 4)
            {
                result[pos] = 'L';
            }
            else
            {
                result[pos] = value;
            }
        }

        return result;
    }

    public IEnumerable<char> AdjacentValues(Dictionary<Position, char> matrix, Position pos)
    {
        foreach (var dir in Position.AllDirections())
        {
            if (matrix.TryGetValue(pos + dir, out var value))
            {
                yield return value;
            }
        }
    }
}