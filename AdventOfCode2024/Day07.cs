
namespace AdventOfCode2024;

public class Day07 : IDaySolution
{
    public int? SolvePart1()
    {
        var input = ParseInput();

        long sum = 0;
        foreach (var (target, numbers) in input)
        {            
            if (CanBeSolved(target, numbers[1 ..], numbers[0])) {
                sum += target;
            }
        }
        Console.WriteLine($"Day 7 Part 1: {sum}");

        return (int)sum;
    }

    private static bool CanBeSolved(long testValue, List<long> numbers, long current, bool useConcatenationOperator = false)
    {
        if (numbers.Count == 0) {
            return testValue == current;
        }

        // Multiply
        var multiplyResult = current * numbers[0];

        // Sum
        var sumResult = current + numbers[0];

        // Overshoot
        if (multiplyResult <= testValue) {
            var solvedByMultiplying = CanBeSolved(testValue, numbers[1 ..], multiplyResult, useConcatenationOperator);

            if (solvedByMultiplying)
                return true;
        }

        // Overshoot
        if (sumResult <= testValue) {
            var solvedBySumming = CanBeSolved(testValue, numbers[1 ..], sumResult, useConcatenationOperator);

            if (solvedBySumming)
                return true;
        }

        if (useConcatenationOperator) {
            var concatResult = long.Parse($"{current}{numbers[0]}");

            // Overshoot
            if (concatResult <= testValue) {
                var solvedByConcat = CanBeSolved(testValue, numbers[1 ..], concatResult, useConcatenationOperator);

                if (solvedByConcat)
                    return true;
            }
        }

        return false;
    }

    public int? SolvePart2()
    {
        var input = ParseInput();

        long sum = 0;
        foreach (var (target, numbers) in input)
        {            
            if (CanBeSolved(target, numbers[1 ..], numbers[0], useConcatenationOperator: true)) {
                sum += target;
            }
        }
        Console.WriteLine($"Day 7 Part 2: {sum}");

        return (int)sum;
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