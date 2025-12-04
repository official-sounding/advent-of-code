[Slug(2025, 03)]
public class Day202503 : Problem
{
    public override long RunPartOne(string[] input)
    {
        return input.Select((b) => MaxJolt(b, 2)).Sum();
    }

    public override long RunPartTwo(string[] input)
    {
        return input.Select((b) => MaxJolt(b, 12)).Sum();
    }

    private static long MaxJolt(string bank, int nrOfBatteries)
    {
        var jolt = 0L;
        var index = 0;

        while (nrOfBatteries-- > 0)
        {
            var (atIndex, digit) = GetMaxDigitFrom(bank, index, nrOfBatteries);
            jolt = jolt * 10 + digit.ToInt();
            index = atIndex + 1;
        }

        return jolt;
    }

    private static (int index, char digit) GetMaxDigitFrom(string bank, int startIndex, int endIndex)
    {
        var maxDigit = '0';
        var index = -1;
        foreach (var (c, i) in bank.Skip(startIndex).Take(bank.Length - endIndex).WithIndex())
        {
            if (c > maxDigit)
            {
                maxDigit = c;
                index = i;
            }
        }

        return (index, maxDigit);
    }




}