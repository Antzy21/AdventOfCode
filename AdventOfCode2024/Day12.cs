

namespace AdventOfCode2024;

public class Day12 : IDaySolution
{
    public int? SolvePart1()
    {
        var plots = ParseInput();

        var regionId = 1;

        foreach (var plot in plots.SelectMany(s => s))
        {
            if (plot.X > 0)
            {
                var abovePlot = plots[plot.X - 1][plot.Y];
                if (plot.Char == abovePlot.Char)
                {
                    plot.regionId = abovePlot.regionId;
                    plot.fencesNeeded --;
                    abovePlot.fencesNeeded --;
                }
            }
            if (plot.Y > 0)
            {
                var leftPlot = plots[plot.X][plot.Y - 1];
                if (plot.Char == leftPlot.Char)
                {

                    if (plot.regionId != 0)
                    {
                        UpdatePlotRegions(plots, plot.regionId, leftPlot.regionId);
                    }
                    else
                    {
                        plot.regionId = leftPlot.regionId;
                    }
                    plot.fencesNeeded --;
                    leftPlot.fencesNeeded --;
                }
            }
            if (plot.regionId == 0)
            {
                plot.regionId = regionId;
                regionId++;
            }
        }

        var result = plots.SelectMany(s => s)
            .GroupBy(p => p.regionId)
            .Select(gp => 
                gp.Select(gp => gp.fencesNeeded).Sum() * 
                gp.Count()
            ).Sum();

        return result;
    }

    private static void UpdatePlotRegions(Plot[][] plots, int newRegionId, int oldRegionId)
    {
        foreach (var plot in plots.SelectMany(p => p).Where(p => p.regionId == oldRegionId))
        {
            plot.regionId = newRegionId;
        }
    }

    public int? SolvePart2()
    {
        return 0;
    }

    internal static Plot[][] ParseInput()
    {
        return File.ReadAllLines("inputs/day12.txt")
            .Select((line, i) =>
                line.ToCharArray().Select((c, j) => new Plot(c, i, j)).ToArray()
            ).ToArray();
    }
}

internal record Plot(char Char, Position Position)
{
    public Plot(char Char, int X, int Y) : this(Char, new(X, Y)) { }

    public int regionId = 0;

    public int fencesNeeded = 4;

    public int X
    {
        get
        {
            return Position.X;
        }
    }
    public int Y
    {
        get
        {
            return Position.Y;
        }
    }
};