using System.Text;

namespace AdventOfCode2024;

public class Day20 : IDaySolution
{
    private char[][] _map = [];

    public int? SolvePart1()
    {
        _map = ParseInput();

        var startPos = FindCharOnMap(_map, 'S');
        PosAndCheatPos startPosWithoutCheat = new(startPos, null);
        var endPos = FindCharOnMap(_map, 'E');
        PosAndCheatPos endPosWithoutCheat = new(endPos, null);

        Dictionary<PosAndCheatPos, int> exploredPositions = [];
        exploredPositions.Add(startPosWithoutCheat, 0);

        var currentStatesToExplore = new List<PosAndCheatPos>() { startPosWithoutCheat };

        while (currentStatesToExplore.Count > 0)
        {
            currentStatesToExplore = ExploreMap(exploredPositions, currentStatesToExplore);
        }

        var endStates = exploredPositions.Where(kvp => kvp.Key.Pos == endPos);

        Console.WriteLine($"{exploredPositions[endPosWithoutCheat]}");
        Console.WriteLine($"---");

        var v = endStates
            .Where(es => es.Value < exploredPositions[endPosWithoutCheat])
            .Where(es => 100 < exploredPositions[endPosWithoutCheat] - es.Value)
            .GroupBy(es => es.Value)
            .Select(ges => $"{ges.Count()}:{exploredPositions[endPosWithoutCheat] - ges.Key}");

        Console.WriteLine($"{string.Join(", ", v)}");

        return endStates
            .Where(es => es.Value < exploredPositions[endPosWithoutCheat])
            .Where(es => 100 <= exploredPositions[endPosWithoutCheat] - es.Value)
            .Count();
    }

    public int? SolvePart2()
    {
        return 0;
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

    private List<PosAndCheatPos> ExploreMap(Dictionary<PosAndCheatPos, int> exploredPositions, List<PosAndCheatPos> currentStatesToExplore)
    {
        var newCurrentStates = new List<PosAndCheatPos>();

        foreach (var posAndCheat in currentStatesToExplore)
        {
            var newScore = exploredPositions[posAndCheat] + 1;
            IEnumerable<PosAndCheatPos> newPositions = GetNewPositions(posAndCheat);

            foreach (var newPosAndCheat in newPositions)
            {
                if (!exploredPositions.TryGetValue(newPosAndCheat, out var existingScore) || newScore < existingScore)
                {
                    exploredPositions[newPosAndCheat] = newScore;
                    newCurrentStates.Add(newPosAndCheat);
                }
            }
        }

        currentStatesToExplore = newCurrentStates;
        return currentStatesToExplore;
    }

    private IEnumerable<PosAndCheatPos> GetNewPositions(PosAndCheatPos posAndCheat)
    {
        var lst = new List<Position>() {
                    new(posAndCheat.Pos.X+1, posAndCheat.Pos.Y),
                    new(posAndCheat.Pos.X-1, posAndCheat.Pos.Y),
                    new(posAndCheat.Pos.X, posAndCheat.Pos.Y+1),
                    new(posAndCheat.Pos.X, posAndCheat.Pos.Y-1)
                };
        if (posAndCheat.CheatPos != null)
            return lst
                .Where(pos => pos.X >= 0 && pos.X < _map[0].Length && pos.Y >= 0 && pos.Y < _map.Length)
                .Where(pos => _map[pos.X][pos.Y] != '#')
                .Select(pos => new PosAndCheatPos(pos, posAndCheat.CheatPos));
        else
            return lst
                .Where(pos => pos.X >= 0 && pos.X <= _map[0].Length && pos.Y >= 0 && pos.Y <= _map.Length)
                .Select(pos =>
                {
                    var cheatPos = _map[pos.X][pos.Y] == '#' ? pos : null;
                    return new PosAndCheatPos(pos, cheatPos);
                });
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

internal record PosAndCheatPos(Position Pos, Position? CheatPos);