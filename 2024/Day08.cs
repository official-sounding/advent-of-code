[Slug(2024, 08)]
public class Day202408 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var antennaLocations = FindAntennas(matrix);

        var result = new HashSet<Position>();

        foreach (var (freq, positions) in antennaLocations)
        {
            var antiNodes = positions
            .AllCombinations(includeIdentities: false)
            .Where(pair => pair.Item1.Y < pair.Item2.Y)
            .SelectMany(pair => GetAntinodes(matrix, pair.Item1, pair.Item2));

            foreach (var node in antiNodes)
            {
                result.Add(node);
            }
        }

        return result.Count;
    }

    public override long RunPartTwo(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var antennaLocations = FindAntennas(matrix);

        var result = new HashSet<Position>();

        foreach (var (freq, positions) in antennaLocations)
        {
            var antiNodes = positions
            .AllCombinations(includeIdentities: false)
            .Where(pair => pair.Item1.Y < pair.Item2.Y)
            .SelectMany(pair => GetAntinodes2(matrix, pair.Item1, pair.Item2));

            foreach (var node in antiNodes)
            {
                result.Add(node);
            }
        }

        return result.Count;
    }

    static Dictionary<char, List<Position>> FindAntennas(Matrix matrix)
    {
        var result = new Dictionary<char, List<Position>>();
        foreach (var (key, value) in matrix)
        {
            if (value != '.')
            {
                var positions = result.GetOrAdd(value, (_) => []);
                positions.Add(key);
            }
        }

        return result;
    }

    static IEnumerable<Position> GetAntinodes(Matrix matrix, Position a1, Position a2)
    {
        var distance = a1 - a2;
        var antinode1 = a1 + distance;
        if (matrix.ValidPosition(antinode1)) yield return antinode1;

        var antinode2 = a2 - distance;
        if (matrix.ValidPosition(antinode2)) yield return antinode2;
    }

    static IEnumerable<Position> GetAntinodes2(Matrix matrix, Position a1, Position a2)
    {
        var distance = a1 - a2;
        var antinode1 = a2 + distance;
        while (matrix.ValidPosition(antinode1))
        {
            yield return antinode1;
            antinode1 += distance;
        }

        var antinode2 = a1 - distance;
        while (matrix.ValidPosition(antinode2))
        {
            yield return antinode2;
            antinode2 -= distance;
        }
    }
}