using System.Data.Common;

namespace AdventOfCode2024;

public class Day09 : IDaySolution
{
    public int? SolvePart1()
    {
        var diskMap = ParseInput();

        var fileStorage = DecompressDiskMap(diskMap).ToArray();

        while (!IsFileStorageSorted(fileStorage))
        {
            fileStorage = ReorganiseStorage(fileStorage);
        }

        long result = CheckSum(fileStorage);
        Console.WriteLine($"Day 9 Part 1: {result}");

        return (int)result;
    }

    private static IEnumerable<char> DecompressDiskMap(IEnumerable<int> diskMap)
    {
        var isFile = true;
        var idCounter = 0;

        var result = new List<char>();

        return diskMap
            .Select(i =>
            {
                var c = isFile ? Converter.IntToChar(idCounter) : '.';

                var r = Enumerable.Repeat(c, i);

                if (isFile)
                    idCounter++;

                isFile = !isFile;

                return r;
            })
            .SelectMany(_ => _);
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

    private static long CheckSum(char[] fileStorage)
    {
        return fileStorage
            .Select((c, i) => {
                return c == '.' ? 0 : (long)(Converter.CharToInt(c) * i);
            })
            .Sum();
    }

    public int? SolvePart2()
    {
        var diskMap = ParseInput().ToArray();

        var fs = new AocFileSystem(diskMap);
        Console.WriteLine($"{fs}");

        var reversedFileList = new List<AocFileSlot>(fs.DiskSlots.Where(ds => ds.File != null));
        reversedFileList.Reverse();

        foreach (var fileSlot in fs.DiskSlots.Where(ds => ds.File != null).OrderByDescending(ds => ds.File?.Id))
        {
            var spaceSlot = fs.DiskSlots.FirstOrDefault(s =>
                s.Size >= fileSlot.Size &&
                s.DiskPos < fileSlot.DiskPos &&
                s.File == null
            );

            if (spaceSlot != null)
            {
                // Handle insert of File at front
                fileSlot.DiskPos = spaceSlot.DiskPos;
                spaceSlot.DiskPos += fileSlot.Size;
                spaceSlot.Size -= fileSlot.Size;

                // Handle remove of File at back
                // Console.WriteLine($"{fs}");
            }
        }

        long result = fs.CheckSum();
        
        Console.WriteLine($"{fs}");
        Console.WriteLine($"Day 9 Part 2: {result}");

        return (int)result;
    }

    private static IEnumerable<int> ParseInput()
    {
        return
            File.ReadAllText("inputs/day9.txt")
            .ToCharArray()
            .Select(x => int.Parse($"{x}"));
    }
}

internal class AocFileSystem
{
    public List<AocFileSlot> DiskSlots { get; set; }

    public AocFileSystem(int[] diskMap)
    {
        DiskSlots = [];

        var idCounter = 0;
        var isFile = true;
        var diskPos = 0;

        for (int i = 0; i < diskMap.Length; i++)
        {
            if (isFile)
            {
                DiskSlots.Add(new(diskMap[i], diskPos, new AocFile(idCounter)));
                idCounter++;
            }
            else
            {
                DiskSlots.Add(new(diskMap[i], diskPos));
            }
            diskPos += diskMap[i];
            isFile = !isFile;
        }
    }

    public long CheckSum() {
        return DiskSlots.Select(f => f.CheckSum()).Sum();
    }

    public override string ToString()
    {
        return string.Concat(DiskSlots.OrderBy(ds => ds.DiskPos).Select(d => d.ToString()));
    }
}

internal record AocFileSlot(int Size, int DiskPos, AocFile? File = null)
{
    public int Size { get; set; } = Size;
    public int DiskPos { get; set; } = DiskPos;
    public AocFile? File {get; set; } = File;
    public override string ToString()
    {
        if (Size < 0)
            throw new Exception($"Size {Size} cannot be less than zero");
        return string.Concat(Enumerable.Repeat(File?.ToString() ?? ".", Size));
    }

    public long CheckSum() {
        var result = 0;
        for (int i = DiskPos; i < DiskPos + Size; i++)
            result += i * (File?.Id ?? 0);
        return result;
    }
}

internal record AocFile(int Id, bool HasBeenMoved = false) {
    public override string ToString()
    {
        return (Id % 10).ToString();
    }
};

internal static class Converter {
    public static int CharToInt(char c)
    {
        return (int)c - 48;
    }
    public static char IntToChar(int i)
    {
        return (char)(i + 48);
    }
}