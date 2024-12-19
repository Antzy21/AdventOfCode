
namespace AdventOfCode2024;

public class Day19 : IDaySolution
{
    private HashSet<string> _towels = [];

    public int? SolvePart1()
    {
        var (towels, designs) = ParseInput();

        _towels = towels;

        return designs.Count(CanBeCreatedWithTowels);
    }

    public int? SolvePart2()
    {
        return null;
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

    private static (HashSet<string>, string[]) ParseInput()
    {
        var input = File.ReadAllText("inputs/day19.txt").Trim().Split("\n\n");

        var towels = input[0].Split(", ").ToHashSet();
        
        var designs = input[1].Split("\n");

        return (towels, designs);
    }
}