[Slug(2023, 5)]
public class Day202305 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var seeds = input[0].Replace("seeds: ", "").ToLongList().ToList();
        var maps = ParseInput(input).ToList();

        return seeds.Min(s => WalkMaps(s, maps));
    }

    public long WalkMaps(long seed, List<List<RangeMap>> maps)
    {
        var result = maps.Aggregate(seed, (prev, curr) =>
        {
            var result = curr.Select(m => m.Convert(prev)).FirstOrDefault(c => c >= 0, prev);
            Console.Write($"{prev} -> ");
            return result;
        });
        Console.WriteLine($"{result}");
        return result;
    }

    public IEnumerable<List<RangeMap>> ParseInput(IEnumerable<string> input)
    {
        var map = new List<RangeMap>();
        foreach (var (row, idx) in input.WithIndex().Skip(2))
        {
            if (string.IsNullOrWhiteSpace(row))
            {
                yield return map;
                map = [];

            }

            else if (row.Contains(':'))
            {
                // Console.WriteLine($"parsing {idx + 1}: {row}");
                continue;
            }

            else
            {
                map.Add(RangeMap.Parse(row));
            }
        }

        yield return map;
    }
}

public record Range(long start, long len);

public record RangeMap(long dest, long min, long len)
{
    public static RangeMap Parse(string row)
    {
        var parts = row.ToLongList().ToArray();
        return new RangeMap(parts[0], parts[1], parts[2]);
    }

    public long SrcMax => min + len;
    public long Convert(long num)
    {
        var dist = num - min;
        if (dist < 0 || dist > len)
        {
            return -1;
        }

        return dest + dist;
    }
}