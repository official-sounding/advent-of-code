[Slug("2023/d06")]
public class Day202306 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var times = input[0].Replace("Time: ", "").Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(s => Convert.ToInt32(s)).ToArray();
        var distances = input[1].Replace("Distance:", "").Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(s => Convert.ToInt32(s)).ToArray();


        var result = 1;
        foreach (var (time, idx) in times.Select((t, i) => (t, i)))
        {
            var record = distances[idx];
            var holdTimes = Enumerable.Range(1, time - 1);

            result = result * holdTimes.Select(holdTime =>
            {
                var runTime = time - holdTime;
                return runTime * holdTime;
            }).Count(dist => dist > record);
        }

        return result;
    }

    public override long RunPartTwo(string[] input)
    {
        var time = Convert.ToInt32(input[0].Replace("Time: ", "").Replace(" ", ""));
        var record = Convert.ToInt64(input[1].Replace("Distance:", "").Replace(" ", ""));

        var holdTimes = Enumerable.Range(1, time - 1);
        return holdTimes.Select(holdTime =>
            {
                var runTime = time - holdTime;
                return (long)runTime * (long)holdTime;
            }).Count(dist => dist > record);
    }
}