using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day03 : IDaySolution
{
    public int? SolvePart1()
    {
        var input = ParseInput();

        var matches = getRegexMulMatches(input);

        return matches.Select(CalculateMul).Sum();
    }

    private MatchCollection getRegexMulMatches(string input)
    {
        var regex = @"mul\((\d+),(\d+)\)";
        return Regex.Matches(input, regex);
    }

    private int CalculateMul(Match m)
    {
        var x = int.Parse(m.Groups[1].ToString());
        var y = int.Parse(m.Groups[2].ToString());
        return x * y;
    }

    public int? SolvePart2()
    {
        var input = ParseInput();
        var dontRegex = @"don't\(\).+?(?=do\(\)|\n)";
        var removeDonts = Regex.Replace(input, dontRegex, "");

        Console.WriteLine($"{removeDonts}");

        var matches = getRegexMulMatches(removeDonts);

        return matches.Select(CalculateMul).Sum();
    }

    private string ParseInput()
    {
        return File.ReadAllText("inputs/day3.txt");
    }
}