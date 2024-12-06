using System.Text.RegularExpressions;

[Slug("2022/d05")]
public class Day202205 : SyncProblem
{
    public override string RunPartOneSync(string[] input)
    {
        var stackRegex = new Regex(@"[\[\]\s]+");

        var stacks = new List<Stack<char>>();
        var instructions = new List<(int, int, int)>();

        foreach (var _ in Enumerable.Range(0, 9))
        {
            stacks.Add(new Stack<char>());
        }

        foreach (var line in input)
        {
            if (line.StartsWith("[") || line.StartsWith("  "))
            {
                var spaced = line.Replace("    ", "[*] ");
                var elements = stackRegex.Replace(spaced, string.Empty).ToCharArray();
                var idx = 0;
                foreach (var element in elements)
                {
                    if (element != '*')
                    {
                        stacks[idx].Push(element);
                    }
                    idx++;
                }
            }
            else if (line.StartsWith("move"))
            {
                var con = line
                    .Replace("move ", string.Empty)
                    .Replace("from ", string.Empty)
                    .Replace("to ", string.Empty)
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

                instructions.Add((con[0], con[1] - 1, con[2] - 1));

            }
        }

        stacks = stacks.Select(q => new Stack<char>(q)).ToList();

        foreach (var instruction in instructions)
        {
            var (howMany, start, end) = instruction;
            while (howMany-- > 0)
            {
                var item = stacks[start].Pop();
                stacks[end].Push(item);
            }
        }

        return string.Join(string.Empty, stacks.Select(s => s.Peek()));
    }

    public override string RunPartTwoSync(string[] input)
    {
        var stackRegex = new Regex(@"[\[\]\s]+");

        var stacks = new List<Stack<char>>();
        var instructions = new List<(int, int, int)>();

        foreach (var _ in Enumerable.Range(0, 9))
        {
            stacks.Add(new Stack<char>());
        }

        foreach (var line in input)
        {
            if (line.StartsWith("[") || line.StartsWith("  "))
            {
                var spaced = line.Replace("    ", "[*] ");
                var elements = stackRegex.Replace(spaced, string.Empty).ToCharArray();
                var idx = 0;
                foreach (var element in elements)
                {
                    if (element != '*')
                    {
                        stacks[idx].Push(element);
                    }
                    idx++;
                }
            }
            else if (line.StartsWith("move"))
            {
                var con = line
                    .Replace("move ", string.Empty)
                    .Replace("from ", string.Empty)
                    .Replace("to ", string.Empty)
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

                instructions.Add((con[0], con[1] - 1, con[2] - 1));

            }
        }

        stacks = stacks.Select(q => new Stack<char>(q)).ToList();

        var tmpStack = new Stack<char>();

        foreach (var instruction in instructions)
        {

            var (howMany, start, end) = instruction;
            while (howMany-- > 0)
            {
                tmpStack.Push(stacks[start].Pop());
            }

            while (tmpStack.TryPop(out var item))
            {
                stacks[end].Push(item);
            }
        }

        return string.Join(string.Empty, stacks.Select(s => s.Peek()));
    }
}