[Slug("2024/d04")]
public class Day202404 : SyncProblem
{
    private int SearchXMAS(Dictionary<(int,int), char> matrix, (int,int) start) {
        int count = 0;
        
        count += SearchWord(matrix, start, (1,0));
        count += SearchWord(matrix, start, (0,1));
        count += SearchWord(matrix, start, (-1,0));
        count += SearchWord(matrix, start, (0,-1));
        count += SearchWord(matrix, start, (1,1));
        count += SearchWord(matrix, start, (-1,-1));
        count += SearchWord(matrix, start, (-1,1));
        count += SearchWord(matrix, start, (1,-1));
        

        return count;   
    }

    private int SearchMAS(Dictionary<(int,int), char> matrix, (int,int) center) {
        // left diagonal - MAS or SAM
        var ldValid = false;
        if(matrix.TryGetValue(ApplyOffset(center, (-1,1)), out var ul) && (ul == 'S' || ul == 'M')) {
            if(matrix.TryGetValue(ApplyOffset(center, (1,-1)), out var lr)) {
                if((ul == 'S' && lr == 'M') || (ul == 'M' && lr == 'S')) {
                    ldValid = true;
                }
            }
        }

        if(!ldValid) { return 0; }

        // right diagonal
        var rdValid = false;
        if(matrix.TryGetValue(ApplyOffset(center, (1,1)), out var ur) && (ur == 'S' || ur == 'M')) {
            if(matrix.TryGetValue(ApplyOffset(center, (-1,-1)), out var ll)) {
                if((ur == 'S' && ll == 'M') || (ur == 'M' && ll == 'S')) {
                    rdValid = true;
                }
            }
        }

        return rdValid ? 1 : 0;
    }

    private (int,int) ApplyOffset((int x, int y) start, (int x, int y) offset, int num = 1)
    {
        return (start.x + num * offset.x, start.y + num * offset.y);
    }
    
    private int SearchWord(Dictionary<(int, int), char> matrix, (int x,int y) start, (int x,int y) offset)
    {
        if(matrix.TryGetValue(start, out var n1) && matrix.TryGetValue(ApplyOffset(start, offset, 1), out var n2) && matrix.TryGetValue(ApplyOffset(start, offset, 2), out var n3) && matrix.TryGetValue(ApplyOffset(start, offset, 3), out var n4))
        {
            return $"{n1}{n2}{n3}{n4}" == "XMAS" ? 1 : 0;
        }

        return 0;
    }

    public Dictionary<(int, int), char> ParseMatrix(string[] lines)
        {
            return lines.SelectMany((l, row) => l.ToCharArray().Select((n, col) => (row, col, n))).ToDictionary((t) =>
            {
                var (row, col, _) = t;
                return (row, col);
            }, (t) => t.n);

        }

    public override string RunPartOneSync(string[] input)
    {
        var matrix = ParseMatrix(input);
        var count = 0;

        for(int x = 0; x < input.Length; x++) {
            for(int y = 0; y < input.Length; y++) {
                if(matrix[(x,y)] == 'X') {
                    count += SearchXMAS(matrix, (x,y));
                }
            }
        }

        return $"{count}";
    }

    public override string RunPartTwoSync(string[] input)
    {
        var matrix = ParseMatrix(input);
        var count = 0;
        for(int x = 0; x < input.Length; x++) {
            for(int y = 0; y < input.Length; y++) {
                if(matrix[(x,y)] == 'A') {
                    count += SearchMAS(matrix, (x,y));
                }
            }
        }

        return $"{count}";
    }
}