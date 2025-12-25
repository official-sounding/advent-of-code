using System.Diagnostics.CodeAnalysis;

[Slug(2021, 04)]
public class Day202104 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var numbers = input[0].ToIntList(',');
        var matricies = ParseInput(input.Skip(2));

        foreach (var number in numbers)
        {
            MarkNumbers(number, matricies);
            if (HasBingo(matricies, out var winner))
            {
                return CalculateScore(winner, number);
            }
        }

        return -1;
    }

    private static void MarkNumbers(int number, IEnumerable<NumMatrix> matricies)
    {

        foreach (var matrix in matricies)
        {
            Position? p = null;
            foreach (var (key, value) in matrix)
            {
                if (value == number)
                {
                    p = key;
                    break;
                }
            }

            if (p is not null)
            {
                matrix[p] = -1;
            }
        }
    }

    private static bool HasBingo(IEnumerable<NumMatrix> matricies, [MaybeNullWhen(false)] out NumMatrix winner)
    {
        winner = null;
        foreach (var matrix in matricies)
        {
            // check rows:
            foreach (var y in matrix.YValues)
            {
                var correct = true;
                foreach (var x in matrix.XValues)
                {
                    if (matrix[(x, y)] != -1)
                    {
                        correct = false;
                        break;
                    }
                }

                if (correct)
                {
                    winner = matrix;
                    return true;
                }
            }

            // check columns:
            foreach (var x in matrix.XValues)
            {
                var correct = true;
                foreach (var y in matrix.YValues)
                {
                    if (matrix[(x, y)] != -1)
                    {
                        correct = false;
                        break;
                    }
                }

                if (correct)
                {
                    winner = matrix;
                    return true;
                }
            }
        }

        return false;
    }

    private static long CalculateScore(NumMatrix winner, int number)
    {
        return winner.Where((kv) => kv.Value != -1).Select(kv => kv.Value).Sum() * number;
    }

    private static List<NumMatrix> ParseInput(IEnumerable<string> input)
    {
        var result = new List<NumMatrix>();
        var current = new List<string>();

        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                result.Add(NumMatrix.Parse(current));
                current = [];
            }

            current.Add(line);
        }

        if (current.Count > 0)
        {
            result.Add(NumMatrix.Parse(current));
        }

        return result;
    }
}