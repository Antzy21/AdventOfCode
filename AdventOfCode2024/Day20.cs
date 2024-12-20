using System.Text;

namespace AdventOfCode2024;

public class Day20 : IDaySolution
{
    private char[][] _map = [];

    public int? SolvePart1()
    {
        _map = ParseInput();

        int maxCheatLength = 2;

        var startPos = FindCharOnMap(_map, 'S');
        var endPos = FindCharOnMap(_map, 'E');

        Dictionary<Position, int> exploredPositions = [];
        exploredPositions.Add(startPos, 0);

        var currentStatesToExplore = new List<Position>() { startPos };

        while (currentStatesToExplore.Count > 0)
        {
            currentStatesToExplore = ExploreMap(exploredPositions, currentStatesToExplore);
        }

        var endStates = exploredPositions.Where(kvp => kvp.Key == endPos);

        var result = FindCheatTimeSavers(maxCheatLength, exploredPositions);

        return result;
    }

    public int? SolvePart2()
    {
        _map = ParseInput();

        int maxCheatLength = 20;

        var startPos = FindCharOnMap(_map, 'S');
        var endPos = FindCharOnMap(_map, 'E');

        Dictionary<Position, int> exploredPositions = [];
        exploredPositions.Add(startPos, 0);

        var currentStatesToExplore = new List<Position>() { startPos };

        while (currentStatesToExplore.Count > 0)
        {
            currentStatesToExplore = ExploreMap(exploredPositions, currentStatesToExplore);
        }

        var endStates = exploredPositions.Where(kvp => kvp.Key == endPos);

        var result = FindCheatTimeSavers(maxCheatLength, exploredPositions);

        return result;
    }

    private static int FindCheatTimeSavers(int maxCheatLength, Dictionary<Position, int> exploredPositions, int minSavings = 100)
    {
        var cheatSavesLessThan100 = 0;

        foreach (var kvp in exploredPositions)
        {
            for (int i = maxCheatLength * -1; i <= maxCheatLength; i++)
            {
                var jLimit = maxCheatLength - Math.Abs(i);
                for (int j = jLimit * -1; j <= jLimit; j++)
                {
                    var newPos = new Position(kvp.Key.X - i, kvp.Key.Y - j);
                    if (exploredPositions.TryGetValue(newPos, out var newPosScore))
                    {
                        if (newPosScore - exploredPositions[kvp.Key] >= minSavings + Math.Abs(i) + Math.Abs(j))
                            cheatSavesLessThan100++;
                    }
                }
            }
        }

        return cheatSavesLessThan100;
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

    private List<Position> ExploreMap(Dictionary<Position, int> exploredPositions, List<Position> currentStatesToExplore)
    {
        var newCurrentStates = new List<Position>();

        foreach (var posAndCheat in currentStatesToExplore)
        {
            var newScore = exploredPositions[posAndCheat] + 1;

            foreach (var newPos in GetNewPositions(posAndCheat))
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

    private IEnumerable<Position> GetNewPositions(Position pos)
    {
        return new List<Position>() {
                    new(pos.X+1, pos.Y),
                    new(pos.X-1, pos.Y),
                    new(pos.X, pos.Y+1),
                    new(pos.X, pos.Y-1)
                }
                .Where(pos => pos.X >= 0 && pos.X < _map[0].Length && pos.Y >= 0 && pos.Y < _map.Length)
                .Where(pos => _map[pos.X][pos.Y] != '#');
    }

    private static string MapToString(char[][] map, Dictionary<Position, int>? exploredPositions = null)
    {
        exploredPositions ??= [];
        var str = new StringBuilder();
        for (int j = 0; j < map.Length; j++)
        {
            for (int i = 0; i < map[j].Length; i++)
            {
                if (exploredPositions.ContainsKey(new(i, j)))
                    str.Append('O');
                else
                    str.Append(map[i][j]);
            }
            str.AppendLine();
        }
        return str.ToString();
    }

    internal static char[][] ParseInput()
    {
        return File.ReadAllLines("inputs/day20.txt")
            .Select(line => line.ToArray())
            .ToArray();
    }
}