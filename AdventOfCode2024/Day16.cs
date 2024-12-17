using System.Text;

namespace AdventOfCode2024;

public class Day16 : IDaySolution
{
    public int? SolvePart1()
    {
        var map = ParseInput();

        var startPos = FindCharOnMap(map, 'S');
        var endPos = FindCharOnMap(map, 'E');

        var exploredPositions = new Dictionary<PosDir, int>() {
            { new (startPos, Direction.East), 0 }
        };

        var currentStatesToExplore = new List<PosDir>() { new(startPos, Direction.East) };

        while (currentStatesToExplore.Count > 0)
        {
            var newCurrentStates = new List<PosDir>();

            foreach (var posDir in currentStatesToExplore)
            {
                if (map[posDir.pos.X - 1][posDir.pos.Y] != '#' && posDir.dir != Direction.South)
                {
                    var newPos = new Position(posDir.pos.X - 1, posDir.pos.Y);
                    ExploreDirection(exploredPositions, newCurrentStates, posDir, newPos, Direction.North);
                }
                if (map[posDir.pos.X + 1][posDir.pos.Y] != '#' && posDir.dir != Direction.North)
                {
                    var newPos = new Position(posDir.pos.X + 1, posDir.pos.Y);
                    ExploreDirection(exploredPositions, newCurrentStates, posDir, newPos, Direction.South);
                }
                if (map[posDir.pos.X][posDir.pos.Y + 1] != '#' && posDir.dir != Direction.West)
                {
                    var newPos = new Position(posDir.pos.X, posDir.pos.Y + 1);
                    ExploreDirection(exploredPositions, newCurrentStates, posDir, newPos, Direction.East);

                }
                if (map[posDir.pos.X][posDir.pos.Y - 1] != '#' && posDir.dir != Direction.East)
                {
                    var newPos = new Position(posDir.pos.X, posDir.pos.Y - 1);
                    ExploreDirection(exploredPositions, newCurrentStates, posDir, newPos, Direction.West);
                }
            }

            currentStatesToExplore = newCurrentStates;
        }

        var result = exploredPositions
            .Where(kvp => kvp.Key.pos == endPos)
            .MinBy(kvp => kvp.Value)
            .Value;

        return result;
    }

    public int? SolvePart2()
    {
        return 0;
    }

    private static string MapToString(char[][] map, Dictionary<PosDir, int> exploredPositions)
    {
        var str = new StringBuilder();
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] != '.')
                {
                    str.Append(map[i][j]);
                }
                else
                {
                    Position pos = new(i, j);
                    var n = exploredPositions.TryGetValue(new(pos, Direction.North), out int _);
                    var s = exploredPositions.TryGetValue(new(pos, Direction.South), out int _);
                    var e = exploredPositions.TryGetValue(new(pos, Direction.East), out int _);
                    var w = exploredPositions.TryGetValue(new(pos, Direction.West), out int _);
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
                        str.Append(exploredPositions.Count(kvp => kvp.Key.pos == pos));
                }
            }
            str.AppendLine();
        }
        return str.ToString();
    }

    private static void ExploreDirection(Dictionary<PosDir, int> exploredPositions, List<PosDir> newCurrentStates, PosDir posDir, Position newPos, Direction dir)
    {
        var newPosDir = new PosDir(newPos, dir);
        var newScore = exploredPositions[posDir] + (posDir.dir == dir ? 1 : 1001);
        if (!exploredPositions.TryGetValue(newPosDir, out var existingScore) || newScore < existingScore)
        {
            exploredPositions[newPosDir] = newScore;
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