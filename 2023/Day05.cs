[Slug(2023, 5)]
public class Day202305 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var seeds = input[0].Replace("seeds: ", "").ToLongList().ToList();
        var maps = ParseInput(input).ToList();

        return seeds.Min(s => WalkMaps(s, maps));
    }

    public override long RunPartTwo(string[] input)
    {
        var seeds = input[0].Replace("seeds: ", "").ToLongList().Pairwise((a, b) => (a, b)).ToList();
        var maps = ParseInput(input).ToList();

        return seeds.SelectMany(s => WalkMapRanges(s, maps)).Min(s => s.Item1);
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

    public (long, long)[] WalkMapRanges((long, long) seed, List<List<RangeMap>> maps)
    {
        return maps.Aggregate(new[] { seed }, (prev, curr) =>
        {
            return prev.SelectMany(s => SplitRangesOnMap(s, curr)).ToArray();
        });
    }

    public IEnumerable<(long, long)> SplitRangesOnMap((long, long) range, List<RangeMap> map)
    {
        var (start, len) = range;
        while (len > 0)
        {
            var (rngSt, rngLn) = map.Select(m => m.ConvertRange(start)).FirstOrDefault(c => c.Item1 != -1, (-1, -1));
            if (rngSt != -1)
            {
                rngLn = Math.Min(rngLn, len);
            }
            else
            {
                var next = map.First(m => m.min > start);
                rngSt = start;
                rngLn = Math.Min(next.min - start, len);
            }

            yield return (rngSt, rngLn);

            len -= rngLn;
            start += rngLn;
        }
    }

    public IEnumerable<List<RangeMap>> ParseInput(IEnumerable<string> input)
    {
        var map = new List<RangeMap>();
        foreach (var (row, idx) in input.WithIndex().Skip(2))
        {
            if (string.IsNullOrWhiteSpace(row))
            {
                yield return [.. map.OrderBy(x => x.min)];
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
    public bool Intersects(long num)
    {
        return num >= min && num <= SrcMax;
    }
    public long Convert(long num)
    {
        var dist = num - min;
        if (dist < 0 || dist > len)
        {
            return -1;
        }

        return dest + dist;
    }

    public (long, long) ConvertRange(long num)
    {
        var dist = num - min;
        if (dist < 0 || dist > len)
        {
            return (-1, -1);
        }

        return (dist, len - num);
    }
}