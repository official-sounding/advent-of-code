public record LongRange(long Start, long End)
{
    public bool Contains(long value)
        => value >= Start && value <= End;

    public bool Overlaps(LongRange range)
        => !(range.End < Start || range.Start > End);

    public Range Merge(LongRange range)
        => new(Math.Min(Start, range.Start), Math.Max(End, range.End));

    public long Length()
        => End - Start + 1;
}