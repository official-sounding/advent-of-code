using System.Text.RegularExpressions;

[Slug(2023, 02)]
public class Day02 : Problem
{

    public override long RunPartOne(string[] input)
    {
        int result = 0;

        foreach (var line in input)
        {
            var game = Game.Parse(line);

            if (game.GetMax("red") <= 12 && game.GetMax("green") <= 13 && game.GetMax("blue") <= 14)
            {
                result = result + game.Id;
            }
        }

        return result;
    }

    public override long RunPartTwo(string[] input)
    {
        int result = 0;

        foreach (var line in input)
        {
            var game = Game.Parse(line);

            var power = game.GetMax("red") * game.GetMax("green") * game.GetMax("blue");
            result = result + power;
        }

        return result;
    }

    private class Game
    {
        public int Id { get; init; }
        public List<Attempt> Attempts { get; } = new List<Attempt>();

        public static Game Parse(string line)
        {
            var parts = line.Split(":", StringSplitOptions.TrimEntries);

            var id = int.Parse(parts[0].Replace("Game ", string.Empty));

            var result = new Game() { Id = id };
            var attempts = parts[1].Split(";", StringSplitOptions.TrimEntries);
            result.Attempts.AddRange(attempts.Select(Attempt.Parse));
            return result;
        }

        public int GetMax(string color)
        {
            return Attempts.Max(a => a.Pulls.ContainsKey(color) ? a.Pulls[color] : 0);
        }
    }

    private class Attempt
    {
        public Dictionary<string, int> Pulls { get; } = new Dictionary<string, int>();

        public static Attempt Parse(string attemptStr)
        {
            var result = new Attempt();
            var cubes = attemptStr.Split(",", StringSplitOptions.TrimEntries);

            foreach (var cube in cubes)
            {
                var parts = cube.Split(" ", StringSplitOptions.TrimEntries);

                var number = int.Parse(parts[0]);
                var color = parts[1];

                result.Pulls.Add(color, number);
            }

            return result;
        }
    }
}