using System.Text.RegularExpressions;

[Slug("2023/d01")]
public class Day01 : SyncProblem
{
    private Dictionary<string, string> spelledOut = new Dictionary<string, string>{
        {"one", "o1e"},
        {"two", "t2o"},
        {"three","t3e"},
        {"four", "f4r"},
        {"five", "f5e"},
        {"six", "s6x"},
        {"seven", "s7n"},
        {"eight", "e8t"},
        {"nine", "n9e"}
    };

    private static Regex regex = new Regex("[a-z:]+");
    public override string RunPartOneSync(string[] input)
    {
        var result = input
            .Select(line => regex.Replace(line, string.Empty))
            .Select(line => $"{line.First()}{line.Last()}")
            .Select(int.Parse)
            .Sum();

        return $"{result}";
    }

    public override string RunPartTwoSync(string[] input)
    {
     var result = input
            .Select(ReplaceSpelledOutNumbers)
            .Select(line => regex.Replace(line, string.Empty))
            .Select(line => $"{line.First()}{line.Last()}")
            .Select(int.Parse)
            .Sum();

        return $"{result}";   
    }

    private string ReplaceSpelledOutNumbers(string line) {
        var result = line;
        foreach(var (str, num) in spelledOut) {
            result = result.Replace(str, $"{num}");
        }
        
        return result;
    }


}