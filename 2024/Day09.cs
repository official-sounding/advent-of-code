using System.Text;

[Slug("2024/d09")]
public class Day202409 : Problem
{
    private readonly Block free = new Block(0, true);
    public override long RunPartOne(string[] input)
    {
        var blks = ParseBlocks(input);
        var start = 0;
        var end = blks.Count - 1;

        if(blks.Count < 100)
            Console.WriteLine(string.Join("", blks));

        while(start <= end) {
            // if block is not free, skip forward
            if(!blks[start].free) {
                start++;
                continue;
            }

            // block at "start" is currently free

            // move "end" back until a non-free block
            while(blks[end].free) {
                end--;
            }

            // "swap" blks
            blks[start] = blks[end];
            blks[end] = free;

            start++;
            end--;
        }

        if(blks.Count < 100)
            Console.WriteLine(string.Join("", blks));

        return blks.Where(b => !b.free).Select((b, idx) => (long)(b.id * idx)).Sum();
    }

    public override long RunPartTwo(string[] input)
    {
        var blks = ParseBlocks(input);
        var start = 0;
        var end = blks.Count - 1;

        if(blks.Count < 100)
            Console.WriteLine(string.Join("", blks));

        while(start <= end) {
            // if block is not free, skip forward
            if(!blks[start].free) {
                start++;
                continue;
            }

            // block at "start" is currently free

            // move "end" back until a non-free block
            while(blks[end].free) {
                end--;
            }

            // "swap" blks
            blks[start] = blks[end];
            blks[end] = free;

            start++;
            end--;
        }

        if(blks.Count < 100)
            Console.WriteLine(string.Join("", blks));

        return blks.Where(b => !b.free).Select((b, idx) => (long)(b.id * idx)).Sum();
    }

    private List<Block> ParseBlocks(string[] input) {
        var numbers = input[0].ToCharArray().Select((c) => int.Parse($"{c}"));
        var blks = new List<Block>();
        int id = 0;
        foreach(var (num, idx) in numbers.Select((n,idx) => (n, idx))) {
            Block blk;
            if(idx % 2 == 0) {
                blk = new Block(id, false);
                id++;
            } else {
                blk = free;
            }

            for(int i = 0; i < num; i++) {
                blks.Add(blk);
            }
        }

        return blks;
    }
}

public record Block(int id, bool free) {
    public override string ToString()
    {
        return free ? "." : $"{id}";
    }
}