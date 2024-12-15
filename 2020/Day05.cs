
[Slug("2020/d05")]
public class Day202005 : Problem
{
    public override long RunPartOne(string[] input)
    {
        return input.Select(line =>
        {

            var rowCoords = line[..7].ToCharArray();
            var colCorods = line.Substring(7, 3).ToCharArray();

            var row = FindNum(rowCoords);
            var col = FindNum(colCorods);

            if (ExampleMode)
            {
                Console.WriteLine($"{row} {col}");
            }

            return SeatID(row, col);

        }).Max();
    }

    public override long RunPartTwo(string[] input)
    {
        var seats = input.Select(line =>
        {

            var rowCoords = line[..7].ToCharArray();
            var colCorods = line.Substring(7, 3).ToCharArray();

            var row = FindNum(rowCoords);
            var col = FindNum(colCorods);

            if (ExampleMode)
            {
                Console.WriteLine($"{row} {col}");
            }

            return SeatID(row, col);

        }).ToHashSet();

        var max = seats.Max();

        foreach (var i in Enumerable.Range(0, max))
        {
            if (!seats.Contains(i) && seats.Contains(i + 1) && seats.Contains(i - 1))
            {
                return i;
            }
        }

        return -1;
    }

    private int FindNum(char[] rowCoords)
    {
        var length = (int)Math.Pow(2, rowCoords.Length);
        var range = Enumerable.Range(0, length).ToArray();
        foreach (var coord in rowCoords)
        {
            if (coord is 'F' || coord is 'L')
            {
                range = range[..(range.Length / 2)];
            }
            else
            {
                range = range[(range.Length / 2)..];
            }
        }

        return range[0];
    }

    private static int SeatID(int row, int col)
    {
        return (row * 8) + col;
    }
}