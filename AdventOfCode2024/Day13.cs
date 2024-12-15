using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day13 : IDaySolution
{
    public int? SolvePart1()
    {
        var clawMachines = ParseInput();

        return clawMachines.Select(GetMinimumTokensToWin).Sum();
    }

    private int GetMinimumTokensToWin(ClawMachine machine)
    {
        int? tokens = null;
        for (int a = 1; a <= 100; a++)
        {
            for (int b = 1; b <= 100; b++)
            {
                if (machine.PressingButtonsGetsPrize(a, b))
                {
                    tokens = Math.Min(a * 3 + b, tokens ?? int.MaxValue);
                }
            }
        }

        return tokens ?? 0;
    }

    public int? SolvePart2()
    {
        var clawMachines = ParseInput();

        foreach (var machine in clawMachines)
        {
            machine.PrizeX += 10000000000000;
            machine.PrizeY += 10000000000000;
        }

        long result = clawMachines.Select(GetMinimumTokensToWinLinearEqSol).Sum();
        Console.WriteLine($"Day 13 Part 2: {result}");

        return (int)result;
    }

    private static long GetMinimumTokensToWinLinearEqSol(ClawMachine m)
    {
        var presses = m.SolveLinearEquations();

        return presses?.Tokens ?? 0;
    }

    private static List<ClawMachine> ParseInput()
    {
        var input = File.ReadAllText("inputs/day13.txt");

        var regex = @"Button A: X\+(\d+), Y\+(\d+)\nButton B: X\+(\d+), Y\+(\d+)\nPrize: X=(\d+), Y=(\d+)";
        var matches = Regex.Matches(input, regex);

        return matches.Select(match =>
        {
            var buttonA = new Button(int.Parse(match.Groups[1].ToString()), int.Parse(match.Groups[2].ToString()));
            var buttonB = new Button(int.Parse(match.Groups[3].ToString()), int.Parse(match.Groups[4].ToString()));
            return new ClawMachine(buttonA, buttonB, int.Parse(match.Groups[5].ToString()), int.Parse(match.Groups[6].ToString()));
        }).ToList();
    }
}

internal record Button(long X, long Y);

internal record ClawMachine(Button BtnA, Button BtnB, long PrizeX, long PrizeY)
{
    public long PrizeX { get; set; } = PrizeX;
    public long PrizeY { get; set; } = PrizeY;

    public bool PressingButtonsGetsPrize(long pressesOfBtnA, long pressesOfBtnB)
    {
        var xMatches = pressesOfBtnA * BtnA.X + pressesOfBtnB * BtnB.X == PrizeX;
        var yMatches = pressesOfBtnA * BtnA.Y + pressesOfBtnB * BtnB.Y == PrizeY;
        return xMatches && yMatches;
    }

    public BtnPresses? SolveLinearEquations()
    {
        var b = BtnB.Y * BtnA.X - BtnB.X * BtnA.Y;
        var b_p = PrizeY * BtnA.X - PrizeX * BtnA.Y;

        var a = BtnA.Y * BtnB.X - BtnA.X * BtnB.Y;
        var a_p = PrizeY * BtnB.X - PrizeX * BtnB.Y;

        if (b_p % b != 0 || a_p % a != 0)
            return null;

        var bPresses = b_p / b;
        var aPresses = a_p / a;

        return new BtnPresses(aPresses, bPresses);
    }
};

internal record BtnPresses(long APresses, long BPresses)
{
    public readonly long Tokens = APresses * 3 + BPresses;
}