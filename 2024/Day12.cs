[Slug("2024/d12")]
public class Day202412 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var regions = FindRegions(matrix);

        var result = 0;

        foreach (var (plant, region) in regions)
        {
            var fencePerimeter = RegionPerimeter(matrix, plant, region);
            var fenceCost = region.Count * fencePerimeter;
            result += fenceCost;
        }
        return result;
    }

    public override long RunPartTwo(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var regions = FindRegions(matrix);

        var result = 0;

        foreach (var (plant, region) in regions)
        {
            var fencePerimeter = RegionSides(matrix, plant, region);
            var fenceCost = region.Count * fencePerimeter;
            result += fenceCost;
        }
        return result;
    }

    List<(char, HashSet<Position>)> FindRegions(Matrix matrix)
    {
        var result = new List<(char, HashSet<Position>)>();
        var allVisited = new HashSet<Position>();

        foreach (var (pos, value) in matrix)
        {
            if (allVisited.Contains(pos))
            {
                continue;
            }

            var region = new HashSet<Position>() { pos };
            VisitNeighbors(matrix, pos, allVisited, region);
            result.Add((value, region));

        }

        return result;
    }

    void VisitNeighbors(Matrix matrix, Position pos, HashSet<Position> allVisited, HashSet<Position> region)
    {
        var value = matrix[pos];
        foreach (var dir in Position.CardinalDirections())
        {
            var candidate = pos + dir;
            if (matrix.ValidPosition(candidate) && !allVisited.Contains(candidate) && matrix[candidate] == value)
            {
                region.Add(candidate);
                allVisited.Add(candidate);
                VisitNeighbors(matrix, candidate, allVisited, region);
            }
        }
    }

    int RegionPerimeter(Matrix matrix, char value, HashSet<Position> region)
    {
        var sum = 0;
        foreach (var pos in region)
        {
            foreach (var dir in Position.CardinalDirections())
            {
                var can = pos + dir;
                if (!matrix.TryGetValue(can, out var plant) || plant != value)
                {
                    sum++;
                }
            }
        }

        return sum;
    }

    int RegionSides(Matrix matrix, char value, HashSet<Position> region)
    {
        var result = 0;

        foreach (var dir in Position.CardinalDirections())
        {
            HashSet<Position> remaining = [.. region];
            while (remaining.Count > 0)
            {
                var head = remaining.First();
                if (ScanSide(matrix, value, dir, remaining, head))
                {
                    result++;
                }
            }
        }

        return result;
    }

    bool ScanSide(Matrix matrix, char value, Position dir, HashSet<Position> region, Position node)
    {
        if (!region.Remove(node)) 
        { 
            return false; 
        }

        // return false if not on the edge defined by `dir`
        if (matrix.TryGetValue(node + dir, out var plant) && plant == value)
        {
            return false;
        }


        // if we are on that edge, scan in either direction to remove all other elements of that edge
        if (dir == Position.N || dir == Position.S)
        {
            ScanSide(matrix, value, dir, region, node + Position.W);
            ScanSide(matrix, value, dir, region, node + Position.E);
        }
        else
        {
            ScanSide(matrix, value, dir, region, node + Position.N);
            ScanSide(matrix, value, dir, region, node + Position.S);
        }

        return true;
    }
}