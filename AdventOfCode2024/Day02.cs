



using Microsoft.Win32.SafeHandles;

namespace AdventOfCode2024;

public class Day02 : IDaySolution
{
    public int? SolvePart1()
    {
        var list = ParseInput();

        var safeCount = list
            .Where(LineIsSafeForwardOrBackward)
            .Count();

        return safeCount;
    }

    public int? SolvePart2()
    {
        Console.WriteLine($"\n---\n");

        var list = ParseInput();

        var safeCount = list
            .Where(LineIsSafe)
            .Count();

        return safeCount;
    }

    private static bool LineIsSafe(List<int> line)
    {
        bool isSafe = LineIsSafeForwardOrBackward(line);

        for (int i = 0; i < line.Count; i++)
        {    
            if (!isSafe)
            {
                //Try removing element
                var lineRemoved = new List<int>(line);
                lineRemoved.RemoveAt(i);
                isSafe = LineIsSafeForwardOrBackward(lineRemoved);
            }
        }
        return isSafe;
    }

    private static bool LineIsSafeForwardOrBackward(List<int> line)
    {
        var isSafe = LineIsSafeAsc(line);
        if (!isSafe)
        {
            var lineRev = new List<int>(line);
            lineRev.Reverse();
            isSafe = LineIsSafeAsc(lineRev);
        }
        return isSafe;
    }

    private static bool LineIsSafeAsc(List<int> line)
    {
        var step1 = line.Select((l, i) =>
        {
            if (i == line.Count - 1)
            {
                return (1, 1);
            }
            if (i == line.Count - 2)
            {
                return (line[i + 1] - l, 1);
            }
            return (
                line[i + 1] - l,
                line[i + 2] - l
            );
        });

        var step2 = step1.Select((l, i) =>
        {
            var (l1, l2) = l;
            return (
                IsWithinTolerationLimit(l1),
                IsWithinTolerationLimit(l2)
            );
        });


        var step3 = step2.Select(x =>
        {
            var (fst, snd) = x;
            if (fst)
                return 0;
            if (snd)
                return 1;
            return 2;
        });

        var sum = step3.Sum();

        return sum == 0;
    }

    private static bool IsWithinTolerationLimit(int arg)
    {
        return arg <= 3 && arg > 0;
    }

    private List<List<int>> ParseInput()
    {
        var lines = File.ReadAllLines("inputs/day2.txt");

        return lines
            .Select(line =>
            {
                return line
                    .Split(" ")
                    .Select(i => int.Parse(i))
                    .ToList();
            })
            .ToList();
    }
}