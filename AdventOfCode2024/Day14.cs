using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day14 : IDaySolution
{
    public int? SolvePart1()
    {
        var robots = ParseInput();

        var width = 101;
        var height = 103;
        var dividingWidth = width / 2;
        var dividingHeight = height / 2;
        Console.WriteLine($"height: {dividingHeight}, width:{dividingWidth}");


        PrintMap(robots, width, height);

        foreach (var robot in robots)
        {
            MoveRobot(robot, width, height, times: 100);
            robot.CalculateQuadrant(dividingWidth, dividingHeight);
        }

        PrintMap(robots, width, height);

        PrintMap(robots, width, height, blockMiddle: true);

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
        return 0;
    }

    private static void PrintMap(List<Robot> robots, int width, int height, bool blockMiddle = false)
    {
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (i == width / 2 || j == height / 2 && blockMiddle)
                {
                    Console.Write($" ");
                }
                else
                {
                    var robotsOnSpot = robots.Count(r => r.Pos == new Position(i, j));
                    var robotsOnSpotChar = robotsOnSpot == 0 ? "." : robotsOnSpot.ToString();
                    Console.Write($"{robotsOnSpotChar}");

                }

            }
            Console.WriteLine();
        }
        Console.WriteLine("---------------");
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