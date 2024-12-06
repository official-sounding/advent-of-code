using System.Text.RegularExpressions;

[Slug("2024/d03")]
public class Day202403 : Problem
{
    private static readonly Regex mulMatcher = new(@"mul\((\d{1,3}),(\d{1,3})\)");
    private static readonly Regex conditionalMulMatcher = new(@"do\(\)|don't\(\)|mul\((\d{1,3}),(\d{1,3})\)");
    public override long RunPartOne(string[] input)
    {
        var result = input.SelectMany(row =>
        {
            var m = mulMatcher.Match(row);
            var nums = new List<int>();
            while (m.Success)
            {
                var num1 = Convert.ToInt32(m.Groups[1].Value);
                var num2 = Convert.ToInt32(m.Groups[2].Value);
                nums.Add(num1 * num2);
                m = m.NextMatch();
            }
            return nums;
        }).Sum();

        return result;
    }

    public override long RunPartTwo(string[] input)
    {
        var enabled = true;
        var result = input.SelectMany(row =>
        {
            var m = conditionalMulMatcher.Match(row);
            var nums = new List<int>();

            while (m.Success)
            {
                if (m.ToString() == "do()")
                {
                    enabled = true;
                }
                else if (m.ToString() == "don't()")
                {
                    enabled = false;
                }
                else if (enabled)
                {
                    var num1 = Convert.ToInt32(m.Groups[1].Value);
                    var num2 = Convert.ToInt32(m.Groups[2].Value);
                    nums.Add(num1 * num2);
                }
                m = m.NextMatch();
            }
            return nums;
        }).Sum();

        return result;
    }
}