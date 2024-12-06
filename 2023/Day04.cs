[Slug("2023/d04")]
public class Day04 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var result = input
            .Select(Game.Parse)
            .Sum(game => game.Score);


        return result;
    }

    public override long RunPartTwo(string[] input)
    {
        var games = input.Select(Game.Parse).ToList();

        var result = games.Select((game, idx) => (game, idx)).Sum((t) => PlayWinners(t.game, t.idx, games));
        return result;
    }

    private int PlayWinners(Game game, int idx, List<Game> games)
    {
        var matches = game.Matches;
        if (matches == 0 || idx + 1 == games.Count)
        {
            return game.Copies;
        }

        var result = game.Copies;
        var max = Math.Min(matches, games.Count - idx + 1);
        foreach (var g in games.GetRange(idx + 1, max))
        {
            g.Copies = g.Copies + game.Copies;
        }
        return result;
    }

    private class Game
    {

        public HashSet<int> Winners { get; init; } = new HashSet<int>();
        public List<int> Numbers { get; init; } = new List<int>();

        private int MatchesCache = -1;

        public int Copies { get; set; } = 1;

        public static Game Parse(string line)
        {
            var parts = line.Split(":");
            var allNumbers = parts[1].Split("|");

            return new Game()
            {
                Winners = allNumbers[0].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet(),
                Numbers = allNumbers[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
            };
        }

        public int Matches => MatchesCache >= 0 ? MatchesCache : (MatchesCache = Numbers.Count(n => Winners.Contains(n)));

        public int Score
        {
            get
            {
                var count = Matches;

                if (count == 0)
                {
                    return 0;
                }
                else
                {
                    return (int)Math.Pow(2, count - 1);
                }
            }
        }
    }
}