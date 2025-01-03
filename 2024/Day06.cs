public enum GuardOrientation
{
    Up,
    Down,
    Left,
    Right
}

public enum MoveResult
{
    Unvisited,
    Visited,
    Obstructed,
    OffMap
}

public record GuardState(int X, int Y, GuardOrientation Orientation)
{
    public Position Position => new(X, Y);
    Position OffsetForDirection => Orientation switch
    {
        GuardOrientation.Up => Position.N,
        GuardOrientation.Down => Position.S,
        GuardOrientation.Left => Position.W,
        GuardOrientation.Right => Position.E,
        _ => Position.Nil
    };

    public GuardState Move()
    {
        var Position = OffsetForDirection;
        return this with { X = X + Position.X, Y = Y + Position.Y };
    }

    public GuardState Rotate()
    {
        return Orientation switch
        {
            GuardOrientation.Up => this with { Orientation = GuardOrientation.Right },
            GuardOrientation.Right => this with { Orientation = GuardOrientation.Down },
            GuardOrientation.Down => this with { Orientation = GuardOrientation.Left },
            GuardOrientation.Left => this with { Orientation = GuardOrientation.Up },
            _ => this
        };
    }
}

[Slug(2024, 06)]
public class Day202406 : Problem
{
    private Dictionary<char, GuardOrientation> GuardOrientationMap = new() { { '^', GuardOrientation.Up }, { 'v', GuardOrientation.Down }, { '<', GuardOrientation.Left }, { '>', GuardOrientation.Right } };
    private List<GuardState> DistinctStates = new();
    public override long RunPartOne(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var guard = FindGuard(matrix);

        DistinctStates.Add(guard);
        var check = CheckMove(matrix, guard);
        while (check != MoveResult.OffMap)
        {
            switch (check)
            {
                case MoveResult.Visited:
                    guard = guard.Move();
                    break;
                case MoveResult.Unvisited:
                    guard = guard.Move();
                    DistinctStates.Add(guard);
                    matrix[guard.Position] = 'X';
                    break;
                case MoveResult.Obstructed:
                    guard = guard.Rotate();
                    break;
            }
            check = CheckMove(matrix, guard);
        }

        return DistinctStates.Count;
    }

    public override long RunPartTwo(string[] input)
    {
        var matrix = Matrix.Parse(input);
        var initial = FindGuard(matrix);

        HashSet<Position> loopPositions = [];

        foreach (var state in DistinctStates)
        {
            if (state.Position == initial.Position)
            {
                continue;
            }

            matrix[state.Position] = '#';

            if (DoesLoop(matrix, initial))
            {
                loopPositions.Add(state.Position);
            }
            matrix[state.Position] = '.';
        }

        return loopPositions.Count;
    }

    public bool DoesLoop(Matrix matrix, GuardState initial)
    {
        var stateHash = new HashSet<GuardState>() { initial };
        var guard = initial;
        var check = CheckMove(matrix, guard);
        while (check != MoveResult.OffMap)
        {
            switch (check)
            {
                case MoveResult.Visited:
                case MoveResult.Unvisited:
                    guard = guard.Move();
                    if (!stateHash.Add(guard))
                    {
                        return true;
                    }
                    break;
                case MoveResult.Obstructed:
                    guard = guard.Rotate();
                    break;
            }

            check = CheckMove(matrix, guard);
        }

        return false;
    }

    GuardState FindGuard(Matrix matrix)
    {
        var ((x, y), value) = matrix.First((kv) => GuardOrientationMap.ContainsKey(kv.Value));
        return new(x, y, GuardOrientationMap[value]);
    }

    MoveResult CheckMove(Matrix matrix, GuardState guard)
    {
        var (x, y, _) = guard.Move();
        if (matrix.TryGetValue(guard.Position, out var value))
        {
            return value switch
            {
                '.' => MoveResult.Unvisited,
                '^' => MoveResult.Visited,
                'X' => MoveResult.Visited,
                '#' => MoveResult.Obstructed,
                _ => throw new Exception($"{value}??")
            };
        }

        return MoveResult.OffMap;
    }
}