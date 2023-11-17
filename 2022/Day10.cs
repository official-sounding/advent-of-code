using System.Text;

[Slug("2022/d10")]
public class Day10 : SyncProblem
{
    public override string RunPartOneSync(string[] input)
    {
        List<(string, int)> instructions = input.SelectMany(line =>
{
    var parts = line.Split(' ');
    if (parts[0] == "noop")
    {
        return new[] { ("noop", 0) };
    }
    else
    {
        return new[] { ("noop", 0), (parts[0], int.Parse(parts[1])) };
    }
}).ToList();

        var x = 1;
        var pc = 1;
        var sum = 0;
        // 20th, 60th, 100th, 140th, 180th, and 220th
        var interestingInstructions = new[] { 20, 60, 100, 140, 180, 220 };
        var line = new StringBuilder();

        foreach (var (instruction, arg) in instructions)
        {
            line.Append(CrtOutput(pc, x));
            if (interestingInstructions.Contains(pc))
            {
                sum = sum + (pc * x);
            }

            if (pc % 40 == 0)
            {
                line.AppendLine();
            }

            if (instruction == "addx")
            {
                x = x + arg;
            }
            pc = pc + 1;
        }

        return $"{sum}";
    }

    public override string RunPartTwoSync(string[] input)
    {
        List<(string, int)> instructions = input.SelectMany(line =>
 {
     var parts = line.Split(' ');
     if (parts[0] == "noop")
     {
         return new[] { ("noop", 0) };
     }
     else
     {
         return new[] { ("noop", 0), (parts[0], int.Parse(parts[1])) };
     }
 }).ToList();

        var x = 1;
        var pc = 1;
        var sum = 0;
        // 20th, 60th, 100th, 140th, 180th, and 220th
        var interestingInstructions = new[] { 20, 60, 100, 140, 180, 220 };
        var line = new StringBuilder();

        foreach (var (instruction, arg) in instructions)
        {
            line.Append(CrtOutput(pc, x));
            if (interestingInstructions.Contains(pc))
            {
                sum = sum + (pc * x);
            }

            if (pc % 40 == 0)
            {
                line.AppendLine();
            }

            if (instruction == "addx")
            {
                x = x + arg;
            }
            pc = pc + 1;
        }

        return line.ToString();
    }

    static char CrtOutput(int pc, int x)
    {
        var horiz = (pc % 40) - 1;
        var pos = Math.Abs(horiz - x);
        if (pos > 1)
        {
            return ' ';
        }
        else
        {
            return '#';
        }
    }
}