[Slug(2024, 17)]
public class Day202417 : AsyncProblem
{
    public override Task<string> RunPartOneAsync(string[] input)
    {
        var computer = new Computer()
        {
            A = Convert.ToInt32(input[0].Replace("Register A: ", "")),
            B = Convert.ToInt32(input[1].Replace("Register B: ", "")),
            C = Convert.ToInt32(input[2].Replace("Register C: ", "")),
            Program = ParseProgram(input[4]),
            ExampleMode = ExampleMode
        };

        return Task.FromResult(string.Join(',', computer.RunProgram()));
    }

    public override Task<string> RunPartTwoAsync(string[] input)
    {
        var program = ParseProgram(input[4]);
        long regA = 0;

        var computer = new Computer()
        {
            Program = program,
            ExampleMode = ExampleMode
        };

        while (true)
        {
            computer.A = regA;
            var output = computer.RunProgram();

            // take last (output.length) digits of program for comparison
            var n = program.Length - output.Count;

            bool match = program[n..]
                .Zip(output)
                .All((pair) => pair.First == pair.Second);

            if (match)
            {
                if (output.Count == program.Length)
                {
                    return Task.FromResult($"{regA}");
                }
                regA <<= 3;
            }
            else
            {
                regA++;
            }
        }
    }

    int[] ParseProgram(string programStr)
    {
        return programStr[9..]
            .Split(',')
            .SelectMany(s => s.ToCharArray())
            .Select(c => int.Parse($"{c}"))
            .ToArray();
    }
}

public class Computer
{
    public long A { get; set; }
    public long B { get; set; }
    public long C { get; set; }
    public required int[] Program { get; set; }
    public bool ExampleMode { get; set; }
    private List<long> _outputs = [];
    private long _ip = 0;

    private long CO(int o) => o switch
    {
        <= 3 => o,
        4 => A,
        5 => B,
        6 => C,
        _ => throw new Exception($"{o} is invalid combo operand")
    };

    private long Div(int operand)
    {
        var numerator = A;
        var denominator = 1L << (int)CO(operand);
        return numerator / denominator;
    }

    public List<long> RunProgram()
    {
        _ip = 0;
        _outputs = [];
        while (_ip < Program.Length - 1)
        {
            var op = Program[_ip++];
            var o = Program[_ip++];

            switch (op)
            {
                case 0:
                    A = Div(o);
                    break;
                case 1:
                    B ^= o;
                    break;
                case 2:
                    B = CO(o) % 8;
                    break;

                case 3:
                    if (A != 0)
                    {
                        _ip = o;
                    }
                    break;
                case 4:
                    B ^= C;
                    break;
                case 5:
                    _outputs.Add(CO(o) % 8);
                    break;
                case 6:
                    B = Div(o);
                    break;
                case 7:
                    C = Div(o);
                    break;
            }
        }
        return _outputs;
    }
}
