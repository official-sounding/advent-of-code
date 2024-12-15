[Slug("2020/d01")]
public class Day202001 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var numbers = input.Select(int.Parse).ToList();

        int i = 0;
        int j = 0;
        for (; i < numbers.Count; i++)
        {
            j = 0;
            for (; j < numbers.Count; j++)
            {
                if (i != j && numbers[i] + numbers[j] == 2020)
                {
                    return numbers[i] * numbers[j];
                }
            }
        }

        return -1;
    }

    public override long RunPartTwo(string[] input)
    {
        var numbers = input.Select(int.Parse).ToList();

        int i = 0;
        int j = 0;
        int k = 0;
        for (; i < numbers.Count; i++)
        {
            j = 0;
            for (; j < numbers.Count; j++)
            {
                k = 0;
                for (; k < numbers.Count; k++)
                {
                    if (i != j && k != j && numbers[i] + numbers[j] + numbers[k] == 2020)
                    {
                        return numbers[i] * numbers[j] * numbers[k];
                    }
                }
            }
        }

        return -1;
    }
}