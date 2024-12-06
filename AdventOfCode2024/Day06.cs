

using System.Text;

namespace AdventOfCode2024;

public class Day06 : IDaySolution
{

    public int? SolvePart1()
    {
        var map = ParseInput();

        var isGuardOutOfBounds = map.IsGuardOutOfBounds();
        while (!isGuardOutOfBounds) {
            map.MoveGuard();
            isGuardOutOfBounds = map.IsGuardOutOfBounds();
        }
        
        Console.WriteLine($"{map}");
        
        return map.PathCount();
    }

    public int? SolvePart2()
    {
        return null;
    }

    private static Map ParseInput()
    {
        var lines = File.ReadAllLines("inputs/day6.txt");

        var charAry = lines.Select(line =>
            line.ToCharArray()
        ).ToArray();

        return new Map(charAry);
    }
}

internal class Map
{
    private static readonly char[] GuardTypes = ['^', 'V', '>', '<'];

    public char[][] charAry;
    public Guard guard;

    public Map(char[][] map)
    {
        charAry = map;
        guard = GetGuard();
    }

    public bool IsGuardOutOfBounds() => IsPositionOutOfBounds(guard.X, guard.Y);

    private bool IsPositionOutOfBounds(int x, int y) => x < 0 || y < 0 || x >= charAry.Length || y >= charAry[0].Length;

    public void MoveGuard() {
        var (x,y) = guard.GetPositionAhead();
        if (IsPositionOutOfBounds(x,y)) {
            charAry[guard.X][guard.Y] = guard.GetTrailType(false);
            guard.X = x;
            guard.Y = y;
            return;
        }
        var valAtPos = charAry[x][y];
        if (valAtPos == '#') {
            charAry[guard.X][guard.Y] = '+';
            guard.RotateRight();
        }
        else {
            charAry[x][y] = guard.GetTrailType(valAtPos != '.');
            guard.X = x;
            guard.Y = y;
        }
    }

    public override string ToString()
    {
        var s = "";
        foreach (var line in charAry)
        {
            foreach (var character in line)
            {
                s += character;
            }
            s += "\n";
        }
        return s;
    }

    private Guard GetGuard()
    {
        for (int i = 0; i < charAry.Length; i++)
        {
            for (int j = 0; j < charAry[i].Length; j++)
            {
                foreach (var type in GuardTypes)
                {
                    if (type == charAry[i][j])
                    {
                        return new Guard(i,j, type);
                    }
                }
            }
        }
        throw new Exception("Cannot find Guard");
    }

    internal int? PathCount()
    {
        var count = 0;
        foreach (var line in charAry)
        {
            foreach (var character in line)
            {
                if (character != '.' && character != '#')
                    count ++;
            }
        }
        return count;
    }
}

internal class Guard(int X, int Y, char Type)
{
    public int X = X;
    public int Y = Y;
    public char Type = Type;

    public (int, int) GetPositionAhead() {
        return Type switch
        {
            '^' => (X - 1, Y),
            'V' => (X + 1, Y),
            '>' => (X, Y + 1),
            '<' => (X, Y - 1),
            _ => throw new Exception("Invalid Guard"),
        };
    }

    public void RotateRight() {
        Type = Type switch
        {
            '^' => '>',
            'V' => '<',
            '>' => 'V',
            '<' => '^',
            _ => throw new Exception("Invalid Guard"),
        };
    }

    internal void Move(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"{X}, {Y}";
    }

    internal char GetTrailType(bool isCross)
    {
        if (isCross) {
            return '+';
        }
        return Type switch
        {
            '^' or 'V' => '|',
            '>' or '<' => '-',
            _ => throw new Exception("Invalid Guard"),
        };
    }
}