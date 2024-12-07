
namespace AdventOfCode2024;

public class Day07 : IDaySolution
{
    public int? SolvePart1()
    {
        var input = ParseInput();

        var sum = 0;
        foreach (var (target, numbers) in input)
        {            
            if (CanBeSolved(target, numbers[1 ..], numbers[0], $"{numbers[0]}")) {
                sum += (int)target;
            }
            Console.WriteLine($"---");
        }
        return sum;
    }

    private static bool CanBeSolved(long testValue, List<long> numbers, long current, string helper)
    {
        Console.WriteLine($"{helper, 20} = {current, -5} {(current==testValue ? "=":"?")} {testValue} - [{string.Join(", ",numbers)}]");

        if (numbers.Count == 0) {
            return testValue == current;
        }

        // Multiply
        var multiplyResult = current * numbers[0];

        // Sum
        var sumResult = current + numbers[0];

        // Overshoot
        if (multiplyResult <= testValue) {
            var solvedByMultiplying = CanBeSolved(testValue, numbers[1 ..], multiplyResult, helper+$" * {numbers[0]}");

            if (solvedByMultiplying)
                return true;
        }

        // Overshoot
        if (sumResult <= testValue) {
            var solvedBySumming = CanBeSolved(testValue, numbers[1 ..], sumResult, helper+$" + {numbers[0]}");

            if (solvedBySumming)
                return true;
        }

        return false;
    }

    public int? SolvePart2()
    {
        return null;
    }

    private List<(long, List<long>)> ParseInput()
    {
        var input = File.ReadAllLines("inputs/day7.txt");
                
        return input.Select(line =>
        {
            var x = line.Split(": ");
            var testValue = long.Parse(x[0]);
            var numbers = x[1].Split(' ').Select(long.Parse).ToList();
            return (testValue, numbers);
        }).ToList();
    }
}