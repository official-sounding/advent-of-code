public record PrintRule(int Before, int After)
{
    public static PrintRule Parse(string s)
    {
        var parts = s.Split('|');
        return new(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]));
    }
}

public record PrintRules(HashSet<int> Befores, HashSet<int> Afters)
{
    public PrintRules() : this([], []) { }
}

public record PrintUpdate(int[] Pages, int Middle)
{
    public static PrintUpdate Parse(string s)
    {
        var nums = s.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
        var mid = nums[nums.Length / 2];
        return new(nums, mid);
    }
}

public class UpdateComparer(Dictionary<int, PrintRules> rules) : Comparer<int>
{
    private readonly Dictionary<int, PrintRules> rules = rules;

    public override int Compare(int x, int y)
    {
        if (this.rules.TryGetValue(x, out var rules))
        {
            if (rules.Befores.Contains(y))
            {
                return 1;
            }

            if (rules.Afters.Contains(y))
            {
                return -1;
            }
        }

        return 0;
    }
}

[Slug(2024, 05)]
public class Day202405 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var rules = new Dictionary<int, HashSet<int>>();
        var updates = new List<PrintUpdate>();
        var i = 0;
        while (!string.IsNullOrWhiteSpace(input[i]))
        {
            var (before, after) = PrintRule.Parse(input[i]);
            var rule = rules.GetValueOrDefault(after, []);
            rule.Add(before);
            rules[after] = rule;
            i++;
        }

        i++; //skip blank line
        while (i < input.Length && !string.IsNullOrWhiteSpace(input[i]))
        {
            updates.Add(PrintUpdate.Parse(input[i]));
            i++;
        }


        var result = 0;
        foreach (var (pages, middle) in updates)
        {
            var valid = true;
            foreach (var (page, idx) in pages.Select((page, idx) => (page, idx)))
            {
                if (rules.TryGetValue(page, out var befores))
                {
                    var afters = pages.Skip(idx);
                    if (afters.Any(a => befores.Contains(a)))
                    {
                        valid = false;
                        break;
                    }
                }
            }

            if (valid)
            {
                result += middle;
            }
        }

        return result;
    }

    public override long RunPartTwo(string[] input)
    {
        var rules = new Dictionary<int, PrintRules>();
        var updates = new List<PrintUpdate>();
        var i = 0;
        while (!string.IsNullOrWhiteSpace(input[i]))
        {
            var (before, after) = PrintRule.Parse(input[i]);
            var beforeRules = rules.GetValueOrDefault(before, new());
            beforeRules.Afters.Add(after);

            var afterRules = rules.GetValueOrDefault(after, new());
            afterRules.Befores.Add(before);

            rules[before] = beforeRules;
            rules[after] = afterRules;
            i++;
        }

        i++; //skip blank line
        while (i < input.Length && !string.IsNullOrWhiteSpace(input[i]))
        {
            updates.Add(PrintUpdate.Parse(input[i]));
            i++;
        }

        var comparer = new UpdateComparer(rules);

        var result = 0;
        foreach (var (pages, middle) in updates)
        {
            var valid = true;
            foreach (var (page, idx) in pages.Select((page, idx) => (page, idx)))
            {
                if (rules.TryGetValue(page, out var rule))
                {
                    var afters = pages.Skip(idx);
                    if (afters.Any(a => rule.Befores.Contains(a)))
                    {
                        valid = false;
                        break;
                    }
                }
            }

            if (!valid)
            {
                var sorted = pages.Order(comparer).ToArray();
                result += sorted[sorted.Length / 2];
            }
        }

        return result;
    }
}