using System.Text;

[Slug(2024, 09)]
public class Day202409 : Problem
{
    private readonly Block free = new Block(0, true);
    public override long RunPartOne(string[] input)
    {
        var blks = ParseBlocks(input);
        var start = 0;
        var end = blks.Count - 1;

        if (blks.Count < 100)
            Console.WriteLine(string.Join("", blks));

        while (start <= end)
        {
            // if block is not free, skip forward
            if (!blks[start].free)
            {
                start++;
                continue;
            }

            // block at "start" is currently free

            // move "end" back until a non-free block
            while (blks[end].free)
            {
                end--;
            }

            // "swap" blks
            blks[start] = blks[end];
            blks[end] = free;

            start++;
            end--;
        }

        if (blks.Count < 100)
            Console.WriteLine(string.Join("", blks));

        return blks.Where(b => !b.free).Select((b, idx) => (long)(b.id * idx)).Sum();
    }

    public override long RunPartTwo(string[] input)
    {
        var blks = ParseBlocks(input).Reverse<Block>().ToList();

        var start = 0;


        if (blks.Count < 100)
            Console.WriteLine(string.Join("", blks));

        while (start <= blks.Count - 1)
        {
            // if block is free, skip forward
            if (blks[start].free)
            {
                start++;
                continue;
            }

            var blk = blks[start];
            var (size, res) = FindBlockSize(blks, start, 1);

            var end = blks.Count - 1;


            while (end > res)
            {
                if (!blks[end].free)
                {
                    end--;
                    continue;
                }

                var (freeSize, freeEnd) = FindBlockSize(blks, end, -1);


                if (freeSize >= size)
                {
                    for (int i = 0; i < size; i++)
                    {
                        blks[end - i] = blk;
                    }

                    for (int i = start; i < res; i++)
                    {
                        blks[i] = free;
                    }
                    break;
                }
                end = freeEnd;
            }

            start = res;
        }

        LogUnderTest(blks, string.Join(string.Empty, string.Join("", blks.Reverse<Block>())));
        return blks.Reverse<Block>().Select((b, idx) => (long)(b.free ? 0 : b.id * idx)).Sum();
    }

    private (int size, int endIdx) FindBlockSize(List<Block> blks, int start, int dir = 1)
    {
        var idx = start;
        // find size of the block
        var id = blks[start].id;
        var size = 0;
        while (idx >= 0 && idx < blks.Count && blks[idx].id == id)
        {
            size++;
            idx += dir;
        }

        return (size, idx);
    }

    private List<Block> ParseBlocks(string[] input)
    {
        var numbers = input[0].ToCharArray().Select((c) => int.Parse($"{c}"));
        var blks = new List<Block>();
        int id = 0;
        foreach (var (num, idx) in numbers.Select((n, idx) => (n, idx)))
        {
            Block blk;
            if (idx % 2 == 0)
            {
                blk = new Block(id, false);
                id++;
            }
            else
            {
                blk = free;
            }

            for (int i = 0; i < num; i++)
            {
                blks.Add(blk);
            }
        }

        return blks;
    }

    private void LogUnderTest(List<Block> blks, string msg)
    {
        if (blks.Count < 100)
            Console.WriteLine(msg);
    }
}

public record Block(int id, bool free)
{
    public override string ToString()
    {
        return free ? "." : $"{id % 10}";
    }
}


public class FileBlock(int id, int size, int freeSpace)
{
    public int Id { get; } = id;
    public int Size { get; set; } = size;
    public int FreeSpace { get; set; } = freeSpace;

    public override string ToString()
    {
        var id = $"{Id}".ToCharArray()[^1];
        var file = new string(id, Size);
        var fs = new string('.', FreeSpace);
        return $"{file}{fs}";
    }
}