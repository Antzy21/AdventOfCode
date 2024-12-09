namespace AdventOfCode2024;

public class Day98 : IDaySolution
{
    public int? SolvePart1()
    {
        var diskMap = ParseInput();

        Console.WriteLine($"Decompressing...");
        var fileStorage = DecompressDiskMap(diskMap).ToArray();
        Console.WriteLine($"{string.Concat(fileStorage)}");

        Console.WriteLine($"Sorting...");
        while (!IsFileStorageSorted(fileStorage))
        {
            fileStorage = ReorganiseStorage(fileStorage);
        }
        Console.WriteLine($"{string.Concat(fileStorage)}");

        Console.WriteLine($"Checking Sum...");
        long result = CheckSum(fileStorage);
        Console.WriteLine($"{result}");

        return (int)result;
    }

    private static long CheckSum(char[] fileStorage)
    {
        return fileStorage
            .Where(c => c != '.')
            .Select((c, i) => (long)(CharIntConverter(c) * i))
            .Sum();
    }

    private static char[] ReorganiseStorage(char[] fileStorage)
    {
        var fstDotIndex = Array.FindIndex(fileStorage, c => c == '.');
        var lastNumIndex = Array.FindLastIndex(fileStorage, c => c != '.');
        fileStorage[fstDotIndex] = fileStorage[lastNumIndex];
        fileStorage[lastNumIndex] = '.';
        return fileStorage;
    }

    private static bool IsFileStorageSorted(char[] fileStorage)
    {
        return Array.IndexOf(fileStorage, '.') > Array.FindLastIndex(fileStorage, c => c != '.');
    }

    private static IEnumerable<char> DecompressDiskMap(IEnumerable<int> diskMap)
    {
        var isFile = true;
        var idCounter = 0;

        var result = new List<char>();

        return diskMap
            .Select(i =>
            {
                var c = isFile ? IntCharConverter(idCounter) : '.';

                var r = Enumerable.Repeat(c, i);

                if (isFile)
                    idCounter++;

                isFile = !isFile;

                return r;
            })
            .SelectMany(_ => _);
    }

    public int? SolvePart2()
    {
        return null;
    }

    private static IEnumerable<int> ParseInput()
    {
        return
            File.ReadAllLines("inputs/day9.txt")[0]
            .ToCharArray()
            .Select(CharIntConverter);
    }

    private static int CharIntConverter(char c)
    {
        return (int)c - 20;
    }
    private static char IntCharConverter(int i)
    {
        return (char)(i + 20);
    }
}