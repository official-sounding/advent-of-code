[Slug(2024, 20)]
public class Day202420 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var distances = Dijkstra.FindAllDistances(matrix, matrix.FindPosition('S'));
        return Cheat(distances, 2, 100);
    }

    public override long RunPartTwo(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var distances = Dijkstra.FindAllDistances(matrix, matrix.FindPosition('S'));
        return Cheat(distances, 20, 100);
    }

    private static int Cheat(Dictionary<Position, int> dist, int maxCheatDuration, int minTimeSaved)
    {
        var cheats = new HashSet<(Position Start, Position End, int Saved)>();
        foreach (var (p1, d1) in dist)
        {
            foreach (var (p2, d2) in dist.Where(d => d.Value > d1))
            {
                var manhattanDistance = p1.ManhattanDistance(p2);
                var saved = d2 - d1 - manhattanDistance;
                if (manhattanDistance <= maxCheatDuration && saved >= minTimeSaved)
                {
                    cheats.Add((p1, p2, saved));
                }
            }
        }

        return cheats.Count;
    }
}