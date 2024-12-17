using System.Text;

namespace AdventOfCode2024;

public class Day16 : IDaySolution
{
    private readonly Dictionary<PosDir, int> _exploredPositions = [];
    private readonly Dictionary<PosDir, List<PosDir>> _positionTracing = [];
    private readonly HashSet<PosDir> _fastestPath = [];
    private Position _startPos = new(0, 0);
    private Position _endPos = new(0, 0);
    private readonly char[][] _map = ParseInput();

    public int? SolvePart1()
    {
        _startPos = FindCharOnMap(_map, 'S');
        _endPos = FindCharOnMap(_map, 'E');

        _exploredPositions.Add(new(_startPos, Direction.East), 0);

        var currentStatesToExplore = new List<PosDir>() { new(_startPos, Direction.East) };

        while (currentStatesToExplore.Count > 0)
        {
            currentStatesToExplore = ExploreMap(_map, currentStatesToExplore);
        }

        var result = _exploredPositions
            .Where(kvp => kvp.Key.pos == _endPos)
            .MinBy(kvp => kvp.Value)
            .Value;

        return result;
    }

    public int? SolvePart2()
    {
        var endPosDir = _exploredPositions
            .Where(kvp => kvp.Key.pos == _endPos)
            .MinBy(kvp => kvp.Value).Key;

        var i = GetCountBack(endPosDir);

        var map = MapToString(true);
        var cnt = map.ToCharArray().Count(c => c == 'O');

        return _fastestPath.GroupBy(fp => fp.pos).Count();
    }

    private int GetCountBack(PosDir endPosDir)
    {
        _fastestPath.Add(endPosDir);

        if (_positionTracing.TryGetValue(endPosDir, out var tracedPositions))
        {
            return tracedPositions.Select(tp =>
            {
                return _fastestPath.Contains(tp) ? 0 : GetCountBack(tp);
            }).Sum() + 1;
        }
        return 0;
    }

    private List<PosDir> ExploreMap(char[][] map, List<PosDir> currentStatesToExplore)
    {
        var newCurrentStates = new List<PosDir>();

        foreach (var posDir in currentStatesToExplore)
        {
            if (map[posDir.pos.X - 1][posDir.pos.Y] != '#' && posDir.dir != Direction.South)
            {
                var newPos = new Position(posDir.pos.X - 1, posDir.pos.Y);
                ExploreDirection(newCurrentStates, posDir, newPos, Direction.North);
            }
            if (map[posDir.pos.X + 1][posDir.pos.Y] != '#' && posDir.dir != Direction.North)
            {
                var newPos = new Position(posDir.pos.X + 1, posDir.pos.Y);
                ExploreDirection(newCurrentStates, posDir, newPos, Direction.South);
            }
            if (map[posDir.pos.X][posDir.pos.Y + 1] != '#' && posDir.dir != Direction.West)
            {
                var newPos = new Position(posDir.pos.X, posDir.pos.Y + 1);
                ExploreDirection(newCurrentStates, posDir, newPos, Direction.East);

            }
            if (map[posDir.pos.X][posDir.pos.Y - 1] != '#' && posDir.dir != Direction.East)
            {
                var newPos = new Position(posDir.pos.X, posDir.pos.Y - 1);
                ExploreDirection(newCurrentStates, posDir, newPos, Direction.West);
            }
        }

        currentStatesToExplore = newCurrentStates;
        return currentStatesToExplore;
    }

    private string MapToString(bool showFastestPath = false, bool showExplored = false)
    {
        var str = new StringBuilder();
        for (int i = 0; i < _map.Length; i++)
        {
            for (int j = 0; j < _map[i].Length; j++)
            {
                Position pos = new(i, j);
                if (showFastestPath && _fastestPath.Any(p => p.pos == pos))
                {
                    str.Append('O');
                }
                else if (_map[i][j] != '.')
                {
                    str.Append(_map[i][j]);
                }
                else if (showExplored)
                {
                    var n = _exploredPositions.TryGetValue(new(pos, Direction.North), out int _);
                    var s = _exploredPositions.TryGetValue(new(pos, Direction.South), out int _);
                    var e = _exploredPositions.TryGetValue(new(pos, Direction.East), out int _);
                    var w = _exploredPositions.TryGetValue(new(pos, Direction.West), out int _);
                    if (!n && !s && !e && !w)
                    {
                        str.Append('.');
                    }
                    else if (n && !s && !e && !w)
                    {
                        str.Append('^');
                    }
                    else if (!n && !s && e && !w)
                    {
                        str.Append('>');
                    }
                    else if (!n && s && !e && !w)
                    {
                        str.Append('v');
                    }
                    else if (!n && !s && !e && w)
                    {
                        str.Append('<');
                    }
                    else
                        str.Append(_exploredPositions.Count(kvp => kvp.Key.pos == pos));
                }
                else
                    str.Append('.');
            }
            str.AppendLine();
        }
        return str.ToString();
    }

    private void ExploreDirection(List<PosDir> newCurrentStates, PosDir posDir, Position newPos, Direction dir)
    {
        var newPosDir = new PosDir(newPos, dir);
        var newScore = _exploredPositions[posDir] + (posDir.dir == dir ? 1 : 1001);
        if (_exploredPositions.TryGetValue(newPosDir, out var existingScore))
        {
            if (newScore < existingScore)
            {
                _exploredPositions[newPosDir] = newScore;
                _positionTracing[newPosDir] = [posDir];
                newCurrentStates.Add(newPosDir);
            }
            else if (newScore == existingScore)
            {
                _positionTracing[newPosDir].Add(posDir);
            }
        }
        else
        {
            _exploredPositions[newPosDir] = newScore;
            _positionTracing[newPosDir] = [posDir];
            newCurrentStates.Add(newPosDir);
        }
    }

    private static Position FindCharOnMap(char[][] map, char c)
    {
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == c)
                    return new(i, j);
            }
        }
        throw new Exception("Can't find start");
    }

    public static char[][] ParseInput()
    {
        return File.ReadAllLines("inputs/day16.txt")
            .Select(line => line.ToArray())
            .ToArray();
    }
}

internal record PosDir(Position pos, Direction dir)
{
    public override string ToString()
    {
        return $"({pos.X},{pos.Y}) - {dir}";
    }
};

internal enum Direction
{
    North,
    East,
    West,
    South
}