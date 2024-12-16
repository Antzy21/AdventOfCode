using System.Text;

namespace AdventOfCode2024;

public class Day15 : IDaySolution
{
    public int? SolvePart1()
    {
        var (map, moves) = ParseInput();

        foreach (var move in moves)
        {
            map.TryMove(move);
        }

        return (int)map.SumOfBoxsGpsCoord();
    }

    public int? SolvePart2()
    {
        var (map, moves) = ParseInput();

        map = DoubleMap(map);

        foreach (var move in moves)
        {
            map.TryMove(move);
        }

        return (int)map.SumOfBoxsGpsCoord();
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

    private static Map15 DoubleMap(Map15 map)
    {
        var doubledMap = map._map.Select((row, i) =>
        {
            return row.SelectMany((c, j) =>
            {
                switch (c)
                {
                    case '#':
                        return new char[] { '#', '#' };
                    case '.':
                        return ['.', '.'];
                    case 'O':
                        return ['[', ']'];
                    case '@':
                        map.BotPos = new(i, j * 2);
                        return ['@', '.'];
                    default:
                        throw new Exception($"Bad char {c}");
                };
            }).ToArray();
        }
        ).ToArray();
        return new(doubledMap, map.BotPos);
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

    public Map15(char[][] map, Position pos)
    {
        _map = map;
        BotPos = pos;
    }
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

    internal bool TryMove(char moveChar, Position? pos = null, bool withoutMoving = false)
    {
        pos ??= BotPos;

        var newPos = GetNewPos(moveChar, pos);

        switch (_map[newPos.X][newPos.Y])
        {
            case '#':
                return false;
            case '.':
                if (!withoutMoving)
                    UpdateMap(pos, newPos);
                return true;
            case 'O':
                if (TryMove(moveChar, newPos))
                {
                    if (!withoutMoving)
                        UpdateMap(pos, newPos);
                    return true;
                }
                else
                {
                    return false;
                }
            case '[':
                if (MoveIsLeftOrRight(moveChar))
                {
                    if (TryMove(moveChar, newPos, withoutMoving))
                    {
                        if (!withoutMoving)
                            UpdateMap(pos, newPos);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    var extraBlockPos = new Position(newPos.X, newPos.Y + 1);
                    if (TryMove(moveChar, newPos, withoutMoving: true) && TryMove(moveChar, extraBlockPos, withoutMoving: true))
                    {
                        TryMove(moveChar, newPos, withoutMoving);
                        TryMove(moveChar, extraBlockPos, withoutMoving);
                        if (!withoutMoving)
                            UpdateMap(pos, newPos);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            case ']':
                if (MoveIsLeftOrRight(moveChar))
                {
                    if (TryMove(moveChar, newPos, withoutMoving))
                    {
                        if (!withoutMoving)
                            UpdateMap(pos, newPos);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    var extraBlockPos = new Position(newPos.X, newPos.Y - 1);
                    if (TryMove(moveChar, newPos, withoutMoving: true) && TryMove(moveChar, extraBlockPos, withoutMoving: true))
                    {
                        TryMove(moveChar, newPos, withoutMoving);
                        TryMove(moveChar, extraBlockPos, withoutMoving);
                        if (!withoutMoving)
                            UpdateMap(pos, newPos);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            default:
                throw new Exception($"Bad char {_map[newPos.X][newPos.Y]}");
        }
    }

    private void UpdateMap(Position pos, Position newPos)
    {
        _map[newPos.X][newPos.Y] = _map[pos.X][pos.Y];
        _map[pos.X][pos.Y] = '.';
        if (pos == BotPos)
            BotPos = new(newPos.X, newPos.Y);
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

    private static bool MoveIsLeftOrRight(char moveChar)
    {
        return moveChar == '<' || moveChar == '>';
    }

    public long SumOfBoxsGpsCoord()
    {
        long sum = 0;
        for (int i = 0; i < _map.Length; i++)
        {
            for (int j = 0; j < _map[0].Length; j++)
            {
                if (_map[i][j] == 'O' || _map[i][j] == '[')
                    sum += CaculateGpsCoord(i, j);
            }
        }
        return sum;
    }

    private long CaculateGpsCoord(int i, int j)
    {
        return i * 100 + j;
    }
}
