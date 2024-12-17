[Slug(2024, 10)]
public class Day202410 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var matrix = Matrix.Parse(input);

        var starts = matrix.Where((kv) => kv.Value == '0').Select(kv => kv.Key).ToArray();

        var score = 0;

        foreach (var pos in starts) {
            var ends = new HashSet<Position>();
            SearchPaths(matrix, pos, ends);
            score += ends.Count;
        }

        return score;
    }

    public override long RunPartTwo(string[] input)
    {
        var matrix = Matrix.Parse(input);

        var starts = matrix.Where((kv) => kv.Value == '0').Select(kv => kv.Key).ToArray();

        var score = 0;

        foreach (var pos in starts) {
            var ends = new List<Position>();
            SearchPaths(matrix, pos, ends);
            score += ends.Count;
        }

        return score;
    }

    void SearchPaths(Matrix matrix, Position pos, ICollection<Position> ends) {
        var value = matrix[pos];

        if(value == '9') {
            ends.Add(pos);
            return;
        }
        var next = NextValue(value);

        foreach(var dir in Position.CardinalDirections()) {
            if(matrix.TryGetValue(pos + dir, out var v) && v == next) {
                SearchPaths(matrix, pos + dir, ends);
            }
        }
    }

    char NextValue(char c) => (char)(c + 1);
}