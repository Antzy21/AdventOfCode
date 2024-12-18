using System.Text;

namespace AdventOfCode2024;

public class Day18 : IDaySolution
{
    private const int _bytesToDrop = 1024;
    private readonly int _mapSize = 70;
    private readonly Dictionary<Position, int> _exploredPositions = [];

    private Position _startPos;
    private Position _endPos;
    private readonly char[][] _map;

    public Day18()
    {
        _startPos = new(0, 0);
        _endPos = new(_mapSize, _mapSize);
        _map = Enumerable.Repeat(Enumerable.Repeat('.', _mapSize + 1), _mapSize + 1)
            .Select(dots => dots.ToArray()).ToArray();
        _exploredPositions.Add(_startPos, 0);
    }

    public int? SolvePart1()
    {
        var bytes = ParseInput();
        DropBytesOnMap(bytes[0.._bytesToDrop]);

        var currentStatesToExplore = new List<Position>() {_startPos};

        while (!_exploredPositions.ContainsKey(_endPos)) {
            currentStatesToExplore = ExploreMap(currentStatesToExplore);
        }

        return _exploredPositions[_endPos];
    }

    public int? SolvePart2()
    {
        return 0;
    }

    private void DropBytesOnMap(Position[] bytes)
    {
        foreach (var b in bytes)
        {
            _map[b.X][b.Y] = '#';
        }
    }

    private List<Position> ExploreMap(List<Position> currentStatesToExplore)
    {
        var newCurrentStates = new List<Position>();

        foreach (var pos in currentStatesToExplore)
        {
            var newScore = _exploredPositions[pos] + 1;
            var newPositions = new List<Position>() {
                new(pos.X+1, pos.Y),
                new(pos.X-1, pos.Y),
                new(pos.X, pos.Y+1),
                new(pos.X, pos.Y-1)
            }
            .Where(pos => pos.X >= 0 && pos.X <= _mapSize && pos.Y >= 0 && pos.Y <= _mapSize)
            .Where(pos => _map[pos.X][pos.Y] != '#');

            foreach (var newPos in newPositions)
            {
                if (!_exploredPositions.TryGetValue(newPos, out var existingScore) || newScore < existingScore)
                {
                    _exploredPositions[newPos] = newScore;
                    newCurrentStates.Add(newPos);
                }
            }
        }

        currentStatesToExplore = newCurrentStates;
        return currentStatesToExplore;
    }

    private string MapToString()
    {
        var str = new StringBuilder();
        for (int j = 0; j < _map.Length; j++)
        {
            for (int i = 0; i < _map[j].Length; i++)
            {
                if (_exploredPositions.ContainsKey(new(i,j)))
                    str.Append('O');
                else
                    str.Append(_map[i][j]);
            }
            str.AppendLine();
        }
        return str.ToString();
    }

    internal static Position[] ParseInput()
    {
        return File.ReadAllLines("inputs/day18.txt")
            .Select(line =>
            {
                var splitLine = line.Split(",");
                return new Position(
                    int.Parse(splitLine[0]),
                    int.Parse(splitLine[1])
                );
            }).ToArray();
    }
}