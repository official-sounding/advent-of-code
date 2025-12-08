[Slug(2025, 8)]
public class Day202508Mine : Problem
{
    public override long RunPartOne(string[] input)
    {
        var queue = ParseInput(input);

        var connections = ExampleMode ? 10 : 1000;
        List<HashSet<Node>> circuits = [];

        while (queue.TryDequeue(out var pair, out var dist) && connections-- > 0)
        {
            MakeConnection(pair, circuits);
        }

        return circuits.OrderByDescending(x => x.Count).Select(x => x.Count).Take(3).Aggregate(1, (curr, next) =>
        {
            return curr * next;
        });
    }

    public override long RunPartTwo(string[] input)
    {
        var connections = ExampleMode ? 20 : 1000;
        var queue = ParseInput(input);
        List<HashSet<Node>> circuits = [];
        while (queue.TryDequeue(out var pair, out var dist))
        {
            MakeConnection(pair, circuits);
            if (circuits.Count == 1 && circuits[0].Count == connections)
            {
                var (a, b) = pair;
                return a.X * b.X;
            }
        }

        return -1;
    }

    private static PriorityQueue<NodePair, long> ParseInput(string[] input)
    {
        List<Node> nodes = [];
        foreach (var line in input)
        {
            if (line.ToIntList(',').ToArray() is [var x, var y, var z])
            {
                nodes.Add(new(x, y, z));
            }
            else
            {
                throw new Exception($"{line}??");
            }
        }

        var queue = new PriorityQueue<NodePair, long>();

        for (var i = 0; i < nodes.Count; i++)
        {
            for (var j = i + 1; j < nodes.Count; j++)
            {
                var pair = new NodePair(nodes[i], nodes[j]);
                queue.Enqueue(pair, pair.Distance);
            }
        }

        return queue;
    }

    private static void MakeConnection(NodePair pair, List<HashSet<Node>> circuits)
    {
        var (a, b) = pair;
        var circuitA = circuits.FirstOrDefault(c => c.Contains(a));
        var circuitB = circuits.FirstOrDefault(c => c.Contains(b));

        if (circuitA is not null && circuitB is not null)
        {
            if (circuitA != circuitB)
            {
                // merge i1 circuit into i2 circuit
                circuitB.UnionWith(circuitA);
                circuits.Remove(circuitA);
            }
        }
        else if (circuitA is not null) { circuitA.Add(b); }
        else if (circuitB is not null) { circuitB.Add(a); }
        else { circuits.Add([a, b]); }
    }


    public record Node(int X, int Y, int Z)
    {
        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }
    }
    public record NodePair(Node A, Node B)
    {
        public long Distance
        {
            get
            {
                var dx = (long)A.X - B.X;
                var dy = (long)A.Y - B.Y;
                var dz = (long)A.Z - B.Z;

                return (dx * dx) + (dy * dy) + (dz * dz);
            }
        }

        public override string ToString()
        {
            return $"{A} -> {B}: {Distance}";
        }
    }
}