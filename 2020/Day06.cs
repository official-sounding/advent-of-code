[Slug("2020/d06")]
public class Day202006 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var sum = 0;
        var group = new HashSet<char>();
        foreach (var line in input)
        {
            if (line.Length == 0)
            {
                sum += group.Count;
                group = [];
                continue;
            }

            foreach (var c in line.ToCharArray())
            {
                group.Add(c);
            }
        }

        sum += group.Count;

        return sum;
    }

    public override long RunPartTwo(string[] input)
    {
        var sum = 0;
        var idx = 0;
        var group = new char[0];
        foreach (var line in input)
        {
            if (line.Length == 0)
            {
                sum += group.Length;
                idx = 0;
                group = [];
                continue;
            }

            if (idx == 0)
            {
                group = line.ToCharArray();
                idx++;
            }
            else
            {
                group = line.ToCharArray().Intersect(group).ToArray();
            }
        }

        sum += group.Length;

        return sum;
    }
}