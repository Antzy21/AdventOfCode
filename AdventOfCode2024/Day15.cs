using System.Text;

namespace AdventOfCode2024;

public class Day15 : IDaySolution
{
    public int? SolvePart1()
    {
        var (map, moves) = ParseInput();

        Console.WriteLine($"{MapToString(map)}");

        foreach (var move in moves)
        {
            map.TryMove(move);
        }
        Console.WriteLine($"{MapToString(map)}");

        return (int)map.SumOfBoxsGpsCoord();
    }

    public int? SolvePart2()
    {
        return 0;
    }


    private static string MapToString(Map15 map)
    {
        var s = new StringBuilder();
        for (int i = 0; i < map._map.Length; i++)
        {
            s.AppendJoin("", map._map[i]);
            s.AppendLine();
        }
        return s.ToString();
    }

    private static Tuple<Map15, char[]> ParseInput()
    {
        var input = File.ReadAllText("inputs/day15.txt");

        var parts = input.Split("\n\n");

        var map = new Map15(parts[0]);
        var moves = parts[1].ToCharArray()
            .Where(c => c != '\n')
            .ToArray();

        return new(map, moves);
    }
}

internal class Map15
{
    public Position BotPos;
    public char[][] _map;

    public Map15(string rawMap)
    {
        _map = rawMap
            .Split("\n")
            .Select((row, i) =>
                row.ToCharArray()
                .Select((c, j) =>
                {
                    if (c == '@')
                    {
                        BotPos = new Position(i, j);
                    }
                    return c;
                }).ToArray()
            )
            .ToArray();
        if (BotPos == null)
            throw new Exception("No bot found");
    }

    internal bool TryMove(char moveChar, Position? pos = null)
    {
        pos ??= BotPos;

        var newPos = GetNewPos(moveChar, pos);

        switch (_map[newPos.X][newPos.Y])
        {
            case '#':
                return false;
            case '.':
                _map[newPos.X][newPos.Y] = _map[pos.X][pos.Y];
                _map[pos.X][pos.Y] = '.';
                if (pos == BotPos)
                    BotPos = new(newPos.X, newPos.Y);
                return true;
            case 'O':
                if (TryMove(moveChar, newPos))
                {
                    _map[newPos.X][newPos.Y] = _map[pos.X][pos.Y];
                    _map[pos.X][pos.Y] = '.';
                    if (pos == BotPos)
                        BotPos = new(newPos.X, newPos.Y);
                    return true;
                }
                else
                {
                    return false;
                }
            default:
                throw new Exception($"Bad char {_map[newPos.X][newPos.Y]}");
        }
    }

    private static Position GetNewPos(char move, Position pos)
    {
        if (move == '<')
            return new Position(pos.X, pos.Y - 1);
        if (move == '^')
            return new Position(pos.X - 1, pos.Y);
        if (move == '>')
            return new Position(pos.X, pos.Y + 1);
        if (move == 'v')
            return new Position(pos.X + 1, pos.Y);
        throw
            new Exception($"Bad move '{move}'");
    }

    public long SumOfBoxsGpsCoord() {
        long sum = 0;
        for (int i = 0; i < _map.Length; i++)
        {
            for (int j = 0; j < _map[0].Length; j++)
            {
                if (_map[i][j] == 'O')
                    sum += CaculateGpsCoord(i,j);
            }
        }
        return sum;    
    }

    private long CaculateGpsCoord(int i, int j)
    {
        return i * 100 + j;
    }
}
