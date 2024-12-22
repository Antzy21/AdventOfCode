using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day21 : IDaySolution
{
    public int? SolvePart1()
    {
        var codes = ParseInput();

        return codes.Select(code =>
        {
            Console.WriteLine($"{code}");

            string buttonSequence1 = GetButtonSequence(code, GetRobotInstructionsToPressKeypad);
            Console.WriteLine($"{buttonSequence1}");
            
            string buttonSequence2 = GetButtonSequence(buttonSequence1, GetRobotInstructionsToPressArrows);
            Console.WriteLine($"{buttonSequence2}");

            string buttonSequence3 = GetButtonSequence(buttonSequence2, GetRobotInstructionsToPressArrows);
            Console.WriteLine($"{buttonSequence3}");

            return GetCodeNum(code) * buttonSequence3.Length;
        }).Sum();
    }

    public int? SolvePart2()
    {
        return 0;
    }

    private static int GetCodeNum(string code)
    {
        var regex = @"(\d+)";
        var match = Regex.Match(code, regex);
        return int.Parse(match.ToString());
    }

    private static string GetButtonSequence(string code, GetRobotInstruction GetRobotInstructionFunc)
    {
        var currentKey = 'A';
        return string.Concat(
            code.ToCharArray()
            .Select(nextKey =>
            {
                var movements = GetRobotInstructionFunc(currentKey, nextKey);
                currentKey = nextKey;
                return movements + 'A';
            })
        );
    }

    private static IEnumerable<string> GetRobotInstructionsToPressKeypad(char currentKey, char nextKey)
    {
        return (currentKey, nextKey) switch
        {
            ('0', '1') => ["^<"],
            ('0', '2') => ["^"],
            ('0', '3') => ["^>", ">^"],
            ('0', '4') => ["^^<"],
            ('0', '5') => ["^^"],
            ('0', '6') => ["^^>", ">^^"],
            ('0', '7') => ["^^^<"],
            ('0', '8') => ["^^^"],
            ('0', '9') => ["^^^>", ">^^^"],
            ('0', 'A') => [">"],
            ('0', '0') => [""],

            ('1', '1') => [""],
            ('1', '2') => [">"],
            ('1', '3') => [">>"],
            ('1', '4') => ["^"],
            ('1', '5') => [">^", "^>"],
            ('1', '6') => [">>^", "^>>"],
            ('1', '7') => ["^^"],
            ('1', '8') => [">^^", "^^>"],
            ('1', '9') => [">>^^", "^^>>"],
            ('1', 'A') => [">>v", ">v>"],
            ('1', '0') => [">v"],

            ('2', '1') => ["<"],
            ('2', '2') => [""],
            ('2', '3') => [">"],
            ('2', '4') => ["<^", "^<"],
            ('2', '5') => ["^"],
            ('2', '6') => [">^", "^>"],
            ('2', '7') => ["<^^", "^^<"],
            ('2', '8') => ["^^"],
            ('2', '9') => [">^^", "^^>"],
            ('2', 'A') => [">v"],
            ('2', '0') => ["v"],

            ('3', '1') => ["<<"],
            ('3', '2') => ["<"],
            ('3', '3') => [""],
            ('3', '4') => ["^<<", "<^<"],
            ('3', '5') => ["^<", "<^"],
            ('3', '6') => ["^"],
            ('3', '7') => ["^^<<", "<<^^"],
            ('3', '8') => ["^^<", "<^^"],
            ('3', '9') => ["^^"],
            ('3', 'A') => [""],
            ('3', '0') => ["v<", "<v"],

            ('4', '1') => ["v"],
            ('4', '2') => ["v>"],
            ('4', '3') => ["v>>", ">>v"],
            ('4', '4') => [""],
            ('4', '5') => [">"],
            ('4', '6') => [">>"],
            ('4', '7') => ["^"],
            ('4', '8') => ["v>", ">^"],
            ('4', '9') => ["^>>", ">>^"],
            ('4', 'A') => [">>vv"],
            ('4', '0') => [">vv"],

            ('5', '1') => ["v<", "<v"],
            ('5', '5') => ["v"],
            ('5', '3') => ["v>", ">v"],
            ('5', '4') => ["<"],
            ('5', '2') => ["v"],
            ('5', '6') => [">"],
            ('5', '7') => ["<^", "^<"],
            ('5', '8') => ["^"],
            ('5', '9') => [">^", "^>"],
            ('5', 'A') => ["vv>", ">vv"],
            ('5', '0') => ["vv"],

            ('6', '1') => ["v<<", "<<v"],
            ('6', '2') => ["v<", "<v"],
            ('6', '3') => ["v"],
            ('6', '4') => ["<<"],
            ('6', '5') => ["<"],
            ('6', '6') => [""],
            ('6', '7') => ["^<<", "<<^"],
            ('6', '8') => ["^<", "<^"],
            ('6', '9') => ["^"],
            ('6', 'A') => ["v"],
            ('6', '0') => ["vv<", "<vv"],

            ('7', '1') => ["vv"],
            ('7', '2') => ["vv>", ">vv"],
            ('7', '3') => ["vv>>", ">>vv"],
            ('7', '4') => ["v"],
            ('7', '5') => ["v>", ">v"],
            ('7', '6') => ["v>>", ">>v"],
            ('7', '7') => [""],
            ('7', '8') => [">"],
            ('7', '9') => [">>"],
            ('7', 'A') => [">>vvv"],
            ('7', '0') => [">vvv"],

            ('8', '1') => ["vv<", "vv<"],
            ('8', '5') => ["vv"],
            ('8', '3') => ["vv>", ">vv"],
            ('8', '4') => ["v<", "<v"],
            ('8', '2') => ["v"],
            ('8', '6') => ["v>", ">v"],
            ('8', '7') => ["<"],
            ('8', '8') => [""],
            ('8', '9') => [">"],
            ('8', 'A') => [">vvv", "vvv>"],
            ('8', '0') => ["vvv"],

            ('9', '1') => ["vv<<", "<<vv"],
            ('9', '2') => ["vv<", "<vv"],
            ('9', '3') => ["v"],
            ('9', '4') => ["<<"],
            ('9', '5') => ["v<", "<v"],
            ('9', '6') => ["v"],
            ('9', '7') => ["<<"],
            ('9', '8') => ["<"],
            ('9', '9') => [""],
            ('9', 'A') => ["vvv"],
            ('9', '0') => ["vvv<", "<vvv"],

            ('A', '1') => ["^<<"],
            ('A', '2') => ["^<", "<^"],
            ('A', '3') => ["^"],
            ('A', '4') => ["^^<<"],
            ('A', '5') => ["^^<", "<^^"],
            ('A', '6') => ["^^"],
            ('A', '7') => ["^^^<<"],
            ('A', '8') => ["^^^<", "<^^^"],
            ('A', '9') => ["^^^"],
            ('A', 'A') => [""],
            ('A', '0') => ["<"],
            _ => ["?"],
        };
    }

    private IEnumerable<string> GetRobotInstructionsToPressArrows(char currentKey, char nextKey)
    {
        return (currentKey, nextKey) switch
        {
            ('A', '^') => ["<"],
            ('A', '<') => ["v<<"],
            ('A', 'v') => ["v<", "v<"],
            ('A', '>') => ["v"],
            ('A', 'A') => [""],
            
            ('^', '^') => [""],
            ('^', '<') => ["v<"],
            ('^', 'v') => ["v"],
            ('^', '>') => ["v>", ">v"],
            ('^', 'A') => [">"],

            ('<', '^') => [">^"],
            ('<', '<') => [""],
            ('<', 'v') => [">"],
            ('<', '>') => [">>"],
            ('<', 'A') => [">>^"],
            
            ('v', '^') => ["^"],
            ('v', '<') => ["<"],
            ('v', 'v') => [""],
            ('v', '>') => [">"],
            ('v', 'A') => [">^", "^>"],
            
            ('>', '^') => ["^<", "<^"],
            ('>', '<') => ["<<"],
            ('>', 'v') => ["<"],
            ('>', '>') => [""],
            ('>', 'A') => ["^"],
            
            _ => ["?"]
        };
    }


    internal static IEnumerable<string> ParseInput()
    {
        return File.ReadAllLines("inputs/day21.txt");
    }
}

internal delegate string GetRobotInstruction(char currentKey, char nextKey);