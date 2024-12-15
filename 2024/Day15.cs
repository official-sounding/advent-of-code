
[Slug("2024/d15")]
public class Day202415 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var matrix = Matrix.Parse(input.Where(l => l.StartsWith("#")));
        var instructions = input.Where(l => l.Length > 0 && !l.StartsWith("#")).SelectMany(l => l.ToCharArray());

        var robot = matrix.FindPosition('@');

        foreach (var i in instructions)
        {
            robot = MoveRobot(matrix, robot, OffsetFromInstruction(i));
        }

        return ScoreBoxes(matrix);
    }

    public override long RunPartTwo(string[] input)
    {
        var doubled = input.Where(l => l.StartsWith("#")).Select(l =>
        {
            return l.Replace("#", "##").Replace("O", "[]").Replace(".", "..").Replace("@", "@.");
        });


        var matrix = Matrix.Parse(doubled);
        var instructions = input.Where(l => l.Length > 0 && !l.StartsWith("#")).SelectMany(l => l.ToCharArray());

        var robot = matrix.FindPosition('@');


        foreach (var i in instructions)
        {
            robot = MoveRobotPart2(matrix, robot, OffsetFromInstruction(i));
        }

        return ScoreBoxes(matrix);

    }

    private Position MoveRobot(Matrix matrix, Position robot, Position os)
    {
        var next = robot + os;
        if (!matrix.TryGetValue(next, out var nextValue) || nextValue == '#')
        {
            return robot;
        }

        if (nextValue == '.' || (nextValue == 'O' && MoveBox(matrix, next, os)))
        {
            matrix[robot] = '.';
            matrix[next] = '@';
            return next;
        }

        return robot;
    }

    private bool MoveBox(Matrix matrix, Position box, Position os)
    {
        var next = box + os;
        if (!matrix.TryGetValue(next, out var value) || value == '#')
        {
            return false;
        }

        if (value == '.' || (value == 'O' && MoveBox(matrix, next, os)))
        {
            matrix[box] = '.';
            matrix[next] = 'O';
            return true;
        }

        return false;
    }

    private Position MoveRobotPart2(Matrix matrix, Position robot, Position os)
    {
        var next = robot + os;
        if (!matrix.TryGetValue(next, out var nextValue) || nextValue == '#')
        {
            return robot;
        }

        if (nextValue == '.')
        {
            matrix[robot] = '.';
            matrix[next] = '@';
            return next;
        }

        if ("[]".Contains(nextValue) && CanMoveBoxPart2(matrix, next, os))
        {
            ShiftBoxPart2(matrix, next, os);
            matrix[robot] = '.';
            matrix[next] = '@';
            return next;
        }

        return robot;
    }

    private bool CanMoveBoxPart2(Matrix matrix, Position box, Position os)
    {
        var (boxL, boxR) = matrix[box] == '[' ? (box, box + Position.E) : (box + Position.W, box);
        var (nextL, nextR) = (boxL + os, boxR + os);

        if (!matrix.TryGetValue(nextL, out var valueL) || !matrix.TryGetValue(nextR, out var valueR) || valueL == '#' || valueR == '#')
        {
            return false;
        }

        if ((os == Position.W && valueL == '.') || (os == Position.E && valueR == '.') || (valueR == '.' && valueL == '.'))
        {
            return true;
        }

        if ((os == Position.W && CanMoveBoxPart2(matrix, nextL, os)) || (os == Position.E && CanMoveBoxPart2(matrix, nextR, os)))
        {
            return true;
        }

        if (os == Position.N || os == Position.S)
        {
            if ((valueL == '[' && CanMoveBoxPart2(matrix, nextL, os)) ||
               (valueR == ']' && CanMoveBoxPart2(matrix, nextR, os)) ||
               (valueL == ']' && valueR == '.' && CanMoveBoxPart2(matrix, nextL, os)) ||
               (valueR == '[' && valueL == '.' && CanMoveBoxPart2(matrix, nextR, os)) ||
               (CanMoveBoxPart2(matrix, nextL, os) && CanMoveBoxPart2(matrix, nextR, os)))
            {
                return true;
            }
        }

        return false;
    }

    private void ShiftBoxPart2(Matrix matrix, Position box, Position os)
    {
        var (boxL, boxR) = matrix[box] == '[' ? (box, box + Position.E) : (box + Position.W, box);
        var (nextL, nextR) = (boxL + os, boxR + os);

        void ShiftBox()
        {
            matrix[boxL] = '.';
            matrix[boxR] = '.';
            matrix[nextL] = '[';
            matrix[nextR] = ']';
        }

        if (!matrix.TryGetValue(nextL, out var valueL) || !matrix.TryGetValue(nextR, out var valueR) || valueL == '#' || valueR == '#')
        {
            return;
        }

        if ((os == Position.W && valueL == '.') || (os == Position.E && valueR == '.') || (valueR == '.' && valueL == '.'))
        {
            ShiftBox();
            return;
        }

        if (os == Position.W && CanMoveBoxPart2(matrix, nextL, os))
        {
            ShiftBoxPart2(matrix, nextL, os);
            ShiftBox();
            return;
        }

        if (os == Position.E && CanMoveBoxPart2(matrix, nextR, os))
        {
            ShiftBoxPart2(matrix, nextR, os);
            ShiftBox();
            return;
        }

        if (os == Position.N || os == Position.S)
        {
            if ((valueL == '[' && CanMoveBoxPart2(matrix, nextL, os)) ||
               (valueL == ']' && valueR == '.' && CanMoveBoxPart2(matrix, nextL, os)))
            {
                ShiftBoxPart2(matrix, nextL, os);
                ShiftBox();

            }
            else if ((valueR == ']' && CanMoveBoxPart2(matrix, nextR, os)) ||
            (valueR == '[' && valueL == '.' && CanMoveBoxPart2(matrix, nextR, os)))
            {
                ShiftBoxPart2(matrix, nextR, os);
                ShiftBox();
            }
            else if (CanMoveBoxPart2(matrix, nextL, os) && CanMoveBoxPart2(matrix, nextR, os))
            {
                ShiftBoxPart2(matrix, nextR, os);
                ShiftBoxPart2(matrix, nextL, os);
                ShiftBox();
            }
        }

        return;
    }



    private Position OffsetFromInstruction(char i)
    {
        return i switch
        {
            '<' => Position.W,
            '>' => Position.E,
            '^' => Position.N,
            'v' => Position.S,
            _ => throw new Exception($"{i}!!")
        };
    }

    private long ScoreBoxes(Matrix matrix)
    {
        var result = 0;
        foreach (var (pos, value) in matrix.Where(kv => kv.Value == 'O' || kv.Value == '['))
        {
            result += pos.X + (100 * (matrix.MaxY - pos.Y));
        }

        return result;
    }
}