
namespace AdventOfCode2024;

public class Day08 : IDaySolution
{
    public int? SolvePart1()
    {
        var (antennas, mapBoundry) = ParseInput();
        var (maxX, maxY) = mapBoundry;

        var antiNodes = GetAllAntinodes(antennas, maxX, maxY);

        // s = MapToString(antennas, maxX, maxY, antiNodes);
        // Console.WriteLine($"{s}");

        return antiNodes.Count();
    }

    public int? SolvePart2()
    {
        var (antennas, mapBoundry) = ParseInput();
        var (maxX, maxY) = mapBoundry;

        var antiNodes = GetAllAntinodes(antennas, maxX, maxY, getMultiple: true);

        return antiNodes.Count();
    }

    private static IEnumerable<(int, int)> GetAllAntinodes(List<Antenna> antennas, int maxX, int maxY, bool getMultiple = false)
    {
        return antennas
            .GroupBy(a => a.C)
            .Select(kvp =>
            {
                return kvp.SelectMany(antennaA => kvp, (antennaA, antennaB) =>
                {
                    return antennaA.GetAntiNodes(antennaB, maxX, maxY, getMultiple);
                })
                .SelectMany(n => n);
            })
            .SelectMany(antiNodes => antiNodes)
            .Distinct();
    }

    private static string MapToString(List<Antenna> antennas, int maxX, int maxY, IEnumerable<(int, int)>? antiNodes = null)
    {
        antiNodes ??= [];

        var s = "";

        for (int i = 0; i < maxX; i++)
        {
            for (int j = 0; j < maxY; j++)
            {
                var firstAntenna = antennas.FirstOrDefault(a => a.X == i && a.Y == j);
                if (firstAntenna is not null)
                {
                    s += $"{firstAntenna.C}";
                }
                else if (antiNodes.Contains((i, j)))
                {
                    s += "#";
                }
                else
                {
                    s += ".";
                }
            }
            s += "\n";
        }
        return s;
    }

    private static (List<Antenna>, (int, int)) ParseInput()
    {
        var input = File.ReadAllLines("inputs/day8.txt");

        var antennas = input
            .SelectMany((line, i) => line.Select((c, j) => (c, i, j)))
            .Where(vars =>
            {
                var (c, i, j) = vars;
                return c != '.';
            })
            .Select(vars =>
            {
                var (c, i, j) = vars;
                return new Antenna(c, i, j);
            }).ToList();

        return (antennas, (input.Length, input[0].Length));
    }
}

public record Antenna(char C, int X, int Y)
{
    public List<(int, int)> GetAntiNodes(Antenna pairedAntenna, int maxX, int maxY, bool getMultiple = false)
    {
        var list = new List<(int, int)>();

        if (this == pairedAntenna)
        {
            return list;
        }

        var difX = X - pairedAntenna.X;
        var difY = Y - pairedAntenna.Y;

        if (getMultiple)
        {
            var antiNodeX = X;
            var antiNodeY = Y;

            while (!IsPositionOutOfBounds(antiNodeX, antiNodeY, maxX, maxY))
            {
                list.Add((antiNodeX, antiNodeY));

                antiNodeX += difX;
                antiNodeY += difY;
            }
        }
        else {
            var antiNodeX = X + difX;
            var antiNodeY = Y + difY;

            if (!IsPositionOutOfBounds(antiNodeX, antiNodeY, maxX, maxY))
            {
                list.Add((antiNodeX, antiNodeY));
            }
        }


        return list;
    }

    private static bool IsPositionOutOfBounds(int x, int y, int maxX, int maxY) => x < 0 || y < 0 || x > maxX - 1 || y > maxY - 1;
}