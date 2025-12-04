[Slug(2025, 03)]
public class Day202503 : Problem
{
    public override long RunPartOne(string[] input)
    {
        return input.Select(MaxJoltage).Sum();
    }

    private static int MaxJoltage(string line)
    {
        var max = -1;
        foreach (var (ch, idx) in line.WithIndex().Take(line.Length - 1))
        {
            var nextlg = line.Skip(idx + 1).Max();
            var num = int.Parse($"{ch}{nextlg}");
            if (num > max)
            {
                max = num;
            }
        }

        return max;
    }




}