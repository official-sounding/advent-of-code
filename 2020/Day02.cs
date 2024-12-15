[Slug("2020/d02")]
public class Day202002 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var passwords = input.Select(x =>
        {
            var parts = x.Split(": ");
            var policy = parts[0].Split(" ");
            var counts = policy[0].Split("-").Select(int.Parse).ToArray();

            return new Password(policy[1], counts[0], counts[1], parts[1]);
        }).ToList();

        return passwords.Count(p => p.IsValidPart1());
    }

    public override long RunPartTwo(string[] input)
    {
        var passwords = input.Select(x =>
        {
            var parts = x.Split(": ");
            var policy = parts[0].Split(" ");
            var counts = policy[0].Split("-").Select(int.Parse).ToArray();

            return new Password(policy[1], counts[0], counts[1], parts[1]);
        }).ToList();

        return passwords.Count(p => p.IsValidPart2());
    }
}


public record Password(string letter, int min, int max, string password)
{
    public bool IsValidPart1() { var count = password.Where(c => $"{c}" == letter).Count(); return count >= min && count <= max; }

    public bool IsValidPart2() => password.Where((c, idx) => (idx == (min - 1) || idx == (max - 1)) && $"{c}" == letter).Count() == 1;
}