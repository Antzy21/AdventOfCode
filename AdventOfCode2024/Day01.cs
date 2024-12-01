using System.IO.Pipelines;

namespace AdventOfCode2024;

public class Day01 : IDaySolution
{
    public int? SolvePart1()
    {
        var (list1, list2) = ParseInput();

        var orderedList1 = list1.Order();
        var orderedList2 = list2.Order();

        var result = orderedList1
            .Zip(orderedList2, (x, y) => Math.Abs(x-y))
            .Sum();

        return result;
    }

    public int? SolvePart2()
    {
        var (list1, list2) = ParseInput();

        var result = list1
            .Select(l1 => list2.Count(l2 => l2 == l1) * l1)
            .Sum();

        return result;
    }

    private (List<int>, List<int>) ParseInput()
    {
        var input = File.ReadAllLines("inputs/day1.txt");
        
        var list1 = new List<int>();
        var list2 = new List<int>();
        
        foreach (var line in input)
        {
            var x = line.Split(" ").Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            list1.Add(int.Parse(x[0]));
            list2.Add(int.Parse(x[1]));
        }

        return (list1, list2);
    }
}