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

public record GuardState(int X, int Y, GuardOrientation Orientation) { 

    public (int x, int y) Position => (X,Y);
    (int x,int y) OffsetForDirection => Orientation switch {
        GuardOrientation.Up => (0,-1),
        GuardOrientation.Down => (0,1),
        GuardOrientation.Left => (-1, 0),
        GuardOrientation.Right => (1,0),
        _ => (0,0)
    };

    public GuardState Move() {
        var offset = OffsetForDirection;
        return this with { X = X + offset.x, Y = Y + offset.y };
    }

    public GuardState Rotate() {
        return Orientation switch {
            GuardOrientation.Up => this with { Orientation = GuardOrientation.Right},
            GuardOrientation.Right => this with {Orientation = GuardOrientation.Down},
            GuardOrientation.Down => this with { Orientation = GuardOrientation.Left},
            GuardOrientation.Left => this with { Orientation = GuardOrientation.Up},
            _ => this
        };
    }
}

[Slug("2024/d06")]
public class Day202406 : SyncProblem
{
    private Dictionary<char, GuardOrientation> GuardOrientationMap = new() { { '^', GuardOrientation.Up }, { 'v', GuardOrientation.Down }, { '<', GuardOrientation.Left }, { '>', GuardOrientation.Right } };
    private List<GuardState> DistinctStates = new();
    public override string RunPartOneSync(string[] input)
    {
        var (matrix, guard) = ParseMatrix(input);
        DistinctStates.Add(guard);
        var check = CheckMove(matrix, guard);
        while(check != MoveResult.OffMap) {
            switch(check) {
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

        return $"{DistinctStates.Count}";
    }

    public override string RunPartTwoSync(string[] input) {
        var (matrix, initial) = ParseMatrix(input);
        HashSet<(int,int)> loopPositions = [];
        
        foreach(var state in DistinctStates) {
            if(state.Position == initial.Position) {
                continue;
            }
            
            matrix[state.Position] = '#';
            
            if(DoesLoop(matrix, initial)) {
                loopPositions.Add(state.Position);
            }
            matrix[state.Position] = '.';
        }

        return $"{loopPositions.Count}";
    }

    public bool DoesLoop(Dictionary<(int,int), char> matrix, GuardState initial) {
        var stateHash = new HashSet<GuardState>() { initial };
        var guard = initial;
        var check = CheckMove(matrix, guard);
        while(check != MoveResult.OffMap) {
            switch(check) {
                case MoveResult.Visited:
                case MoveResult.Unvisited:
                    guard = guard.Move();
                    if(!stateHash.Add(guard)) {
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

    (Dictionary<(int, int), char>, GuardState) ParseMatrix(string[] lines)
    {
        GuardState guardStart = new(0, 0, GuardOrientation.Up);
        var guardFound = false;
        var matrix = lines.SelectMany((l, y) => l.ToCharArray().Select((n, x) => (x, y, n))).ToDictionary((t) =>
        {
            var (x, y, v) = t;
            if (!guardFound && GuardOrientationMap.TryGetValue(v, out var direction))
            {
                guardStart = new(x, y, direction);
                guardFound = true;
            }
            return (x, y);
        }, (t) => t.n);

        return (matrix, guardStart);
    }

    MoveResult CheckMove(Dictionary<(int, int), char> matrix, GuardState guard) {
        var (x,y, _) = guard.Move();
        if(matrix.TryGetValue((x,y), out var value)) {
            return value switch {
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