[Slug("2024/d04")]
public class Day202404 : SyncProblem
{
    private int SearchXMAS(Matrix matrix, Position start)
    {
        return Offset.AllDirections().Sum(o => SearchWord(matrix, start, o));
    }

    private int SearchMAS(Matrix matrix, Position center)
    {
        // left diagonal - MAS or SAM
        var ldValid = false;
        if (matrix.TryGetValue(center.ApplyOffset(Offset.NW), out var ul) && (ul == 'S' || ul == 'M'))
        {
            if (matrix.TryGetValue(center.ApplyOffset(Offset.SE), out var lr))
            {
                if ((ul == 'S' && lr == 'M') || (ul == 'M' && lr == 'S'))
                {
                    ldValid = true;
                }
            }
        }

        if (!ldValid) { return 0; }

        // right diagonal
        var rdValid = false;
        if (matrix.TryGetValue(center.ApplyOffset(Offset.NE), out var ur) && (ur == 'S' || ur == 'M'))
        {
            if (matrix.TryGetValue(center.ApplyOffset(Offset.SW), out var ll))
            {
                if ((ur == 'S' && ll == 'M') || (ur == 'M' && ll == 'S'))
                {
                    rdValid = true;
                }
            }
        }

        return rdValid ? 1 : 0;
    }

    private int SearchWord(Matrix matrix, Position start, Offset offset)
    {
        if (matrix.TryGetValue(start, out var n1)
            && matrix.TryGetValue(start.ApplyOffset(offset), out var n2)
            && matrix.TryGetValue(start.ApplyOffset(offset, 2), out var n3)
            && matrix.TryGetValue(start.ApplyOffset(offset, 3), out var n4))
        {
            return $"{n1}{n2}{n3}{n4}" == "XMAS" ? 1 : 0;
        }

        return 0;
    }

    public override string RunPartOneSync(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var count = 0;
        foreach (var ((x, y), value) in matrix)
        {
            if (value == 'X')
            {
                count += SearchXMAS(matrix, new(x, y));
            }
        }

        return $"{count}";
    }

    public override string RunPartTwoSync(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var count = 0;
        foreach (var ((x, y), value) in matrix)
        {
            if (value == 'A')
            {
                count += SearchMAS(matrix, new(x, y));
            }
        }

        return $"{count}";
    }
}