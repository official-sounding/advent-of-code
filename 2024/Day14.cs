[Slug("2024/d14")]
public class Day202414 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var robots = input.Select(Robot.Parse).ToList();

        foreach (var _ in Enumerable.Range(0, 100))
        {
            foreach (var i in Enumerable.Range(0, robots.Count))
            {
                robots[i] = robots[i].Move();
            }
        }

        return SafetyScore(robots);
    }

    public override long RunPartTwo(string[] input)
    {
        var robots = input.Select(Robot.Parse).ToList();

        foreach (var idx in Enumerable.Range(0, int.MaxValue))
        {
            foreach (var i in Enumerable.Range(0, robots.Count))
            {
                robots[i] = robots[i].Move();
            }


            // Assumption: no robots will overlap if they are in the easter egg condition
            if (robots.GroupBy(r => r.p).All(grp => grp.Count() == 1))
            {
                PrintRobots(robots);
                return idx + 1;
            }
        }

        return -1;
    }

    public void PrintRobots(List<Robot> robots)
    {
        var hs = robots.Select(r => r.p).ToHashSet();
        for (int x = 0; x < 101; x++)
        {
            for (int y = 0; y < 103; y++)
            {
                var vale = hs.Contains(new(x, y)) ? '#' : '.';
                Console.Write(vale);
            }
            Console.WriteLine();
        }
    }

    public int SafetyScore(List<Robot> robots)
    {
        var (q1, q2, q3, q4) = (0, 0, 0, 0);

        foreach (var (p, _) in robots)
        {
            switch (p)
            {
                case ( < 50, < 51):
                    q1++;
                    break;
                case ( < 50, > 51):
                    q2++;
                    break;
                case ( > 50, < 51):
                    q3++;
                    break;
                case ( > 50, > 51):
                    q4++;
                    break;
            }
        }

        return q1 * q2 * q3 * q4;
    }

}

public record Robot(Position p, Position v)
{
    public static Robot Parse(string input)
    {
        var stripped = input.Replace("p=", "").Replace("v=", "");
        if (stripped.Split(' ') is [var pStr, var vStr])
        {
            return new(Position.FromString(pStr), Position.FromString(vStr));
        }

        throw new ArgumentException(input);
    }

    public Robot Move()
    {
        return this with
        {
            p = (NumberUtils.Mod(p.X + v.X, 101), NumberUtils.Mod(p.Y + v.Y, 103))
        };
    }
}