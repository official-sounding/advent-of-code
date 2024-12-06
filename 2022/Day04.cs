[Slug("2022/d04")]
public class Day202204 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var result = input.Select(Parse).Where(FullyContains).Count();
        return result;
    }

    public override long RunPartTwo(string[] input)
    {
        var result = input.Select(Parse).Where(AnyOverlap).Count();
        return result;
    }

    static ((int, int), (int, int)) Parse(string line)
    {
        var pairs = line.Split(",");
        var firstAssign = pairs[0].Split("-").Select((s) => Convert.ToInt32(s)).ToList();
        var secondAssign = pairs[1].Split("-").Select((s) => Convert.ToInt32(s)).ToList();

        return ((firstAssign[0], firstAssign[1]), (secondAssign[0], secondAssign[1]));
    }

    static bool FullyContains(((int, int), (int, int)) assignments)
    {
        var (first, second) = assignments;

        var (fStart, fEnd) = first;
        var (sStart, sEnd) = second;

        return (fStart >= sStart && fEnd <= sEnd) || (fStart <= sStart && fEnd >= sEnd);
    }

    static bool AnyOverlap(((int, int), (int, int)) assignments)
    {
        var (first, second) = assignments;

        var (fStart, fEnd) = first;
        var (sStart, sEnd) = second;

        // *234****
        // **345***
        // 123*****
        // ******67

        return !(sStart > fEnd || fStart > sEnd);
    }
}