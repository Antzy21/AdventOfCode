using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day21 : IDaySolution
{
    public int? SolvePart1()
    {
        var codes = ParseInput();

        return codes.Select(code =>
        {
            var buttonSequence = GetButtonSequence(code, RobotInstructionsToPressKeypad);

            for (int i = 0; i < 2; i++)
            {
                buttonSequence = buttonSequence
                    .SelectMany(bs => GetButtonSequence(bs, RobotInstructionsToPressArrows))
                    .ToList();

                buttonSequence = buttonSequence
                    .Where(bs => !buttonSequence.Any(bs2 => bs2.Length < bs.Length))
                    .ToList();
            }

            var minBs = buttonSequence.MinBy(bs => bs.Length)!;

            return GetCodeNum(code) * minBs.Length;
        }).Sum();
    }

    public int? SolvePart2()
    {
        var codes = ParseInput();

        return codes.Select(code =>
        {
            Console.WriteLine($"{code}");

            var buttonSequence = GetButtonSequence(code, RobotInstructionsToPressKeypad);

            // i < 25; Memoisation required.
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine($"{buttonSequence[0]}");

                buttonSequence = buttonSequence
                    .SelectMany(bs => GetButtonSequence(bs, RobotInstructionsToPressArrows))
                    .ToList();

                buttonSequence = buttonSequence
                    .Where(bs => !buttonSequence.Any(bs2 => bs2.Length < bs.Length))
                    .ToList();
            }

            var minBs = buttonSequence.MinBy(bs => bs.Length)!;

            return GetCodeNum(code) * minBs.Length;
        }).Sum();
    }

    private static int GetCodeNum(string code)
    {
        var regex = @"(\d+)";
        var match = Regex.Match(code, regex);
        return int.Parse(match.ToString());
    }

    private static List<string> GetButtonSequence(string code, Dictionary<KeyPair, IEnumerable<string>> RobotInstructions)
    {
        var currentKey = 'A';

        var possibleSequences = new List<string> { "" };

        foreach (var nextKey in code)
        {
            var movements = RobotInstructions[new (currentKey, nextKey)];
            possibleSequences = possibleSequences
                .SelectMany(s => movements.Select(m =>
                    s + m + 'A'
                ))
                .ToList();
            currentKey = nextKey;
        }

        return possibleSequences;
    }

    private static readonly Dictionary<KeyPair, IEnumerable<string>> RobotInstructionsToPressKeypad = new()
    {
        { new('0', '1'), ["^<"] },
        { new('0', '2'), ["^"] },
        { new('0', '3'), ["^>", ">^"] },
        { new('0', '4'), ["^^<"] },
        { new('0', '5'), ["^^"] },
        { new('0', '6'), ["^^>", ">^^"] },
        { new('0', '7'), ["^^^<"] },
        { new('0', '8'), ["^^^"] },
        { new('0', '9'), ["^^^>", ">^^^"] },
        { new('0', 'A'), [">"] },
        { new('0', '0'), [""] },

        { new('1', '1'), [""] },
        { new('1', '2'), [">"] },
        { new('1', '3'), [">>"] },
        { new('1', '4'), ["^"] },
        { new('1', '5'), ["^>", ">^"] },
        { new('1', '6'), ["^>>", ">>^"] },
        { new('1', '7'), ["^^"] },
        { new('1', '8'), ["^^>", ">^^"] },
        { new('1', '9'), ["^^>>", ">>^^"] },
        { new('1', 'A'), [">>v"] },
        { new('1', '0'), [">v"] },

        { new('2', '1'), ["<"] },
        { new('2', '2'), [""] },
        { new('2', '3'), [">"] },
        { new('2', '4'), ["^<", "<^"] },
        { new('2', '5'), ["^"] },
        { new('2', '6'), ["^>", ">^"] },
        { new('2', '7'), ["^^<", "<^^"] },
        { new('2', '8'), ["^^"] },
        { new('2', '9'), ["^^>", ">^^"] },
        { new('2', 'A'), ["v>", ">v"] },
        { new('2', '0'), ["v"] },

        { new('3', '1'), ["<<"] },
        { new('3', '2'), ["<"] },
        { new('3', '3'), [""] },
        { new('3', '4'), ["^<<", "<<^"] },
        { new('3', '5'), ["^<", "<^"] },
        { new('3', '6'), ["^"] },
        { new('3', '7'), ["<<^^", "^^<<"] },
        { new('3', '8'), ["^^<", "<^^"] },
        { new('3', '9'), ["^^"] },
        { new('3', 'A'), ["v"] },
        { new('3', '0'), ["<v", "v<"] },

        { new('4', '1'), ["v"] },
        { new('4', '2'), ["v>", ">v"] },
        { new('4', '3'), ["v>>", ">>v"] },
        { new('4', '4'), [""] },
        { new('4', '5'), [">"] },
        { new('4', '6'), [">>"] },
        { new('4', '7'), ["^"] },
        { new('4', '8'), ["^>", ">^"] },
        { new('4', '9'), ["^>>", ">>^"] },
        { new('4', 'A'), [">>vv"] },
        { new('4', '0'), [">vv"] },

        { new('5', '1'), ["<v", "v<"] },
        { new('5', '2'), ["v"] },
        { new('5', '3'), ["v>", ">v"] },
        { new('5', '4'), ["<"] },
        { new('5', '5'), [""] },
        { new('5', '6'), [">"] },
        { new('5', '7'), ["^<", "<^"] },
        { new('5', '8'), ["^"] },
        { new('5', '9'), ["^>", ">^"] },
        { new('5', 'A'), ["vv>", ">vv"] },
        { new('5', '0'), ["vv"] },

        { new('6', '1'), ["<<v", "v<<"] },
        { new('6', '2'), ["<v", "v<"] },
        { new('6', '3'), ["v"] },
        { new('6', '4'), ["<<"] },
        { new('6', '5'), ["<"] },
        { new('6', '6'), [""] },
        { new('6', '7'), ["^<<", "<<^"] },
        { new('6', '8'), ["^<", "<^"] },
        { new('6', '9'), ["^"] },
        { new('6', 'A'), ["vv"] },
        { new('6', '0'), ["<vv", "vv<"] },

        { new('7', '1'), ["vv"] },
        { new('7', '2'), ["vv>", ">vv"] },
        { new('7', '3'), ["vv>>", ">>vv"] },
        { new('7', '4'), ["v"] },
        { new('7', '5'), ["v>", ">v"] },
        { new('7', '6'), ["v>>", ">>v"] },
        { new('7', '7'), [""] },
        { new('7', '8'), [">"] },
        { new('7', '9'), [">>"] },
        { new('7', 'A'), [">>vvv"] },
        { new('7', '0'), [">vvv"] },

        { new('8', '1'), ["<vv", "vv<"] },
        { new('8', '2'), ["vv"] },
        { new('8', '3'), ["vv>", ">vv"] },
        { new('8', '4'), ["<v", "v<"] },
        { new('8', '5'), ["v"] },
        { new('8', '6'), ["v>", ">v"] },
        { new('8', '7'), ["<"] },
        { new('8', '8'), [""] },
        { new('8', '9'), [">"] },
        { new('8', 'A'), ["vvv>", ">vvv"] },
        { new('8', '0'), ["vvv"] },

        { new('9', '1'), ["<<vv", "vv<<"] },
        { new('9', '2'), ["<vv", "vv<"] },
        { new('9', '3'), ["vv"] },
        { new('9', '4'), ["<<v", "v<<"] },
        { new('9', '5'), ["<v", "v<"] },
        { new('9', '6'), ["v"] },
        { new('9', '7'), ["<<"] },
        { new('9', '8'), ["<"] },
        { new('9', '9'), [""] },
        { new('9', 'A'), ["vvv"] },
        { new('9', '0'), ["<vvv", "vvv<"] },

        { new('A', '1'), ["^<<"] },
        { new('A', '2'), ["^<", "<^"] },
        { new('A', '3'), ["^"] },
        { new('A', '4'), ["^^<<"] },
        { new('A', '5'), ["^^<", "<^^"] },
        { new('A', '6'), ["^^"] },
        { new('A', '7'), ["^^^<<"] },
        { new('A', '8'), ["^^^<", "<^^^"] },
        { new('A', '9'), ["^^^"] },
        { new('A', 'A'), [""] },
        { new('A', '0'), ["<"] },
    };

    private static readonly Dictionary<KeyPair, IEnumerable<string>> RobotInstructionsToPressArrows = new()
    {
        {new ('A', '^'), ["<"]},
        {new ('A', '<'), ["v<<"]},
        {new ('A', 'v'), ["<v", "v<"]},
        {new ('A', '>'), ["v"]},
        {new ('A', 'A'), [""]},

        {new ('^', '^'), [""]},
        {new ('^', '<'), ["v<"]},
        {new ('^', 'v'), ["v"]},
        {new ('^', '>'), ["v>", ">v"]},
        {new ('^', 'A'), [">"]},

        {new ('<', '^'), [">^"]},
        {new ('<', '<'), [""]},
        {new ('<', 'v'), [">"]},
        {new ('<', '>'), [">>"]},
        {new ('<', 'A'), [">>^"]},

        {new ('v', '^'), ["^"]},
        {new ('v', '<'), ["<"]},
        {new ('v', 'v'), [""]},
        {new ('v', '>'), [">"]},
        {new ('v', 'A'), ["^>", ">^"]},

        {new ('>', '^'), ["^<", "<^"]},
        {new ('>', '<'), ["<<"]},
        {new ('>', 'v'), ["<"]},
        {new ('>', '>'), [""]},
        {new ('>', 'A'), ["^"]}
    };

    private static char GetArrowFromInstructions(string instructions, char startArrow = 'A')
    {
        if (instructions == "")
            return startArrow;
        return (startArrow, instructions[0]) switch
        {
            ('A', '<') => GetArrowFromInstructions(instructions[1..], '^'),
            ('A', 'v') => GetArrowFromInstructions(instructions[1..], '>'),

            ('^', 'v') => GetArrowFromInstructions(instructions[1..], 'v'),
            ('^', '>') => GetArrowFromInstructions(instructions[1..], 'A'),

            ('<', '>') => GetArrowFromInstructions(instructions[1..], 'v'),

            ('v', '^') => GetArrowFromInstructions(instructions[1..], '^'),
            ('v', '<') => GetArrowFromInstructions(instructions[1..], '<'),
            ('v', '>') => GetArrowFromInstructions(instructions[1..], '>'),

            ('>', '^') => GetArrowFromInstructions(instructions[1..], 'A'),
            ('>', '<') => GetArrowFromInstructions(instructions[1..], 'v'),

            _ => '?'
        };
    }

    private static string TraverseInstructions(string instructions)
    {
        var startingChar = 'A';
        return string.Concat(instructions.Split('A').Select(c =>
        {
            startingChar = GetArrowFromInstructions(c, startingChar);
            return $"{startingChar}";
        }));
    }

    internal static IEnumerable<string> ParseInput()
    {
        return File.ReadAllLines("inputs/day21.txt");
    }
}

internal delegate IEnumerable<string> GetRobotInstruction(char currentKey, char nextKey);

internal record KeyPair(char CurrentKey, char NextKey);