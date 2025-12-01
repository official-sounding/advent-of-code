[Slug(2025, 1)]
public class Day202501 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var position = 50;
        var password = 0;
        foreach (var i in input)
        {
            var distance = Distance(i);

            position = Rotate(position, distance);
            if(position == 0)
            {
                password++;
            }
        }

        return password;
    }

    public override long RunPartTwo(string[] input)
    {
        var position = 50;
        var password = 0;
        foreach (var i in input)
        {
            var distance = Distance(i);
            password += CountZeroPasses(position, distance);
            position = Rotate(position, distance);
        }

        return password;
    }

    private static int CountZeroPasses(int startValue, int distance)
    {
        var step = distance > 0 ? 1 : -1;
        var position = startValue;
        var count = 0;
        foreach (var _ in Enumerable.Range(0, Math.Abs(distance)))
        {
            position = EnsureInBounds(position + step);
            if (position == 0)
            {
                count++;
            }
        }

        return count;
    }


    private static int Rotate(int startValue, int distance)
    {
        var position = startValue + distance;
        return EnsureInBounds(position);
    }

    private static int EnsureInBounds(int position)
    {
        while (position < 0)
        {
            position += 100;
        }

        while (position >= 100)
        {
            position -= 100;
        }

        return position;
    }

    private static int Distance(string step)
    {
        var dir = step[..1];
        var count = int.Parse(step[1..]);
        return (dir == "R" ? 1 : -1) * count;
    }
}
