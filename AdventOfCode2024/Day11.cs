namespace AdventOfCode2024;

public class Day11 : IDaySolution
{
    private readonly Dictionary<StoneAndIterations, long> _dict = new()
    {
        { new (new Stone(0), 1), 1 },
        { new (new Stone(0), 2), 1 }
    };

    public int? SolvePart1()
    {
        var stones = ParseInput();

        var result = stones.Select(s =>
        {
            var r = Iterate(s, 25);
            Console.WriteLine($"Completed {s}: size: {r}");
            return r;
        }).Sum();

        return (int)result;
    }

    public int? SolvePart2()
    {
        var stones = ParseInput();

        var result = stones.Select(s =>
        {
            var r = Iterate(s, 75);
            Console.WriteLine($"Completed {s}: size: {r}");
            return r;
        }).Sum();

        Console.WriteLine($"Day 11 Part 2: {result}");

        return (int)result;
    }

    internal long Iterate(Stone stone, int remainingIterations)
    {
        if (remainingIterations == 0)
        {
            return 1;
        }

        if (_dict.TryGetValue(new(stone, remainingIterations), out long iteratedStonesCount))
        {
            return iteratedStonesCount;
        }

        var stoneString = $"{stone.Value}";

        if (stone.Value == 0)
        {
            var newStone = new Stone(1);
            return Iterate(newStone, remainingIterations - 1);
        }

        if (stoneString.Length % 2 != 0)
        {
            var newStone = new Stone(stone.Value * 2024);
            var result = Iterate(newStone, remainingIterations - 1);
            _dict[new(stone, remainingIterations)] = result;
        }
        else
        {
            var stoneString1 = stoneString[0..(stoneString.Length / 2)];
            var stoneString2 = stoneString[(stoneString.Length / 2)..];
            var newStone1 = new Stone(long.Parse(stoneString1));
            var newStone2 = new Stone(long.Parse(stoneString2));
            var result =
                Iterate(newStone1, remainingIterations - 1) +
                Iterate(newStone2, remainingIterations - 1);
            _dict[new(stone, remainingIterations)] = result;
        }
        return _dict[new(stone, remainingIterations)];
    }

    internal static IEnumerable<Stone> ParseInput()
    {
        return File.ReadAllText("inputs/day11.txt")
            .Trim()
            .Split(" ")
            .Select(c => new Stone(long.Parse(c)));
    }

    internal record Stone(long Value);

    internal record StoneAndIterations(Stone Stone, int Iterations)
    {
        public override string ToString()
        {
            return $"{Stone.Value}-it{Iterations}";
        }
    };
}