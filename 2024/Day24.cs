using System.Text;

[Slug(2024, 24)]
public class Day202424 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var dict = ParseValues(input);
        var sb = new StringBuilder();

        foreach (var z in dict.Keys.Where(k => k[0] == 'z').OrderDescending().ToList())
        {
            sb.Append(dict[z].Value);
        }
        return Convert.ToInt64(sb.ToString(), 2);
    }

    public override Task<string> RunPartTwoAsync(string[] input)
    {
        var result = string.Empty;
        var dict = ParseValues(input);

        List<string> swaps = [];
        int index = 0;
        IValue? carryReg = null;
        while (dict.ContainsKey($"x{index:00}") && swaps.Count < 8)
        {
            string xReg = $"x{index:00}";
            string yReg = $"y{index:00}";
            string zReg = $"z{index:00}";
            if (index == 0)
            {
                carryReg = FindExpression(dict, xReg, "AND", yReg);
            }
            else
            {
                var XORReg = FindExpression(dict, xReg, "XOR", yReg);
                var ANDReg = FindExpression(dict, xReg, "AND", yReg);
                var carryInReg = FindExpression(dict, XORReg?.c, "XOR", carryReg?.c);

                if (carryInReg == null && XORReg != null && ANDReg != null)
                {
                    Swap(dict, swaps, XORReg, ANDReg);
                    index = 0;
                    continue;
                }
                if (carryInReg != null && carryInReg.c != zReg)
                {
                    Swap(dict, swaps, carryInReg, dict[zReg]);
                    index = 0;
                    continue;
                }
                carryInReg = FindExpression(dict, XORReg?.c, "AND", carryReg?.c);
                carryReg = FindExpression(dict, ANDReg?.c, "OR", carryInReg?.c);
            }
            index++;
        }
        return Task.FromResult(string.Join(',', swaps.Order()));
    }

    IValue? FindExpression(Dictionary<string, IValue> dict, string? a, string op, string? b)
    {
        var try1 = dict.Values.FirstOrDefault(r => r.a == a && r.op == op && r.b == b);
        var try2 = dict.Values.FirstOrDefault(r => r.b == a && r.op == op && r.a == b);
        return try1 ?? try2;
    }

    void Swap(Dictionary<string, IValue> dict, List<string> swaps, IValue l, IValue r)
    {
        swaps.Add(l.c);
        swaps.Add(r.c);
        (l.c, r.c) = (r.c, l.c);
        (dict[l.c], dict[r.c]) = (dict[r.c], dict[l.c]);
    }



    Dictionary<string, IValue> ParseValues(string[] input)
    {
        var result = new Dictionary<string, IValue>();
        bool immediates = true;
        foreach (var line in input)
        {
            if (line == string.Empty)
            {
                immediates = false;
            }
            else if (immediates)
            {
                if (line.Split(":", StringSplitOptions.TrimEntries) is [var key, var value])
                {
                    result[key] = new ImmediateValue(int.Parse(value), key);
                }
            }
            else
            {
                if (line.Split(" ", StringSplitOptions.RemoveEmptyEntries) is [var a, var op, var b, _, var key])
                {
                    result[key] = new Gate(result, a, b, key, op);
                }
            }
        }

        return result;
    }

    interface IValue
    {
        public string a { get; }
        public string b { get; }
        string c { get; set; }
        string op { get; }
        int Value { get; }
    }

    class ImmediateValue(int val, string _c) : IValue
    {
        public string a => string.Empty;
        public string b => string.Empty;
        public string op => "IMM";
        public string c { get; set; } = _c;
        public int Value => val;
    }

    class Gate(Dictionary<string, IValue> values, string _a, string _b, string _c, string _op) : IValue
    {
        public string a { get; set; } = _a;
        public string b { get; set; } = _b;
        public string c { get; set; } = _c;
        public string op => _op;
        public int Value => op switch
        {
            "AND" => values[a].Value & values[b].Value,
            "OR" => values[a].Value | values[b].Value,
            "XOR" => values[a].Value ^ values[b].Value,
            _ => throw new Exception($"{op}!!")
        };
    }
}

