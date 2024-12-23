[Slug(2024, 23)]
public class Day202423 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var nodes = Parse(input);
        return GenerateTriples(nodes.Where(n => n.Id[0] == 't')).Count();
    }

    public override Task<string> RunPartTwoAsync(string[] input)
    {
        var nodes = Parse(input);
        var degree = nodes.Max(n => n.Connections.Count);
        List<Node> password = [];
        while (degree-- > 0)
        {
            foreach (var node in nodes.Where(n => n.Id[0] == 't'))
            {
                foreach (var conn in node.Connections)
                {
                    List<Node> candidate = [node, conn];
                    foreach (var c in conn.Connections)
                    {
                        if (node.Connections.Contains(c))
                        {
                            candidate.Add(c);
                        }
                    }

                    if (candidate.Count > degree && IsClique(candidate) && candidate.Count > password.Count)
                    {
                        password = candidate;
                        break;
                    }
                }
            }

            if (password.Count >= 0)
            {
                break;
            }
        }

        return Task.FromResult(string.Join(',', password.Select(ls => ls.Id).Order()));
    }

    bool IsClique(List<Node> candidate)
    {
        foreach (var (n1, idx) in candidate.WithIndex())
        {
            foreach (var n2 in candidate.Skip(idx + 1))
            {
                if (!n1.Connections.Contains(n2)) return false;
            }
        }

        return true;
    }

    List<Node> Parse(string[] input)
    {
        Dictionary<string, Node> result = [];

        foreach (var line in input)
        {
            if (line.Split('-') is [var l, var r])
            {
                var lNode = result.GetOrAdd(l, (_) => new(l));
                var rNode = result.GetOrAdd(r, (_) => new(r));

                lNode.Connections.Add(rNode);
                rNode.Connections.Add(lNode);
            }
        }

        return [.. result.Values];
    }

    IEnumerable<(string, string, string)> GenerateTriples(IEnumerable<Node> nodes)
    {
        HashSet<string> sigs = [];
        foreach (var node in nodes)
        {
            foreach (var (l, r) in node.Connections.AllPairs(false).Where((p) => p.Item1.Connections.Contains(p.Item2)))
            {
                var sig = string.Join(',', new[] { node.Id, l.Id, r.Id }.Order());
                if (sigs.Add(sig))
                    yield return (node.Id, l.Id, r.Id);
            }
        }
    }

    record Node(string Id)
    {
        public HashSet<Node> Connections { get; init; } = [];

        public override string ToString()
        {
            return Id;
        }
    }
}
