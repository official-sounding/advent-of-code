[Slug("2024/d11")]
public class Day202411 : Problem
{

    public override long RunPartOne(string[] input)
    {
        var numbers = ParseInput(input);
        return numbers.Sum((s) => CountStone((s, 25)));
    }

    public override long RunPartTwo(string[] input)
    {
        var numbers = ParseInput(input);
        return numbers.Sum((s) => CountStone((s, 75)));
    }

    long[] ParseInput(string[] input)
    {
        return input[0].ToLongList().ToArray();
    }

    long CountStone((long stone, int step) arg) => this.Memoized(arg, (arg) =>
        arg switch
        {
            (_, 0) => 1,
            (0, _) => CountStone((1, arg.step - 1)),
            (var stone, var step) when Digits(stone) % 2 == 0 =>
                CountStone((stone / SplitPoint(stone), step - 1)) +
                CountStone((stone % SplitPoint(stone), step - 1)),
            _ => CountStone((arg.stone * 2024, arg.step - 1))
        }
    );

    int Digits(long stone) => (int)Math.Floor(Math.Log10(stone)) + 1;
    int SplitPoint(long stone) => this.Memoized(stone, (s) => (int)Math.Pow(10, Digits(s) / 2));


    // perform one iteration of the algorithm
    // works for part 1, but will take forever to run for part 2
    IEnumerable<long> Blink(IEnumerable<long> input)
    {
        var bs = 10;

        foreach (var item in input)
        {
            if (item == 0)
            {
                yield return 1;
            }

            else if (Digits(item) % 2 == 0)
            {
                var split = SplitPoint(item);

                yield return item / split;
                yield return item % split;
            }
            else
            {
                yield return item * 2024;
            }
        }
    }
}