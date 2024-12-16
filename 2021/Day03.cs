using System.Text;

[Slug("2021/d03")]
public class Day202103 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var (epsilonStr, gammaStr) = BuildStrings(input, true);

        var epsilon = Convert.ToInt64(epsilonStr.ToString(), 2);
        var gamma = Convert.ToInt64(gammaStr.ToString(), 2);

        return epsilon * gamma;
    }

    public override long RunPartTwo(string[] input)
    {
        // var (epsilonStr, gammaStr) = BuildStrings(input);

        var idx = 0;
        var oxRatingCandidates = input.ToArray();
        while (oxRatingCandidates.Length != 1)
        {
            var (_, gammaStr) = BuildStrings(oxRatingCandidates, true);
            oxRatingCandidates = FilterCandidates(oxRatingCandidates, idx, gammaStr);
            idx++;
        }

        idx = 0;
        var co2RatingCandidates = input.ToArray();
        while (co2RatingCandidates.Length != 1)
        {
            var (epsilonStr, _) = BuildStrings(co2RatingCandidates, true);
            co2RatingCandidates = FilterCandidates(co2RatingCandidates, idx, epsilonStr);
            idx++;
        }

        return Convert.ToInt64(oxRatingCandidates[0], 2) * Convert.ToInt64(co2RatingCandidates[0], 2);
    }

    private string[] FilterCandidates(string[] candidates, int idx, string filterStr)
    {
        return candidates.Where((c) => c[idx] == filterStr[idx]).ToArray();
    }

    private (string epsilon, string gamma) BuildStrings(string[] input, bool gammaBiased)
    {
        var len = input[0].Length;
        var oneCounts = new int[len];

        foreach (var line in input)
        {
            foreach (var (c, idx) in line.ToCharArray().WithIndex())
            {
                if (c == '1')
                {
                    oneCounts[idx]++;
                }
            }
        }

        StringBuilder epsilonStr = new();
        StringBuilder gammaStr = new();

        foreach (var count in oneCounts)
        {
            if (count > (input.Length / 2.0))
            {
                gammaStr.Append('1');
                epsilonStr.Append('0');
            }
            else if (count == (input.Length / 2.0) && gammaBiased)
            {
                gammaStr.Append('1');
                epsilonStr.Append('0');
            }
            else
            {
                epsilonStr.Append('1');
                gammaStr.Append('0');
            }
        }

        return (epsilonStr.ToString(), gammaStr.ToString());
    }
}