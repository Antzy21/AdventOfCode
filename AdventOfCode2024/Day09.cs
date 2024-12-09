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

        var reversedFileList = new List<AocFile>(fs.Files);
        reversedFileList.Reverse();

        foreach (var file in reversedFileList)
        {
            var spaceToFitIndex = fs.Spaces.FindIndex(s => s.Size >= file.Size);

            if (file.Id < spaceToFitIndex)
                continue;

            if (spaceToFitIndex != -1)
            {
                var spaceToFit = fs.Spaces[spaceToFitIndex];
                var fileIndex = fs.Files.FindIndex(f => f == file);

                // Handle insert of File at front
                var newSpace = new AocFileSpace(spaceToFit.Size-file.Size);
                spaceToFit.Size = 0;
                fs.Files.Insert(spaceToFitIndex+1, file);
                fs.Spaces.Insert(spaceToFitIndex+1, newSpace);

                // Handle remove of File at back
                fs.Spaces[fileIndex].Size += file.Size + fs.Spaces[fileIndex+1].Size;
                fs.Files.RemoveAt(fileIndex+1);
                fs.Spaces.RemoveAt(fileIndex+1);
            }
        }

        long result = CheckSum(fs.ToDiskMap());
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
    public List<AocFile> Files { get; set; }
    public List<AocFileSpace> Spaces { get; set; }

    public AocFileSystem(int[] diskMap)
    {
        Spaces = [];
        Files = [];

        var idCounter = 0;
        var isFile = true;

        for (int i = 0; i < diskMap.Length; i++)
        {
            if (isFile)
            {
                Files.Add(new(idCounter, diskMap[i]));
                idCounter++;
            }
            else
            {
                Spaces.Add(new(diskMap[i]));
            }
            isFile = !isFile;
        }
        if (Files.Count > Spaces.Count)
            Spaces.Add(new(0));
    }

    public char[] ToDiskMap() {
        return ToString().ToCharArray();
    }

    public override string ToString()
    {
        var s = "";
        foreach (var (file, space) in Files.Zip(Spaces))
        {
            s += $"{file}{space}";
        }
        return s;
    }
}

internal record AocFileSpace(int Size)
{
    public int Size { get; set; } = Size;
    public override string ToString()
    {
        if (Size < 0)
            throw new Exception($"Size {Size} cannot be less than zero");
        return string.Concat(Enumerable.Repeat('.', Size));
    }
}

internal record AocFile(int Id, int Size)
{
    public override string ToString()
    {
        if (Size == 0)
            return "";
        return string.Concat(Enumerable.Repeat(Converter.IntToChar(Id), Size));
    }
}

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