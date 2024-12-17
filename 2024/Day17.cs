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
            Program = input[4].Replace("Program: ","").Split(',').SelectMany(s => s.ToCharArray()).ToArray(),
            ExampleMode = ExampleMode
        };

        return Task.FromResult(computer.RunProgram());
    }
}

public class Computer
{
    public int A { get; set; }
    public int B { get; set; }
    public int C { get; set; }
    public required char[] Program { get; set; }
    public bool ExampleMode { get; set; }
    private List<int> _outputs = [];
    private int _ip = 0;

    private int ComboOperand(char o) => o switch
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

    private int LiteralOperand(char o) => Convert.ToInt32($"{o}");

    public string RunProgram()
    {
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
                        var denominator = (long)Math.Pow(2, ComboOperand(operand));

                        var result = numerator / denominator;
                        A = unchecked((int)(uint)(result & uint.MaxValue));
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

                        _outputs.Add(ComboOperand(operand) % 8);
                        break;
                    }
                case '6':
                    {
                        OutputInstruction("bdv", ComboOperand(operand));
                        var numerator = A;
                        var denominator = (long)Math.Pow(2, ComboOperand(operand));

                        var result = numerator / denominator;
                        B = unchecked((int)(uint)(result & uint.MaxValue));
                        break;
                    }
                case '7':
                    {
                        OutputInstruction("cdv", ComboOperand(operand));
                        var numerator = A;
                        var denominator = (long)Math.Pow(2, ComboOperand(operand));

                        var result = numerator / denominator;
                        C = unchecked((int)(uint)(result & uint.MaxValue));
                        break;
                    }
            }
        }
        return string.Join(',', _outputs);
    }

    void OutputInstruction(string inst, int op)
    {
        if (ExampleMode)
        {
            Console.WriteLine($"{inst} {op,10}");
        }
    }
}
