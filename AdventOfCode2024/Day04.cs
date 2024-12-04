namespace AdventOfCode2024;

public class Day04 : IDaySolution
{
    public int? SolvePart1()
    {
        var input = ParseInput();

        var XmasCount = 0;

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {                
                XmasCount += CheckForXmas(i, j, input, 1, 0);
                XmasCount += CheckForXmas(i, j, input, 1, 1);
                XmasCount += CheckForXmas(i, j, input, 1, -1);
                XmasCount += CheckForXmas(i, j, input, 0, 1);
                XmasCount += CheckForXmas(i, j, input, 0, -1);
                XmasCount += CheckForXmas(i, j, input, -1, 0);
                XmasCount += CheckForXmas(i, j, input, -1, 1);
                XmasCount += CheckForXmas(i, j, input, -1, -1);
            }
        }

        return XmasCount;
    }

    private static int CheckForXmas(int i, int j, char[][] input, int xDir, int yDir)
    {
        var hasX = input[i][j] == 'X';
        if (!hasX)
            return 0;
        
        try {
            var hasM = input[i+xDir][j+yDir] == 'M';
            if (!hasM)
                return 0;

            var hasA = input[i+xDir*2][j+yDir*2] == 'A';
            if (!hasA)
                return 0;
            
            var hasS = input[i+xDir*3][j+yDir*3] == 'S';
            if (!hasS)
                return 0;
        }
        catch {
            return 0;
        }
        return 1;
    }

    public int? SolvePart2()
    {
        var input = ParseInput();

        var XmasCount = 0;

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {                
                XmasCount += CheckForX_Mmas(i, j, input);
            }
        }

        return XmasCount;
    }

    private static int CheckForX_Mmas(int i, int j, char[][] input)
    {
        var hasA = input[i][j] == 'A';
        if (!hasA)
            return 0;
        
        try {
            var hasM_TopLeft = input[i-1][j-1] == 'M';
            var hasS_BotRght = input[i+1][j+1] == 'S';

            var hasS_TopLeft = input[i-1][j-1] == 'S';
            var hasM_BotRght = input[i+1][j+1] == 'M';

            if ((hasM_TopLeft && hasS_BotRght) || (hasS_TopLeft && hasM_BotRght)) {
                var hasM_BotLeft = input[i+1][j-1] == 'M';
                var hasS_TopRght = input[i-1][j+1] == 'S';

                var hasS_BotLeft = input[i+1][j-1] == 'S';
                var hasM_TopRght = input[i-1][j+1] == 'M';

                if ((hasM_BotLeft && hasS_TopRght) || (hasS_BotLeft && hasM_TopRght)) {
                    return 1;
                }
            }

        }
        catch {
            return 0;
        }
        return 0;
    }

    private char[][] ParseInput()
    {
        var lines = File.ReadAllLines("inputs/day4.txt");

        return lines.Select(line =>
            line.ToCharArray()
        ).ToArray();
    }
}