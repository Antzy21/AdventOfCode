using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day14 : IDaySolution
{
    private readonly int width = 101;
    private readonly int height = 103;
    
    public int? SolvePart1()
    {
        var robots = ParseInput();

        var width = 101;
        var height = 103;
        var dividingWidth = width / 2;
        var dividingHeight = height / 2;

        MapToString(robots);

        foreach (var robot in robots)
        {
            MoveRobot(robot, width, height, times: 100);
            robot.CalculateQuadrant(dividingWidth, dividingHeight);
        }

        MapToString(robots);

        var quadrantCounts = robots
            .Where(r => r.QuadrantId != 0)
            .GroupBy(r => r.QuadrantId)
            .Select(gr =>
            {
                return gr.Count();
            });

        return quadrantCounts.Aggregate(1, (prod, quadCount) => prod * quadCount);
    }

    public int? SolvePart2()
    {
        var robots = ParseInput();
        var width = 101;
        var height = 103;

        MoveRobots(robots, width, height, 6355);

        var map = MapToString(robots, standardiseNumber: true);

        return 6355;
    }

    private static void MoveRobots(List<Robot> robots, int width, int height, int times = 1)
    {
        foreach (var robot in robots)
        {
            MoveRobot(robot, width, height, times);
        }
    }

    private string MapToString(List<Robot> robots, int dividor = 1, bool standardiseNumber = false)
    {
        var strBldr = new StringBuilder();
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (dividor != 1 && IsPosOnBoundry(j, i, dividor))
                {
                    strBldr.Append(" ");
                }
                if (standardiseNumber) {
                    var c = robots.Any(r => r.Pos == new Position(i, j)) ? "#" : ".";
                    strBldr.Append($"{c}");
                }
                else
                {
                    var robotsOnSpot = robots.Count(r => r.Pos == new Position(i, j));
                    var robotsOnSpotChar = robotsOnSpot == 0 ? "." : robotsOnSpot.ToString();
                    strBldr.Append($"{robotsOnSpotChar}");
                }
            }
            strBldr.AppendLine();
        }
        return strBldr.ToString();
    }

    private bool IsPosOnBoundry(int j, int i, int dividor)
    {
        return i == width / dividor || j == height / dividor;
    }

    private static void MoveRobot(Robot robot, int width, int height, int times = 1)
    {
        var newX = (robot.Pos.X + robot.Vel.X * times) % width;
        if (newX < 0)
            newX += width;
        var newY = (robot.Pos.Y + robot.Vel.Y * times) % height;
        if (newY < 0)
            newY += height;
        robot.Pos = new(newX, newY);
    }

    internal static List<Robot> ParseInput()
    {
        var input = File.ReadAllText("inputs/day14.txt");

        var regex = @"p=(\d+),(\d+) v=(-?\d+),(-?\d+)";
        var matches = Regex.Matches(input, regex);

        return matches.Select(match =>
        {
            var pos = new Position(int.Parse(match.Groups[1].ToString()), int.Parse(match.Groups[2].ToString()));
            var vel = new Velocity(int.Parse(match.Groups[3].ToString()), int.Parse(match.Groups[4].ToString()));
            return new Robot(pos, vel);
        }).ToList();
    }
}

internal record Robot(Position Pos, Velocity Vel)
{
    public Position Pos { get; set; } = Pos;
    public int QuadrantId = 0;

    public void CalculateQuadrant(int dividingWidth, int dividingHeight)
    {
        if (Pos.X < dividingWidth)
        {
            if (Pos.Y < dividingHeight)
            {
                QuadrantId = 1;
            }
            else if (Pos.Y > dividingHeight)
            {
                QuadrantId = 3;
            }
        }
        else if (Pos.X > dividingWidth)
        {
            if (Pos.Y < dividingHeight)
            {
                QuadrantId = 2;
            }
            else if (Pos.Y > dividingHeight)
            {
                QuadrantId = 4;
            }
        }
    }
}

internal record Velocity(int X, int Y);