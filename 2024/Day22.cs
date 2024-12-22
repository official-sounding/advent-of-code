[Slug(2024, 22)]
public class Day202422 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var numbers = input.Select(long.Parse).ToList();
        return numbers.Sum((s) => ApplyIteration(s));
    }

    public override long RunPartTwo(string[] input)
    {
        var dict = new Dictionary<(int, int, int, int), int>();
        var numbers = input.Select(long.Parse).ToList();

        numbers.ForEach((s) => ApplyIteration(s, dict));
        return dict.Max(kv => kv.Value);
    }


    long NextPermutation(long secret)
    {
        var output = MixAndPrune(secret * 64L, secret);
        output = MixAndPrune(output / 32L, output);
        output = MixAndPrune(output * 2048L, output);
        return output;
    }

    long MixAndPrune(long a, long b) => (a ^ b) % 16777216;

    long ApplyIteration(long input, Dictionary<(int, int, int, int), int>? seqs = null)
    {
        seqs ??= [];
        var seq = (0, 0, 0, 0);
        var used = new HashSet<(int, int, int, int)>();

        foreach (var i in Enumerable.Range(0, 2000))
        {
            var (_, b, c, d) = seq;
            var newNumber = NextPermutation(input);

            var price = (int)newNumber % 10;
            var diff = price - (int)input % 10;

            seq = (b, c, d, diff);
            input = newNumber;
            // if sequences are fully populated 
            // and this is the first time the sequence is seen
            if (i >= 3 && used.Add(seq))
            {
                if (!seqs.TryAdd(seq, price))
                {
                    seqs[seq] += price;
                }

            }
        }
        return input;
    }
}