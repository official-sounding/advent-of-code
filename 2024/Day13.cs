using System.Security.Cryptography.X509Certificates;

[Slug("2024/d13")]
public class Day202413 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var result = 0L;
        for (long i = 0; i < input.Length; i++)
        {
            var aLine = input[i++];
            var bLine = input[i++];
            var prize = input[i++];

            var scenario = Scenario.Parse(aLine, bLine, prize);
            result += scenario.Cost();
        }

        return result;
    }

    public override long RunPartTwo(string[] input)
    {
        var result = 0L;
        for (long i = 0; i < input.Length; i++)
        {
            var aLine = input[i++];
            var bLine = input[i++];
            var prize = input[i++];

            var s = Scenario.Parse(aLine, bLine, prize);
            var p2 = s with { prize = (s.prize.x + 10000000000000, s.prize.y + 10000000000000) };
            result += p2.Cost();
        }

        return result;
    }

    public record Scenario((long x, long y) a, (long x, long y) b, (long x, long y) prize)
    {
        public static Scenario Parse(string aStr, string bStr, string prizeStr)
        {
            var a = BtnParse(aStr);
            var b = BtnParse(bStr);
            var prize = PrizeParse(prizeStr);

            return new(a, b, prize);
        }

        private static (long, long) BtnParse(string str)
        {
            var stripped = str[10..].Replace("X+", "").Replace("Y+", "");
            if (stripped.Split(',', StringSplitOptions.TrimEntries) is [var x, var y])
            {
                return (Convert.ToInt64(x), Convert.ToInt64(y));
            }
            throw new Exception("!!");
        }

        private static (long, long) PrizeParse(string str)
        {
            var stripped = str[7..].Replace("X=", "").Replace("Y=", "");
            if (stripped.Split(',', StringSplitOptions.TrimEntries) is [var x, var y])
            {
                return (Convert.ToInt64(x), Convert.ToInt64(y));
            }
            throw new Exception("!!");
        }

        public long Cost()
        {
            double D = a.x * b.y - a.y * b.x;
            double Dx = prize.x * b.y - prize.y * b.x;
            double Dy = a.x * prize.y - a.y * prize.x;

            if (D == 0)
            {
                return 0;
            }

            // Cramer's Rule - https://en.wikipedia.org/wiki/Cramer%27s_rule
            double A = Dx / D;
            double B = Dy / D;

            if (Math.Floor(A) != A || Math.Floor(B) != B)
            {
                return 0;
            }

            return (long)A * 3 + (long)B;
        }
    }
}