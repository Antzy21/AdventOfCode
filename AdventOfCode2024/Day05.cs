
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualBasic;

namespace AdventOfCode2024;

public class Day05 : IDaySolution
{
    public int? SolvePart1()
    {
        var (pageOrderingRules, updates) = ParseInput();

        var sum = 0;

        foreach (var update in updates)
        {
            var rulesForUpdate = GetRulesForUpdate(update, pageOrderingRules);
            var orderedRules = OrderRules(rulesForUpdate);
            bool updateIsCorrect = UpdateFitsAllRules(rulesForUpdate, update, out var failedUpdate);

            if (updateIsCorrect)
            {
                sum += update.numbers[update.numbers.Count / 2];
            }

        }

        return sum;
    }

    private object OrderRules(List<PageOrderingRule> rules)
    {
        var newList = new List<PageOrderingRule>();

        while (rules.Count != 0) {
            var highestRuleNumber = rules.First(r =>
                rules.All(rule => rule.Second != r.First)
            ).First;

            newList.AddRange(rules.Where(r => r.First == highestRuleNumber));
            rules.RemoveAll(r => r.First == highestRuleNumber);
        }
        return newList;
    }

    private static List<PageOrderingRule> GetRulesForUpdate(Update update, List<PageOrderingRule> pageOrderingRules)
    {
        return pageOrderingRules.Where(r =>
            update.numbers.Contains(r.First) && update.numbers.Contains(r.Second)
        ).ToList();
    }

    private static bool UpdateFitsAllRules(List<PageOrderingRule> pageOrderingRules, Update update, [NotNullWhen(false)] out PageOrderingRule? failedRule)
    {
        failedRule = null;

        foreach (var rule in pageOrderingRules)
        {
            if (!UpdateFitsRule(update, rule)) {
                failedRule = rule;
                return false;
            }
        }

        return true;
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

    public int? SolvePart2()
    {
        var (pageOrderingRules, updates) = ParseInput();

        var sum = 0;

        foreach (var update in updates)
        {
            bool updateIsCorrect = false;

            foreach (var rule in pageOrderingRules)
            {
                updateIsCorrect = updateIsCorrect && UpdateFitsRule(update, rule);
            }

        }
        return sum;
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