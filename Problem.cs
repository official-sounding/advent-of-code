public abstract class AsyncProblem
{
    public bool ExampleMode { get; set; }
    public abstract Task<string> RunPartOneAsync(string[] input);
    public virtual Task<string> RunPartTwoAsync(string[] input)
    {
        Console.WriteLine("Part Two Not Implemented");
        return Task.FromResult(string.Empty);
    }
}

public abstract class Problem : AsyncProblem
{
    public abstract long RunPartOne(string[] input);
    public virtual long RunPartTwo(string[] input) => -1;

    public override Task<string> RunPartOneAsync(string[] input)
    {
        return Task.FromResult($"{RunPartOne(input)}");
    }

    public override Task<string> RunPartTwoAsync(string[] input)
    {
        var result = RunPartTwo(input);
        var str = result == -1 ? string.Empty : $"{result}";
        return Task.FromResult(str);
    }
}