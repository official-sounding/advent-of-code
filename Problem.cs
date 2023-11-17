public abstract class Problem {
    public abstract Task<int> RunPartOne(string[] input);
    public virtual Task<int> RunPartTwo(string[] input)
    {
        Console.WriteLine("Part Two Not Implemented");
        return Task.FromResult(0);
    }
}

public abstract class SyncProblem : Problem {
    public abstract int RunPartOneSync(string[] input);
    public virtual int RunPartTwoSync(string[] input) => 0;

    public override Task<int> RunPartOne(string[] input) {
        return Task.FromResult(RunPartOneSync(input));
    }

    public override Task<int> RunPartTwo(string[] input) {
        return Task.FromResult(RunPartTwoSync(input));
    }
}