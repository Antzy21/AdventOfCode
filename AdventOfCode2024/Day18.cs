using System.Text;

namespace AdventOfCode2024;

public class Day18 : IDaySolution
{
    private const int _bytesToDrop = 1024;
    private readonly int _mapSize = 70;

    private Position _startPos;
    private Position _endPos;
    private readonly char[][] _map;

    public Day18()
    {
        _startPos = new(0, 0);
        _endPos = new(_mapSize, _mapSize);
        _map = Enumerable.Repeat(Enumerable.Repeat('.', _mapSize + 1), _mapSize + 1)
            .Select(dots => dots.ToArray()).ToArray();
    }

    public int? SolvePart1()
    {
        var bytes = ParseInput();
        DropBytesOnMap(bytes[0.._bytesToDrop]);

        Dictionary<Position, int> exploredPositions = [];
        exploredPositions.Add(_startPos, 0);

        var currentStatesToExplore = new List<Position>() {_startPos};

        while (!exploredPositions.ContainsKey(_endPos)) {
            currentStatesToExplore = ExploreMap(exploredPositions, currentStatesToExplore);
        }

        return exploredPositions[_endPos];
    }

    public int? SolvePart2()
    {
        var bytes = ParseInput();
        DropBytesOnMap(bytes[0.._bytesToDrop]);

        Position? killerByte = null;
        int byteIndex = _bytesToDrop;
        while (killerByte == null)
        {
            byteIndex ++;
            DropBytesOnMap([bytes[byteIndex]]);

            Dictionary<Position, int> exploredPositions = [];
            exploredPositions.Add(_startPos, 0);

            var currentStatesToExplore = new List<Position>() {_startPos};

            while (!exploredPositions.ContainsKey(_endPos)) {
                currentStatesToExplore = ExploreMap(exploredPositions, currentStatesToExplore);
                if (currentStatesToExplore.Count == 0) {
                    killerByte = bytes[byteIndex];
                    break;
                }
            }
        }

        Console.WriteLine($"Day 18 Part 2: ({killerByte.X},{killerByte.Y})");
        
        return byteIndex;
    }

    private void DropBytesOnMap(Position[] bytes)
    {
        foreach (var b in bytes)
        {
            _map[b.X][b.Y] = '#';
        }
    }

    private List<Position> ExploreMap(Dictionary<Position, int> exploredPositions, List<Position> currentStatesToExplore)
    {
        var newCurrentStates = new List<Position>();

        foreach (var pos in currentStatesToExplore)
        {
            var newScore = exploredPositions[pos] + 1;
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
                if (!exploredPositions.TryGetValue(newPos, out var existingScore) || newScore < existingScore)
                {
                    exploredPositions[newPos] = newScore;
                    newCurrentStates.Add(newPos);
                }
            }
        }

        currentStatesToExplore = newCurrentStates;
        return currentStatesToExplore;
    }

    private string MapToString(Dictionary<Position, int>? exploredPositions = null)
    {
        exploredPositions ??= [];
        var str = new StringBuilder();
        for (int j = 0; j < _map.Length; j++)
        {
            for (int i = 0; i < _map[j].Length; i++)
            {
                if (exploredPositions.ContainsKey(new(i,j)))
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