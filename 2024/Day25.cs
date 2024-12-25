[Slug(2024, 25)]
public class Day202425 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var (locks, keys) = ParseInput(input);
        var count = 0;

        foreach (var l in locks)
        {
            foreach (var k in keys)
            {
                var valid = true;
                foreach (var x in k.XValues)
                {
                    var y = 0;
                    while (k[(x, y)] == '#')
                    {
                        if (l[(x, y)] != '.')
                        {
                            valid = false;
                            break;
                        }
                        y++;
                    }
                }

                if (valid)
                {
                    count++;
                }
            }
        }

        return count;
    }

    (List<Matrix> locks, List<Matrix> keys) ParseInput(string[] input)
    {
        var locks = new List<Matrix>();
        var keys = new List<Matrix>();

        List<string> toParse = [];

        void MatrixComplete()
        {
            var matrix = Matrix.Parse(toParse);
            toParse = [];

            if (matrix[(0, 0)] == '.')
            {
                locks.Add(matrix);
            }
            else
            {
                keys.Add(matrix);
            }
        }

        foreach (var line in input)
        {
            if (line == string.Empty)
            {
                MatrixComplete();
            }
            else
            {
                toParse.Add(line);
            }
        }

        MatrixComplete();

        return (locks, keys);
    }
}