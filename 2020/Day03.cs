[Slug("2020/d03")]
public class Day202003 : Problem
{
    public override long RunPartOne(string[] input)
    {
        int width = input[0].Length;

        var slopes = new[]
        {
            new Slope() { Rise = 1, Run = 1, Width = width },
            new Slope() { Rise = 1, Run = 3, Width = width },
            new Slope() { Rise = 1, Run = 5, Width = width },
            new Slope() { Rise = 1, Run = 7, Width = width },
            new Slope() { Rise = 2, Run = 1, Width = width },
        };


        foreach (var (line, idx) in input.Skip(1).Select((line, idx) => (line, idx)))
        {
            foreach (var slope in slopes)
            {
                slope.ProgressLine(line, idx + 1);
            }
        }

        return slopes[1].TreeCount;
    }

    public override long RunPartTwo(string[] input)
    {
        int width = input[0].Length;

        var slopes = new[]
        {
            new Slope() { Rise = 1, Run = 1, Width = width },
            new Slope() { Rise = 1, Run = 3, Width = width },
            new Slope() { Rise = 1, Run = 5, Width = width },
            new Slope() { Rise = 1, Run = 7, Width = width },
            new Slope() { Rise = 2, Run = 1, Width = width },
        };


        foreach (var (line, idx) in input.Skip(1).Select((line, idx) => (line, idx)))
        {
            foreach (var slope in slopes)
            {
                slope.ProgressLine(line, idx + 1);
            }
        }

        return slopes.Aggregate(1L, (res, slope) => res * slope.TreeCount);
    }
}

public class Slope
{
    int position = 0;


    public int Rise { get; init; }
    public int Run { get; init; }

    public int Width { get; init; }

    public int TreeCount { get; private set; }
    public void ProgressLine(string line, int idx)
    {
        if (idx % Rise == 0)
        {
            position = (position + Run) % Width;
            if (line[position] == '#')
            {
                TreeCount++;
            }
        }
    }

}