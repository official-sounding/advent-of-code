[Slug(2023, 11)]
public class Day202311 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var expanded = ExpandInput(input);
        var matrix = Matrix.Parse(expanded);

        var galaxies = new List<Position>();
        foreach (var (pos, c) in matrix)
        {
            if (c == '#')
            {
                galaxies.Add(pos);
            }
        }

        var pairs = galaxies.AllPairs(false);
        return AllPairsShortestPath(pairs, matrix, 2);
    }

    public override long RunPartTwo(string[] input)
    {
        var expanded = ExpandInput(input);
        var matrix = Matrix.Parse(expanded);

        var galaxies = new List<Position>();
        foreach (var (pos, c) in matrix)
        {
            if (c == '#')
            {
                galaxies.Add(pos);
            }
        }

        var pairs = galaxies.AllPairs(false);
        return AllPairsShortestPath(pairs, matrix, 1000000);
    }

    long AllPairsShortestPath(IEnumerable<(Position, Position)> pairs, Matrix matrix, int expansionFactor)
    {
        return pairs.Sum(p =>
        {
            var cost = 0L;
            var (src, dest) = p;
            var pos = src;
            while (pos != dest)
            {
                var delta = dest - pos;
                var dir = DirectionFromDelta(delta);
                pos += dir;
                cost += matrix[pos] == 'e' ? expansionFactor : 1;
            }
            return cost;
        });
    }

    Position DirectionFromDelta(Position delta)
    {
        return delta switch
        {
            ( > 0, _) => Position.E,
            ( < 0, _) => Position.W,
            (0, > 0) => Position.N,
            (0, < 0) => Position.S,
            _ => Position.Nil
        };
    }

    string[] ExpandInput(string[] orig)
    {
        var result = new List<string>();
        var expand = orig.Select((_) => true).ToArray();

        foreach (var (line, lidx) in orig.WithIndex())
        {
            var lcount = 0;
            foreach (var (c, idx) in line.ToCharArray().WithIndex())
            {
                if (c == '.')
                {
                    lcount++;
                }
                else
                {
                    expand[idx] = false;
                }
            }

            if (lcount == line.Length)
            {
                result.Add(new string('e', line.Length));
            }
            else
            {
                result.Add(line);
            }
        }

        return result.Select(l => new string(ExpandRow(l, expand))).ToArray();
    }

    char[] ExpandRow(IEnumerable<char> orig, bool[] expand)
    {
        return orig.WithIndex().Select<(char, int), char>((arg) =>
        {
            var (c, idx) = arg;
            if (expand[idx])
            {
                return 'e';
            }
            else
            {
                return c;
            }
        }).ToArray();
    }
}