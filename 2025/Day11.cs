[Slug(2025, 11)]
public class Day202511 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var mappings = input.Select(Mapping.Parse).ToDictionary(m => m.Id);

        var node = mappings["you"];

        return FindEnds((node, mappings));
    }

    public override long RunPartTwo(string[] input)
    {
        var mappings = input.Select(Mapping.Parse).ToDictionary(m => m.Id);

        var node = mappings["svr"];

        return FindEndsTwo((node, mappings, false, false));
    }

    private long FindEndsTwo((Mapping n, Dictionary<string, Mapping> m, bool fft, bool dac) arg) => this.Memoized(arg, WrappedTwo);

    private long FindEnds((Mapping n, Dictionary<string, Mapping> m) arg) => this.Memoized(arg, WrappedCount);

    private long WrappedCount((Mapping n, Dictionary<string, Mapping> m) arg)
    {
        var (node, map) = arg;

        if (node.Outputs is ["out"])
        {
            return 1;
        }

        return node.Outputs.Sum((k) => FindEnds((map[k], map)));
    }

    private long WrappedTwo((Mapping n, Dictionary<string, Mapping> m, bool fft, bool dac) arg)
    {
        var (node, map, fft, dac) = arg;

        if (node.Outputs is ["out"])
        {
            return fft && dac ? 1 : 0;
        }

        if (node.Id == "fft")
        {
            fft = true;
        }

        if (node.Id == "dac")
        {
            dac = true;
        }

        return node.Outputs.Sum((k) => FindEndsTwo((map[k], map, fft, dac)));
    }

    private record Mapping(string Id, string[] Outputs)
    {
        public static Mapping Parse(string row)
        {
            var parts = row.Split(':', StringSplitOptions.TrimEntries);
            var id = parts[0];
            var outputs = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();

            return new(id, outputs);
        }
    }
}