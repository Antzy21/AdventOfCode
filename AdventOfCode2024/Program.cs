using System.CommandLine;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("Advent of Code 2024 - CLI");

        var allOption = new Option<bool>(
            name: "--all",
            getDefaultValue: () => false,
            description: "Run solutions for all days"
        );

        rootCommand.AddOption(allOption);

        rootCommand.SetHandler(Handler, allOption);

        return await rootCommand.InvokeAsync(args);
    }

    static void Handler(bool runAll)
    {
        IEnumerable<IDaySolution> daySolutions =
            AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(t => typeof(IDaySolution).IsAssignableFrom(t) && t.IsClass)
            .Select(Activator.CreateInstance)
            .OfType<IDaySolution>();

        if(runAll) {
            foreach (var daySolution in daySolutions)
            {
                PrintDaySolution(daySolution);
            }
        }
        else if (daySolutions.Count() != 0) {
            PrintDaySolution(daySolutions.Last());
        }
    }

    static private void PrintDaySolution(IDaySolution daySolution) {
        var solution1 = daySolution.SolvePart1();
        var solution2 = daySolution.SolvePart2();
        Console.WriteLine($"{daySolution.GetType().Name} - 1: {solution1?.ToString() ?? "?"}");
        Console.WriteLine($"{daySolution.GetType().Name} - 2: {solution2?.ToString() ?? "?"}");
    }
}