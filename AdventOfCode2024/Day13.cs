using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day13 : IDaySolution
{
    public int? SolvePart1()
    {
        var clawMachines = ParseInput();

        return clawMachines.Select(GetMinimumTokensToWin).Sum();
    }

    private int GetMinimumTokensToWin(ClawMachine machine, int arg2)
    {
        int? tokens = null;
        for (int a = 1; a <= 100; a++)
        {
            for (int b = 1; b <= 100; b++)
            {
                if (machine.PressingButtonsGetsPrize(a, b)) {
                    tokens = Math.Min(a * 3 + b, tokens ?? int.MaxValue);
                }
            }
        }
        
        return tokens ?? 0;
    }

    public int? SolvePart2()
    {
        return null;
    }

    private List<ClawMachine> ParseInput()
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

internal record Button(int X, int Y);

internal record ClawMachine(Button BtnA, Button BtnB, int PrizeX, int PrizeY) {
    public bool PressingButtonsGetsPrize(int pressesOfBtnA, int pressesOfBtnB) {
        var xMatches = pressesOfBtnA * BtnA.X + pressesOfBtnB * BtnB.X == PrizeX;
        var yMatches = pressesOfBtnA * BtnA.Y + pressesOfBtnB * BtnB.Y == PrizeY;
        return xMatches && yMatches;
    }
};
