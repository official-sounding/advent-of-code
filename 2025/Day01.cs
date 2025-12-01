[Slug(2025, 1)]
public class Day202501 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var position = 50;
        var password = 0;
        foreach (var i in input)
        {
            var dir = i[..1];
            var count = int.Parse(i[1..]);

            position = dir == "L" ? position - count : position + count;

            while (position < 0)
            {
                position += 100;
            }

            while (position >= 100)
            {
                position -= 100;
            }

            if (position == 0)
            {
                password++;
            }
        }

        return password;
    }


}