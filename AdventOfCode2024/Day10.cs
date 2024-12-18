
namespace AdventOfCode2024;

public class Day10 : IDaySolution
{
    public int? SolvePart1()
    {
        var map = ParseInput();

        var trailHeads = CalculateTrails<HashSet<Position>>(map);

        return trailHeads.Select(th => th._Trail[9].Count).Sum();
    }

    private static List<TrailHead<TPositionCollection>> CalculateTrails<TPositionCollection>(int[][] map)
        where TPositionCollection : ICollection<Position>, new()
    {
        var trailHeads = new List<TrailHead<TPositionCollection>>();

        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[0].Length; j++)
            {
                if (map[i][j] == 0)
                {
                    trailHeads.Add(new TrailHead<TPositionCollection>(i, j));
                }
            }
        }

        foreach (var trailHead in trailHeads)
        {
            for (int i = 0; i < 10; i++)
            {
                trailHead.FindAscPath(i, map);
            }
        }

        return trailHeads;
    }

    public int? SolvePart2()
    {
        var map = ParseInput();

        var trailHeads = CalculateTrails<List<Position>>(map);

        return trailHeads.Select(th => th._Trail[9].Count).Sum();
    }

    private static int[][] ParseInput()
    {
        return File.ReadAllLines("inputs/day10.txt")
            .Select(line =>
                line.ToCharArray()
                .Select(c => int.Parse($"{c}"))
                .ToArray()
            ).ToArray();
    }
}

internal record TrailHead<TPositionCollection>
    where TPositionCollection : ICollection<Position>, new()
{
    public Position init;
    public Dictionary<int, TPositionCollection> _Trail;

    public TrailHead(int X, int Y)
    {
        init = new(X, Y);
        _Trail = new Dictionary<int, TPositionCollection> {
            { 0, [init] }
        };
    }

    internal void FindAscPath(int height, int[][] map)
    {
        var trailTailPositions = _Trail[height];
        _Trail[height + 1] = [];
        foreach (var pos in trailTailPositions)
        {
            try
            {
                if (map[pos.X + 1][pos.Y] == height + 1)
                    _Trail[height + 1].Add(new(pos.X + 1, pos.Y));
            }
            catch { }
            try
            {
                if (map[pos.X - 1][pos.Y] == height + 1)
                    _Trail[height + 1].Add(new(pos.X - 1, pos.Y));
            }
            catch { }
            try
            {
                if (map[pos.X][pos.Y + 1] == height + 1)
                    _Trail[height + 1].Add(new(pos.X, pos.Y + 1));
            }
            catch { }
            try
            {
                if (map[pos.X][pos.Y - 1] == height + 1)
                    _Trail[height + 1].Add(new(pos.X, pos.Y - 1));
            }
            catch { }
        }
    }
}

internal record Position(int X, int Y);