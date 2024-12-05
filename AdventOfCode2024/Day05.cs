
namespace AdventOfCode2024;

public class Day05 : IDaySolution
{
    public int? SolvePart1()
    {
        var (pageOrderingRules, updates) = ParseInput();

        var sum = 0;

        foreach (var update in updates)
        {
            bool updateIsCorrect = true;

            foreach (var rule in pageOrderingRules)
            {
                updateIsCorrect = updateIsCorrect && UpdateFitsRule(update, rule);
            }

            if (updateIsCorrect)
            {
                sum += update.numbers[update.numbers.Count / 2];
            }

        }
        
        return sum;
    }

    private static bool UpdateFitsRule(Update update, PageOrderingRule rule)
    {
        var indexOfFirst = update.numbers.IndexOf(rule.First);
        var indexOfSecond = update.numbers.IndexOf(rule.Second);

        if (indexOfFirst != -1 && indexOfSecond != -1)
        {
            if (indexOfFirst > indexOfSecond)
            {
                return false;
            }
        }
        return true;
    }

    private static (List<PageOrderingRule>, List<Update>) ParseInput()
    {
        var lines = File.ReadAllLines("inputs/day5.txt");

        var part1 = lines
            .Where(l => l.Contains('|'))
            .Select(l => new PageOrderingRule(l))
            .ToList();

        var part2 = lines
            .Where(l => l.Contains(','))
            .Select(l => new Update(l))
            .ToList();

        return (part1, part2);
    }

    public int? SolvePart2()
    {
        return null;
    }
}

internal record Update
{
    public List<int> numbers;

    public Update(string l)
    {
        numbers = l
            .Split(',')
            .Select(int.Parse)
            .ToList();
    }
}

internal record PageOrderingRule
{
    public int First;
    public int Second;

    public PageOrderingRule(string line)
    {
        var nums = line.Split('|');
        First = int.Parse(nums[0]);
        Second = int.Parse(nums[1]);
    }
};