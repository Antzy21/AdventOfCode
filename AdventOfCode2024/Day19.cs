
namespace AdventOfCode2024;

public class Day19 : IDaySolution
{
    private HashSet<string> _towels = [];
    private readonly Dictionary<string, long> _cachedCount = [];

    public int? SolvePart1()
    {
        var (towels, designs) = ParseInput();

        _towels = towels;

        return designs.Count(CanBeCreatedWithTowels);
    }

    public int? SolvePart2()
    {
        var (towels, designs) = ParseInput();

        _towels = towels;

        long result = designs.Select(NumberOfWaysToBeCreatedWithTowels).Sum();
        Console.WriteLine($"Day 19 Part 2: {result}");
        

        return (int)result;
    }

    private bool CanBeCreatedWithTowels(string design)
    {
        if (design == "")
            return true;
        
        foreach (var towel in _towels.OrderByDescending(t => t.Length))
        {
            if (towel.Length > design.Length) {
                continue;
            }

            if (towel == design[0..towel.Length] && CanBeCreatedWithTowels(design[towel.Length ..])) {
                _towels.Add(design);
                return true;
            }
        }

        return false;
    }

    private long NumberOfWaysToBeCreatedWithTowels(string design)
    {
        if (design == "")
            return 1;

        if (_cachedCount.TryGetValue(design, out var count))
            return count;
        
        long creationWays = 0;

        foreach (var towel in _towels.OrderByDescending(t => t.Length))
        {
            if (towel.Length <= design.Length && towel == design[0..towel.Length]) {
                creationWays += NumberOfWaysToBeCreatedWithTowels(design[towel.Length ..]);
            }
        }

        _cachedCount[design] = creationWays;

        return creationWays;
    }

    private static (HashSet<string>, string[]) ParseInput()
    {
        var input = File.ReadAllText("inputs/day19.txt").Trim().Split("\n\n");

        var towels = input[0].Split(", ").ToHashSet();
        
        var designs = input[1].Split("\n");

        return (towels, designs);
    }
}