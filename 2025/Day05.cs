[Slug(2025, 05)]
public class Day202505 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var db = Database.Parse(input);
        var fresh = 0;

        foreach (var ing in db.Ingredients)
        {
            foreach (var range in db.Ranges)
            {
                if (ing >= range.Start && ing <= range.End)
                {
                    fresh++;
                    break;
                }
            }
        }

        return fresh;
    }

    public override long RunPartTwo(string[] input)
    {
        var db = Database.Parse(input);
        var fresh = 0L;
        foreach (var (range, idx) in db.Ranges.WithIndex())
        {
            var (start, end) = range;
            foreach (var (os, oe) in db.Ranges.Take(idx))
            {
                // if (os < start && oe > end)
                // {
                //     start = -1;
                //     end = 0;
                //     break;
                // }

                if (start < oe && os < start)
                {
                    start = oe + 1;
                }

                if (end > os && oe > start)
                {
                    end = os - 1;
                }
            }

            Console.WriteLine($"* ({start},{end})");



            fresh += end - start + 1;

        }

        return fresh;
    }

    private class Database
    {
        public List<LongRange> Ranges { get; private set; } = [];
        public List<long> Ingredients { get; } = [];

        public static Database Parse(string[] input)
        {
            var result = new Database();

            var ingredient = false;
            foreach (var row in input)
            {
                if (string.IsNullOrEmpty(row))
                {
                    ingredient = true;
                    continue;
                }

                if (ingredient)
                {
                    result.Ingredients.Add(long.Parse(row));
                }
                else
                {
                    result.Ranges.Add(NumberUtils.SplitRangeString(row));
                }
            }

            return result;
        }
    }
}

