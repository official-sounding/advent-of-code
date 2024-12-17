[Slug(2024, 17)]
public class Day202417 : AsyncProblem
{
    public override Task<string> RunPartOneAsync(string[] input)
    {
        var computer = new Computer()
        {
            Program = input[4].Replace("Program: ", "").Split(',').SelectMany(s => s.ToCharArray()).ToArray(),
            ExampleMode = ExampleMode
        };

        return Task.FromResult(computer.RunProgram(Convert.ToInt32(input[0].Replace("Register A: ", ""))));
    }

    public override Task<string> RunPartTwoAsync(string[] input)
    {
        var programStr = input[4].Replace("Program: ", "");
        var program = programStr.Split(',').SelectMany(s => s.ToCharArray()).ToArray();
        var output = string.Empty;
        var computer = new Computer()
        {
            Program = program,
            ExampleMode = ExampleMode
        };
        long a = 0;
        for (int i = 0; i < programStr.Length; i++)
        {
            Console.WriteLine(i);
            for (a <<= 3; ; ++a)
            {
                if (computer.RunProgram(a)[..(i + 1)].SequenceEqual(program[^(i + 1)..]))
                    break;
            }
        }
        return Task.FromResult($"{a}");
    }
}

public class Computer
{
    public long A { get; set; }
    public long B { get; set; }
    public long C { get; set; }
    public required char[] Program { get; set; }
    public bool ExampleMode { get; set; }
    private List<int> _outputs = [];
    private int _ip = 0;

    private long ComboOperand(char o) => o switch
    {
        '0' => 0,
        '1' => 1,
        '2' => 2,
        '3' => 3,
        '4' => A,
        '5' => B,
        '6' => C,
        _ => throw new Exception($"{o} is invalid combo operand")
    };

    private int LiteralOperand(char o) => o switch
    {
        '0' => 0,
        '1' => 1,
        '2' => 2,
        '3' => 3,
        '4' => 4,
        '5' => 5,
        '6' => 6,
        '7' => 7,
        _ => throw new Exception("!!")
    };
    private void Reset(long a)
    {
        A = a;
        B = 0;
        C = 0;
        _ip = 0;
        _outputs =[];
    }
    public string RunProgram(long a)
    {
        Reset(a);
        while (_ip < Program.Length - 1)
        {
            var op = Program[_ip++];
            var operand = Program[_ip++];

            switch (op)
            {
                case '0':
                    { //adv
                        OutputInstruction("adv", ComboOperand(operand));
                        var numerator = A;
                        var denominator = 1 << (int)ComboOperand(operand);

                        A = numerator / denominator;
                        break;
                    }
                case '1':
                    {
                        OutputInstruction("bxl", LiteralOperand(operand));
                        B ^= LiteralOperand(operand);
                        break;
                    }
                case '2':
                    {
                        OutputInstruction("bst", ComboOperand(operand));
                        B = ComboOperand(operand) % 8;
                        break;
                    }

                case '3':
                    {
                        OutputInstruction("jnz", LiteralOperand(operand));
                        if (A != 0)
                        {

                            _ip = LiteralOperand(operand);
                        }
                        break;
                    }
                case '4':
                    {
                        OutputInstruction("bxc", LiteralOperand(operand));
                        B ^= C;
                        break;
                    }
                case '5':
                    {
                        OutputInstruction("out", ComboOperand(operand));

                        _outputs.Add((int)ComboOperand(operand) % 8);
                        break;
                    }
                case '6':
                    {
                        OutputInstruction("bdv", ComboOperand(operand));
                        var numerator = A;
                        var denominator = 1L << (int)ComboOperand(operand);
                        B = numerator / denominator;
                        break;
                    }
                case '7':
                    {
                        OutputInstruction("cdv", ComboOperand(operand));
                        var numerator = A;
                        var denominator = 1L << (int)ComboOperand(operand);
                        C = numerator / denominator;
                        break;
                    }
            }
        }
        return string.Join(',', _outputs);
    }

    void OutputInstruction(string inst, long op)
    {
        if (ExampleMode)
        {
            Console.WriteLine($"{inst} {op,20}");
        }
    }
}
