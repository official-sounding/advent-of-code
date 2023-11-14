public abstract class Problem {
    public abstract Task<int> RunPartOne(string[] input);
    public virtual Task<int> RunPartTwo(string[] input)
    {
        Console.WriteLine("Part Two Not Implemented");
        return Task.FromResult(0);
    }
}