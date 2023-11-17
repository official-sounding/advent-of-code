[Slug("2022/d11")]
public class Day11 : SyncProblem
{
    public override string RunPartOneSync(string[] input)
    {
        var monkeys = input.Select((line) =>
{
    var parts = line.Split(";");
    var starting = parts[0].Split(",").Select(long.Parse).ToArray();
    var worryModifier = ParseModifierPartOne(parts[1]);
    var divisor = int.Parse(parts[2]);
    var trueDest = int.Parse(parts[3]);
    var falseDest = int.Parse(parts[4]);

    return new Monkey(starting, worryModifier, divisor, trueDest, falseDest);
}).ToList();

        foreach (var round in Enumerable.Range(0, 20))
        {
            foreach (var monkey in monkeys)
            {
                foreach (var (item, dest) in monkey.ProcessItems())
                {
                    monkeys[dest].Items.Enqueue(item);
                }
            }
        }

        var sorted = monkeys.OrderByDescending(m => m.InspectionCount).ToList();

        return $"{sorted[0].InspectionCount * sorted[1].InspectionCount}";

    }
    public override string RunPartTwoSync(string[] input)
    {

        var monkeys = input.Select((line) =>
        {
            var parts = line.Split(";");
            var starting = parts[0].Split(",").Select(long.Parse).ToArray();
            var worryModifier = ParseModifierPartTwo(parts[1]);
            var divisor = long.Parse(parts[2]);
            var trueDest = int.Parse(parts[3]);
            var falseDest = int.Parse(parts[4]);

            return new Monkey(starting, worryModifier, divisor, trueDest, falseDest);
        }).ToList();

        var commonDivisor = monkeys.Aggregate(1L, (div, monkey) => div * monkey.Divisor);


        foreach (var round in Enumerable.Range(0, 10000))
        {
            if (round % 1000 == 0)
            {
                Console.Write("+");
            }
            else if (round % 100 == 0)
            {
                Console.Write(".");
            }
            foreach (var monkey in monkeys)
            {
                foreach (var (item, dest) in monkey.ProcessItems(commonDivisor))
                {
                    monkeys[dest].Items.Enqueue(item);
                }
            }
        }
        var sorted = monkeys.OrderByDescending(m => m.InspectionCount).ToList();
        return $"{sorted[0].InspectionCount * sorted[1].InspectionCount}";

    }

    static Func<long, long> ParseModifierPartOne(string input)
    {
        var parts = input.Split(" ");
        var sec = long.Parse(parts[1]);

        return parts[0] switch
        {
            "+" => (old) => (old + sec) / 3,
            "*" => (old) => (old * sec) / 3,
            "**" => (old) => (old * old) / 3,
            _ => (old) => old,
        };
    }

    static Func<long, long> ParseModifierPartTwo(string input)
    {
        var parts = input.Split(" ");
        var sec = long.Parse(parts[1]);

        return parts[0] switch
        {
            "+" => (old) => (old + sec),
            "*" => (old) => (old * sec),
            "**" => (old) => (old * old),
            _ => (old) => old,
        };
    }

    public class Monkey
    {
        private Func<long, long> worryModifier;
        private int trueDest;
        private int falseDest;

        public Monkey(IEnumerable<long> startingItems, Func<long, long> worryModifier, long divisor, int trueDest, int falseDest)
        {
            foreach (long item in startingItems)
            {
                Items.Enqueue(item);
            }

            Divisor = divisor;
            this.worryModifier = worryModifier;
            this.trueDest = trueDest;
            this.falseDest = falseDest;
        }

        public long Divisor { get; init; }
        public Queue<long> Items { get; } = new Queue<long>();
        public long InspectionCount { get; set; }

        public IEnumerable<(long, int)> ProcessItems()
        {
            while (Items.TryDequeue(out long item))
            {
                InspectionCount = InspectionCount + 1;
                var updated = worryModifier(item);
                if (updated % Divisor == 0)
                {
                    yield return (updated, trueDest);
                }
                else
                {
                    yield return (updated, falseDest);
                }
            }
        }

        public IEnumerable<(long, int)> ProcessItems(long commonDivisor)
        {
            while (Items.TryDequeue(out long item))
            {
                InspectionCount = InspectionCount + 1;
                var updated = worryModifier(item) % commonDivisor;
                if (updated % Divisor == 0)
                {
                    yield return (updated, trueDest);
                }
                else
                {
                    yield return (updated, falseDest);
                }
            }
        }
    }
}
