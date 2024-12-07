[Slug("2023/d07")]
public class Day202307 : Problem
{
    private char[] cardOrder = ['2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A'];
    private char[] cardOrderithJoker = ['J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A'];
    private Dictionary<char, int> cardRank;
    private Dictionary<char, int> cardRankWithJoker;

    public Day202307()
    {
        cardRank = cardOrder.Select((c, idx) => (c, idx)).ToDictionary((k) => k.c, k => k.idx);
        cardRankWithJoker = cardOrder.Select((c, idx) => (c, idx)).ToDictionary((k) => k.c, k => k.idx);
    }

    public override long RunPartOne(string[] input)
    {
        return input.Select(r => r.Split(" ")).Select(parts =>
        {
            var hand = parts[0];
            var bid = Convert.ToInt32(parts[1]);
            var type = CalcHandType(hand);
            var sort = CalcHandSort(hand, false);

            return new Hand(hand, sort, bid);
        })
            .OrderBy(h => h.sort)
            .Select((h, idx) => (h, idx + 1))
            .Sum(h => h.h.bid * h.Item2);
    }

    HandType CalcHandType(string hand)
    {
        var groups = hand.GroupBy(c => c).Select(grp => grp.Count()).OrderDescending().ToArray();
        var groupSize = groups[0];
        if (groupSize == 5)
        {
            return HandType.FiveOfAKind;
        }
        if (groupSize == 4)
        {
            return HandType.FourOfAKind;
        }
        if (groupSize == 3)
        {
            return groups[1] == 2 ? HandType.FullHouse : HandType.ThreeOfAKind;
        }
        if (groupSize == 2)
        {
            return groups[1] == 2 ? HandType.TwoPair : HandType.OnePair;
        }

        return HandType.HighCard;
    }

    int CalcHandSort(string hand, bool withJoker)
    {
        var result = (int)CalcHandType(hand);
        foreach (var c in hand)
        {
            result <<= 4;
            result |= cardRank[c];

        }
        return result;
    }
}

public record Hand(string hand, int sort, int bid);


public enum HandType
{
    FiveOfAKind = 7,
    FourOfAKind = 6,
    FullHouse = 5,
    ThreeOfAKind = 4,
    TwoPair = 3,
    OnePair = 2,
    HighCard = 1,
}