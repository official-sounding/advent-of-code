[Slug(2023, 08)]
public class Day202308 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var instructions = input[0].ToCharArray();
        var tree = BuildTree(input.Skip(2));

        var node = "AAA";
        long idx = 0;
        while (node != "ZZZ")
        {
            var instruction = instructions[idx % instructions.Length];
            node = instruction switch
            {
                'L' => tree[node].l,
                'R' => tree[node].r,
                _ => throw new Exception($"{instruction}!!"),
            };
            idx++;
        }

        return idx;
    }

    public override long RunPartTwo(string[] input)
    {
        var instructions = input[0].ToCharArray();
        var tree = BuildTree(input.Skip(2));

        var nodes = tree.Keys.Where(k => k.EndsWith("A")).ToArray();
        var count = nodes.Length;

        return nodes.Select((node, ni) =>
        {
            long idx = 0;
            while (!node.EndsWith('Z'))
            {
                var instruction = instructions[idx % instructions.Length];
                node = instruction switch
                {
                    'L' => tree[node].l,
                    'R' => tree[node].r,
                    _ => throw new Exception($"{instruction}!!"),
                };
                idx++;
            }
            return idx;
        }).LeastCommonMultiple();
    }

    Dictionary<string, (string l, string r)> BuildTree(IEnumerable<string> nodes)
    {
        return nodes
            .Select(n => n.Split("=", StringSplitOptions.TrimEntries))
            .ToDictionary(p => p[0], p => (p[1].Substring(1, 3), p[1].Substring(6, 3)));
    }
}
