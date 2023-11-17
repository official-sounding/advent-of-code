public abstract class Problem
{
    public abstract Task<string> RunPartOne(string[] input);
    public virtual Task<string> RunPartTwo(string[] input)
    {
        Console.WriteLine("Part Two Not Implemented");
        return Task.FromResult(string.Empty);
    }
}

public abstract class SyncProblem : Problem
{
    public abstract string RunPartOneSync(string[] input);
    public virtual string RunPartTwoSync(string[] input) => string.Empty;

    public override Task<string> RunPartOne(string[] input)
    {
        return Task.FromResult(RunPartOneSync(input));
    }

    public override Task<string> RunPartTwo(string[] input)
    {
        return Task.FromResult(RunPartTwoSync(input));
    }
}