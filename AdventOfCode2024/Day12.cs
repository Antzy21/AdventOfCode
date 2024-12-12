

namespace AdventOfCode2024;

public class Day12 : IDaySolution
{
    public int? SolvePart1()
    {
        var plots = ParseInput();
        
        EstablishRegionsOnPlots(plots);

        var result = plots.SelectMany(s => s)
            .GroupBy(p => p.regionId)
            .Select(gp =>
                gp.Select(gp => gp.fencesNeeded).Sum() *
                gp.Count()
            ).Sum();

        return result;
    }

    public int? SolvePart2()
    {
        var plots = ParseInput();
        
        EstablishRegionsOnPlots(plots);

        var result = plots.SelectMany(s => s)
            .GroupBy(p => p.regionId)
            .Select(gp =>
                gp.Select(gp => gp.fencesNeeded - gp.bulkDiscountOnFences).Sum() *
                gp.Count()
            ).Sum();

        return result;
    }
    
    private static void EstablishRegionsOnPlots(Plot[][] plots)
    {
        var regionId = 1;

        foreach (var plot in plots.SelectMany(s => s))
        {
            if (plot.X > 0)
            {
                var abovePlot = plots[plot.X - 1][plot.Y];
                if (plot.Char == abovePlot.Char)
                {
                    plot.regionId = abovePlot.regionId;
                    plot.plotAbove = abovePlot;
                    abovePlot.plotBelow = plot;
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
                    plot.plotToLeft = leftPlot;
                    leftPlot.plotToRight = plot;
                }
            }
            if (plot.regionId == 0)
            {
                plot.regionId = regionId;
                regionId++;
            }
        }

        foreach (var plot in plots.SelectMany(s => s))
        {
            if (plot.plotAbove != null)
            {
                if (plot.plotToLeft == null && plot.plotAbove.plotToLeft == null)
                    plot.bulkDiscountOnFences++;
                if (plot.plotToRight == null && plot.plotAbove.plotToRight == null)
                    plot.bulkDiscountOnFences++;
            }
            if (plot.plotToLeft != null)
            {
                if (plot.plotAbove == null && plot.plotToLeft.plotAbove == null)
                    plot.bulkDiscountOnFences++;
                if (plot.plotBelow == null && plot.plotToLeft.plotBelow == null)
                    plot.bulkDiscountOnFences++;
            }
        }
    }

    private static void UpdatePlotRegions(Plot[][] plots, int newRegionId, int oldRegionId)
    {
        foreach (var plot in plots.SelectMany(p => p).Where(p => p.regionId == oldRegionId))
        {
            plot.regionId = newRegionId;
        }
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

    public int bulkDiscountOnFences = 0;

    public int fencesNeeded
    {
        get
        {
            var r = 4;
            if (plotAbove != null)
                r --;
            if (plotToLeft != null)
                r --;
            if (plotBelow != null)
                r --;
            if (plotToRight != null)
                r --;
            return r;
        }
    }

    public Plot? plotToLeft = null;
    public Plot? plotToRight = null;
    public Plot? plotAbove = null;
    public Plot? plotBelow = null;

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